using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 用户的管理者对象
    /// </summary>
    public class AuthorManager : SimplifyCategoryManagerBase<Author>, IManager<Author>, ICategoryManager<Author>, ISimplify<Author>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AuthorManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 修改用户的返点数
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="normalReturnPoints">普通返点</param>
        /// <param name="uncertainReturnPoints">不定位返点</param>
        /// <param name="operatorId">操作人的存储指针</param>
        public void ChangeReturnPoints(int userId, double normalReturnPoints, double uncertainReturnPoints, int operatorId)
        {
            NChecker.CheckEntity<Author>(userId, "用户", db);
            var aSet = db.Set<Author>();
            var aR = aSet.Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.NormalReturnPoints,
                    x.UncertainReturnPoints,
                    x.LeftKey,
                    x.RightKey,
                    x.Layer
                })
                .FirstOrDefault();
            if (normalReturnPoints < aR.NormalReturnPoints
                || uncertainReturnPoints < aR.UncertainReturnPoints)
            {
                throw new Exception("不允许降低用户的返点数");
            }
            var parentId = aSet.Where(x => x.LeftKey < aR.LeftKey
                && x.RightKey > aR.RightKey
                && x.Layer == aR.Layer - 1)
                .Select(x => x.Id)
                .FirstOrDefault();
            if (parentId != operatorId) { throw new Exception("只有目标用户的直属上级用户有权修改用户的返点数"); }
            NChecker.CheckerReturnPoints(normalReturnPoints, uncertainReturnPoints, operatorId, db);

            Author user = aSet.Find(userId);
            ChangeReturnPointsEventArgs eventArgs = new ChangeReturnPointsEventArgs(db, user, aR.NormalReturnPoints
                , aR.UncertainReturnPoints, normalReturnPoints, uncertainReturnPoints); //实例化监视对象
            if (ChangingReturnPointsEventHandler != null)
            {
                ChangingReturnPointsEventHandler(this, eventArgs);  //重发前置事件
            }
            user.NormalReturnPoints = normalReturnPoints;
            user.UncertainReturnPoints = uncertainReturnPoints;
            db.SaveChanges();
            if (ChangedReturnPointsEventHandler != null)
            {
                ChangedReturnPointsEventHandler(this, eventArgs);   //触发后置事件
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">网络地址</param>
        /// <returns>返回所登录的用户的封装信息</returns>
        public Author Login(string username, string password, string ip)
        {
            Author user = db.Set<Author>().FirstOrDefault(x => x.Username == username);
            if (user == null) { throw new Exception("指定用户名的用户不存在"); }
            string _password = EncryptHelper.EncryptByMd5(password);
            if (user.Password != _password) { throw new Exception("密码错误"); }

            DateTime now = DateTime.Now;
            LoginEventArgs eventArgs = new LoginEventArgs(db, user, now, ip);   //实例化监视对象
            if (LoginingEventHandler != null)
            {
                LoginingEventHandler(this, eventArgs);  //触发前置事件
            }
            user.LastLoginTime = now;
            user.LastLoginIp = ip;
            db.SaveChanges();
            if (LoginedEventHandler != null)
            {
                LoginedEventHandler(this, eventArgs);  //触发后置事件
            }

            return user;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <returns>返回一个字符串，代表随机生成的用户的新密码</returns>
        public string ResetPassage(int userId)
        {
            NChecker.CheckEntity<Author>(userId, "用户", db);
            string newPassword = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                int t = random.Next(0, 9);
                newPassword += t;
            }
            Author user = db.Set<Author>().Find(userId);
            ResetPassageEventArgs eventArgs = new ResetPassageEventArgs(db, user, newPassword); //实例化监视对象
            if (ResetingPassageEventHandler != null)
            {
                ResetingPassageEventHandler(this, eventArgs);   //触发前置事件
            }
            user.Password = EncryptHelper.EncryptByMd5(newPassword);
            user.ModifiedTime = DateTime.Now;
            db.SaveChanges();
            if (ResetedPassageEventHandler != null)
            {
                ResetedPassageEventHandler(this, eventArgs);    //触发后置事件
            }

            return newPassword;
        }

        /// <summary>
        /// 重置用户的安全码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <returns>返回一个字符串，代表随机生成的用户的新的安全码</returns>
        public string ResetSafeCode(int userId)
        {
            NChecker.CheckEntity<Author>(userId, "用户", db);
            string newSafeCode = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                int t = random.Next(0, 9);
                newSafeCode += t;
            }
            Author user = db.Set<Author>().Find(userId);
            ResetSafeCodeEventArgs eventArgs = new ResetSafeCodeEventArgs(db, user, newSafeCode);   //实例化监视对象
            if (ResetingSafeCodeEventHandler != null)
            {
                ResetingSafeCodeEventHandler(this, eventArgs);  //触发前置事件
            }
            user.SafeCode = EncryptHelper.EncryptByMd5(newSafeCode);
            user.ModifiedTime = DateTime.Now;
            db.SaveChanges();
            if (ResetedSafeCodeEventHandler != null)
            {
                ResetedSafeCodeEventHandler(this, eventArgs);   //触发后置事件
            }

            return newSafeCode;
        }

        /// <summary>
        /// 重置用户的安全邮箱
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        public void ResetEmail(int userId)
        {
            NChecker.CheckEntity<Author>(userId, "用户", db);
            Author user = db.Set<Author>().Find(userId);
            NEventArgs eventArgs = new NEventArgs(db, user);    //实例化监视对象
            if (ResetingEmailEventHandler != null)
            {
                ResetingEmailEventHandler(this, eventArgs);  //触发后置事件
            }
            user.Email = "";
            user.BindingEmail = false;
            db.SaveChanges();
            if (ResetedEmailEventHandler != null)
            {
                ResetedEmailEventHandler(this, eventArgs);  //触发后置事件
            }
        }

        /// <summary>
        /// 重置用户的银行卡信息
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        public void ResetCard(int userId)
        {
            NChecker.CheckEntity<Author>(userId, "用户", db);
            Author user = db.Set<Author>().Find(userId);
            NEventArgs eventArgs = new NEventArgs(db, user);    //实例化监视对象
            if (ResetingCardEventHandler != null)
            {
                ResetingCardEventHandler(this, eventArgs);  //触发后置事件
            }
            user.Holder = "";
            user.Card = "";
            user.Bank = Bank.无;
            user.BindingCard = false;
            db.SaveChanges();
            if (ResetedCardEventHandler != null)
            {
                ResetedCardEventHandler(this, eventArgs);  //触发后置事件
            }
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工厂
        /// </summary>
        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建用户的数据集
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="isAgents">是否代理用户</param>
            /// <param name="normalReturnPoints">正常返点数</param>
            /// <param name="uncertainReturnPoints">不定位返点数</param>
            /// <param name="groupId">最多拥有直属下级数量限制</param>
            /// <param name="parentId">上级用户的存储指针</param>
            /// <returns>返回用于新建用户的数据集</returns>
            public static ICreateCategoryPackagr<Author> CreatePackageForCreate(string username, string password, bool isAgents
                , double normalReturnPoints, double uncertainReturnPoints, int MaxOfSubordinate, int parentId)
            {
                return new PackageForCreate(username, password, isAgents, normalReturnPoints, uncertainReturnPoints
                    , MaxOfSubordinate, parentId);
            }

            /// <summary>
            /// 创建一个用于修改用户信息的数据集（用于激活）
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="email">安全邮箱</param>
            /// <param name="holder">开户人</param>
            /// <param name="card">银行卡</param>
            /// <param name="bank">银行</param>
            /// <param name="oldPassword">原密码</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="confirmPassword">新密码确认</param>
            /// <param name="safeCode">安全码</param>
            /// <param name="confirmSafeCode">安全码确认</param>
            /// <returns></returns>
            public static IUpdatePackage<Author> CreatePackageForActivation(int id, string email, string holder, string card
                , Bank bank, string oldPassword, string newPassword, string confirmPassword, string safeCode
                , string confirmSafeCode)
            {
                return new PackageForActivation(id, email, holder, card, bank, oldPassword, newPassword, confirmPassword, safeCode
                    , confirmSafeCode);
            }

            /// <summary>
            /// 创建一个用于绑定用户的安全邮箱的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="newEmail">新的安全邮箱</param>
            /// <param name="confirmEmail">确认安全邮箱</param>
            /// <returns></returns>
            public static IUpdatePackage<Author> CreatePackageForBindingEmail(int id, string newEmail, string confirmEmail)
            {
                Bank _bank = Bank.无;

                return new PackageForUpdate(id, newEmail, confirmEmail, "", "", _bank, "", "", "", "", "", "");
            }

            /// <summary>
            /// 创建一个用于绑定用户的银行卡信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="holder">开户人</param>
            /// <param name="card">银行卡</param>
            /// <param name="bank">银行</param>
            /// <returns>返回用于修改用户信息的数据集</returns>
            public static IUpdatePackage<Author> CreatePackageForBindingCard(int id, string holder, string card, string bank)
            {
                Bank _bank = EnumHelper.Parse<Bank>(bank);

                return new PackageForUpdate(id, "", "", holder, card, _bank, "", "", "", "", "", "");
            }

            /// <summary>
            /// 创建一个用于修改用户密码的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="oldPassword">原密码</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="confirmPassword">新密码确认</param>
            /// <returns>返回用于修改用户信息的数据集</returns>
            public static IUpdatePackage<Author> CreatePackageForUpdatePassword(int id, string oldPassword, string newPassword
                , string confirmPassword)
            {
                Bank _bank = Bank.无;

                return new PackageForUpdate(id, "", "", "", "", _bank, oldPassword, newPassword, confirmPassword, "", "", "");
            }

            /// <summary>
            /// 创建一个用于修改用户的安全码的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="oldSafeCode">原安全码</param>
            /// <param name="newSafeCode">新安全码</param>
            /// <param name="confirmSafeCode">新安全码确认</param>
            /// <returns>返回用于修改用户信息的数据集</returns>
            public static IUpdatePackage<Author> CreatePackageForUpdateSafeCode(int id, string oldSafeCode, string newSafeCode
                , string confirmSafeCode)
            {
                Bank _bank = Bank.无;

                return new PackageForUpdate(id, "", "", "", "", _bank, "", "", "", oldSafeCode, newSafeCode, confirmSafeCode);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建用户的数据集
            /// </summary>
            private class PackageForCreate : PackageForCreateCategoryBase<Author>, IPackage<Author>, ICreateCategoryPackagr<Author>
            {
                #region 公开属性

                /// <summary>
                /// 用户名
                /// </summary>
                public string Username { get; set; }

                /// <summary>
                /// 密码
                /// </summary>
                public string Password { get; set; }

                /// <summary>
                /// 代理（拥有创建下级用户的权限）
                /// </summary>
                public bool IsAgents { get; set; }

                /// <summary>
                /// 正常返点数
                /// </summary>
                public double NormalReturnPoints { get; set; }

                /// <summary>
                /// 不定位返点数
                /// </summary>
                public double UncertainReturnPoints { get; set; }

                /// <summary>
                /// 最多拥有直属下级数量限制
                /// </summary>
                public int MaxOfSubordinate { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建用户的数据集
                /// </summary>
                /// <param name="username">用户名</param>
                /// <param name="password">密码</param>
                /// <param name="isAgents">是否代理用户</param>
                /// <param name="normalReturnPoints">正常返点数</param>
                /// <param name="uncertainReturnPoints">不定位返点数</param>
                /// <param name="parentId">上级用户的存储指针</param>
                public PackageForCreate(string username, string password, bool isAgents, double normalReturnPoints
                    , double uncertainReturnPoints, int MaxOfSubordinate, int parentId)
                    : base(parentId)
                {
                    this.Username = username;
                    this.Password = password;
                    this.IsAgents = isAgents;
                    this.NormalReturnPoints = normalReturnPoints;
                    this.UncertainReturnPoints = uncertainReturnPoints;
                    this.MaxOfSubordinate = MaxOfSubordinate;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    TextHelper.Check(this.Username, TextHelper.Key.Nickname);
                    TextHelper.Check(this.Password, TextHelper.Key.Password);
                    base.CheckData(db);
                    if (db.Set<Author>().Any(x => x.Username == this.Username)) { throw new Exception("已经存在同名用户"); }

                    NChecker.CheckerReturnPoints(this.NormalReturnPoints, this.UncertainReturnPoints, this.ParentId, db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Author GetEntity(DbContext db)
                {
                    string password = EncryptHelper.EncryptByMd5(this.Password);
                    UserGroup group = db.Set<UserGroup>().Where(x => x.LimitOfConsumption <= 0 && x.UpperOfConsumption >= 0)
                        .OrderBy(x => x.Grade).FirstOrDefault();
                    if (group == null)
                    {
                        group = new UserGroup("普通会员", "", 1, 0, 1000000, 0, 0, 0, 0, 0, false, 0);
                    }

                    return new Author(this.Username, password, this.IsAgents, this.NormalReturnPoints, this.UncertainReturnPoints
                        , group, this.MaxOfSubordinate);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改用户信息的数据集（用于激活）
            /// </summary>
            private class PackageForActivation : PackageForUpdateBase<Author>, IPackage<Author>, IUpdatePackage<Author>
            {
                #region 公开属性

                /// <summary>
                /// 安全邮箱
                /// </summary>
                public string Email { get; set; }

                /// <summary>
                /// 开户人
                /// </summary>
                public string Holder { get; set; }

                /// <summary>
                /// 银行卡
                /// </summary>
                public string Card { get; set; }

                /// <summary>
                /// 银行
                /// </summary>
                public Bank Bank { get; set; }

                /// <summary>
                /// 原密码
                /// </summary>
                public string OldPassword { get; set; }

                /// <summary>
                /// 新密码
                /// </summary>
                public string NewPassword { get; set; }

                /// <summary>
                /// 新密码确认
                /// </summary>
                public string ConfirmPassword { get; set; }

                /// <summary>
                /// 安全码
                /// </summary>
                public string SafeCode { get; set; }

                /// <summary>
                /// 安全码确认
                /// </summary>
                public string ConfirmSafeCode { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改用户信息的数据集（用于激活）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="email">安全邮箱</param>
                /// <param name="holder">开户人</param>
                /// <param name="card">银行卡</param>
                /// <param name="bank">银行</param>
                /// <param name="oldPassword">原密码</param>
                /// <param name="newPassword">新密码</param>
                /// <param name="confirmPassword">新密码确认</param>
                /// <param name="safeCode">安全码</param>
                /// <param name="confirmSafeCode">安全码确认</param>
                public PackageForActivation(int id, string email, string holder, string card, Bank bank, string oldPassword, string newPassword
                    , string confirmPassword, string safeCode, string confirmSafeCode)
                    : base(id)
                {
                    this.Email = email;
                    this.Holder = holder;
                    this.Card = card;
                    this.Bank = bank;
                    this.OldPassword = oldPassword;
                    this.NewPassword = newPassword;
                    this.ConfirmPassword = confirmPassword;
                    this.SafeCode = safeCode;
                    this.ConfirmSafeCode = confirmSafeCode;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    base.CheckData(db);
                    TextHelper.Check(this.Email, TextHelper.Key.Email);
                    if (this.NewPassword != "")
                    {
                        TextHelper.Check(this.OldPassword, TextHelper.Key.Password);
                        TextHelper.Check(this.NewPassword, TextHelper.Key.Password);
                        if (this.NewPassword != this.ConfirmPassword)
                        {
                            throw new Exception("新密码与第二次输入的确认不一致");
                        }
                        string oldPassword = EncryptHelper.EncryptByMd5(this.OldPassword);
                        bool correct = db.Set<Author>().Any(x => x.Id == this.Id
                            && x.Password == oldPassword);
                        if (!correct) { throw new Exception("原密码不正确"); }
                    }
                    TextHelper.Check(this.SafeCode, TextHelper.Key.Password);
                    if (this.SafeCode != this.ConfirmSafeCode)
                    {
                        throw new Exception("新安全码与第二次输入的确认不一致");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Author GetEntity(DbContext db)
                {
                    string newPassword = EncryptHelper.EncryptByMd5(this.NewPassword);
                    string safeCode = EncryptHelper.EncryptByMd5(this.SafeCode);

                    this.AddToUpdating("Email", this.Email);
                    this.AddToUpdating("BindingEmail", true);
                    this.AddToUpdating("Holder", this.Holder);
                    this.AddToUpdating("Card", this.Card);
                    this.AddToUpdating("Bank", this.Bank);
                    this.AddToUpdating("BindingCard", true);
                    this.AddToUpdating("Password", newPassword);
                    this.AddToUpdating("SafeCode", safeCode);
                    this.AddToUpdating("Status", UserStatus.正常);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于修改用户信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<Author>, IPackage<Author>, IUpdatePackage<Author>
            {
                #region 公开属性

                /// <summary>
                /// 新的安全邮箱
                /// </summary>
                public string NewEmail { get; set; }

                /// <summary>
                /// 确认安全邮箱
                /// </summary>
                public string ConfirmEmail { get; set; }

                /// <summary>
                /// 开户人
                /// </summary>
                public string Holder { get; set; }

                /// <summary>
                /// 银行卡
                /// </summary>
                public string Card { get; set; }

                /// <summary>
                /// 银行
                /// </summary>
                public Bank Bank { get; set; }

                /// <summary>
                /// 原密码
                /// </summary>
                public string OldPassword { get; set; }

                /// <summary>
                /// 新密码
                /// </summary>
                public string NewPassword { get; set; }

                /// <summary>
                /// 新密码确认
                /// </summary>
                public string ConfirmPassword { get; set; }

                /// <summary>
                /// 原安全码
                /// </summary>
                public string OldSafeCode { get; set; }

                /// <summary>
                /// 新安全码
                /// </summary>
                public string NewSafeCode { get; set; }

                /// <summary>
                /// 新安全码确认
                /// </summary>
                public string ConfirmSafeCode { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于修改用户信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="newEmail">新的安全邮箱</param>
                /// <param name="confirmEmail">确认安全邮箱</param>
                /// <param name="holder">开户人</param>
                /// <param name="card">银行卡</param>
                /// <param name="bank">银行</param>
                /// <param name="oldPassword">原密码</param>
                /// <param name="newPassword">新密码</param>
                /// <param name="confirmPassword">新密码确认</param>
                /// <param name="oldSafeCode">原安全码</param>
                /// <param name="newSafeCode">新安全码</param>
                /// <param name="confirmSafeCode">新安全码确认</param>
                public PackageForUpdate(int id, string newEmail, string confirmEmail, string holder, string card, Bank bank
                    , string oldPassword, string newPassword, string confirmPassword, string oldSafeCode, string newSafeCode
                    , string confirmSafeCode)
                    : base(id)
                {
                    this.NewEmail = newEmail;
                    this.ConfirmEmail = confirmEmail;
                    this.Holder = holder;
                    this.Card = card;
                    this.Bank = bank;
                    this.OldPassword = oldPassword;
                    this.NewPassword = newPassword;
                    this.ConfirmPassword = confirmPassword;
                    this.OldSafeCode = oldSafeCode;
                    this.NewSafeCode = newSafeCode;
                    this.ConfirmSafeCode = confirmSafeCode;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    base.CheckData(db);
                    if (this.NewEmail != "")
                    {
                        TextHelper.Check(this.NewEmail, TextHelper.Key.Email);
                        if (this.NewEmail != this.ConfirmEmail)
                        {
                            throw new Exception("新的安全邮箱与第二次输入的确认不一致");
                        }
                        bool bindingEmail = db.Set<Author>().Where(x => x.Id == this.Id)
                            .Select(x => x.BindingEmail)
                            .FirstOrDefault();
                        if (bindingEmail)
                        {
                            throw new Exception("禁止修改已经被绑定的安全邮箱");
                        }
                    }
                    if (this.Bank != Model.Bank.无)
                    {
                        bool bindingCard = db.Set<Author>().Where(x => x.Id == this.Id)
                            .Select(x => x.BindingCard)
                            .FirstOrDefault();
                        if (bindingCard)
                        {
                            throw new Exception("禁止修改已经被绑定的银行卡信息");
                        }
                    }
                    if (this.NewPassword != "")
                    {
                        TextHelper.Check(this.OldPassword, TextHelper.Key.Password);
                        TextHelper.Check(this.NewPassword, TextHelper.Key.Password);
                        if (this.NewPassword != this.ConfirmPassword)
                        {
                            throw new Exception("新密码与第二次输入的确认不一致");
                        }
                        string oldPassword = EncryptHelper.EncryptByMd5(this.OldPassword);
                        bool correct = db.Set<Author>().Any(x => x.Id == this.Id
                            && x.Password == oldPassword);
                        if (!correct) { throw new Exception("原密码不正确"); }
                    }
                    if (this.NewSafeCode != "")
                    {
                        TextHelper.Check(this.OldSafeCode, TextHelper.Key.Password);
                        TextHelper.Check(this.NewSafeCode, TextHelper.Key.Password);
                        if (this.NewSafeCode != this.ConfirmSafeCode)
                        {
                            throw new Exception("新安全码与第二次输入的确认不一致");
                        }
                        string oldSafeCode = EncryptHelper.EncryptByMd5(this.OldPassword);
                        bool correct = db.Set<Author>().Any(x => x.Id == this.Id
                            && x.SafeCode == oldSafeCode);
                        if (!correct) { throw new Exception("原安全码不正确"); }
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override Author GetEntity(DbContext db)
                {
                    if (this.NewEmail != "")
                    {
                        this.AddToUpdating("Email", this.NewEmail);
                    }
                    if (this.Bank != Model.Bank.无)
                    {
                        this.AddToUpdating("Holder", this.Holder);
                        this.AddToUpdating("Card", this.Card);
                        this.AddToUpdating("Bank", this.Bank);
                    }
                    if (this.NewPassword != "")
                    {
                        string newPassword = EncryptHelper.EncryptByMd5(this.NewPassword);
                        this.AddToUpdating("Password", newPassword);
                    }
                    if (this.NewSafeCode != "")
                    {
                        string newSafeCode = EncryptHelper.EncryptByMd5(this.NewSafeCode);
                        this.AddToUpdating("SafeCode", NewSafeCode);
                    }

                    return base.GetEntity(db);
                }

                #endregion
            }

            #endregion
        }

        #region 监视对象

        /// <summary>
        /// 监视修改用户的返点数动作的对象
        /// </summary>
        public class ChangeReturnPointsEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原普通返点
            /// </summary>
            public double OldNormalReturnPoints { get; set; }

            /// <summary>
            /// 原不定位返点
            /// </summary>
            public double OldUncertainReturnPoints { get; set; }

            /// <summary>
            /// 新普通返点
            /// </summary>
            public double NewNormalReturnPoints { get; set; }

            /// <summary>
            /// 新不定位返点
            /// </summary>
            public double NewUncertainReturnPoints { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的监视修改用户的返点数动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="State">参数实体</param>
            /// <param name="oldNormalReturnPoints">原普通返点</param>
            /// <param name="oldUncertainReturnPoints">原不定位返点</param>
            /// <param name="newNormalReturnPoints">新普通返点</param>
            /// <param name="newUncertainReturnPoints">新不定位返点</param>
            public ChangeReturnPointsEventArgs(DbContext db, object state, double oldNormalReturnPoints
                , double oldUncertainReturnPoints, double newNormalReturnPoints, double newUncertainReturnPoints)
                : base(db, state)
            {
                this.OldNormalReturnPoints = oldNormalReturnPoints;
                this.OldUncertainReturnPoints = oldUncertainReturnPoints;
                this.NewNormalReturnPoints = newNormalReturnPoints;
                this.NewUncertainReturnPoints = newUncertainReturnPoints;
            }

            #endregion
        }

        /// <summary>
        /// 监视登录动作的对象
        /// </summary>
        public class LoginEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 登录时间
            /// </summary>
            public DateTime LoginTime { get; set; }

            /// <summary>
            /// 登录的网络地址
            /// </summary>
            public string LoginIp { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的监视登录动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="State">参数实体</param>
            /// <param name="loginTime">登录时间</param>
            /// <param name="loginIp">登录的网络地址</param>
            public LoginEventArgs(DbContext db, object state, DateTime loginTime, string loginIp)
                : base(db, state)
            {
                this.LoginTime = loginTime;
                this.LoginIp = loginIp;
            }

            #endregion
        }

        /// <summary>
        /// 监视重置用户密码动作的对象
        /// </summary>
        public class ResetPassageEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 新密码
            /// </summary>
            public string NewPassword { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的监视重置用户密码动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="State">参数实体</param>
            /// <param name="newPassword">新密码</param>
            public ResetPassageEventArgs(DbContext db, object state, string newPassword)
                : base(db, state)
            {
                this.NewPassword = newPassword;
            }

            #endregion
        }

        /// <summary>
        /// 监视重置用户安全码动作的对象
        /// </summary>
        public class ResetSafeCodeEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 新的安全码
            /// </summary>
            public string NewSafeCode { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的监视重置用户的安全码动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="State">参数实体</param>
            /// <param name="newSafeCode">新的安全码</param>
            public ResetSafeCodeEventArgs(DbContext db, object state, string newSafeCode)
                : base(db, state)
            {
                this.NewSafeCode = newSafeCode;
            }

            #endregion
        }

        #endregion

        #region 内嵌委托

        /// <summary>
        /// 修改用户的返点数动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeReturnPointsDelegate(object sender, ChangeReturnPointsEventArgs e);

        /// <summary>
        /// 登录动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void LoginDelegate(object sender, LoginEventArgs e);

        /// <summary>
        /// 重置用户密码动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ResetPassageDelegate(object sender, ResetPassageEventArgs e);

        /// <summary>
        /// 重置用户的安全码动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ResetSafeCodeDelegate(object sender, ResetSafeCodeEventArgs e);

        #endregion

        #endregion

        #region 有关事件

        /// <summary>
        /// 修改用户的返点数前将触发的事件
        /// </summary>
        public static event ChangeReturnPointsDelegate ChangingReturnPointsEventHandler;

        /// <summary>
        /// 修改用户的返点数后将触发的事件
        /// </summary>
        public static event ChangeReturnPointsDelegate ChangedReturnPointsEventHandler;

        /// <summary>
        /// 用户登录前将触发的事件
        /// </summary>
        public static event LoginDelegate LoginingEventHandler;

        /// <summary>
        /// 用户登录后将触发的事件
        /// </summary>
        public static event LoginDelegate LoginedEventHandler;

        /// <summary>
        /// 重置用户密码前将触发的事件
        /// </summary>
        public static event ResetPassageDelegate ResetingPassageEventHandler;

        /// <summary>
        /// 重置用户密码后将触发的事件
        /// </summary>
        public static event ResetPassageDelegate ResetedPassageEventHandler;

        /// <summary>
        /// 重置用户的安全码前将触发的事件
        /// </summary>
        public static event ResetSafeCodeDelegate ResetingSafeCodeEventHandler;

        /// <summary>
        /// 重置用户的安全码后将触发的事件
        /// </summary>
        public static event ResetSafeCodeDelegate ResetedSafeCodeEventHandler;

        /// <summary>
        /// 重置用户的安全邮箱前将触发的事件
        /// </summary>
        public static event NDelegate ResetingEmailEventHandler;

        /// <summary>
        /// 重置用户的安全邮箱后将触发的事件
        /// </summary>
        public static event NDelegate ResetedEmailEventHandler;

        /// <summary>
        /// 重置用户的银行卡信息前将触发的事件
        /// </summary>
        public static event NDelegate ResetingCardEventHandler;

        /// <summary>
        /// 重置用户的银行卡信息后将触发的事件
        /// </summary>
        public static event NDelegate ResetedCardEventHandler;

        #endregion

        #region 静态方法

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void ApplyToCash(object sender, NEventArgs e)
        {
            WithdrawalsRecord wr = (WithdrawalsRecord)e.State;
            Author owner = e.Db.Set<Author>().Find(wr.Owner.Id);
            owner.Money -= wr.Sum;
            owner.MoneyBeFrozen += wr.Sum;
        }

        /// <summary>
        /// 撤销提现
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void RevocationOfWithdrawals(object sender, NEventArgs e)
        {
            WithdrawalsRecord wr = (WithdrawalsRecord)e.State;
            if (wr.Status == WithdrawalsStatus.处理中)
            {
                Author owner = e.Db.Set<Author>().Find(wr.Owner.Id);
                owner.Money += wr.Sum;
                owner.MoneyBeFrozen -= wr.Sum;
            }
        }

        /// <summary>
        /// 反馈提现结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void GetResultOfWithdrawal(object sender, WithdrawalsRecordManager.ChangeStatusEventArgs e)
        {
            WithdrawalsRecord wr = (WithdrawalsRecord)e.State;
            Author owner = e.Db.Set<Author>().Find(wr.Owner.Id);
            switch (e.NewStatus)
            {
                case WithdrawalsStatus.提现成功:
                    owner.MoneyBeFrozen -= wr.Sum;
                    break;
                case WithdrawalsStatus.失败:
                    owner.Money += wr.Sum;
                    owner.MoneyBeFrozen -= wr.Sum;
                    break;
            }
        }

        /// <summary>
        /// 成功充值
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void SuccessfulRecharge(object sender, RechargeRecordManager.ChangeStatusEventArgs e)
        {
            RechargeRecord rr = (RechargeRecord)e.State;
            if (e.NewStatus == RechargeStatus.充值成功)
            {
                Author owner = e.Db.Set<Author>().Find(rr.Owner.Id);
                owner.Money += rr.Sum;
                owner.Integral += rr.Sum;
            }
        }

        /// <summary>
        /// 从下级用户的投注获得返点
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void RebateBySubordinate(object sender, NEventArgs e)
        {
            SubordinateDynamic sd = (SubordinateDynamic)e.State;
            Author to = e.Db.Set<Author>().Find(sd.To.Id);
            to.Money += sd.Give;
        }

        /// <summary>
        /// 为投注付费
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void PayForBetting(object sender, NEventArgs e)
        {
            Betting b = (Betting)e.State;
            Author owner = e.Db.Set<Author>().Find(b.Owner.Id);
            owner.Money -= b.Pay;
            owner.Consumption += b.Pay;
        }

        /// <summary>
        /// 反馈投注结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void GetResultOfBetting(object sender, BettingManager.ChangeStatusEventArgs e)
        {
            Betting b = (Betting)e.State;
            Author owner = e.Db.Set<Author>().Find(b.Owner.Id);
            switch (e.NewStatus)
            {
                case BettingStatus.中奖:
                    owner.Money += e.Bonus;
                    break;
                case BettingStatus.用户撤单:
                    if (e.OldStatus == BettingStatus.等待开奖)
                    {
                        owner.Money += b.Pay;
                        owner.Consumption -= b.Pay;
                    }
                    break;
            }
        }

        /// <summary>
        /// 为追号付款
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void PayForChasing(object sender, NEventArgs e)
        {
            Chasing c = (Chasing)e.State;
            Author owner = e.Db.Set<Author>().Find(c.Owner.Id);
            owner.Money -= c.Pay;
            owner.Consumption += c.Pay;
        }

        /// <summary>
        /// 返还被终止的追号的余额
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void ReturnTheBalanceOfChasing(object sender, ChasingManager.ChangeStatusEventArgs e)
        {
            Chasing c = (Chasing)e.State;
            if (e.NewStatus == ChasingStatus.因为所追号码已经开出而终止
                || e.NewStatus == ChasingStatus.因为中奖而终止
                || e.NewStatus == ChasingStatus.用户中止追号
                || e.NewStatus == ChasingStatus.追号结束)
            {
                Author owner = e.Db.Set<Author>().Find(c.Owner.Id);
                double surplus = c.Bettings
                    .Where(x => x.Status == BettingStatus.等待开奖)
                    .Sum(x => x.Pay);
                owner.Money += surplus;
                c.Pay -= surplus;
            }
        }

        /// <summary>
        /// 反馈投注（追号）结果
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void GetResultOfBettingForChasing(object sender, BettingForCgasingManager.ChangeStatusEventArgs e)
        {
            if (e.NewStatus == BettingStatus.中奖)
            {
                BettingForCgasing bfc = (BettingForCgasing)e.State;
                Author owner = e.Db.Set<Author>().Find(bfc.Chasing.Owner.Id);
                owner.Money += e.Bonus;
            }
        }

        /// <summary>
        /// 参与默认活动（获得相应奖励）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void ParticipateInActivity(object sender, NEventArgs e)
        {
            ActivityParticipateRecord participateRecord = (ActivityParticipateRecord)e.State;
            Author owner = e.Db.Set<Author>().Find(participateRecord.Owner.Id);
            double sum = participateRecord.Activity.RewardValueIsAbsolute ?
                participateRecord.Activity.Reward :
                Math.Round(participateRecord.Activity.Reward * participateRecord.Amount, 2);
            switch (participateRecord.Activity.RewardType)
            {
                case ActivityRewardType.人民币:
                    owner.Money += sum;
                    break;
                case ActivityRewardType.积分:
                    owner.Integral += sum;
                    break;
            }
        }

        /// <summary>
        /// 参与兑换活动（获得相应奖励）
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void ParticipateInExchange(object sender, NEventArgs e)
        {
            ExchangeParticipateRecord participateRecord = (ExchangeParticipateRecord)e.State;
            Author owner = e.Db.Set<Author>().Find(participateRecord.Owner.Id);
            double sum = 0;
            participateRecord.Exchange.Prizes.ForEach(x =>
                {
                    if (x.Type == PrizeType.人民币)
                    {
                        sum += x.Price * x.Sum;
                    }
                });
            sum *= participateRecord.Sum;
            owner.Money += sum;
            owner.Integral -= participateRecord.Exchange.UnitPrice * participateRecord.Sum;
        }

        #endregion
    }
}
