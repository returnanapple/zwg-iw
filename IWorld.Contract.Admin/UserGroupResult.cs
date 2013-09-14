using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    [DataContract]
    public class UserGroupResult
    {
        /// <summary>
        /// 用户组的存储指针
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [DataMember]
        public int Grade { get; set; }

        /// <summary>
        /// 名称的显示颜色（如为空则显示系统默认颜色）
        /// </summary>
        [DataMember]
        public string ColorOfName { get; set; }

        /// <summary>
        /// 消费量下限
        /// </summary>
        [DataMember]
        public double LimitOfConsumption { get; set; }

        /// <summary>
        /// 消费量上限
        /// </summary>
        [DataMember]
        public double UpperOfConsumption { get; set; }

        /// <summary>
        /// 每日允许提现次数（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public int Withdrawals { get; set; }

        /// <summary>
        /// 单笔最低取款金额（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double MinimumWithdrawalAmount { get; set; }

        /// <summary>
        /// 单笔最高取款金额（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double MaximumWithdrawalAmount { get; set; }

        /// <summary>
        /// 最小充值额度（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double MinimumRechargeAmount { get; set; }

        /// <summary>
        /// 最大充值额度（如为0则采用系统参数）
        /// </summary>
        [DataMember]
        public double MaximumRechargeAmount { get; set; }

        /// <summary>
        /// 随时提现
        /// </summary>
        [DataMember]
        public bool WithdrawalsAtAnyTime { get; set; }

        /// <summary>
        /// 最多拥有直属下级数量限制
        /// </summary>
        [DataMember]
        public int MaxOfSubordinate { get; set; }

        /// <summary>
        /// 实例化一个新的用户组信息
        /// </summary>
        /// <param name="group">用户组信息数据封装</param>
        public UserGroupResult(UserGroup group)
        {
            this.GroupId = group.Id;
            this.Name = group.Name;
            this.Grade = group.Grade;
            this.ColorOfName = group.ColorOfName;
            this.LimitOfConsumption = group.LimitOfConsumption;
            this.UpperOfConsumption = group.UpperOfConsumption;
            this.Withdrawals = group.Withdrawals;
            this.MinimumWithdrawalAmount = group.MinimumWithdrawalAmount;
            this.MaximumWithdrawalAmount = group.MaximumWithdrawalAmount;
            this.MinimumRechargeAmount = group.MinimumRechargeAmount;
            this.MaximumRechargeAmount = group.MaximumRechargeAmount;
            this.WithdrawalsAtAnyTime = group.WithdrawalsAtAnyTime;
            this.MaxOfSubordinate = group.MaxOfSubordinate;
        }
    }
}
