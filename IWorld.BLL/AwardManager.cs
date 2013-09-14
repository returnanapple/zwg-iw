using System.Collections.Generic;
using System.Linq;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 开奖行为的管理者对象
    /// </summary>
    public class AwardManager
    {
        /// <summary>
        /// 判断投注是否中奖
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        public static int AwardByBetting(Lottery lottery, Betting betting)
        {
            switch (betting.HowToPlay.Interface)
            {
                case LotteryInterface.任N直选:
                    return DirectElectionByBetting(lottery, betting);
                case LotteryInterface.任N组选:
                    return GroupSelectionByBetting(lottery, betting);
                case LotteryInterface.任N不定位:
                    return DoesNotLocateByBetting(lottery, betting);
                case LotteryInterface.任N定位胆:
                    return PositioningBileByBetting(lottery, betting);
                case LotteryInterface.大小单双:
                    return LargeOrSmallAndSingleOrDoubleByBetting(lottery, betting);
            }

            return 0;
        }

        /// <summary>
        /// 判断追号是否中奖
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        public static int AwardByChasing(Lottery lottery, Chasing chasing)
        {
            switch (chasing.HowToPlay.Interface)
            {
                case LotteryInterface.任N直选:
                    return DirectElectionByChasing(lottery, chasing);
                case LotteryInterface.任N组选:
                    return GroupSelectionByChasing(lottery, chasing);
                case LotteryInterface.任N不定位:
                    return DoesNotLocateByChasing(lottery, chasing);
                case LotteryInterface.任N定位胆:
                    return PositioningBileByChasing(lottery, chasing);
                case LotteryInterface.大小单双:
                    return LargeOrSmallAndSingleOrDoubleByChasing(lottery, chasing);
            }

            return 0;
        }

        #region 处理投注记录

        /// <summary>
        /// 任N直选(投注记录)
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int DirectElectionByBetting(Lottery lottery, Betting betting)
        {
            int result = 1;
            if (betting.HowToPlay.Parameter3 == 1)//复式投注
            {
                betting.Seats.ForEach(x =>
                {
                    if (!lottery.Seats.Any(s => s.Name == x.Name
                        && x.ValueList.Contains(s.Value)))
                    {
                        result = 0;
                    }
                });
            }
            else if (betting.HowToPlay.Parameter3 == 0)//单式投注
            {
                result = 0;
                List<string> seatName = new List<string> { "万位", "千位", "百位", "十位", "个位" };
                if (lottery.Seats.Count == 3)
                {
                    seatName = new List<string> { "百位", "十位", "个位" };
                }
                betting.Seats.First().ValueList.ForEach(x =>
                    {
                        List<string> tBettingValues = x.ToArray().ToList().ConvertAll(y => y.ToString());
                        Dictionary<string, string> tSeats = new Dictionary<string, string>();
                        for (int i = 0; i < tBettingValues.Count; i++)
                        {
                            tSeats.Add(seatName[i], tBettingValues[i]);
                        }
                        if (lottery.Seats.All(t => t.Value == tSeats[t.Name]))
                        {
                            result = 1;
                        }
                    });
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 任N组选(投注记录)
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int GroupSelectionByBetting(Lottery lottery, Betting betting)
        {
            int t = 0;
            List<BettingSeat> seats = new List<BettingSeat>();
            betting.Seats.ForEach(bettingSeat =>
            {
                if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == bettingSeat.Name
                    && bettingSeat.ValueList.Contains(lotterySeat.Value)))
                {
                    t++;
                    seats.Add(bettingSeat);
                }
            });
            if (t < betting.HowToPlay.Parameter1)
            {
                return 0;
            }
            if (betting.HowToPlay.Parameter2 == 0)
            {
                return 1;
            }
            List<LotterySeat> _seats = lottery.Seats.Where(x => seats.Any(s => s.Name == x.Name)).ToList();
            List<int> nums = new List<int>();
            _seats.ForEach(x =>
                {
                    int num = _seats.Count(s => s.Value == x.Value
                        && s.Name != x.Name);
                    nums.Add(num);
                });
            if (betting.HowToPlay.Parameter2 == nums.Max())
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 任n不定位(投注记录)
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int DoesNotLocateByBetting(Lottery lottery, Betting betting)
        {
            int result = 0;
            betting.Seats.ForEach(bettingSeat =>
            {
                if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == bettingSeat.Name
                    && bettingSeat.ValueList.Contains(lotterySeat.Value)))
                {
                    result = 1;
                }
            });
            return result;
        }

        /// <summary>
        /// 任n定位胆(投注记录)
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int PositioningBileByBetting(Lottery lottery, Betting betting)
        {
            int result = 0;
            betting.Seats.ForEach(bettingSeat =>
                {
                    if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == bettingSeat.Name
                        && bettingSeat.ValueList.Contains(lotterySeat.Value)))
                    {
                        result++;
                    }
                });
            return result;
        }

        /// <summary>
        /// 大小单双(投注记录)
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="betting">投注记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int LargeOrSmallAndSingleOrDoubleByBetting(Lottery lottery, Betting betting)
        {
            int result = 1;
            betting.Seats.ForEach(bettingSeat =>
            {
                List<string> values = new List<string>();
                bettingSeat.ValueList.ForEach(value =>
                    {
                        values.AddRange(GetValues(value, betting.HowToPlay.Seats.FirstOrDefault(s => s.Name == bettingSeat.Name)));
                    });
                List<string> _values = values.Distinct().ToList();
                if (!lottery.Seats.Any(lotterySeat => lotterySeat.Name == bettingSeat.Name
                    && _values.Contains(lotterySeat.Value)))
                {
                    result = 0;
                }
            });
            return result;
        }

        #endregion

        #region 处理追号记录

        /// <summary>
        /// 任N直选（追号记录）
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int DirectElectionByChasing(Lottery lottery, Chasing chasing)
        {
            int result = 1;
            if (chasing.HowToPlay.Parameter3 == 1) //复式投注
            {
                chasing.Seats.ForEach(x =>
                {
                    if (!lottery.Seats.Any(s => s.Name == x.Name
                        && x.ValuesList.Contains(s.Value)))
                    {
                        result = 0;
                    }
                });
            }
            else if (chasing.HowToPlay.Parameter3 == 0) //单式投注
            {
                List<string> seatName = new List<string> { "万位", "千位", "百位", "十位", "个位" };
                if (lottery.Seats.Count == 3)
                {
                    seatName = new List<string> { "百位", "十位", "个位" };
                }
                chasing.Seats.First().ValuesList.ForEach(x =>
                    {
                        List<string> tBettingValues = x.ToArray().ToList().ConvertAll(y => y.ToString());
                        Dictionary<string, string> tSeats = new Dictionary<string, string>();
                        for (int i = 0; i < tBettingValues.Count; i++)
                        {
                            tSeats.Add(seatName[i], tBettingValues[i]);
                        }
                        if (lottery.Seats.All(t => t.Value == tSeats[t.Name]))
                        {
                            result = 1;
                        }
                    });
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 任N组选（追号记录）
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int GroupSelectionByChasing(Lottery lottery, Chasing chasing)
        {
            int t = 0;
            List<ChasingSeat> seats = new List<ChasingSeat>();
            chasing.Seats.ForEach(chasingSeat =>
            {
                if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == chasingSeat.Name
                    && chasingSeat.ValuesList.Contains(lotterySeat.Value)))
                {
                    t++;
                    seats.Add(chasingSeat);
                }
            });
            if (t < chasing.HowToPlay.Parameter1)
            {
                return 0;
            }
            if (chasing.HowToPlay.Parameter2 == 0)
            {
                return 1;
            }
            List<LotterySeat> _seats = lottery.Seats.Where(x => seats.Any(s => s.Name == x.Name)).ToList();
            List<int> nums = new List<int>();
            _seats.ForEach(x =>
            {
                int num = _seats.Count(s => s.Value == x.Value
                    && s.Name != x.Name);
                nums.Add(num);
            });
            if (chasing.HowToPlay.Parameter2 == nums.Max())
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 任n不定位（追号记录）
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int DoesNotLocateByChasing(Lottery lottery, Chasing chasing)
        {
            int result = 0;
            chasing.Seats.ForEach(chasingSeat =>
            {
                if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == chasingSeat.Name
                    && chasingSeat.ValuesList.Contains(lotterySeat.Value)))
                {
                    result = 1;
                }
            });
            return result;
        }

        /// <summary>
        /// 任n定位胆（追号记录）
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int PositioningBileByChasing(Lottery lottery, Chasing chasing)
        {
            int result = 0;
            chasing.Seats.ForEach(chasingSeat =>
            {
                if (lottery.Seats.Any(lotterySeat => lotterySeat.Name == chasingSeat.Name
                    && chasingSeat.ValuesList.Contains(lotterySeat.Value)))
                {
                    result++;
                }
            });
            return result;
        }

        /// <summary>
        /// 大小单双（追号记录）
        /// </summary>
        /// <param name="lottery">开奖记录</param>
        /// <param name="chasing">追号记录</param>
        /// <returns>返回一个数字，代表中奖的注数</returns>
        private static int LargeOrSmallAndSingleOrDoubleByChasing(Lottery lottery, Chasing chasing)
        {
            int result = 1;
            chasing.Seats.ForEach(chasingSeat =>
            {
                List<string> values = new List<string>();
                chasingSeat.ValuesList.ForEach(value =>
                {
                    values.AddRange(GetValues(value, chasing.HowToPlay.Seats.FirstOrDefault(s => s.Name == chasingSeat.Name)));
                });
                List<string> _values = values.Distinct().ToList();
                if (!lottery.Seats.Any(lotterySeat => lotterySeat.Name == chasingSeat.Name
                    && _values.Contains(lotterySeat.Value)))
                {
                    result = 0;
                }
            });
            return result;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取大小单双的实际对应值
        /// </summary>
        /// <param name="value">“大小单双”的值</param>
        /// <param name="seat">可选位</param>
        /// <returns>返回一个string的list，包含所有对应的值</returns>
        private static List<string> GetValues(string value, OptionalSeat seat)
        {
            string values = "";
            switch (value)
            {
                case "大":
                    values = seat.ValuesForLarge;
                    break;
                case "小":
                    values = seat.ValuesForSmall;
                    break;
                case "单":
                    values = seat.ValuesForSingle;
                    break;
                case "双":
                    values = seat.ValuesForDouble;
                    break;
            }
            return values.Split(new char[] { ',' }).ToList();
        }

        #endregion
    }
}
