using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义默认活动管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IActivityService
    {
        /// <summary>
        /// 获取默认活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">活动类型</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的分页列表</returns>
        [OperationContract]
        PaginationList<ActivityResult> GetActivityList(string keyword, ActivityTypeSelectType type
            , RegularlyStatusSelectType status, int page, string token);

        /// <summary>
        /// 获取默认活动的参与记录的分页列表
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的参与记录的分页列表</returns>
        [OperationContract]
        PaginationList<ActivityParticipateRecordResult> GetParticipateRecordList(int activityId, int ownerId, string beginTime
            , string endTime, int page, string token);

        /// <summary>
        /// 添加默认活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddActivity(AddActivityImport import, string token);

        /// <summary>
        /// 编辑默认活动的基本信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditActivity_Basic(EditActivityImport_Basic import, string token);

        /// <summary>
        /// 编辑默认活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditActivity(EditActivityImport import, string token);

        /// <summary>
        /// 删除默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveActivity(int activityId, string token);

        /// <summary>
        /// 隐藏默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HideActivity(int activityId, string token);

        /// <summary>
        /// 显示默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowActivity(int activityId, string token);
    }
}
