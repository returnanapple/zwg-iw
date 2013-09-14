using System;
using System.Linq;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 管理员管理的数据服务（后台）
    /// </summary>
    public class ManagerService : IManagerService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回登陆结果</returns>
        public LoginResult Login(string username, string password)
        {
            try
            {
                string ip = WebHepler.GetEndpoint().Address;
                using (WebMapContext db = new WebMapContext())
                {
                    AdministratorManager aministratorManager = new AdministratorManager(db);
                    Administrator administrator = aministratorManager.Login(username, password, ip);
                    string token = WebHepler.SetAdministratorIn(administrator.Id);
                    ManagerGroupResult groupResult = new ManagerGroupResult(administrator.Group);

                    return new LoginResult(token, administrator.Username, administrator.LastLoginTime, groupResult);
                }
            }
            catch (Exception e)
            {
                return new LoginResult(e.Message);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Logout(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                WebHepler.SetAdministratorOut(token);
                return new OperateResult();
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 心跳协议
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Heartbeat(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                return new OperateResult();
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取管理员信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="groupId">用户组的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理员信息的分页列表</returns>
        public PaginationList<ManagerInfoResult> GetList(string keyword, int groupId, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ManagerInfoResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new PaginationList<ManagerInfoResult>("该用户所属的用户组无权查看管理员信息");
                    }

                    AdminManagersReader reader = new AdminManagersReader(db);
                    return reader.ReadManagerList(keyword, groupId, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ManagerInfoResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="group">用户组</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Add(string username, string password, string group, string token)
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
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new OperateResult("该用户所属的用户组无权添加管理员");
                    }
                    int groupId = db.Set<AdministratorGroup>().First(x => x.Name == group).Id;

                    ICreatePackage<Administrator> pfc = AdministratorManager.Factory
                        .CreatePackAgeForCreate(username, password, groupId);
                    new AdministratorManager(db).Create(pfc);

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 移除管理员
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Remove(int userId, string token)
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
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new OperateResult("该用户所属的用户组无权移除管理员");
                    }

                    new AdministratorManager(db).Remove(userId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ResetPassage(int userId, string oldPassword, string newPassword, string token)
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
                    if (userId > 0)
                    {
                        Administrator administrator = db.Set<Administrator>().Find(administratorId);
                        Administrator tUser = db.Set<Administrator>().Find(userId);
                        if (!administrator.Group.CanViewAndEditManagers
                            || administrator.Group.Grade <= tUser.Group.Grade)
                        {
                            return new OperateResult("该用户所属的用户组无权编辑管理员");
                        }
                    }
                    else
                    {
                        userId = administratorId;
                    }

                    new AdministratorManager(db).ResetPassage(userId, oldPassword, newPassword);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 修改管理用户组
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ChangeGroup(int userId, int groupId, string token)
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
                    Administrator tUser = db.Set<Administrator>().Find(userId);
                    if (!administrator.Group.CanViewAndEditManagers
                        || administrator.Group.Grade <= tUser.Group.Grade)
                    {
                        return new OperateResult("该用户所属的用户组无权移除管理员");
                    }

                    new AdministratorManager(db).ChangeGroup(userId, groupId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取管理用户组的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理用户组的分页列表</returns>
        public PaginationList<ManagerGroupResult> GetGroupList(string keyword, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ManagerGroupResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new PaginationList<ManagerGroupResult>("该用户所属的用户组无权查看管理员用户组信息");
                    }

                    AdminManagersReader reader = new AdminManagersReader(db);
                    return reader.ReadGroupList(keyword, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ManagerGroupResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加管理用户组
        /// </summary>
        /// <param name="import">参数集</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
        public OperateResult AddGroup(AddManagerGroupImport import, string token)
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
                    if (!administrator.Group.CanViewAndEditManagers
                        || import.Grade == 255)
                    {
                        return new OperateResult("该用户所属的用户组无权添加管理用户组");
                    }

                    ICreatePackage<AdministratorGroup> pfc = AdministratorGroupManager.Factory
                        .CreatePackageForCreate(import.Name, import.Grade, import.CanViewUsers, import.CanEditUsers, import.CanViewTickets
                        , import.CanEditTickets, import.CanViewActivities, import.CanEditActivities, import.CanSettingSite, import.CanViewDataReports
                        , import.CanViewAndAddFundsReports, import.CanViewAndEditMessageBox, import.CanViewAndEditManagers);
                    new AdministratorGroupManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑管理用户组
        /// </summary>
        /// <param name="import">参数集</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
        public OperateResult EditGroup(EditManagerGroupImport import, string token)
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
                    AdministratorGroup tGroup = db.Set<AdministratorGroup>().Find(import.GroupId);
                    if (!administrator.Group.CanViewAndEditManagers
                        || tGroup.Grade == 255)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑管理用户组");
                    }

                    IUpdatePackage<AdministratorGroup> pfu = AdministratorGroupManager.Factory
                        .CreatePackageForUpdate(import.GroupId, import.Name, import.Grade, import.CanViewUsers, import.CanEditUsers
                        , import.CanViewTickets, import.CanEditTickets, import.CanViewActivities, import.CanEditActivities
                        , import.CanSettingSite, import.CanViewDataReports, import.CanViewAndAddFundsReports, import.CanViewAndEditMessageBox);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 移除管理用户组
        /// </summary>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
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
                    AdministratorGroup tGroup = db.Set<AdministratorGroup>().Find(groupId);
                    if (!administrator.Group.CanViewAndEditManagers
                        || tGroup.Grade == 255)
                    {
                        return new OperateResult("该用户所属的用户组无权移除管理用户组");
                    }

                    new AdministratorGroupManager(db).Remove(groupId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取管理员登陆记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理员登陆记录的分页列表</returns>
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
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new PaginationList<LandingRecordResult>("该用户所属的用户组无权查看管理员登陆记录");
                    }

                    AdminManagersReader reader = new AdminManagersReader(db);
                    return reader.ReadLandingRecordList(userId, beginTime, endTime, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<LandingRecordResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取管理记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理记录的分页列表</returns>
        public PaginationList<OperatedRecordResult> GetOperatedRecordList(int userId, string beginTime, string endTime, int page
            , string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<OperatedRecordResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewAndEditManagers)
                    {
                        return new PaginationList<OperatedRecordResult>("该用户所属的用户组无权查看管理操作记录");
                    }

                    AdminManagersReader reader = new AdminManagersReader(db);
                    return reader.ReadOperatedRecordList(userId, beginTime, endTime, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<OperatedRecordResult>(e.Message);
            }
        }
    }
}
