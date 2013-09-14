
namespace IWorld.Model
{
    /// <summary>
    /// 支取记录
    /// </summary>
    public class TransferRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 用户
        /// </summary>
        public virtual Administrator Owner { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// 支取记录
        /// </summary>
        public string Remark { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的支取记录
        /// </summary>
        public TransferRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的支取记录
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="sum">金额</param>
        /// <param name="remark">支取记录</param>
        public TransferRecord(Administrator owner, double sum, string remark)
        {
            this.Owner = owner;
            this.Sum = sum;
            this.Remark = remark;
        }
        
        #endregion
    }
}
