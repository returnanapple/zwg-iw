using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    [DataContract]
    public class UserGroupResult
    {
        #region 公开属性

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用户组信息
        /// </summary>
        /// <param name="userGroup">用户组信息的数据封装</param>
        public UserGroupResult(UserGroup userGroup)
        {
            this.Name = userGroup.Name;
            this.Grade = userGroup.Grade;
            this.ColorOfName = userGroup.ColorOfName;
            this.Withdrawals = userGroup.Withdrawals;
            this.MinimumWithdrawalAmount = userGroup.MinimumWithdrawalAmount;
            this.MaximumWithdrawalAmount = userGroup.MaximumWithdrawalAmount;
            this.MinimumRechargeAmount = userGroup.MinimumRechargeAmount;
            this.MaximumRechargeAmount = userGroup.MaximumRechargeAmount;
            this.WithdrawalsAtAnyTime = userGroup.WithdrawalsAtAnyTime;
        }

        #endregion
    }
}
