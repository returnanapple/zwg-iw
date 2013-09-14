using System.ServiceModel;
using IWorld.Contract.Admin;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 定义系统设置管理的数据服务（后台）
    /// </summary>
    [ServiceContract]
    public interface ISystemSettingService
    {
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点设置</returns>
        [OperationContract]
        WebSettingResult GetWebSetting(string token);

        /// <summary>
        /// 编辑站点设置
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditWebSetting(EditWebSettingImport import, string token);

        /// <summary>
        /// 获取银行账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回银行账户的分页列表</returns>
        [OperationContract]
        PaginationList<BankAccountResult> GetBankAccountList(int page, string token);

        /// <summary>
        /// 添加银行账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddBankAccount(AddBankAccountImport import, string token);

        /// <summary>
        /// 编辑银行账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditBankAccount(EditBankAccountImport import, string token);

        /// <summary>
        /// 设置默认银行账户
        /// </summary>
        /// <param name="bankAccountId">目标银行账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult SetDefaultBankAccount(int bankAccountId, string token);

        /// <summary>
        /// 删除银行账户
        /// </summary>
        /// <param name="bankAccountId">目标银行账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveBankAccount(int bankAccountId, string token);

        /// <summary>
        /// 获取系统邮件账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回系统邮件账户的分页列表</returns>
        [OperationContract]
        PaginationList<EmailAccountResult> GetEmailAccountList(int page, string token);

        /// <summary>
        /// 添加系统邮件账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddEmailAccount(AddEmailAccountImport import, string token);

        /// <summary>
        /// 编辑系统邮件账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditEmailAccount(EditEmailAccountImport import, string token);

        /// <summary>
        /// 设置默认邮件账户
        /// </summary>
        /// <param name="emailAccountId">目标邮件账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult SetDefaultEmailAccount(int emailAccountId, string token);

        /// <summary>
        /// 删除系统邮件账户
        /// </summary>
        /// <param name="emailAccountId">目标系统邮件账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveEmailAccount(int emailAccountId, string token);

        /// <summary>
        /// 获取邮件服务地址的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回邮件服务地址的分页列表</returns>
        [OperationContract]
        PaginationList<EmailClientResult> GetEmailClientList(int page, string token);

        /// <summary>
        /// 添加邮件服务地址
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult AddEmailClient(AddEmailClientImport import, string token);

        /// <summary>
        /// 编辑邮件服务地址
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult EditEmailClient(EditEmailClientImport import, string token);

        /// <summary>
        /// 设置默认邮件服务地址
        /// </summary>
        /// <param name="emailAccountId">目标邮件服务地址的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult SetDefaultEmailClient(int emailClientId, string token);

        /// <summary>
        /// 删除邮件服务地址
        /// </summary>
        /// <param name="emailClientId">目标邮件服务地址的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult RemoveEmailClient(int emailClientId, string token);

        /// <summary>
        /// 获取采集程序运行状态
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回采集程序运行状态</returns>
        [OperationContract]
        CollectionStatusResult GetCollectionStatusResult(string token);

        /// <summary>
        /// 关闭或打开数据采集
        /// </summary>
        /// <param name="close">标识 | 关闭</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult CloseOrOpenCollection(bool close, string token);

        /// <summary>
        /// 重置缓存车
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        [OperationContract]
        OperateResult ClearCachePond(string token);
    }
}
