using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义管理员管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IManagerService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回登陆结果</returns>
        [OperationContract]
        LoginResult Login(string username, string password);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Logout(string token);

        /// <summary>
        /// 心跳协议
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Heartbeat(string token);

        /// <summary>
        /// 获取管理员信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="groupId">用户组的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理员信息的分页列表</returns>
        [OperationContract]
        PaginationList<ManagerInfoResult> GetList(string keyword, int groupId, int page, string token);

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="group">用户组</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Add(string username, string password, string group, string token);

        /// <summary>
        /// 移除管理员
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult Remove(int userId, string token);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ResetPassage(int userId, string oldPassword, string newPassword, string token);

        /// <summary>
        /// 修改管理用户组
        /// </summary>
        /// <param name="userId">目标管理员的存储指针</param>
        /// <param name="GroupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ChangeGroup(int userId, int GroupId, string token);

        /// <summary>
        /// 获取管理用户组的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理用户组的分页列表</returns>
        [OperationContract]
        PaginationList<ManagerGroupResult> GetGroupList(string keyword, int page, string token);

        /// <summary>
        /// 添加管理用户组
        /// </summary>
        /// <param name="import">参数集</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
        [OperationContract]
        OperateResult AddGroup(AddManagerGroupImport import, string token);

        /// <summary>
        /// 编辑管理用户组
        /// </summary>
        /// <param name="import">参数集</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
        [OperationContract]
        OperateResult EditGroup(EditManagerGroupImport import, string token);

        /// <summary>
        /// 移除管理用户组
        /// </summary>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>操作结果</returns>
        [OperationContract]
        OperateResult RemoveGroup(int groupId, string token);

        /// <summary>
        /// 获取管理员登陆记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理员登陆记录的分页列表</returns>
        [OperationContract]
        PaginationList<LandingRecordResult> GetLandingRecordList(int userId, string beginTime, string endTime, int page, string token);

        /// <summary>
        /// 获取管理记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回管理记录的分页列表</returns>
        [OperationContract]
        PaginationList<OperatedRecordResult> GetOperatedRecordList(int userId, string beginTime, string endTime, int page, string token);
    }
}
