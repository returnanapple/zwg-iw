using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IWorld.Contract.Client;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 定义系统设置相关的数据服务
    /// </summary>
    [ServiceContract]
    public interface ISystemSettingService
    {
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <returns>返回站点设置信息</returns>
        [OperationContract]
        WebSettingResult GetWebSetting();

        /// <summary>
        /// 获取官方银行帐户信息
        /// </summary>
        /// <returns>返回官方银行帐户信息</returns>
        [OperationContract]
        BankAccountResult GetBankAccount();
    }
}
