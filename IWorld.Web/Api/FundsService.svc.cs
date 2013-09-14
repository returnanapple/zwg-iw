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
    /// 资金相关的数据服务
    /// </summary>
    public class FundsService : IFundsService
    {
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="userId">用户的数据库存储指针</param>
        /// <param name="sum">充值金额</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值结果</returns>
        public RechargeResult Recharge(int userId, double sum, string token)
        {
            try
            {
                int _userId = CacheManager.GetUserId(token);
                if (_userId <= 0)
                {
                    return new RechargeResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ICreatePackage<RechargeRecord> pfc = RechargeRecordManager.Factory
                        .CreatePackageForCreate(userId, _userId, sum);
                    RechargeRecord record = new RechargeRecordManager(db).Create(pfc);
                    BankAccount bankAccount = db.Set<BankAccount>().FirstOrDefault(x => x.IsDefault);

                    return new RechargeResult(bankAccount.Name, bankAccount.Card, bankAccount.Bank, record.Code);
                }
            }
            catch (Exception ex)
            {
                return new RechargeResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取充值明细
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值明细的分页列表</returns>
        public PaginationList<RechargeDetailsResult> GetRechargeDetails(int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<RechargeDetailsResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientFundsReader reader = new ClientFundsReader(db);
                    return reader.ReadRechargeDetails(userId, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<RechargeDetailsResult>(ex.Message);
            }
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="sum">提现金额</param>
        /// <param name="safeWord">安全码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Withdraw(double sum, string safeWord, string token)
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
                    Author user = db.Set<Author>().Find(userId);
                    string _safeWord = EncryptHelper.EncryptByMd5(safeWord);
                    if (_safeWord != user.SafeCode)
                    {
                        return new OperateResult("安全密码不正确");
                    }
                    if (!user.BindingCard)
                    {
                        return new OperateResult("没有绑定银行卡信息");
                    }
                    ICreatePackage<WithdrawalsRecord> pfc = WithdrawalsRecordManager.Factory
                        .CreatePackageForCreate(userId, sum, user.Card, user.Holder, user.Bank.ToString());
                    new WithdrawalsRecordManager(db).Create(pfc);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取提现明细
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回提现明细的分页列表</returns>
        public PaginationList<WithdrawDetailsResult> GetWithdrawDetails(int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<WithdrawDetailsResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientFundsReader reader = new ClientFundsReader(db);
                    return reader.ReadWithdrawDetails(userId, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<WithdrawDetailsResult>(ex.Message);
            }
        }
    }
}
