using System.Xml.Linq;

namespace IWorld.Setting
{
    /// <summary>
    /// 综合信息统计
    /// </summary>
    public sealed class ComprehensiveInformation : SettingBase<ComprehensiveInformation>
    {
        #region 保护方法

        /// <summary>
        /// 创建新的配置文件
        /// </summary>
        /// <param name="path">所要放置新的配置文件的路径</param>
        protected override void SetFile(string path)
        {
            XElement _e = new XElement("Websetting"
                , new XElement("CountOfSetUp", 0)
                , new XElement("AmountOfBets", 0)
                , new XElement("ReturnPoints", 0)
                , new XElement("Bonus", 0)
                , new XElement("Expenditures", 0)
                , new XElement("Recharge", 0)
                , new XElement("Withdrawal", 0)
                , new XElement("Transfer", 0));
            _e.Save(path);
        }

        #endregion

        #region 公开属性

        /// <summary>
        /// 注册人数
        /// </summary>
        public int CountOfSetUp
        {
            get { return GetIntValue("CountOfSetUp", 0); }
            set { SetValue("CountOfSetUp", value); }
        }

        /// <summary>
        /// 投注额
        /// </summary>
        public double AmountOfBets
        {
            get { return GetDoubleValue("AmountOfBets", 0); }
            set { SetValue("AmountOfBets", value); }
        }

        /// <summary>
        /// 返点
        /// </summary>
        public double ReturnPoints
        {
            get { return GetDoubleValue("ReturnPoints", 0); }
            set { SetValue("ReturnPoints", value); }
        }

        /// <summary>
        /// 奖金
        /// </summary>
        public double Bonus
        {
            get { return GetDoubleValue("Bonus", 0); }
            set { SetValue("Bonus", value); }
        }

        /// <summary>
        /// 活动返还
        /// </summary>
        public double Expenditures
        {
            get { return GetDoubleValue("Expenditures", 0); }
            set { SetValue("Expenditures", value); }
        }

        /// <summary>
        /// 盈亏
        /// </summary>
        public double GainsAndLosses
        {
            get { return this.AmountOfBets - this.ReturnPoints - this.Bonus - this.Expenditures; }
        }

        /// <summary>
        /// 充值
        /// </summary>
        public double Recharge
        {
            get { return GetDoubleValue("Recharge", 0); }
            set { SetValue("Recharge", value); }
        }

        /// <summary>
        /// 提现
        /// </summary>
        public double Withdrawal
        {
            get { return GetDoubleValue("Withdrawal", 0); }
            set { SetValue("Withdrawal", value); }
        }

        /// <summary>
        /// 支取
        /// </summary>
        public double Transfer
        {
            get { return GetDoubleValue("Transfer", 0); }
            set { SetValue("Transfer", value); }
        }

        /// <summary>
        /// 现金流
        /// </summary>
        public double Cash
        {
            get { return this.Recharge - this.Withdrawal - this.Transfer; }
        }

        #endregion
    }
}
