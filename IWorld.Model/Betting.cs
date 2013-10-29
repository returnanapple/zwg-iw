using System;
using System.Linq;
using System.Collections.Generic;

namespace IWorld.Model
{
    /// <summary>
    /// 投注记录
    /// </summary>
    public class Betting : ModelBase, IConsumption
    {
        #region 公开属性

        /// <summary>
        /// 投注人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public string Phases { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public double Multiple { get; set; }

        /// <summary>
        /// 用于转换为赔率的点数
        /// </summary>
        public double Points { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        public virtual HowToPlay HowToPlay { get; set; }

        /// <summary>
        /// 位
        /// </summary>
        public virtual List<BettingSeat> Seats { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public BettingStatus Status { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public double Pay { get; set; }

        /// <summary>
        /// 中奖金额（未中奖时候为0）
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 一个布尔值 标识是否作弊
        /// </summary>
        public bool Cheat { get; set; }

        /// <summary>
        /// 关于作弊的备注
        /// </summary>
        public string RemarkForCheat { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的投注记录
        /// </summary>
        public Betting()
        {
        }

        /// <summary>
        /// 实例化一个新的投注记录
        /// </summary>
        /// <param name="owner">投注人</param>
        /// <param name="phases">期数</param>
        /// <param name="sum">注数</param>
        /// <param name="multiple">倍数</param>
        /// <param name="points">用于转换为赔率的点数</param>
        /// <param name="howToPlay">玩法</param>
        /// <param name="seats">位</param>
        /// <param name="pay">投注金额</param>
        public Betting(Author owner, string phases, int sum, double multiple, double points, HowToPlay howToPlay, List<BettingSeat> seats
            , double pay)
        {
            this.Owner = owner;
            this.Phases = phases;
            this.Sum = sum;
            this.Multiple = multiple;
            this.Points = points;
            this.HowToPlay = howToPlay;
            this.Seats = seats;
            this.Status = BettingStatus.等待开奖;
            this.Pay = pay;
            this.Bonus = 0;
            this.Cheat = false;
            this.RemarkForCheat = "";
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 获取消费量
        /// </summary>
        /// <returns>返回消费量</returns>
        public double GetConsumption()
        {
            return this.Pay;
        }

        /// <summary>
        /// 执行作弊
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        public void DoCheat(Lottery lottery)
        {
            if (this.HowToPlay.Interface == LotteryInterface.任N直选
                && this.HowToPlay.Parameter3 == 1)
            {
                DoCheatWhenHowToPlayIsDirectElection_Duplex(lottery);
            }
            else if (this.HowToPlay.Interface == LotteryInterface.任N直选
                && this.HowToPlay.Parameter3 == 0)
            {
                DoCheatWhenHowToPlayIsDirectElection_SingleEntry(lottery);
            }
            else if (this.HowToPlay.Interface == LotteryInterface.任N组选)
            {
                DoCheatWhenHowToPlayIsGroupSelection(lottery);
            }
            else if (this.HowToPlay.Interface == LotteryInterface.任N不定位)
            {
                DoCheatWhenHowToPlayIsDoesNotLocate(lottery);
            }
            else if (this.HowToPlay.Interface == LotteryInterface.任N定位胆)
            {
                DoCheatWhenHowToPlayIsPositioningBile(lottery);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 对直选单式进行作弊
        /// </summary>
        /// <param name="lottery"></param>
        void DoCheatWhenHowToPlayIsDirectElection_SingleEntry(Lottery lottery)
        {
            int countOfAllSum = 1;
            string sts = "";
            this.HowToPlay.Seats.ForEach(x =>
                {
                    countOfAllSum *= x.ValueList.Count;
                    sts += "0";
                });
            if (this.Sum == countOfAllSum)
            {
                this.RemarkForCheat = "没有可以被安全改变的单式投注 作弊无效";
                return;
            }
            List<string> vs = this.Seats.First().Values.Split(new char[] { ',', ' ' }).ToList();
            string v = string.Join("", lottery.Seats.ConvertAll(x => x.Value));
            vs.Remove(v);

            for (int i = 0; i < countOfAllSum; i++)
            {
                string t = i.ToString(sts);
                if (!vs.Contains(t) && t != v)
                {
                    vs.Add(t);
                    break;
                }
            }

            string tValues = string.Join(" ", vs);
            this.Seats.ForEach(x =>
                {
                    x.Values = tValues;
                });
        }

        /// <summary>
        /// 对直选复式进行作弊
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        void DoCheatWhenHowToPlayIsDirectElection_Duplex(Lottery lottery)
        {
            int countOfAllValuesInSeat = this.HowToPlay.Seats.First().ValueList.Count;
            BettingSeat seat = this.Seats.Where(s => s.ValueList.Count < countOfAllValuesInSeat)
                .OrderByDescending(s => s.ValueList.Count)
                .FirstOrDefault();
            if (seat == null)
            {
                this.RemarkForCheat = "因为没有可以被安全改变的位 作弊无效";
                return;
            }
            List<string> vs = seat.Values.Split(new char[] { ',', ' ' }).ToList();
            string v = lottery.Seats.First(x => x.Name == seat.Name).Value;
            vs.Remove(v);
            bool hadInsertValue = false;
            this.HowToPlay.Seats.First().ValueList.ForEach(x =>
                {
                    if (hadInsertValue) { return; }
                    if (!vs.Contains(x) && x != v)
                    {
                        vs.Add(x);
                        hadInsertValue = true;
                    }
                });
            List<int> vsByInt = vs.ConvertAll(x => Convert.ToInt32(x)).OrderBy(x => x).ToList();
            seat.Values = string.Join(",", vsByInt);
        }

        /// <summary>
        /// 对组选进行作弊
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        void DoCheatWhenHowToPlayIsGroupSelection(Lottery lottery)
        {
            List<string> vs = this.Seats.First().Values.Split(new char[] { ',', ' ' }).ToList();
            List<string> winVs = vs.Where(x => lottery.Seats.Any(s => s.Value == x)).ToList();
            List<string> lostVs = this.HowToPlay.Seats.First().ValueList
                .Where(x => !lottery.Seats.Any(s => s.Value == x) && !vs.Contains(x))
                .ToList();
            int tc = winVs.Count - this.HowToPlay.Parameter1 + 1;
            if (lostVs.Count < tc)
            {
                this.RemarkForCheat = "因为没有足够的位进行修改，作弊无效";
                return;
            }
            for (int i = 0; i < tc; i++)
            {
                vs.Remove(winVs[i]);
                vs.Add(lostVs[i]);
            }
            string v = string.Join(",", vs.OrderBy(x => x));
            this.Seats.ForEach(x =>
                {
                    x.Values = v;
                });
        }

        /// <summary>
        /// 对不定位进行作弊
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        void DoCheatWhenHowToPlayIsDoesNotLocate(Lottery lottery)
        {
            List<string> vs = this.Seats.First().Values.Split(new char[] { ',', ' ' }).ToList();
            List<string> winVs = vs.Where(x => lottery.Seats.Any(s => s.Value == x)).ToList();
            List<string> lostVs = this.HowToPlay.Seats.First().ValueList
                .Where(x => !lottery.Seats.Any(s => s.Value == x) && !vs.Contains(x))
                .ToList();
            int tc = winVs.Count;
            if (lostVs.Count < tc)
            {
                this.RemarkForCheat = "因为没有足够的位进行修改，作弊无效";
                return;
            }
            for (int i = 0; i < tc; i++)
            {
                vs.Remove(winVs[i]);
                vs.Add(lostVs[i]);
            }
            string v = string.Join(",", vs.OrderBy(x => x));
            this.Seats.ForEach(x =>
            {
                x.Values = v;
            });
        }

        /// <summary>
        /// 对定位胆进行作弊
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        void DoCheatWhenHowToPlayIsPositioningBile(Lottery lottery)
        {
            this.Seats.ForEach(seat =>
                {
                    List<string> vs = seat.Values.Split(new char[] { ',', ' ' }).ToList();
                    List<string> lostVs = this.HowToPlay.Seats.First().ValueList
                        .Where(x => !vs.Contains(x) && x != lottery.Seats.First(s => s.Name == seat.Name).Value)
                        .ToList();
                    if (lostVs.Count == 0)
                    {
                        this.RemarkForCheat = string.Format("{0}没有能够安全修改的空位，{0}部分作弊无效 ", seat.Name);
                        return;
                    }
                    vs.RemoveAll(x => x == lottery.Seats.First(s => s.Name == seat.Name).Value);
                    vs.Add(lostVs.First());
                    seat.Values = string.Join(",", vs.ConvertAll(x => Convert.ToInt32(x)).OrderBy(x => x));
                });
        }

        #endregion
    }
}
