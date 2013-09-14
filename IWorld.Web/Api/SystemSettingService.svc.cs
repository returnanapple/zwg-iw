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
    /// 系统设置相关的数据服务
    /// </summary>
    public class SystemSettingService : ISystemSettingService
    {
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <returns>返回站点设置信息</returns>
        public WebSettingResult GetWebSetting()
        {
            try
            {
                return new WebSettingResult();
            }
            catch (Exception ex)
            {
                return new WebSettingResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取官方银行帐户信息
        /// </summary>
        /// <returns>返回官方银行帐户信息</returns>
        public BankAccountResult GetBankAccount()
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    BankAccount ba = db.Set<BankAccount>().FirstOrDefault(x => x.IsDefault);
                    if (ba == null)
                    {
                        throw new Exception("官方帐号目前没有公布");
                    }
                    return new BankAccountResult(ba);
                }
            }
            catch (Exception ex)
            {
                return new BankAccountResult(ex.Message);
            }
        }
    }
}
