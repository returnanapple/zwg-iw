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
    /// 博彩相关的数据服务
    /// </summary>
    public class GamingService : IGamingService
    {
        /// <summary>
        /// 获取彩票和彩票的最新动态
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        public NormalList<LotteryTicketResult> GetLotteryTickets()
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    ClientGamingReader reader = new ClientGamingReader(db);
                    return reader.ReadLotteryTickets();
                }
            }
            catch (Exception ex)
            {
                return new NormalList<LotteryTicketResult>(ex.Message);
            }
        }

        /// <summary>
        /// 获取玩法信息
        /// </summary>
        /// <returns>返回彩票信息的普通列表</returns>
        public NormalList<LotteryTicketResult> GetHowToPlays()
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    ClientGamingReader reader = new ClientGamingReader(db);
                    return reader.ReadHowToPlays();
                }
            }
            catch (Exception ex)
            {
                return new NormalList<LotteryTicketResult>(ex.Message);
            }
        }

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="import">用于投注的数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public BettingActionResult Betting(BettingImport import, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new BettingActionResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    WebSetting webSetting = new WebSetting();
                    HowToPlay _howToPlay = db.Set<HowToPlay>().FirstOrDefault(x => x.Id == import.BettingInfo.HowToPlayId);
                    if (_howToPlay == null)
                    {
                        throw new Exception("没有这个玩法");
                    }
                    int _sum = 1;
                    List<string> tValues = new List<string>();
                    #region 注数

                    switch (_howToPlay.Interface)
                    {
                        case LotteryInterface.任N组选:
                            #region 组选
                            int _tNum = import.Values[0].Values.Count;
                            if (_howToPlay.Name.Contains("组三"))
                            {
                                #region 组三
                                _sum = _tNum * (_tNum - 1);
                                #endregion
                            }
                            else if (_howToPlay.Name.Contains("组六"))
                            {
                                #region 组六
                                _sum *= _tNum < 3 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                    / (DigitalHelper.GetFactorialIn0To12(3) * DigitalHelper.GetFactorialIn0To12(_tNum - 3));
                                #endregion
                            }
                            else if (_howToPlay.Name.Contains("组选"))
                            {
                                #region 二星组选
                                _sum *= _tNum < 2 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                    / (DigitalHelper.GetFactorialIn0To12(2) * DigitalHelper.GetFactorialIn0To12(_tNum - 2));
                                #endregion
                            }
                            #endregion
                            break;
                        case LotteryInterface.任N定位胆:
                            #region 定位胆
                            _sum = 0;
                            import.Values.ForEach(x =>
                                {
                                    _sum += x.Values.Count;
                                });
                            #endregion
                            break;
                        case LotteryInterface.任N直选:
                            #region 任N直选
                            if (_howToPlay.Parameter3 == 0)
                            {
                                for (int i = 0; i < import.Codes.Count; i++)
                                {
                                    tValues.AddRange(_BettingValues[import.Codes[i]]);
                                    _BettingValues.Remove(import.Codes[i]);
                                }
                                _sum = tValues.Count;
                            }
                            else
                            {
                                import.Values.ForEach(x =>
                                {
                                    _sum *= x.Values.Count;
                                });
                            }
                            #endregion
                            break;
                        case LotteryInterface.任N不定位:
                            #region 不定位
                            _sum = import.Values.FirstOrDefault().Values.Count;
                            #endregion
                            break;
                    }
                    if (_sum == 0)
                    {
                        throw new Exception("投注数不能为0");
                    }

                    #endregion

                    double _pay = Math.Round(webSetting.UnitPrice * _sum * import.BettingInfo.Multiple, 2);

                    #region 生成投注记录
                    List<BettingManager.IPackageForSeat> seatsForBetting = new List<BettingManager.IPackageForSeat>();
                    if (_howToPlay.Parameter3 == 0)
                    {
                        string _tValues = string.Join(" ", tValues);
                        _howToPlay.Seats.ForEach(x =>
                            {
                                seatsForBetting.Add(BettingManager.Factory.CreatePackageForSeat(x.Name
                                    , _tValues));
                            });
                    }
                    else
                    {
                        import.Values.ForEach(x =>
                            {
                                seatsForBetting.Add(BettingManager.Factory.CreatePackageForSeat(x.Seat
                                    , string.Join(",", x.Values)));
                            });
                    }
                    ICreatePackage<Betting> pfcForBetting = BettingManager.Factory.CreatePackageForCreate(userId
                        , import.BettingInfo.Phases, _sum, import.BettingInfo.Multiple, import.BettingInfo.Points
                        , _howToPlay.Id, seatsForBetting, _pay);
                    var betting = new BettingManager(db).Create(pfcForBetting);

                    #endregion
                    #region 生成追号记录
                    if (import.ChasingInfo.IsChasing)
                    {
                        double pays = 0;
                        List<ChasingManager.Fanctory.IPackageForSeat> seatsForChasing
                            = new List<ChasingManager.Fanctory.IPackageForSeat>();
                        import.Values.ForEach(x =>
                            {
                                seatsForChasing.Add(ChasingManager.Fanctory.CreatePackageForSeat(x.Seat
                                    , string.Join(",", x.Values)));
                            });
                        List<ChasingManager.Fanctory.IPackageForBetting> bettingsForChasing
                            = new List<ChasingManager.Fanctory.IPackageForBetting>();
                        import.ChasingInfo.Bettings.ForEach(x =>
                            {
                                double tPay = Math.Round(_pay * x.Exponent, 2);
                                bettingsForChasing.Add(ChasingManager.Fanctory.CreatePackageForBetting(x.Phases, x.Exponent
                                    , tPay));
                                pays += tPay;
                            });
                        ICreatePackage<Chasing> pfcForChasing = ChasingManager.Fanctory.CreatePackageForCreate(userId
                            , import.ChasingInfo.Postpone, import.ChasingInfo.Continuance, _sum, import.BettingInfo.Multiple
                            , import.BettingInfo.Multiple, _howToPlay.Id, seatsForChasing, bettingsForChasing, pays
                            , import.ChasingInfo.EndIfLotteryBeforeBegin, import.ChasingInfo.EndIfLotteryAtGoing);
                        new ChasingManager(db).Create(pfcForChasing);
                    }
                    #endregion

                    return new BettingActionResult(betting);
                }
            }
            catch (Exception ex)
            {
                return new BettingActionResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取投注明细
        /// </summary>
        /// <param name="selectType">筛选类型</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注明细的分页列表</returns>
        public PaginationList<BettingDetailsResult> GetBettingDetails(BettingDetailsSelectType selectType, string beginTime
            , string endTime, int page, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new PaginationList<BettingDetailsResult>("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientGamingReader reader = new ClientGamingReader(db);
                    return reader.ReadBettingDetails(userId, selectType, beginTime, endTime, page);
                }
            }
            catch (Exception ex)
            {
                return new PaginationList<BettingDetailsResult>(ex.Message);
            }
        }

        /// <summary>
        /// 获取中奖排行
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回中奖排行信息的普通列表</returns>
        public NormalList<RankingResult> GetRanking(int ticketId)
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    ClientGamingReader reader = new ClientGamingReader(db);
                    return reader.ReadRanking(ticketId);
                }
            }
            catch (Exception ex)
            {
                return new NormalList<RankingResult>(ex.Message);
            }
        }

        /// <summary>
        /// 获取开奖历史
        /// </summary>
        /// <param name="ticketId">目标裁判的存储指针</param>
        /// <returns>返回开奖历史的普通列表</returns>
        public NormalList<HistoryOfLotteryResult> GetHistoryOfLottery(int ticketId)
        {
            try
            {
                using (WebMapContext db = new WebMapContext())
                {
                    ClientGamingReader reader = new ClientGamingReader(db);
                    return reader.ReadHistoryOfLottery(ticketId);
                }
            }
            catch (Exception ex)
            {
                return new NormalList<HistoryOfLotteryResult>(ex.Message);
            }
        }

        /// <summary>
        /// 删除投注
        /// </summary>
        /// <param name="bettingId">投注记录的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveBetting(int bettingId, string token)
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
                    new BettingManager(db).ChangeStatus(bettingId, BettingStatus.用户撤单);
                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        static Dictionary<string, List<string>> _BettingValues = new Dictionary<string, List<string>>();
        public AddBettingValuesResult AddBettingValues(List<string> values, string token)
        {
            try
            {
                string code = Guid.NewGuid().ToString("N");
                _BettingValues.Add(code, values);
                return new AddBettingValuesResult { Code = code };
            }
            catch (Exception ex)
            {
                return new AddBettingValuesResult(ex.Message);
            }
        }
    }
}
