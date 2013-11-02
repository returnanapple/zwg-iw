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

namespace IWorld.Shark.Control.Classes
{
    public class VM : INotifyPropertyChanged
    {
        #region 构造函数
        public VM()
        {
            Result = -3;
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
            ResultNoteList = new ObservableCollection<ResultNote>
            {
                new ResultNote(IconOfJaw.猴子,"a"),
                new ResultNote(IconOfJaw.猴子,"b")
            };
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 结果
        /// </summary>
        int result;
        /// <summary>
        /// 下注信息列表
        /// </summary>
        List<BetInfo> betInfoList;
        /// <summary>
        /// 赔率信息列表
        /// </summary>
        List<OddsInfo> oddsInfoList;
        /// <summary>
        /// 当前期号
        /// </summary>
        string currentNumber;
        /// <summary>
        /// 开奖记录列表
        /// </summary>
        ObservableCollection<ResultNote> resultNoteList;
        /// <summary>
        /// 下注总额
        /// </summary>
        int betAll;
        #endregion

        #region 属性
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
        #endregion

        #region 前台函数
        /// <summary>
        /// 清空下注信息
        /// </summary>
        public void ClearBetSum()
        {
            foreach (BetInfo i in BetInfoList)
            {
                BetInfoList.Clear();
                OnPropertyChanged(this, "BetInfoList");
            }
        }
        #endregion

        #region 后台函数
        #region 下注

        #endregion
        #region 续投

        #endregion
        #region 撤单

        #endregion
        #region 获得开奖结果

        #endregion
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
