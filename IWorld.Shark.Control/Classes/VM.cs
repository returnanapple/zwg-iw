using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Shark.Control.JawService;
using System.Windows.Threading;
using IWorld.Shark.Control.SystemSettingService;

namespace IWorld.Shark.Control.Classes
{
    public class VM : INotifyPropertyChanged
    {
        #region 构造函数
        public VM()
        {
            token = "app.token";
            jawClient = new JawServiceClient();
            jawClient.GetMainOfJawCompleted += GetMessageCompleted;
            jawClient.GetLotterysCompleted += GetNotesCompleted;

            systemSettingClient = new SystemSettingServiceClient();
            systemSettingClient.GetWebSettingCompleted += systemSettingClientGetWebSettingCompleted;

            GetMessage();
            timerOfSurplusTime = new DispatcherTimer { Interval = TimeSpan.Parse("0:0:1") };
            timerOfSurplusTime.Tick += timerOfSurplusTimeTick;
            timerOfSurplusTime.Start();

            timeOfGetMessage = new DispatcherTimer { Interval = TimeSpan.Parse("0:0:3") };
            timeOfGetMessage.Tick += timeOfGetMessageTick;
            timeOfGetMessage.Start();

            BetInfoList = new List<BetInfo>();
            OddsInfoList = new List<OddsInfo> 
            { 
                new OddsInfo(IconOfJaw.蓝色鲨鱼,24),
                new OddsInfo(IconOfJaw.燕子,6),
                new OddsInfo(IconOfJaw.鸽子,8),
                new OddsInfo(IconOfJaw.孔雀,8),
                new OddsInfo(IconOfJaw.老鹰,12),
                new OddsInfo(IconOfJaw.狮子,12),
                new OddsInfo(IconOfJaw.熊猫,8),
                new OddsInfo(IconOfJaw.猴子,8),
                new OddsInfo(IconOfJaw.兔子,6),
                new OddsInfo(IconOfJaw.飞禽,2),
                new OddsInfo(IconOfJaw.走兽,2)
            };
            ClearCommand = new BaseCommand(Clear);
            BetOrCancelCommand = new BaseCommand(BetOrCancel);
        }


        #endregion

        #region 私有变量
        /// <summary>
        /// 当前期号
        /// </summary>
        string currentNumber;
        /// <summary>
        /// 结果
        /// </summary>
        int result;

        /// <summary>
        /// 下期期号
        /// </summary>
        string nextNumber;
        /// <summary>
        /// 剩余时间
        /// </summary>
        string surplusTime;
        /// <summary>
        /// 下注信息列表
        /// </summary>
        List<BetInfo> betInfoList;
        /// <summary>
        /// 下注总额
        /// </summary>
        int betAll;
        /// <summary>
        /// 是否下注
        /// </summary>
        bool beted;
        /// <summary>
        /// 是否封单
        /// </summary>
        bool closed;
        /// <summary>
        /// 下期开奖时间
        /// </summary>
        DateTime nextTime;
        /// <summary>
        /// 封单时间
        /// </summary>
        int closeTime;

        /// <summary>
        /// 赔率信息列表
        /// </summary>
        List<OddsInfo> oddsInfoList;
        /// <summary>
        /// 开奖记录列表
        /// </summary>
        ObservableCollection<ResultNote> resultNoteList;
        /// <summary>
        /// 整体盈亏
        /// </summary>
        int profit;
        #endregion

        #region 服务变量
        string token;
        JawServiceClient jawClient;
        SystemSettingServiceClient systemSettingClient;
        DispatcherTimer timerOfSurplusTime;
        DispatcherTimer timeOfGetMessage;
        #endregion

        #region 属性
        /// <summary>
        /// 当前期号
        /// </summary>
        public string CurrentNumber
        {
            get
            { return currentNumber; }
            set
            {
                currentNumber = value;
                OnPropertyChanged(this, "CurrentNumber");
            }
        }
        /// <summary>
        /// 结果
        /// </summary>
        public int Result
        {
            get
            { return result; }
            set
            {
                result = value;
                OnPropertyChanged(this, "Result");
            }
        }

