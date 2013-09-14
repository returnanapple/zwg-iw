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
    /// 彩票信息的阅读者对象（后台）
    /// </summary>
    public class AdminLotteryTicketReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的彩票信息的阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminLotteryTicketReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取彩票信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <returns>返回彩票信息的分页列表</returns>
        public PaginationList<TicketResult> ReadTicketList(string keyword, HideOrNotSelectType isHide, int page)
        {
            Expression<Func<LotteryTicket, bool>> predicate1 = ticket => ticket.Id > 0;
            Expression<Func<LotteryTicket, bool>> predicate2 = ticket => ticket.Id > 0;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = ticket => kws.All(kw => ticket.Name.Contains(kw));
            }
            switch (isHide)
            {
                case HideOrNotSelectType.显示:
                    predicate2 = ticket => ticket.Hide == false;
                    break;
                case HideOrNotSelectType.隐藏:
                    predicate2 = ticket => ticket.Hide == true;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var ltSet = db.Set<LotteryTicket>();
            var lSet = db.Set<Lottery>();
            var ptSet = db.Set<PlayTag>();
            var htpSet = db.Set<HowToPlay>();

            int tCount = ltSet
                .Where(predicate1)
                .Where(predicate2)
                .Count();
            List<TicketResult> tList = ltSet
                .Where(predicate1)
                .Where(predicate2)
                .OrderBy(ticket => ticket.Order)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(ticket => new TicketResult(ticket
                    , lSet.Where(lottery => lottery.Ticket.Id == ticket.Id).OrderByDescending(lottery => lottery.CreatedTime).FirstOrDefault()
                    , ptSet.Count(tag => tag.Ticket.Id == ticket.Id)
                    , htpSet.Count(howToPlay => howToPlay.Tag.Ticket.Id == ticket.Id)));

            return new PaginationList<TicketResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取开奖时间的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖时间的分页列表</returns>
        public PaginationList<LotteryTimeResult> ReadLotteryTimeList(int ticketId, int page)
        {
            Expression<Func<LotteryTime, bool>> predicate1 = time => time.Id > 0;
            if (ticketId != 0)
            {
                predicate1 = time => time.TicketId == ticketId;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var ltSet = db.Set<LotteryTime>();

            int tCount = ltSet
                .Where(predicate1)
                .Count();
            List<LotteryTimeResult> tList = ltSet
                .Where(predicate1)
                .OrderBy(time => time.Phases)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(time => new LotteryTimeResult(time));

            return new PaginationList<LotteryTimeResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取开奖记录的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="sources">来源</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖记录的分页列表</returns>
        public PaginationList<LotteryResult> ReadLotteryList(int ticketId, LotterySourcesSelectType sources, int page)
        {
            Expression<Func<Lottery, bool>> predicate1 = lottery => lottery.Id > 0;
            Expression<Func<Lottery, bool>> predicate2 = lottery => lottery.Id > 0;
            if (ticketId != 0)
            {
                predicate1 = lottery => lottery.Ticket.Id == ticketId;
            }
            switch (sources)
            {
                case LotterySourcesSelectType.系统采集:
                    predicate2 = lottery => lottery.Sources == LotterySources.系统采集;
                    break;
                case LotterySourcesSelectType.手动:
                    predicate2 = lottery => lottery.Sources == LotterySources.手动;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var lSet = db.Set<Lottery>();

            int tCount = lSet
                .Where(predicate1)
                .Where(predicate2)
                .Count();
            List<LotteryResult> tList = lSet
                .Where(predicate1)
                .Where(predicate2)
                .OrderByDescending(lottery => lottery.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(lottery => new LotteryResult(lottery));

            return new PaginationList<LotteryResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取玩法标签信息的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <returns>返回玩法标签信息的分页列表</returns>
        public PaginationList<PlayTagResult> ReadPlayTagList(int ticketId, string keyword, HideOrNotSelectType isHide
            , int page)
        {
            Expression<Func<PlayTag, bool>> predicate1 = tag => tag.Id > 0;
            Expression<Func<PlayTag, bool>> predicate2 = tag => tag.Id > 0;
            Expression<Func<PlayTag, bool>> predicate3 = tag => tag.Id > 0;
            if (ticketId != 0)
            {
                predicate1 = tag => tag.Ticket.Id == ticketId;
            }
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate2 = tag => kws.All(kw => tag.Name.Contains(kw));
            }
            switch (isHide)
            {
                case HideOrNotSelectType.显示:
                    predicate3 = tag => tag.Hide == false;
                    break;
                case HideOrNotSelectType.隐藏:
                    predicate3 = tag => tag.Hide == true;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);

            int tCount = db.Set<PlayTag>()
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<PlayTagResult> tList = db.Set<PlayTag>()
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderBy(tag => new { tag.Ticket.Id, tag.Order })
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(tag => new PlayTagResult(tag
                    , db.Set<HowToPlay>().Count(howToPlay => howToPlay.Tag.Id == tag.Id)));

            return new PaginationList<PlayTagResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取玩法的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <returns>返回玩法的分页列表</returns>
        public PaginationList<HowToPlayResult> ReadHowToPlayList(int ticketId, int playTagId, string keyword
            , HideOrNotSelectType isHide, int page)
        {
            Expression<Func<HowToPlay, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<HowToPlay, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<HowToPlay, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<HowToPlay, bool>> predicate4 = x => x.Id > 0;
            if (ticketId != 0)
            {
                predicate1 = howToPlay => howToPlay.Tag.Ticket.Id == ticketId;
            }
            if (playTagId != 0)
            {
                predicate2 = howToPlay => howToPlay.Tag.Id == playTagId;
            }
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate3 = x => kws.All(kw => x.Name.Contains(kw));
            }
            switch (isHide)
            {
                case HideOrNotSelectType.显示:
                    predicate4 = howToPlay => howToPlay.Hide == false;
                    break;
                case HideOrNotSelectType.隐藏:
                    predicate4 = howToPlay => howToPlay.Hide == true;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var htpSet = db.Set<HowToPlay>();

            int tCount = htpSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<HowToPlayResult> tList = htpSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderBy(x => new { x.Tag.Ticket.Id, x.Order })
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new HowToPlayResult(x));

            return new PaginationList<HowToPlayResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取虚拟排行信息的分页列表
        /// </summary>
        /// <param name="ticketId">对应的彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回虚拟排行信息的分页列表</returns>
        public PaginationList<VirtualTopResult> ReadVirtualTopList(int ticketId, int page)
        {
            Expression<Func<VirtualTop, bool>> predicate1 = x => x.Id > 0;
            if (ticketId != 0)
            {
                predicate1 = x => x.Ticket.Id == ticketId;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var vtSet = db.Set<VirtualTop>();

            int tCount = vtSet
                .Where(predicate1)
                .Count();
            List<VirtualTopResult> tList = vtSet
                .Where(predicate1)
                .OrderByDescending(x => x.Sum)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new VirtualTopResult(x));

            return new PaginationList<VirtualTopResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
