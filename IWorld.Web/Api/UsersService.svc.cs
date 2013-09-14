using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 用户信息相关的数据服务
    /// </summary>
    public class UsersService : IUsersService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回登录操作的结果</returns>
        public LoginResult Login(string username, string password)
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    AuthorManager authorManager = new AuthorManager(db);
                    string ip = WebHepler.GetEndpoint().Address;
                    Author author = authorManager.Login(username, password, ip);
                    string token = CacheManager.SetUserIn(author.Id);

                    return new LoginResult(token, true);
                }
            }
            catch (Exception ex)
            {
                return new LoginResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回用户信息</returns>
        public UserInfoResult GetUserInfo(string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new UserInfoResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    Author user = db.Set<Author>().Find(userId);
                    return new UserInfoResult(user);
                }
            }
            catch (Exception ex)
            {
                return new UserInfoResult(ex.Message);
            }
        }

        /// <summary>
        /// 绑定用户初始信息
        /// </summary>
        /// <param name="import">用于绑定用户初始信息的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Binding(UserInfoBindingImport import, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    IUpdatePackage<Author> pfu = AuthorManager.Factory.CreatePackageForActivation(userId, import.Email, import.Holder
                        , import.Card, import.Bank, import.OldPassword, import.NewPassword, import.NewPassword, import.SafeCode
                        , import.SafeCode);
                    new AuthorManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="onlyImmediate">一个布尔值 标记是否只看直属下级</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回基础用户信息的分页列表</returns>
        public PaginationList<BasicUserInfoResult> GetUsers(string keyword, bool onlyImmediate, int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<BasicUserInfoResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientUsersReader reader = new ClientUsersReader(db);
                    return reader.ReadUsers(userId, keyword, onlyImmediate, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<BasicUserInfoResult>(ex.Message);
            }
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="import">用于创建新用户的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回创建新用户结果</returns>
        public CreateUserResult CreateUser(CreateUserImport import, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new CreateUserResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    Author user = db.Set<Author>().Find(userId);
                    string newPassword = "";
                    Random random = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        int t = random.Next(0, 9);
                        newPassword += t;
                    }
                    int maxOfSubordinate = user.Group.MaxOfSubordinate == 0
                        ? new WebSetting().Subordinate : user.Group.MaxOfSubordinate;

                    ICreateCategoryPackagr<Author> pfc = AuthorManager.Factory.CreatePackageForCreate(import.Username, newPassword
                        , true, import.NormalReturnPoints, import.UncertainReturnPoints, maxOfSubordinate, userId);
                    Author newUser = new AuthorManager(db).Create(pfc);

                    return new CreateUserResult(newUser.Username, newPassword);
                }
            }
            catch (Exception ex)
            {
                return new CreateUserResult(ex.Message);
            }
        }

        /// <summary>
        /// 升点
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="normalReturnPoints">普通返点</param>
        /// <param name="uncertainReturnPoints">不定位返点</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult UpgradePorn(int userId, double normalReturnPoints, double uncertainReturnPoints, string token)
        {
            try
            {
                int _userId = CacheManager.GetUserId(token);
                if (_userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    new AuthorManager(db).ChangeReturnPoints(userId, normalReturnPoints, uncertainReturnPoints, _userId);
                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 编辑银行卡绑定信息
        /// </summary>
        /// <param name="card">银行卡卡号</param>
        /// <param name="holder">开户人</param>
        /// <param name="bank">开户银行</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditBank(string card, string holder, Bank bank, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    IUpdatePackage<Author> pfu = AuthorManager.Factory.CreatePackageForBindingCard(userId, holder, card
                        , bank.ToString());
                    new AuthorManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 编辑安全邮箱
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditEmail(string email, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    IUpdatePackage<Author> pfu = AuthorManager.Factory.CreatePackageForBindingEmail(userId, email, email);
                    new AuthorManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditPassword(string oldPassword, string newPassword, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    IUpdatePackage<Author> pfu = AuthorManager.Factory.CreatePackageForUpdatePassword(userId, oldPassword
                        , newPassword, newPassword);
                    new AuthorManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 修改安全码
        /// </summary>
        /// <param name="oldSafeWord">原安全码</param>
        /// <param name="newSafeWord">新安全码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditSafeWord(string oldSafeWord, string newSafeWord, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    IUpdatePackage<Author> pfu = AuthorManager.Factory.CreatePackageForUpdateSafeCode(userId, oldSafeWord
                        , newSafeWord, newSafeWord);
                    new AuthorManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }
    }
}
