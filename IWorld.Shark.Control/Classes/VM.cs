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

namespace IWorld.Shark.Control.Classes
{
    public class VM : INotifyPropertyChanged
    {
        #region 构造函数
        public VM()
        {
            this.token = "app.token";
            jawClient = new JawServiceClient();
            jawClient.BetCompleted += BetCompleted;
            jawClient.RevocationCompleted += CancelCompleted;
            jawClient.GetLotterysCompleted += GetNotesCompleted;
            time.Tick += timeTick;
            time.Interval = TimeSpan.Parse("0:0:3");
            GetResult();
            GetNotes();
            time.Start();

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
            BetOrCancelCommand = new BaseCommand(BetOrCnacel);
        }


        #endregion

        #region 私有变量
        #region 当期信息
        /// <summary>
        /// 当前期号
        /// </summary>
        string currentNumber;
        /// <summary>
        /// 结果
        /// </summary>
        int result;
        #endregion

        #region 下期信息
        /// <summary>
        /// 下期期号
        /// </summary>
        string nextNumber;
        /// <summary>
        /// 下期开奖时间
        /// </summary>
        DateTime nextLotteryTime;
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
        #endregion

        #region 其他信息
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
        #endregion

        #region 服务变量
        string token;
        JawServiceClient jawClient;
        DispatcherTimer time;
        #endregion

        #region 属性
        #region 当期信息
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
                GetNotes();
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
        #endregion

        #region 下期信息
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
        /// 下期开奖时间
        /// </summary>
        public DateTime NextLotteryTime
        {
            get { return nextLotteryTime; }
            set
            {
                nextLotteryTime = value;
                OnPropertyChanged(this, "NextLotteryTime");
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
        #endregion

        #region 其他信息
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
        #endregion

        #region 命令
        /// <summary>
        /// 清除命令
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// 下注或者取消命令
        /// </summary>
        public ICommand BetOrCancelCommand { get; set; }
        #endregion
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
        #region 下注或取消
        public void BetOrCnacel(object p)
        {
            if (Beted)
            {
                jawClient.RevocationAsync(token);
            }
            else
            {
                BetChildWindow bcw = new BetChildWindow();
                bcw.DataContext = this;
                bcw.OKButton.Click += Bet;
                bcw.Show();
            }
        }
        public void Bet(object sender, RoutedEventArgs e)
        {
            BettingOfJawImport bji = new BettingOfJawImport();
            bji.Issue = NextNumber;
            bji.Details = new List<BettingDetailOfJawImport>();
            foreach (BetInfo i in BetInfoList)
            {
                bji.Details.Add(new BettingDetailOfJawImport { Icon = i.BetName, Sum = i.BetValue });
            }
            jawClient.BetAsync(bji, token);
        }
        void BetCompleted(object sender, BetCompletedEventArgs e)
        {
            GetResult();
        }
        void CancelCompleted(object sender, RevocationCompletedEventArgs e)
        {
            GetResult();
        }
        #endregion

        #region 获得开奖结果
        public void GetResult()
        {
            jawClient.GetMainOfJawAsync(token);
            jawClient.GetMainOfJawCompleted += GetResultCompleted;
        }

        void GetResultCompleted(object sender, GetMainOfJawCompletedEventArgs e)
        {
            CurrentNumber = e.Result.Phases;
            Result = e.Result.LoteryValue;

            NextNumber = e.Result.NextPhases;
            NextLotteryTime = e.Result.NextLotteryTime;
            Beted = e.Result.HadLottery;

            Profit = Convert.ToInt32(e.Result.Profit.ToString());
        }
        #endregion

        #region 获得历史开奖记录
        public void GetNotes()
        {
            jawClient.GetLotterysAsync(token);

        }
        void GetNotesCompleted(object sender, GetLotterysCompletedEventArgs e)
        {
            ResultNoteList.Clear();
            foreach (LotteryOfJawResult i in e.Result)
            {
                ResultNoteList.Add(new ResultNote(i.Icon, i.Issue));
            }
        }
        #endregion
        #endregion

        #region Tick事件
        void timeTick(object sender, EventArgs e)
        {
            GetResult();
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
