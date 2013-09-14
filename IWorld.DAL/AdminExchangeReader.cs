using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Admin;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 兑换活动阅读者对象（后台）
    /// </summary>
    public class AdminExchangeReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的兑换活动阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminExchangeReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取兑换活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回兑换活动的分页列表</returns>
        public PaginationList<ExchangeResult> ReadExchangeList(string keyword, RegularlyStatusSelectType status, int page)
        {
            Expression<Func<Exchange, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Exchange, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Exchange, bool>> predicate3 = x => x.AutoDelete == false
                || x.EndTime >= DateTime.Now;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Name.Contains(kw));
            }
            switch (status)
            {
                case RegularlyStatusSelectType.未过期:
                    predicate2 = x => x.EndTime > DateTime.Now;
                    break;
                case RegularlyStatusSelectType.正常:
                    predicate2 = x => x.EndTime > DateTime.Now && x.Hide == false;
                    break;
                case RegularlyStatusSelectType.暂停:
                    predicate2 = x => x.EndTime > DateTime.Now && x.Hide == true;
                    break;
                case RegularlyStatusSelectType.已过期:
                    predicate2 = x => x.EndTime <= DateTime.Now;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var eSet = db.Set<Exchange>();

            int tCount = eSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<ExchangeResult> tList = eSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .OrderBy(x => x.Name)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ExchangeResult(x));

            return new PaginationList<ExchangeResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取兑换活动的参与记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <returns>返回兑换活动的参与记录的分页列表</returns>
        public PaginationList<ExchangeParticipateRecordResult> ReadParticipateRecordList(int exchangeId, int ownerId, string beginTime
            , string endTime, int page)
        {
            Expression<Func<ExchangeParticipateRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<ExchangeParticipateRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<ExchangeParticipateRecord, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<ExchangeParticipateRecord, bool>> predicate4 = x => x.Id > 0;
            if (exchangeId != 0)
            {
                predicate1 = x => x.Exchange.Id == exchangeId;
            }
            if (ownerId != 0)
            {
                predicate2 = x => x.Owner.Id == ownerId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate4 = x => x.CreatedTime <= tTime;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var eprSet = db.Set<ExchangeParticipateRecord>();

            int tCount = eprSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<ExchangeParticipateRecordResult> tList = eprSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ExchangeParticipateRecordResult(x));

            return new PaginationList<ExchangeParticipateRecordResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取实体奖品赠送记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="status">赠送状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回实体奖品赠送记录的分页列表</returns>
        public PaginationList<GiftResult> ReadGiftList(int exchangeId, int ownerId, GiftStatusSelectType status, int page)
        {
            Expression<Func<GiftRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<GiftRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<GiftRecord, bool>> predicate3 = x => x.Id > 0;
            if (exchangeId != 0)
            {
                predicate1 = x => x.Exchange.Id == exchangeId;
            }
            if (ownerId != 0)
            {
                predicate2 = x => x.Owner.Id == ownerId;
            }
            switch (status)
            {
                case GiftStatusSelectType.未赠送:
                    predicate3 = x => x.Status == GiftStatus.未赠送;
                    break;
                case GiftStatusSelectType.已赠送:
                    predicate3 = x => x.Status == GiftStatus.已赠送;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var grSet = db.Set<GiftRecord>();

            int tCount = grSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<GiftResult> tList = grSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .OrderBy(x => x.Name)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new GiftResult(x));

            return new PaginationList<GiftResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
