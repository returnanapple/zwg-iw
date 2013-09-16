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
    /// 博彩相关的阅读者对象（前台）
    /// </summary>
    public class ClientGamingReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的博彩相关的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientGamingReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取彩票和彩票的最新动态
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        public NormalList<LotteryTicketResult> ReadLotteryTickets()
        {
            var lSet = db.Set<Lottery>();
            List<LotteryTicketResult> tList = db.Set<LotteryTicket>()
                .Where(x => x.Hide == false)
                .OrderBy(x => x.Order)
                .ToList()
                .ConvertAll(ticket => new LotteryTicketResult(ticket
                    , lSet.Where(x => x.Ticket.Id == ticket.Id)
                        .OrderByDescending(x => x.CreatedTime)
                        .FirstOrDefault()));

            return new NormalList<LotteryTicketResult>(tList);
        }

        /// <summary>
        /// 读取玩法信息
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        public NormalList<LotteryTicketResult> ReadHowToPlays()
        {
            var lSet = db.Set<Lottery>();
            List<LotteryTicketResult> tList = db.Set<LotteryTicket>()
                .Where(x => x.Hide == false)
                .OrderBy(x => x.Order)
                .ToList()
                .ConvertAll(ticket => new LotteryTicketResult(ticket
                    , lSet.Where(x => x.Ticket.Id == ticket.Id)
                        .OrderByDescending(x => x.CreatedTime)
                        .FirstOrDefault()
                        , true));

            return new NormalList<LotteryTicketResult>(tList);
        }

        /// <summary>
        /// 读取投注明细
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="selectType">筛选类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <returns>返回投注明细的分页列表</returns>
        public PaginationList<BettingDetailsResult> ReadBettingDetails(int userId, BettingDetailsSelectType selectType
            , string beginTime, string endTime, int page)
        {
            var aSet = db.Set<Author>();
            Author user = aSet.Find(userId);

            Expression<Func<Betting, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Betting, bool>> predicate3 = x => x.Id > 0;
            switch (selectType)
            {
                case BettingDetailsSelectType.个人:
                    predicate1 = x => x.Owner.Id == userId;
                    break;
                case BettingDetailsSelectType.团队:
                    predicate1 = x => x.Owner.LeftKey >= user.LeftKey && x.Owner.RightKey <= user.RightKey
                        && x.Owner.Tree == user.Tree;
                    break;
                case BettingDetailsSelectType.直属下级:
                    predicate1 = x => x.Owner.LeftKey >= user.LeftKey && x.Owner.RightKey <= user.RightKey
                        && x.Owner.Tree == user.Tree && x.Owner.Layer == user.Layer + 1;
                    break;
            }
            if (beginTime != "")
            {
                string[] tTime = beginTime.Split(new char[] { '-' });
                DateTime _time = new DateTime(Convert.ToInt32(tTime[0]), Convert.ToInt32(tTime[1]), Convert.ToInt32(tTime[2]))
                    .AddDays(1);
                predicate2 = x => x.CreatedTime >= _time;
            }
            if (endTime != "")
            {
                string[] tTime = endTime.Split(new char[] { '-' });
                DateTime _time = new DateTime(Convert.ToInt32(tTime[0]), Convert.ToInt32(tTime[1]), Convert.ToInt32(tTime[2]))
                    .AddDays(1);
                predicate2 = x => x.CreatedTime <= _time;
            }

            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForClient);
            var bSet = db.Set<Betting>();

            int tCount = bSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<BettingDetailsResult> tList = bSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForClient)
                .ToList()
                .ConvertAll(x => new BettingDetailsResult(x
                    , db.Set<Lottery>().FirstOrDefault(t => t.Phases == x.Phases && t.Ticket.Id == x.HowToPlay.Tag.Ticket.Id)));

            return new PaginationList<BettingDetailsResult>(page, webSetting.PageSizeForClient, tCount, tList);
        }

        /// <summary>
        /// 读取中奖排行
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回中奖排行信息的普通列表</returns>
        public NormalList<RankingResult> ReadRanking(int ticketId)
        {
            int pageSize = 8;

            List<RankingResult> tList = db.Set<Betting>()
                .Where(x => x.HowToPlay.Tag.Ticket.Id == ticketId
                    && x.Status == BettingStatus.中奖)
                .OrderByDescending(x => x.Bonus)
                .Take(pageSize)
                .ToList()
                .ConvertAll(x => new RankingResult(x));
            tList.AddRange(db.Set<VirtualTop>()
                .Where(x => x.Ticket.Id == ticketId)
                .OrderBy(x => x.Sum)
                .Take(pageSize)
                .ToList()
                .ConvertAll(x => new RankingResult(x)));

            return new NormalList<RankingResult>(tList.OrderByDescending(x => x.Bonus).Take(pageSize).ToList());
        }

        /// <summary>
        /// 读取开奖历史
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回开奖历史的普通列表</returns>
        public NormalList<HistoryOfLotteryResult> ReadHistoryOfLottery(int ticketId)
        {
            int pageSize = 5;

            List<HistoryOfLotteryResult> tList = db.Set<Lottery>()
                .Where(x => x.Ticket.Id == ticketId)
                .OrderByDescending(x => x.CreatedTime)
                .Take(pageSize)
                .ToList()
                .ConvertAll(x => new HistoryOfLotteryResult(x));

            return new NormalList<HistoryOfLotteryResult>(tList);
        }

        #endregion
    }
}
