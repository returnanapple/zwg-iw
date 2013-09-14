using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义公告管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface IBulletinService
    {
        /// <summary>
        /// 获取公告的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回公告的分页列表</returns>
        [OperationContract]
        PaginationList<BulletinResult> GetBulletinList(string keyword, RegularlyStatusSelectType status, int page, string token);

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddBulletin(AddBulletinImport import, string token);

        /// <summary>
        /// 编辑公告信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditBulletin(EditBulletinImport import, string token);

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveBulletin(int bulletinId, string token);

        /// <summary>
        /// 隐藏公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult HideBulletin(int bulletinId, string token);

        /// <summary>
        /// 显示公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ShowBulletin(int bulletinId, string token);
    }
}
