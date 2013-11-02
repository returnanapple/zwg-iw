using System.Linq;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 初始事件的管理者对象
    /// </summary>
    public class EventManager
    {
        /// <summary>
        /// 注册静态事件链
        /// </summary>
        public static void Initialization()
        {
            #region 用户

            WithdrawalsRecordManager.CreatingEventHandler += AuthorManager.ApplyToCash;
            WithdrawalsRecordManager.RemovingEventHandler += AuthorManager.RevocationOfWithdrawals;
            WithdrawalsRecordManager.ChangingStatusEventHandler += AuthorManager.GetResultOfWithdrawal;
            RechargeRecordManager.ChangingStatusEventHandler += AuthorManager.SuccessfulRecharge;

            SubordinateDynamicManager.CreatingEventHandler += AuthorManager.RebateBySubordinate;
            BettingManager.CreatingEventHandler += AuthorManager.PayForBetting;
            BettingManager.ChangingStatusEventHandler += AuthorManager.GetResultOfBetting;
            ChasingManager.CreatingEventHandler += AuthorManager.PayForChasing;
            ChasingManager.ChangingStatusEventHandler += AuthorManager.ReturnTheBalanceOfChasing;
            BettingForCgasingManager.ChangingStatusEventHandler += AuthorManager.GetResultOfBettingForChasing;

            ActivityParticipateRecordManager.CreatingEventHandler += AuthorManager.ParticipateInActivity;
            ExchangeParticipateRecordManager.CreatingEventHandler += AuthorManager.ParticipateInExchange;

            AuthorManager.CreatedEventHandler += AuthorManager.AddSub;

            #endregion

            #region 登陆记录

            AuthorManager.LoginedEventHandler += UserLandingRecordManager.CreateLandingRecord;
            AdministratorManager.LoginedEventHandler += AdministratorLandingRecordManager.CreateLandingRecord;

            #endregion

            #region 投注（追号）

            ChasingManager.ChangingStatusEventHandler += BettingForCgasingManager.SystemCancellation;

            #endregion

            #region 下级用户的动态

            BettingManager.ChangedStatusEventHandler += SubordinateDynamicManager.CreateSubordinateDynamicsForBetting;

            #endregion

            #region 开奖

            LotteryManager.CreatedEventHandler += BettingManager.GetResultOfLottery;
            //LotteryManager.CreatedEventHandler += ChasingManager.UpdateChasingStatus;
            //LotteryManager.CreatedEventHandler += BettingForCgasingManager.GetResultOfLottery;
            LotteryManager.CreatedEventHandler += LotteryTicketManager.UpdateLotteryTime;

            #endregion

            #region 活动

            AuthorManager.CreatedEventHandler += ActivityParticipateRecordManager.CreateRecordWhenUserCreated;
            RechargeRecordManager.ChangedStatusEventHandler += ActivityParticipateRecordManager.CreateRecordWhenRecharge;
            BettingManager.ChangedStatusEventHandler += ActivityParticipateRecordManager.CreateRecordWhenBetting;
            BettingManager.ChangingStatusEventHandler += ActivityParticipateRecordManager.CreateRecordWhenLottery;
            ChasingManager.ChangedStatusEventHandler += ActivityParticipateRecordManager.CreateRecordWhenChasing;
            PersonalDataManager.CountedForDayEventHandler += ActivityParticipateRecordManager.CreateRecordWhenCountedForDay;

            #endregion

            #region 时间线

            TimeLineManager.Interval20SecondEventHandler += BettingManager.UpdateBettingStatus;
            //TimeLineManager.Interval20SecondEventHandler += BettingForCgasingManager.UpdateBettingStatus;
            //TimeLineManager.Interval20SecondEventHandler += BettingManager.GetResultOfLotteryOnTimeLine;
            TimeLineManager.Interval20SecondEventHandler += (sender, e) =>
                {
                    RechargeRecordManager manager = new RechargeRecordManager(e.Db);
                    e.Db.Set<RechargeRecord>().Where(x => x.Status == RechargeStatus.用户已经支付)
                        .ToList().ForEach(x =>
                            {
                                manager.ChangeStatus(x.Id, RechargeStatus.充值成功);
                            });
                };
            TimeLineManager.Interval20SecondEventHandler += BettingOfJawManager.UpdateBettingStatus;

            #endregion

            #region 通知/站内信

            BettingManager.ChangedStatusEventHandler += NoticeManager.CreateNoticeWhenLottery;
            BettingForCgasingManager.ChangedStatusEventHandler += NoticeManager.CreateNoticeWhenLottery;
            RechargeRecordManager.ChangedStatusEventHandler += NoticeManager.CreateNoticeWhenRechargeHadResult;
            WithdrawalsRecordManager.ChangedStatusEventHandler += NoticeManager.CreateNoticeWhenWithdrawalsHadResult;

            #endregion

            #region 数据统计

            AuthorManager.LoginedEventHandler += SiteDataManager.HadSetedIn;
            AuthorManager.CreatedEventHandler += SiteDataManager.HadSetedUp;
            BettingManager.CreatedEventHandler += SiteDataManager.HadBeted;
            ChasingManager.ChangedStatusEventHandler += SiteDataManager.HadBeted;
            BettingManager.ChangedStatusEventHandler += SiteDataManager.HadLottery;
            BettingManager.ChangedStatusEventHandler += SiteDataManager.HadRevoked;
            //BettingForCgasingManager.ChangedStatusEventHandler += SiteDataManager.HadLottery;
            ActivityParticipateRecordManager.CreatedEventHandler += SiteDataManager.HadParticipatedActivity;
            ExchangeParticipateRecordManager.CreatedEventHandler += SiteDataManager.HadParticipatedExchange;
            RechargeRecordManager.ChangedStatusEventHandler += SiteDataManager.HadRecharged;
            WithdrawalsRecordManager.ChangedStatusEventHandler += SiteDataManager.HadWithdrawal;
            TransferRecordManager.CreatedEventHandler += SiteDataManager.HadTransfer;
            SubordinateDynamicManager.CreatedEventHandler += SiteDataManager.HadReturnedPoints;

            BettingManager.CreatedEventHandler += PersonalDataManager.HadBeted;
            ChasingManager.ChangedStatusEventHandler += PersonalDataManager.HadBeted;
            BettingManager.ChangedStatusEventHandler += PersonalDataManager.HadLottery;
            BettingManager.ChangedStatusEventHandler += PersonalDataManager.HadRevoked;
            //BettingForCgasingManager.ChangedStatusEventHandler += PersonalDataManager.HadLottery;
            ActivityParticipateRecordManager.CreatedEventHandler += PersonalDataManager.HadParticipatedActivity;
            ExchangeParticipateRecordManager.CreatedEventHandler += PersonalDataManager.HadParticipatedExchange;
            RechargeRecordManager.ChangedStatusEventHandler += PersonalDataManager.HadRecharged;
            WithdrawalsRecordManager.ChangedStatusEventHandler += PersonalDataManager.HadWithdrawal;
            SubordinateDynamicManager.CreatedEventHandler += PersonalDataManager.HadReturnedPoints;

            #endregion

            #region 缓存池

            CustomerRecordManager.CreatedEventHandler += CacheManager.SetCustomerMessageIn;

            #endregion

            #region 大白鲨游戏

            LotteryOfJawManager.CreatedEventHandler += BettingOfJawManager.GetResultOfLottery;
            BettingOfJawManager.CreatingEventHandler += AuthorManager.PayForJaw;
            BettingOfJawManager.ChangingStatusEventHandler += AuthorManager.GetResultOfJaw;

            #endregion
        }
    }
}
