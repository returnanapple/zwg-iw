using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 资金管理的阅读者对象（前台）
    /// </summary>
    public class ClientFundsReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的资金管理的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientFundsReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取充值明细
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回充值明细的分页列表</returns>
        public PaginationList<RechargeDetailsResult> ReadRechargeDetails(int userId, int page)
        {
            Expression<Func<RechargeRecord, bool>> predicate = x => x.Owner.Id == userId;

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var rSet = db.Set<RechargeRecord>();

            int tCount = rSet
                .Where(predicate)
                .Count();
            List<RechargeDetailsResult> tList = rSet
                .Where(predicate)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new RechargeDetailsResult(x));

            return new PaginationList<RechargeDetailsResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        /// <summary>
        /// 读取提现明细
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回提现明细的分页列表</returns>
        public PaginationList<WithdrawDetailsResult> ReadWithdrawDetails(int userId, int page)
        {
            Expression<Func<WithdrawalsRecord, bool>> predicate = x => x.Owner.Id == userId;

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var wSet = db.Set<WithdrawalsRecord>();

            int tCount = wSet
                .Where(predicate)
                .Count();
            List<WithdrawDetailsResult> tList = wSet
                .Where(predicate)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new WithdrawDetailsResult(x));

            return new PaginationList<WithdrawDetailsResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
