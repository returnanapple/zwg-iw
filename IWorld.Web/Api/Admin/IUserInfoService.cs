using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义用户管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IUserInfoService
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
        [OperationContract]
        PaginationList<UserInfoResult> GetList(string keyword, int groupId, int teamForUser, int page, string token);

        /// <summary>
        /// 获取用户登录记录的分页列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间（xxxx-xx-xx 格式）</param>
        /// <param name="endTime">结束时间（xxxx-xx-xx 格式）</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回用户登录记录的分页列表</returns>
        [OperationContract]
        PaginationList<LandingRecordResult> GetLandingRecordList(int userId, string beginTime, string endTime, int page, string token);

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddUser(AddUserImport import, string token);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果和用户的随机生成的新密码</returns>
        [OperationContract]
        ResetPasswordResult ResetPassword(int userId, string token);

        /// <summary>
        /// 重置用户的安全码
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果和用户的随机生成的新安全码</returns>
        [OperationContract]
        ResetSafeWordResult ResetSafeCode(int userId, string token);

        /// <summary>
        /// 重置用户的银行卡绑定
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ResetBankCard(int userId, string token);

        /// <summary>
        /// 重置用户的安全邮箱
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ResetEmail(int userId, string token);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveUser(int userId, string token);

        /// <summary>
        /// 获取用户组信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        PaginationList<UserGroupResult> GetGroupList(string keyword, int page, string token);

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddGroup_Basic(AddUserGroupImport_Basic import, string token);

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddGroup(AddUserGroupImport import, string token);

        /// <summary>
        /// 修改用户组信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditGroup_Basic(EditUserGroupImport_Basic import, string token);

        /// <summary>
        /// 修改用户组信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditGroup(EditUserGroupImport import, string token);

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveGroup(int groupId, string token);
    }
}
