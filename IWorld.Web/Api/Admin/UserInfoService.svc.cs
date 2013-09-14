using System;
using System.Linq;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;
using IWorld.Helper;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 用户管理的数据服务（后台）
    /// </summary>
    public class UserInfoService : IUserInfoService
    {
        /// <summary>
        /// 获取用户信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="teamForUser">目标用户的存储指针（指向该用户的团队）</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回用户信息的分页列表</returns>
        public PaginationList<UserInfoResult> GetList(string keyword, int groupId, int teamForUser, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<UserInfoResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewUsers)
                    {
                        return new PaginationList<UserInfoResult>("该用户所属的用户组无权查看用户信息");
                    }

                    AdminUsersReader reader = new AdminUsersReader(db);
                    return reader.ReadUserList(keyword, groupId, teamForUser, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<UserInfoResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取用户登录记录的分页列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间（xxxx-xx-xx 格式）</param>
        /// <param name="endTime">结束时间（xxxx-xx-xx 格式）</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回用户登录记录的分页列表</returns>
        public PaginationList<LandingRecordResult> GetLandingRecordList(int userId, string beginTime, string endTime, int page
            , string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<LandingRecordResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewUsers)
                    {
                        return new PaginationList<LandingRecordResult>("该用户所属的用户组无权查看用户信息");
                    }

                    AdminUsersReader reader = new AdminUsersReader(db);
                    return reader.ReadLandingRecordList(userId, beginTime, endTime, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<LandingRecordResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddUser(AddUserImport import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权添加新用户");
                    }

                    bool hadUsedName = db.Set<Author>().Any(x => x.Username == import.Username);
                    if (hadUsedName)
                    {
                        return new OperateResult(string.Format("用户名 {0} 已经被使用", import.Username));
                    }

                    UserGroup group = db.Set<UserGroup>().OrderBy(x => x.LimitOfConsumption).FirstOrDefault();
                    string password = EncryptHelper.EncryptByMd5(import.Password);
                    Author newUser = new Author(import.Username, password, import.IsAgents, import.NormalReturnPoints
                        , import.UncertainReturnPoints, group, import.MaxOfSubordinate);
                    db.Set<Author>().Add(newUser);
                    db.SaveChanges();

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果和用户的随机生成的新密码</returns>
        public ResetPasswordResult ResetPassword(int userId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new ResetPasswordResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new ResetPasswordResult("该用户所属的用户组无权重置用户密码");
                    }

                    string newPassword = new AuthorManager(db).ResetPassage(userId);
                    return new ResetPasswordResult(newPassword, administrator.Username);
                }
            }
            catch (Exception e)
            {
                return new ResetPasswordResult(e.Message);
            }
        }

        /// <summary>
        /// 重置用户的安全码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果和用户的随机生成的新安全码</returns>
        public ResetSafeWordResult ResetSafeCode(int userId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new ResetSafeWordResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new ResetSafeWordResult("该用户所属的用户组无权重置用户的安全码");
                    }

                    string newSafeCode = new AuthorManager(db).ResetSafeCode(userId);
                    return new ResetSafeWordResult(newSafeCode, administrator.Username);
                }
            }
            catch (Exception e)
            {
                return new ResetSafeWordResult(e.Message);
            }
        }

        /// <summary>
        /// 重置用户的银行卡绑定
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ResetBankCard(int userId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权重置用户的银行卡绑定");
                    }

                    new AuthorManager(db).ResetCard(userId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 重置用户的安全邮箱
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ResetEmail(int userId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权重置用户的安全邮箱");
                    }

                    new AuthorManager(db).ResetEmail(userId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveUser(int userId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权删除用户");
                    }

                    new AuthorManager(db).Remove(userId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取用户组信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public PaginationList<UserGroupResult> GetGroupList(string keyword, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<UserGroupResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewUsers)
                    {
                        return new PaginationList<UserGroupResult>("该用户所属的用户组无权查看用户信息");
                    }

                    AdminUsersReader reader = new AdminUsersReader(db);
                    return reader.ReadGroupList(keyword, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<UserGroupResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddGroup_Basic(AddUserGroupImport_Basic import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权添加用户组");
                    }

                    ICreatePackage<UserGroup> pfc = UserGroupManager.Factory
                        .CreatePackageForCreate(import.Name, import.Grade, import.LimitOfConsumption, import.UpperOfConsumption);
                    new UserGroupManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddGroup(AddUserGroupImport import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权添加用户组");
                    }

                    ICreatePackage<UserGroup> pfc = UserGroupManager.Factory
                        .CreatePackageForCreate(import.Name, import.ColorOfName, import.Grade, import.LimitOfConsumption
                        , import.UpperOfConsumption, import.Withdrawals, import.MinimumWithdrawalAmount, import.MaximumWithdrawalAmount
                        , import.MinimumRechargeAmount, import.MaximumRechargeAmount, import.WithdrawalsAtAnyTime, import.MaxOfSubordinate);
                    new UserGroupManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 修改用户组信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditGroup_Basic(EditUserGroupImport_Basic import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权修改用户组信息");
                    }

                    IUpdatePackage<UserGroup> pfu = UserGroupManager.Factory
                        .CreatePackageForUpdate(import.GroupId, import.Grade, import.Name, import.LimitOfConsumption
                        , import.UpperOfConsumption);
                    new UserGroupManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 修改用户组信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditGroup(EditUserGroupImport import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权修改用户组信息");
                    }

                    IUpdatePackage<UserGroup> pfu = UserGroupManager.Factory
                        .CreatePackageForUpdate(import.GroupId, import.Name, import.ColorOfName, import.Grade, import.LimitOfConsumption
                        , import.UpperOfConsumption, import.Withdrawals, import.MinimumWithdrawalAmount, import.MaximumWithdrawalAmount
                        , import.MinimumRechargeAmount, import.MaximumRechargeAmount, import.WithdrawalsAtAnyTime, import.MaxOfSubordinate);
                    new UserGroupManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveGroup(int groupId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditUsers)
                    {
                        return new OperateResult("该用户所属的用户组无权删除用户组");
                    }

                    bool hasUser = db.Set<Author>().Any(x => x.Group.Id == groupId);
                    if (hasUser)
                    {
                        return new OperateResult("该用户组中仍有用户，不能删除");
                    }

                    new UserGroupManager(db).Remove(groupId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }
    }
}
