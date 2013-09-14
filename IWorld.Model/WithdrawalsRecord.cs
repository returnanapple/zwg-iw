
namespace IWorld.Model
{
    /// <summary>
    /// 提现记录
    /// </summary>
    public class WithdrawalsRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// 目标银行卡的卡号
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 目标银行卡的开户人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 目标银行卡的开户银行
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public WithdrawalsStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的提现记录
        /// </summary>
        public WithdrawalsRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的提现记录
        /// </summary>
        /// <param name="owner">申请人</param>
        /// <param name="sum">金额</param>
        /// <param name="card">目标银行卡的卡号</param>
        /// <param name="name">目标银行卡的开户人姓名</param>
        /// <param name="bank">目标银行卡的开户银行</param>
        public WithdrawalsRecord(Author owner, double sum, string card, string name, Bank bank)
        {
            this.Owner = owner;
            this.Sum = sum;
            this.Card = card;
            this.Name = name;
            this.Bank = bank;
            this.Status = WithdrawalsStatus.处理中;
            this.Remark = "";
        }

        #endregion
    }
}
