using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using IWorld.Admin.Framework.DataReportService;
using IWorld.Admin.Framework.ManagerService;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 首页的视图模型
    /// </summary>
    public class IndexViewModel : ManagerViewModelBase
    {
        #region 私有字段

        double _setUpAtDay = 0;
        double _setUpAtMonth = 0;
        double _setUpAtAll = 0;

        double _setInAtDay = 0;
        double _setInAtMonth = 0;
        double _setInAtAll = 0;

        double _amountOfBetsAtDay = 0;
        double _amountOfBetsAtMonth = 0;
        double _amountOfBetsAtAll = 0;

        double _rebateAtDay = 0;
        double _rebateAtMonth = 0;
        double _rebateAtAll = 0;

        double _bonusAtDay = 0;
        double _bonusAtMonth = 0;
        double _bonusAtAll = 0;

        double _expendituresAtDay = 0;
        double _expendituresAtMonth = 0;
        double _expendituresAtAll = 0;

        double _gainsAndLossesAtDay = 0;
        double _gainsAndLossesAtMonth = 0;
        double _gainsAndLossesAtAll = 0;

        double _rechargeAtDay = 0;
        double _rechargeAtMonth = 0;
        double _rechargeAtAll = 0;

        double _withdrawalAtDay = 0;
        double _withdrawalAtMonth = 0;
        double _withdrawalAtAll = 0;

        double _transferAtDay = 0;
        double _transferAtMonth = 0;
        double _transferAtAll = 0;

        double _cashAtDay = 0;
        double _cashAtMonth = 0;
        double _cashAtAll = 0;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的首页的视图模型
        /// </summary>
        public IndexViewModel()
            : base("首页", "")
        {
            LoadData();
        }

        #endregion

        #region 公开属性

        /// <summary>
        /// 注册用户数（日）
        /// </summary>
        public double SetUpAtDay
        {
            get
            {
                return _setUpAtDay;
            }
            set
            {
                if (_setUpAtDay != value)
                {
                    _setUpAtDay = value;
                    OnPropertyChanged("SetUpAtDay");
                }
            }
        }

        /// <summary>
        /// 注册用户数（月）
        /// </summary>
        public double SetUpAtMonth
        {
            get
            {
                return _setUpAtMonth;
            }
            set
            {
                if (_setUpAtMonth != value)
                {
                    _setUpAtMonth = value;
                    OnPropertyChanged("SetUpAtMonth");
                }
            }
        }

        /// <summary>
        /// 注册用户数（总）
        /// </summary>
        public double SetUpAtAll
        {
            get
            {
                return _setUpAtAll;
            }
            set
            {
                if (_setUpAtAll != value)
                {
                    _setUpAtAll = value;
                    OnPropertyChanged("SetUpAtAll");
                }
            }
        }

        /// <summary>
        /// 登陆用户数（日）
        /// </summary>
        public double SetInAtDay
        {
            get
            {
                return _setInAtDay;
            }
            set
            {
                if (_setInAtDay != value)
                {
                    _setInAtDay = value;
                    OnPropertyChanged("SetInAtDay");
                }
            }
        }

        /// <summary>
        /// 登陆用户数（月）
        /// </summary>
        public double SetInAtMonth
        {
            get
            {
                return _setInAtMonth;
            }
            set
            {
                if (_setInAtMonth != value)
                {
                    _setInAtMonth = value;
                    OnPropertyChanged("SetInAtMonth");
                }
            }
        }

        /// <summary>
        /// 登陆用户数（总）
        /// </summary>
        public double SetInAtAll
        {
            get
            {
                return _setInAtAll;
            }
            set
            {
                if (_setInAtAll != value)
                {
                    _setInAtAll = value;
                    OnPropertyChanged("SetInAtAll");
                }
            }
        }

        /// <summary>
        /// 投注额（日）
        /// </summary>
        public double AmountOfBetsAtDay
        {
            get
            {
                return _amountOfBetsAtDay;
            }
            set
            {
                if (_amountOfBetsAtDay != value)
                {
                    _amountOfBetsAtDay = value;
                    OnPropertyChanged("AmountOfBetsAtDay");
                }
            }
        }

        /// <summary>
        /// 投注额（月）
        /// </summary>
        public double AmountOfBetsAtMonth
        {
            get
            {
                return _amountOfBetsAtMonth;
            }
            set
            {
                if (_amountOfBetsAtMonth != value)
                {
                    _amountOfBetsAtMonth = value;
                    OnPropertyChanged("AmountOfBetsAtMonth");
                }
            }
        }

        /// <summary>
        /// 投注额（总）
        /// </summary>
        public double AmountOfBetsAtAll
        {
            get
            {
                return _amountOfBetsAtAll;
            }
            set
            {
                if (_amountOfBetsAtAll != value)
                {
                    _amountOfBetsAtAll = value;
                    OnPropertyChanged("AmountOfBetsAtAll");
                }
            }
        }

        /// <summary>
        /// 返点（日）
        /// </summary>
        public double RebateAtDay
        {
            get
            {
                return _rebateAtDay;
            }
            set
            {
                if (_rebateAtDay != value)
                {
                    _rebateAtDay = value;
                    OnPropertyChanged("RebateAtDay");
                }
            }
        }

        /// <summary>
        /// 返点（月）
        /// </summary>
        public double RebateAtMonth
        {
            get
            {
                return _rebateAtMonth;
            }
            set
            {
                if (_rebateAtMonth != value)
                {
                    _rebateAtMonth = value;
                    OnPropertyChanged("RebateAtMonth");
                }
            }
        }

        /// <summary>
        /// 返点（总）
        /// </summary>
        public double RebateAtAll
        {
            get
            {
                return _rebateAtAll;
            }
            set
            {
                if (_rebateAtAll != value)
                {
                    _rebateAtAll = value;
                    OnPropertyChanged("RebateAtAll");
                }
            }
        }

        /// <summary>
        /// 奖金（日）
        /// </summary>
        public double BonusAtDay
        {
            get
            {
                return _bonusAtDay;
            }
            set
            {
                if (_bonusAtDay != value)
                {
                    _bonusAtDay = value;
                    OnPropertyChanged("BonusAtDay");
                }
            }
        }

        /// <summary>
        /// 奖金（月）
        /// </summary>
        public double BonusAtMonth
        {
            get
            {
                return _bonusAtMonth;
            }
            set
            {
                if (_bonusAtMonth != value)
                {
                    _bonusAtMonth = value;
                    OnPropertyChanged("BonusAtMonth");
                }
            }
        }

        /// <summary>
        /// 奖金（总）
        /// </summary>
        public double BonusAtAll
        {
            get
            {
                return _bonusAtAll;
            }
            set
            {
                if (_bonusAtAll != value)
                {
                    _bonusAtAll = value;
                    OnPropertyChanged("BonusAtAll");
                }
            }
        }

        /// <summary>
        /// 活动返还（日）
        /// </summary>
        public double ExpendituresAtDay
        {
            get
            {
                return _expendituresAtDay;
            }
            set
            {
                if (_expendituresAtDay != value)
                {
                    _expendituresAtDay = value;
                    OnPropertyChanged("ExpendituresAtDay");
                }
            }
        }

        /// <summary>
        /// 活动返还（月）
        /// </summary>
        public double ExpendituresAtMonth
        {
            get
            {
                return _expendituresAtMonth;
            }
            set
            {
                if (_expendituresAtMonth != value)
                {
                    _expendituresAtMonth = value;
                    OnPropertyChanged("ExpendituresAtMonth");
                }
            }
        }

        /// <summary>
        /// 活动返还（总）
        /// </summary>
        public double ExpendituresAtAll
        {
            get
            {
                return _expendituresAtAll;
            }
            set
            {
                if (_expendituresAtAll != value)
                {
                    _expendituresAtAll = value;
                    OnPropertyChanged("ExpendituresAtAll");
                }
            }
        }

        /// <summary>
        /// 盈亏（日）
        /// </summary>
        public double GainsAndLossesAtDay
        {
            get
            {
                return _gainsAndLossesAtDay;
            }
            set
            {
                if (_gainsAndLossesAtDay != value)
                {
                    _gainsAndLossesAtDay = value;
                    OnPropertyChanged("GainsAndLossesAtDay");
                }
            }
        }

        /// <summary>
        /// 盈亏（月）
        /// </summary>
        public double GainsAndLossesAtMonth
        {
            get
            {
                return _gainsAndLossesAtMonth;
            }
            set
            {
                if (_gainsAndLossesAtMonth != value)
                {
                    _gainsAndLossesAtMonth = value;
                    OnPropertyChanged("GainsAndLossesAtMonth");
                }
            }
        }

        /// <summary>
        /// 盈亏（总）
        /// </summary>
        public double GainsAndLossesAtAll
        {
            get
            {
                return _gainsAndLossesAtAll;
            }
            set
            {
                if (_gainsAndLossesAtAll != value)
                {
                    _gainsAndLossesAtAll = value;
                    OnPropertyChanged("GainsAndLossesAtAll");
                }
            }
        }

        /// <summary>
        /// 充值（日）
        /// </summary>
        public double RechargeAtDay
        {
            get
            {
                return _rechargeAtDay;
            }
            set
            {
                if (_rechargeAtDay != value)
                {
                    _rechargeAtDay = value;
                    OnPropertyChanged("RechargeAtDay");
                }
            }
        }

        /// <summary>
        /// 充值（月）
        /// </summary>
        public double RechargeAtMonth
        {
            get
            {
                return _rechargeAtMonth;
            }
            set
            {
                if (_rechargeAtMonth != value)
                {
                    _rechargeAtMonth = value;
                    OnPropertyChanged("RechargeAtMonth");
                }
            }
        }

        /// <summary>
        /// 充值（总）
        /// </summary>
        public double RechargeAtAll
        {
            get
            {
                return _rechargeAtAll;
            }
            set
            {
                if (_rechargeAtAll != value)
                {
                    _rechargeAtAll = value;
                    OnPropertyChanged("RechargeAtAll");
                }
            }
        }

        /// <summary>
        /// 提现（日）
        /// </summary>
        public double WithdrawalAtDay
        {
            get
            {
                return _withdrawalAtDay;
            }
            set
            {
                if (_withdrawalAtDay != value)
                {
                    _withdrawalAtDay = value;
                    OnPropertyChanged("WithdrawalAtDay");
                }
            }
        }

        /// <summary>
        /// 提现（月）
        /// </summary>
        public double WithdrawalAtMonth
        {
            get
            {
                return _withdrawalAtMonth;
            }
            set
            {
                if (_withdrawalAtMonth != value)
                {
                    _withdrawalAtMonth = value;
                    OnPropertyChanged("WithdrawalAtMonth");
                }
            }
        }

        /// <summary>
        /// 提现（总）
        /// </summary>
        public double WithdrawalAtAll
        {
            get
            {
                return _withdrawalAtAll;
            }
            set
            {
                if (_withdrawalAtAll != value)
                {
                    _withdrawalAtAll = value;
                    OnPropertyChanged("WithdrawalAtAll");
                }
            }
        }

        /// <summary>
        /// 支取（日）
        /// </summary>
        public double TransferAtDay
        {
            get
            {
                return _transferAtDay;
            }
            set
            {
                if (_transferAtDay != value)
                {
                    _transferAtDay = value;
                    OnPropertyChanged("TransferAtDay");
                }
            }
        }

        /// <summary>
        /// 支取（月）
        /// </summary>
        public double TransferAtMonth
        {
            get
            {
                return _transferAtMonth;
            }
            set
            {
                if (_transferAtMonth != value)
                {
                    _transferAtMonth = value;
                    OnPropertyChanged("TransferAtMonth");
                }
            }
        }

        /// <summary>
        /// 支取（总）
        /// </summary>
        public double TransferAtAll
        {
            get
            {
                return _transferAtAll;
            }
            set
            {
                if (_transferAtAll != value)
                {
                    _transferAtAll = value;
                    OnPropertyChanged("TransferAtAll");
                }
            }
        }

        /// <summary>
        /// 现金流（日）
        /// </summary>
        public double CashAtDay
        {
            get
            {
                return _cashAtDay;
            }
            set
            {
                if (_cashAtDay != value)
                {
                    _cashAtDay = value;
                    OnPropertyChanged("CashAtDay");
                }
            }
        }

        /// <summary>
        /// 现金流（月）
        /// </summary>
        public double CashAtMonth
        {
            get
            {
                return _cashAtMonth;
            }
            set
            {
                if (_cashAtMonth != value)
                {
                    _cashAtMonth = value;
                    OnPropertyChanged("CashAtMonth");
                }
            }
        }

        /// <summary>
        /// 现金流（总）
        /// </summary>
        public double CashAtAll
        {
            get
            {
                return _cashAtAll;
            }
            set
            {
                if (_cashAtAll != value)
                {
                    _cashAtAll = value;
                    OnPropertyChanged("CashAtAll");
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载数据
        /// </summary>
        void LoadData()
        {
            DataReportServiceClient client = new DataReportServiceClient();
            client.GetComprehensiveInformationCompleted += RefreshData;
            string dataKeyOfManagerInfo = DataKey.IWorld_ManagerInfo.ToString();
            LoginResult lr = (LoginResult)IsolatedStorageSettings.ApplicationSettings[dataKeyOfManagerInfo];
            client.GetComprehensiveInformationAsync(lr.Token);
        }
        #region 加载完毕时候执行

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RefreshData(object sender, GetComprehensiveInformationCompletedEventArgs e)
        {
            this.SetUpAtDay = e.Result.SetUpAtDay;
            this.SetUpAtMonth = e.Result.SetUpAtMonth;
            this.SetUpAtAll = e.Result.SetUpAtAll;

            this.SetInAtDay = e.Result.SetInAtDay;
            this.SetInAtMonth = e.Result.SetInAtMonth;
            this.SetInAtAll = e.Result.SetInAtAll;

            this.AmountOfBetsAtDay = e.Result.AmountOfBetsAtDay;
            this.AmountOfBetsAtMonth = e.Result.AmountOfBetsAtMonth;
            this.AmountOfBetsAtAll = e.Result.AmountOfBetsAtAll;

            this.RebateAtDay = e.Result.RebateAtDay;
            this.RebateAtMonth = e.Result.RebateAtMonth;
            this.RebateAtAll = e.Result.RebateAtAll;

            this.BonusAtDay = e.Result.BonusAtDay;
            this.BonusAtMonth = e.Result.BonusAtMonth;
            this.BonusAtAll = e.Result.BonusAtAll;

            this.ExpendituresAtDay = e.Result.ExpendituresAtDay;
            this.ExpendituresAtMonth = e.Result.ExpendituresAtMonth;
            this.ExpendituresAtAll = e.Result.ExpendituresAtAll;

            this.GainsAndLossesAtDay = e.Result.GainsAndLossesAtDay;
            this.GainsAndLossesAtMonth = e.Result.GainsAndLossesAtMonth;
            this.GainsAndLossesAtAll = e.Result.GainsAndLossesAtAll;

            this.RechargeAtDay = e.Result.RechargeAtDay;
            this.RechargeAtMonth = e.Result.RechargeAtMonth;
            this.RechargeAtAll = e.Result.RechargeAtAll;

            this.WithdrawalAtDay = e.Result.WithdrawalAtDay;
            this.WithdrawalAtMonth = e.Result.WithdrawalAtMonth;
            this.WithdrawalAtAll = e.Result.WithdrawalAtAll;

            this.TransferAtDay = e.Result.TransferAtDay;
            this.TransferAtMonth = e.Result.TransferAtMonth;
            this.TransferAtAll = e.Result.TransferAtAll;

            this.CashAtDay = e.Result.CashAtDay;
            this.CashAtMonth = e.Result.CashAtMonth;
            this.CashAtAll = e.Result.CashAtAll;
        }

        #endregion

        #endregion
    }
}
