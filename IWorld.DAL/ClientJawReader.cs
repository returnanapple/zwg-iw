using IWorld.Contract.Client;
using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace IWorld.DAL
{
    /// <summary>
    /// 大白鲨游戏的阅读者对象
    /// </summary>
    public class ClientJawReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的大白鲨游戏的阅读者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientJawReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 方法

        /// <summary>
        /// 读取大白鲨游戏的主显信息
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <returns>返回大白鲨游戏的主显信息</returns>
        public MainOfJawResult ReadMainOfJaw(int userId)
        {
            MainOfJaw moj = db.Set<MainOfJaw>().First();
            LotteryOfJaw loj = db.Set<LotteryOfJaw>().OrderByDescending(x => x.CreatedTime).First();
            int cOfHistory = db.Set<BettingOfJaw>().Count(x => x.Owner.Id == userId &&
                (x.Status == BettingStatus.未中奖 || x.Status == BettingStatus.中奖));
            double profit = cOfHistory == 0
                ? 0
                : db.Set<BettingOfJaw>().Where(x => x.Owner.Id == userId &&
                    (x.Status == BettingStatus.未中奖 || x.Status == BettingStatus.中奖))
                    .Sum(x => x.Bonus);
            bool hadLotteery = db.Set<BettingOfJaw>().Any(x => x.Owner.Id == userId
                && x.Issue == moj.NextPhases
                && (x.Status == BettingStatus.等待开奖 || x.Status == BettingStatus.即将开奖));

            return new MainOfJawResult
            {
                Phases = loj.Issue,
                LoteryValue = loj.Value,
                NextPhases = moj.NextPhases,
                NextLotteryTime = moj.NextLotteryTime,
                Profit = profit,
                HadLottery = hadLotteery
            };
        }

        /// <summary>
        /// 获取开奖记录（前十期）
        /// </summary>
        /// <returns>返回开奖记录（前十期）</returns>
        public List<LotteryOfJawResult> GetLotterys()
        {
            var ms = db.Set<MarkOfJaw>().ToList();
            return db.Set<LotteryOfJaw>().OrderByDescending(x => x.CreatedTime)
                .Take(10)
                .ToList()
                .ConvertAll(x =>
                    {
                        IconOfJaw icon = ms.Where(m => m.TouchOffList.Contains(x.Value))
                            .Select(m => m.Icon)
                            .First();
                        return new LotteryOfJawResult
                        {
                            Icon = icon,
                            Issue = x.Issue
                        };
                    });
        }

        #endregion
    }
}