        /// <summary>
        /// 下期期号
        /// </summary>
        public string NextNumber
        {
            get
            { return nextNumber; }
            set
            {
                nextNumber = value;
                OnPropertyChanged(this, "NextNumber");
            }
        }
        /// <summary>
        /// 剩余时间
        /// </summary>
        public string SurplusTime
        {
            get
            {
                return surplusTime;
            }
            set
            {
                surplusTime = value;
                OnPropertyChanged(this, "SurplusTime");
            }
        }
        /// <summary>
        /// 下注信息列表
        /// </summary>
        public List<BetInfo> BetInfoList
        {
            get
            { return betInfoList; }
            set
            {
                betInfoList = value;
                int sum = 0;
                BetInfoList.ForEach(x =>
                {
                    sum = sum + x.BetValue;
                });
                BetAll = sum;
            }
        }
        /// <summary>
        /// 下注总额
        /// </summary>
        public int BetAll
        {
            get
            { return betAll; }
            set
            {
                betAll = value;
                OnPropertyChanged(this, "BetAll");
            }
        }
        /// <summary>
        /// 是否下注
        /// </summary>
        public bool Beted
        {
            get
            { return beted; }
            set
            {
                beted = value;
                OnPropertyChanged(this, "Beted");
            }
        }
        /// <summary>
        /// 是否封单
        /// </summary>
        public bool Closed
        {
            get
            { return closed; }
            set
            {
                closed = value;
                OnPropertyChanged(this, "Closed");
            }
        }

        /// <summary>
        /// 赔率信息列表
        /// </summary>
        public List<OddsInfo> OddsInfoList
        {
            get
            { return oddsInfoList; }
            set
            { oddsInfoList = value; }
        }
        /// <summary>
        /// 开奖记录列表
        /// </summary>
        public ObservableCollection<ResultNote> ResultNoteList
        {
            get
            { return resultNoteList; }
            set
            {
                resultNoteList = value;
            }
        }
        /// <summary>
        /// 整体盈亏
        /// </summary>
        public int Profit
        {
            get
            { return profit; }
            set
            {
                profit = value;
                OnPropertyChanged(this, "Profit");
            }
        }

        /// <summary>
        /// 清除命令
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// 下注或者取消命令
        /// </summary>
        public ICommand BetOrCancelCommand { get; set; }
        #endregion

        #region 前台函数
        /// <summary>
        /// 清空下注信息
        /// </summary>
        public void Clear(object p)
        {
            BetInfoList.Clear();
            OnPropertyChanged(this, "BetInfoList");
            BetAll = 0;
        }
        #endregion

        #region 后台函数
        /// <summary>
        /// 获取信息
        /// </summary>
        public void GetMessage()
        {
            jawClient.GetMainOfJawAsync(token);
        }
        /// <summary>
        /// 获取信息完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetMessageCompleted(object sender, GetMainOfJawCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                if (CurrentNumber != e.Result.Phases)
                {
                    CurrentNumber = e.Result.Phases;
                    Result = e.Result.LoteryValue;
                    NextNumber = e.Result.NextPhases;
                    nextTime = e.Result.NextLotteryTime;
                    Beted = e.Result.HadLottery;
                    Profit = Convert.ToInt32(e.Result.Profit);
                    GetNotes();
                }
                else
                {
                    Beted = e.Result.HadLottery;
                }
            }
        }
        /// <summary>
        /// 获取开奖历史记录
        /// </summary>
        public void GetNotes()
        {
            jawClient.GetLotterysAsync(token);
        }
        /// <summary>
        /// 获取开奖历史记录完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetNotesCompleted(object sender, GetLotterysCompletedEventArgs e)
        {
            ResultNoteList.Clear();
            e.Result.ForEach(x =>
            {
                ResultNoteList.Add(new ResultNote(x.Icon, x.Issue));
            });
        }
        /// <summary>
        /// 下注或者取消
        /// </summary>
        public void BetOrCancel(object p)
        {
            if (Beted == false && Closed == false)
            {
                BettingOfJawImport bji = new BettingOfJawImport();
                bji.Issue = CurrentNumber;
                bji.Details = new List<BettingDetailOfJawImport>();
                BetInfoList.ForEach(x =>
                {
                    bji.Details.Add(new BettingDetailOfJawImport { Icon = x.BetName, Sum = x.BetValue });
                });
                jawClient.BetAsync(bji, token);
            }
            else if (Beted == true && Closed == false)
            {
                jawClient.RevocationAsync(token);
            }
            GetMessage();
        }
        /// <summary>
        /// 获取封单时间完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void systemSettingClientGetWebSettingCompleted(object sender, GetWebSettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                closeTime = e.Result.ClosureSingleTime;
            }
        }
        #endregion

        #region Tick事件
        public void timerOfSurplusTimeTick(object sender, EventArgs e)
        {
            systemSettingClient.GetWebSettingAsync(token);
            double s = (nextTime - DateTime.Now).TotalSeconds;
            if (Convert.ToInt32(s) <= closeTime)
            {
                Closed = true;
            }

            if (s >= 0)
            {
                SurplusTime = (nextTime - DateTime.Now).ToString("HH:mm:ss");
            }

            if (Convert.ToInt32(s) == 0)
            {
                GetMessage();
            }
        }
        public void timeOfGetMessageTick(object sender, EventArgs e)
        {
            GetMessage();
        }
        #endregion

        #region 属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
