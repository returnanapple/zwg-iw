using System;

namespace IWorld.Model
{
    /// <summary>
    /// 充值记录
    /// </summary>
    public class RechargeRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 目标账户
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 支付人
        /// </summary>
        public virtual Author Payer { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// 来源卡号（交易成功前为空白）
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 来源卡的开户人（交易成功前为空白）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 来源卡的开户银行（交易成功前为空白）
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public RechargeStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 标识码
        /// </summary>
        public string Code { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的充值记录
        /// </summary>
        public RechargeRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的充值记录
        /// </summary>
        /// <param name="owner">目标账户</param>
        /// <param name="payer">支付人</param>
        /// <param name="sum">金额</param>
        public RechargeRecord(Author owner, Author payer, double sum)
        {
            this.Owner = owner;
            this.Payer = payer;
            this.Sum = sum;
            this.Card = "";
            this.Name = "";
            this.Bank = Model.Bank.无;
            this.Status = RechargeStatus.等待支付;
            this.Remark = "";
            this.Code = Guid.NewGuid().ToString("N");
        }

        #endregion
    }
}
