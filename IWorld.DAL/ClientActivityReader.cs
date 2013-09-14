using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 活动信息的阅读者对象（前台）
    /// </summary>
    public class ClientActivityReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的活动信息的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientActivityReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取默认活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回默认活动的分页列表</returns>
        public PaginationList<NormalActivitiesResult> ReadNormalActivities(int page)
        {
            DateTime now = DateTime.Now;
            Expression<Func<Activity, bool>> predicate = x => x.BeginTime < now 
                && x.EndTime > now 
                && x.Hide == false;

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var aSet = db.Set<Activity>();

            int tCount = aSet
                .Where(predicate)
                .Count();
            List<NormalActivitiesResult> tList = aSet
                .Where(predicate)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new NormalActivitiesResult(x));

            return new PaginationList<NormalActivitiesResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        /// <summary>
        /// 读取兑换活动列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回兑换活动的分页列表</returns>
        public PaginationList<ExchangeActivitiesResult> ReadExchangeActivities(int page)
        {
            DateTime now = DateTime.Now;
            Expression<Func<Exchange, bool>> predicate = x => x.BeginTime < now
                && x.EndTime > now
                && x.Hide == false;

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var eSet = db.Set<Exchange>();

            int tCount = eSet
                .Where(predicate)
                .Count();
            List<ExchangeActivitiesResult> tList = eSet
                .Where(predicate)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new ExchangeActivitiesResult(x));

            return new PaginationList<ExchangeActivitiesResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        #endregion
    }
}
