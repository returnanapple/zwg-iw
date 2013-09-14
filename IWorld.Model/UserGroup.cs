
namespace IWorld.Model
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class UserGroup : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 名称的显示颜色（如为空则显示系统默认颜色）
        /// </summary>
        public string ColorOfName { get; set; }

        /// <summary>
        /// 消费量下限
        /// </summary>
        public double LimitOfConsumption { get; set; }

        /// <summary>
        /// 消费量上限
        /// </summary>
        public double UpperOfConsumption { get; set; }

        /// <summary>
        /// 每日允许提现次数（如为0则采用系统参数）
        /// </summary>
        public int Withdrawals { get; set; }

        /// <summary>
        /// 单笔最低取款金额（如为0则采用系统参数）
        /// </summary>
        public double MinimumWithdrawalAmount { get; set; }

        /// <summary>
        /// 单笔最高取款金额（如为0则采用系统参数）
        /// </summary>
        public double MaximumWithdrawalAmount { get; set; }

        /// <summary>
        /// 最小充值额度（如为0则采用系统参数）
        /// </summary>
        public double MinimumRechargeAmount { get; set; }

        /// <summary>
        /// 最大充值额度（如为0则采用系统参数）
        /// </summary>
        public double MaximumRechargeAmount { get; set; }

        /// <summary>
        /// 随时提现
        /// </summary>
        public bool WithdrawalsAtAnyTime { get; set; }

        /// <summary>
        /// 最多拥有直属下级数量限制
        /// </summary>
        public int MaxOfSubordinate { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户组
        /// </summary>
        public UserGroup()
        {
        }

        /// <summary>
        /// 实例化一个新的用户组
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="colorOfName">名称的显示颜色</param>
        /// <param name="grade">等级</param>
        /// <param name="limitOfConsumption">消费量下限</param>
        /// <param name="upperOfConsumption">消费量上限</param>
        /// <param name="withdrawals">每日允许提现次数</param>
        /// <param name="minimumWithdrawalAmount">单笔最低取款金额</param>
        /// <param name="maximumWithdrawalAmount">单笔最高取款金额</param>
        /// <param name="minimumRechargeAmount">最小充值额度</param>
        /// <param name="maximumRechargeAmount">最大充值额度</param>
        /// <param name="withdrawalsAtAnyTime">随时提现</param>
        /// <param name="maxOfSubordinate">最多拥有直属下级数量限制</param>
        public UserGroup(string name, string colorOfName, int grade, double limitOfConsumption, double upperOfConsumption
            , int withdrawals, double minimumWithdrawalAmount, double maximumWithdrawalAmount, double minimumRechargeAmount
            , double maximumRechargeAmount, bool withdrawalsAtAnyTime, int maxOfSubordinate)
        {
            this.Name = name;
            this.Grade = grade;
            this.ColorOfName = colorOfName;
            this.LimitOfConsumption = limitOfConsumption;
            this.UpperOfConsumption = upperOfConsumption;
            this.Withdrawals = withdrawals;
            this.MinimumWithdrawalAmount = minimumWithdrawalAmount;
            this.MaximumWithdrawalAmount = maximumWithdrawalAmount;
            this.MinimumRechargeAmount = minimumRechargeAmount;
            this.MaximumRechargeAmount = maximumRechargeAmount;
            this.WithdrawalsAtAnyTime = withdrawalsAtAnyTime;
            this.MaxOfSubordinate = maxOfSubordinate;
        }

        #endregion
    }
}
