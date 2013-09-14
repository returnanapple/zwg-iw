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
    /// 数据报表的阅读者对象后台）
    /// </summary>
    public class AdminDataReportReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的数据报表的阅读者对象后台（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminDataReportReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取站点综合信息
        /// </summary>
        /// <returns>返回站点综合信息</returns>
        public ComprehensiveInformationResult ReadComprehensiveInformation()
        {
            DateTime now = DateTime.Now;
            var sdadSet = db.Set<SiteDataAtDay>();
            var sSet = db.Set<SiteDataAtMonth>();
            var ulrSet = db.Set<UserLandingRecord>();

            Expression<Func<SiteDataAtDay, bool>> predicate1 = x => x.Year == now.Year && x.Month == now.Month && x.Day == now.Day;
            Expression<Func<SiteDataAtMonth, bool>> predicate2 = x => x.Year == now.Year && x.Month == now.Month;

            SiteDataAtDay siteDataAtDay = sdadSet.Any(predicate1) ? sdadSet.FirstOrDefault(predicate1) : new SiteDataAtDay(now);
            SiteDataAtMonth siteDataAtMonth = sSet.Any(predicate2) ? sSet.FirstOrDefault(predicate2) : new SiteDataAtMonth(now);

            return new ComprehensiveInformationResult(siteDataAtDay, siteDataAtMonth); ;
        }

        /// <summary>
        /// 读取站点信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="page">页码</param>
        /// <returns>返回站点信息统计的分页列表</returns>
        public PaginationList<SiteDataResult> ReadSiteDataList(string beginTime, string endTime, TimePeriodSelectType timePeriod
            , int page)
        {
            PaginationList<SiteDataResult> result = new PaginationList<SiteDataResult>("操作失败");
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            switch (timePeriod)
            {
                case TimePeriodSelectType.日:
                    #region 日统计

                    Expression<Func<SiteDataAtDay, bool>> predicate1 = x => x.Id > 0;
                    Expression<Func<SiteDataAtDay, bool>> predicate2 = x => x.Id > 0;
                    if (beginTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        int day = Convert.ToInt32(tTimeStr[2]);
                        predicate1 = x => x.Year > year
                            || (x.Year == year && x.Month > month)
                            || (x.Year == year && x.Month == month && x.Day >= day);
                    }
                    if (endTime != "")
                    {
                        string[] tTimeStr = endTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        int day = Convert.ToInt32(tTimeStr[2]);
                        predicate2 = x => x.Year < year
                            || (x.Year == year && x.Month < month)
                            || (x.Year == year && x.Month == month && x.Day <= day);
                    }
                    var sdadSet = db.Set<SiteDataAtDay>();

                    int tCount = sdadSet
                        .Where(predicate1)
                        .Where(predicate2)
                        .Count();
                    List<SiteDataResult> tList = sdadSet
                        .Where(predicate1)
                        .Where(predicate2)
                        .OrderByDescending(x => x.Year)
                        .OrderByDescending(x => x.Month)
                        .OrderByDescending(x => x.Day)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForAdmin)
                        .ToList()
                        .ConvertAll(x => new SiteDataResult(x));
                    result = new PaginationList<SiteDataResult>(page, webSetting.PageSizeForAdmin, tCount, tList);

                    #endregion
                    break;
                case TimePeriodSelectType.月:
                    #region 月统计

                    Expression<Func<SiteDataAtMonth, bool>> _predicate1 = x => x.Id > 0;
                    Expression<Func<SiteDataAtMonth, bool>> _predicate2 = x => x.Id > 0;
                    if (beginTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        _predicate1 = x => x.Year > year
                            || (x.Year == year && x.Month >= month);
                    }
                    if (endTime != "")
                    {
                        string[] tTimeStr = endTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        _predicate2 = x => x.Year < year
                            || (x.Year == year && x.Month <= month);
                    }
                    var sdamSet = db.Set<SiteDataAtMonth>();

                    int _tCount = sdamSet
                        .Where(_predicate1)
                        .Where(_predicate2)
                        .Count();
                    List<SiteDataResult> _tList = sdamSet
                        .Where(_predicate1)
                        .Where(_predicate2)
                        .OrderByDescending(x => x.Year)
                        .OrderByDescending(x => x.Month)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForAdmin)
                        .ToList()
                        .ConvertAll(x => new SiteDataResult(x));
                    result = new PaginationList<SiteDataResult>(page, webSetting.PageSizeForAdmin, _tCount, _tList);

                    #endregion
                    break;
            }
            return result;
        }

        /// <summary>
        /// 读取个人信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回个人信息统计的分页列表</returns>
        public PaginationList<PersonalDataResult> ReadPersonalDataList(string beginTime, string endTime
            , TimePeriodSelectType timePeriod, int userId, int page)
        {
            PaginationList<PersonalDataResult> result = new PaginationList<PersonalDataResult>("操作失败");
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            switch (timePeriod)
            {
                case TimePeriodSelectType.日:
                    #region 日统计

                    Expression<Func<PersonalDataAtDay, bool>> predicate1 = x => x.Id > 0;
                    Expression<Func<PersonalDataAtDay, bool>> predicate2 = x => x.Id > 0;
                    Expression<Func<PersonalDataAtDay, bool>> predicate3 = x => x.Id > 0;
                    if (beginTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        int day = Convert.ToInt32(tTimeStr[2]);
                        predicate1 = x => x.Year > year
                            || (x.Year == year && x.Month > month)
                            || (x.Year == year && x.Month == month && x.Day >= day);
                    }
                    if (endTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        int day = Convert.ToInt32(tTimeStr[2]);
                        predicate2 = x => x.Year < year
                            || (x.Year == year && x.Month < month)
                            || (x.Year == year && x.Month == month && x.Day <= day);
                    }
                    if (userId != 0)
                    {
                        predicate3 = x => x.Owner.Id == userId;
                    }
                    var pdadSet = db.Set<PersonalDataAtDay>();

                    int tCount = pdadSet
                        .Where(predicate1)
                        .Where(predicate2)
                        .Where(predicate3)
                        .Count();
                    List<PersonalDataResult> tList = pdadSet
                        .Where(predicate1)
                        .Where(predicate3)
                        .OrderByDescending(x => x.Year)
                        .OrderByDescending(x => x.Month)
                        .OrderByDescending(x => x.Day)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForAdmin)
                        .ToList()
                        .ConvertAll(x => new PersonalDataResult(x));
                    result = new PaginationList<PersonalDataResult>(page, webSetting.PageSizeForAdmin, tCount, tList);

                    #endregion
                    break;
                case TimePeriodSelectType.月:
                    #region 月统计

                    Expression<Func<PersonalDataAtMonth, bool>> _predicate1 = x => x.Id > 0;
                    Expression<Func<PersonalDataAtMonth, bool>> _predicate2 = x => x.Id > 0;
                    Expression<Func<PersonalDataAtMonth, bool>> _predicate3 = x => x.Id > 0;
                    if (beginTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        _predicate1 = x => x.Year > year
                            || (x.Year == year && x.Month >= month);
                    }
                    if (endTime != "")
                    {
                        string[] tTimeStr = beginTime.Split(new char[] { '-' });
                        int year = Convert.ToInt32(tTimeStr[0]);
                        int month = Convert.ToInt32(tTimeStr[1]);
                        _predicate2 = x => x.Year < year
                            || (x.Year == year && x.Month <= month);
                    }
                    if (userId != 0)
                    {
                        _predicate3 = x => x.Owner.Id == userId;
                    }
                    var pdamSet = db.Set<PersonalDataAtMonth>();

                    int _tCount = pdamSet
                        .Where(_predicate1)
                        .Where(_predicate2)
                        .Where(_predicate3)
                        .Count();
                    List<PersonalDataResult> _tList = pdamSet
                        .Where(_predicate1)
                        .Where(_predicate2)
                        .Where(_predicate3)
                        .OrderByDescending(x => x.CreatedTime)
                        .Skip(startRow)
                        .Take(webSetting.PageSizeForAdmin)
                        .ToList()
                        .ConvertAll(x => new PersonalDataResult(x));
                    result = new PaginationList<PersonalDataResult>(page, webSetting.PageSizeForAdmin, _tCount, _tList);

                    #endregion
                    break;
            }
            return result;
        }

        /// <summary>
        /// 读取投注信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回投注信息的分页列表</returns>
        public PaginationList<BettingResult> ReadBettingList(string beginTime, string endTime, int ownerId, int ticketId, int tagId
            , int howToPlatId, BettingStatusSelectType status, int page)
        {
            Expression<Func<Betting, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate4 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate5 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate6 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate7 = x => x.Id > 0;
            if (ownerId != 0)
            {
                predicate1 = x => x.Owner.Id == ownerId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            if (ticketId != 0)
            {
                predicate4 = x => x.HowToPlay.Tag.Ticket.Id == ownerId;
            }
            if (tagId != 0)
            {
                predicate5 = x => x.HowToPlay.Tag.Id == ownerId;
            }
            if (howToPlatId != 0)
            {
                predicate6 = x => x.HowToPlay.Id == ownerId;
            }
            switch (status)
            {
                case BettingStatusSelectType.等待开奖:
                    predicate7 = x => x.Status == BettingStatus.等待开奖;
                    break;
                case BettingStatusSelectType.即将开奖:
                    predicate7 = x => x.Status == BettingStatus.即将开奖;
                    break;
                case BettingStatusSelectType.中奖:
                    predicate7 = x => x.Status == BettingStatus.中奖;
                    break;
                case BettingStatusSelectType.未中奖:
                    predicate7 = x => x.Status == BettingStatus.未中奖;
                    break;
                case BettingStatusSelectType.用户撤单:
                    predicate7 = x => x.Status == BettingStatus.用户撤单;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);

            int tCount = db.Set<Betting>()
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Where(predicate5)
                .Where(predicate6)
                .Where(predicate7)
                .Count();
            List<BettingResult> tList = db.Set<Betting>()
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Where(predicate5)
                .Where(predicate6)
                .Where(predicate7)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new BettingResult(x));

            return new PaginationList<BettingResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取追号信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回追号信息的分页列表</returns>
        public PaginationList<ChasingResult> ReadChasingList(string beginTime, string endTime, int ownerId, int ticketId, int tagId
            , int howToPlatId, ChasingStatusSelectType status, int page)
        {
            Expression<Func<Chasing, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate4 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate5 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate6 = x => x.Id > 0;
            Expression<Func<Chasing, bool>> predicate7 = x => x.Id > 0;
            if (ownerId != 0)
            {
                predicate1 = x => x.Owner.Id == ownerId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            if (ticketId != 0)
            {
                predicate4 = x => x.HowToPlay.Tag.Ticket.Id == ownerId;
            }
            if (tagId != 0)
            {
                predicate5 = x => x.HowToPlay.Tag.Id == ownerId;
            }
            if (howToPlatId != 0)
            {
                predicate6 = x => x.HowToPlay.Id == ownerId;
            }
            switch (status)
            {
                case ChasingStatusSelectType.未开始:
                    predicate7 = x => x.Status == ChasingStatus.未开始;
                    break;
                case ChasingStatusSelectType.追号中:
                    predicate7 = x => x.Status == ChasingStatus.追号中;
                    break;
                case ChasingStatusSelectType.因为所追号码已经开出而终止:
                    predicate7 = x => x.Status == ChasingStatus.因为所追号码已经开出而终止;
                    break;
                case ChasingStatusSelectType.因为中奖而终止:
                    predicate7 = x => x.Status == ChasingStatus.因为中奖而终止;
                    break;
                case ChasingStatusSelectType.用户中止追号:
                    predicate7 = x => x.Status == ChasingStatus.用户中止追号;
                    break;
                case ChasingStatusSelectType.追号结束:
                    predicate7 = x => x.Status == ChasingStatus.追号结束;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var cSet = db.Set<Chasing>();

            int tCount = cSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Where(predicate5)
                .Where(predicate6)
                .Where(predicate7)
                .Count();
            List<ChasingResult> tList = cSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Where(predicate5)
                .Where(predicate6)
                .Where(predicate7)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ChasingResult(x));

            return new PaginationList<ChasingResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取投注（追号）信息的分页列表
        /// </summary>
        /// <param name="chasingId"></param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回投注（追号）信息的分页列表</returns>
        public PaginationList<BettingForChasingResult> ReadBettingForChasingList(int chasingId, BettingStatusSelectType status
            , int page)
        {
            Expression<Func<BettingForCgasing, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<BettingForCgasing, bool>> predicate2 = x => x.Id > 0;
            if (chasingId != 0)
            {
                predicate1 = x => x.Chasing.Id == chasingId;
            }
            switch (status)
            {
                case BettingStatusSelectType.等待开奖:
                    predicate2 = x => x.Status == BettingStatus.等待开奖;
                    break;
                case BettingStatusSelectType.即将开奖:
                    predicate2 = x => x.Status == BettingStatus.即将开奖;
                    break;
                case BettingStatusSelectType.中奖:
                    predicate2 = x => x.Status == BettingStatus.中奖;
                    break;
                case BettingStatusSelectType.未中奖:
                    predicate2 = x => x.Status == BettingStatus.未中奖;
                    break;
                case BettingStatusSelectType.用户撤单:
                    predicate2 = x => x.Status == BettingStatus.用户撤单;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var afcSet = db.Set<BettingForCgasing>();

            int tCount = afcSet
                .Where(predicate1)
                .Where(predicate2)
                .Count();
            List<BettingForChasingResult> tList = afcSet
                .Where(predicate1)
                .Where(predicate2)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new BettingForChasingResult(x));

            return new PaginationList<BettingForChasingResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取充值申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回充值申请的分页列表</returns>
        public PaginationList<RechargeResult> ReadRechargeList(string beginTime, string endTime, RechargeStatusSelectType status
            , int userId, int page)
        {
            Expression<Func<RechargeRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<RechargeRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<RechargeRecord, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<RechargeRecord, bool>> predicate4 = x => x.Id > 0;
            if (userId != 0)
            {
                predicate1 = x => x.Owner.Id == userId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            switch (status)
            {
                case RechargeStatusSelectType.等待支付:
                    predicate4 = x => x.Status == RechargeStatus.等待支付;
                    break;
                case RechargeStatusSelectType.充值成功:
                    predicate4 = x => x.Status == RechargeStatus.充值成功;
                    break;
                case RechargeStatusSelectType.失败:
                    predicate4 = x => x.Status == RechargeStatus.失败;
                    break;
                case RechargeStatusSelectType.用户已经支付:
                    predicate4 = x => x.Status == RechargeStatus.用户已经支付;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var rrSet = db.Set<RechargeRecord>();

            int tCount = rrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<RechargeResult> tList = rrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new RechargeResult(x));

            return new PaginationList<RechargeResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取未处理的充值申请的数量
        /// </summary>
        /// <returns>返回未处理的充值申请的数量</returns>
        public UntreatedRecharCountResult ReadUntreatedRecharCount()
        {
            int count = db.Set<RechargeRecord>().Count(x => x.Status == RechargeStatus.用户已经支付);
            return new UntreatedRecharCountResult(count);
        }

        /// <summary>
        /// 读取提现申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回提现申请的分页列表</returns>
        public PaginationList<WithdrawalResult> ReadWithdrawalList(string beginTime, string endTime
            , WithdrawalsStatusSelectType status, int userId, int page)
        {
            Expression<Func<WithdrawalsRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<WithdrawalsRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<WithdrawalsRecord, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<WithdrawalsRecord, bool>> predicate4 = x => x.Id > 0;
            if (userId != 0)
            {
                predicate1 = x => x.Owner.Id == userId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            switch (status)
            {
                case WithdrawalsStatusSelectType.处理中:
                    predicate4 = x => x.Status == WithdrawalsStatus.处理中;
                    break;
                case WithdrawalsStatusSelectType.失败:
                    predicate4 = x => x.Status == WithdrawalsStatus.失败;
                    break;
                case WithdrawalsStatusSelectType.提现成功:
                    predicate4 = x => x.Status == WithdrawalsStatus.提现成功;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var wrSet = db.Set<WithdrawalsRecord>();

            int tCount = wrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<WithdrawalResult> tList = wrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new WithdrawalResult(x));

            return new PaginationList<WithdrawalResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取未处理的提现申请的数量
        /// </summary>
        /// <returns>返回操作结果</returns>
        public UntreatedWithdrawalCountResult ReadUntreatedWithdrawalCount()
        {
            int count = db.Set<WithdrawalsRecord>().Count(x => x.Status == WithdrawalsStatus.处理中);
            return new UntreatedWithdrawalCountResult(count);
        }

        /// <summary>
        /// 读取支取记录的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回支取记录的分页列表</returns>
        public PaginationList<TransferResult> ReadTransferList(string beginTime, string endTime, int userId, int page)
        {
            Expression<Func<TransferRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<TransferRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<TransferRecord, bool>> predicate3 = x => x.Id > 0;
            if (userId != 0)
            {
                predicate1 = x => x.Owner.Id == userId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var trSet = db.Set<TransferRecord>();

            int tCount = trSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<TransferResult> tList = trSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new TransferResult(x));

            return new PaginationList<TransferResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
