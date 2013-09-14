using System;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 管理员的管理者对象
    /// </summary>
    public class AdministratorManager : SimplifyManagerBase<Administrator>, IManager<Administrator>, ISimplify<Administrator>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理员的管理者对象
        /// </summary>
        /// <param name="db"></param>
        public AdministratorManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 管理用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">网络地址</param>
        /// <returns>返回所登录的管理用户的封装信息</returns>
        public Administrator Login(string username, string password, string ip)
        {
            Administrator administrator = db.Set<Administrator>().FirstOrDefault(x => x.Username == username);
            if (administrator == null) { throw new Exception("指定用户名的管理用户不存在"); }
            string _password = EncryptHelper.EncryptByMd5(password);
            if (administrator.Password != _password) { throw new Exception("密码错误"); }

            DateTime now = DateTime.Now;
            LoginEventArgs eventArgs = new LoginEventArgs(db, administrator, now, ip);   //实例化监视对象
            if (LoginingEventHandler != null)
            {
                LoginingEventHandler(this, eventArgs);  //触发前置事件
            }
            administrator.LastLoginTime = now;
            administrator.LastLoginIp = ip;
            db.SaveChanges();
            if (LoginedEventHandler != null)
            {
                LoginedEventHandler(this, eventArgs);  //触发后置事件
            }

            return administrator;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="administratorId">目标管理员的存储指针</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>返回一个字符串，代表随机生成的用户的新密码</returns>
        public void ResetPassage(int administratorId, string oldPassword, string newPassword)
        {
            NChecker.CheckEntity<Administrator>(administratorId, "管理员账户", db);
            TextHelper.Check(oldPassword, TextHelper.Key.Password);
            TextHelper.Check(newPassword, TextHelper.Key.Password);
            Administrator administrator = db.Set<Administrator>().Find(administratorId);
            string _oldPassword = EncryptHelper.EncryptByMd5(oldPassword);
            if (_oldPassword != administrator.Password)
            {
                throw new Exception("原密码不正确");
            }

            ResetPassageEventArgs eventArgs = new ResetPassageEventArgs(db, administrator, newPassword); //实例化监视对象
            if (ResetingPassageEventHandler != null)
            {
                ResetingPassageEventHandler(this, eventArgs);   //触发前置事件
            }
            administrator.Password = EncryptHelper.EncryptByMd5(newPassword);
            administrator.ModifiedTime = DateTime.Now;
            db.SaveChanges();
            if (ResetedPassageEventHandler != null)
            {
                ResetedPassageEventHandler(this, eventArgs);    //触发后置事件
            }
        }

        /// <summary>
        /// 修改管理用户组
        /// </summary>
        /// <param name="administratorId">目标管理员的存储指针</param>
        /// <param name="newGroupId">新的管理用户组的存储指针</param>
        public void ChangeGroup(int administratorId, int newGroupId)
        {
            NChecker.CheckEntity<Administrator>(administratorId, "管理员账户", db);
            NChecker.CheckEntity<AdministratorGroup>(newGroupId, "管理员用户组", db);
            AdministratorGroup newGroup = db.Set<AdministratorGroup>().Find(newGroupId);
            if (newGroup.Grade == 255)
            {
                throw new Exception("严禁向“系统管理员”组添加成员");
            }
            Administrator administrator = db.Set<Administrator>().Find(administratorId);
            if (administrator.Group.Grade == 255)
            {
                throw new Exception("严禁修改系统管理员的管理用户组");
            }
            if (administrator.Group.Id == newGroup.Id)
            {
                throw new Exception("该用户已经在这个组内");
            }

            ChangeGroupEventArgs eventArgs = new ChangeGroupEventArgs(db, administrator, administrator.Group, newGroup);
            if (ChangingGroupEventHandler != null)
            {
                ChangingGroupEventHandler(this, eventArgs); //触发前置事件
            }
            administrator.Group = newGroup;
            administrator.ModifiedTime = DateTime.Now;
            db.SaveChanges();
            if (ChangedGroupEventHandler != null)
            {
                ChangedGroupEventHandler(this, eventArgs); //触发后置事件
            }
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工程
        /// </summary>
        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建管理员的数据集
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="groupId">用户组的存储指针</param>
            /// <returns>返回用于新建管理员的数据集</returns>
            public static ICreatePackage<Administrator> CreatePackAgeForCreate(string username, string password, int groupId)
            {
                return new PackAgeForCreate(username, password, groupId);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建管理员的数据集
            /// </summary>
            private class PackAgeForCreate : IPackage<Administrator>, ICreatePackage<Administrator>
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
                /// 用户组的存储指针
                /// </summary>
                public int GroupId { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建管理员的数据集
                /// </summary>
                /// <param name="username">用户名</param>
                /// <param name="password">密码</param>
                /// <param name="groupId">用户组的存储指针</param>
                public PackAgeForCreate(string username, string password, int groupId)
                {
                    this.Username = username;
                    this.Password = password;
                    this.GroupId = groupId;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    TextHelper.Check(this.Username, TextHelper.Key.Nickname);
                    TextHelper.Check(this.Password, TextHelper.Key.Password);
                    bool hadUsedName = db.Set<Administrator>().Any(x => x.Username == this.Username);
                    if (hadUsedName)
                    {
                        throw new Exception(string.Format("已经存在同名的管理员账户：{0}", this.Username));
                    }
                    NChecker.CheckEntity<AdministratorGroup>(this.GroupId, "管理用户组", db);
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public Administrator GetEntity(DbContext db)
                {
                    AdministratorGroup group = db.Set<AdministratorGroup>().Find(this.GroupId);
                    string password = EncryptHelper.EncryptByMd5(this.Password);

                    return new Administrator(this.Username, password, group);
                }

                #endregion
            }

            #endregion
        }

        #region 监视对象

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
        /// 监视改变管理用户组动作的对象
        /// </summary>
        public class ChangeGroupEventArgs : NEventArgs
        {
            #region 公开属性

            /// <summary>
            /// 原管理用户组
            /// </summary>
            public AdministratorGroup OldGroup { get; set; }

            /// <summary>
            /// 新的管理用户组
            /// </summary>
            public AdministratorGroup NewGroup { get; set; }

            #endregion

            #region 构造方法

            /// <summary>
            /// 实例化一个新的监视改变管理用户组动作的对象
            /// </summary>
            /// <param name="db">数据库连接对象</param>
            /// <param name="State">参数实体</param>
            /// <param name="oldGroup">原管理用户组</param>
            /// <param name="newGroup">新的管理用户组</param>
            public ChangeGroupEventArgs(DbContext db, object state, AdministratorGroup oldGroup, AdministratorGroup newGroup)
                : base(db, state)
            {
                this.OldGroup = oldGroup;
                this.NewGroup = newGroup;
            }

            #endregion
        }

        #endregion

        #region 内嵌委托

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
        /// 改变管理用户组动作的实例委托
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public delegate void ChangeGroupDelegate(object sender, ChangeGroupEventArgs e);

        #endregion

        #endregion

        #region 有关事件

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
        /// 改变管理用户组前将触发的事件
        /// </summary>
        public static event ChangeGroupDelegate ChangingGroupEventHandler;

        /// <summary>
        /// 改变管理用户组后将触发的事件
        /// </summary>
        public static event ChangeGroupDelegate ChangedGroupEventHandler;

        #endregion
    }
}
