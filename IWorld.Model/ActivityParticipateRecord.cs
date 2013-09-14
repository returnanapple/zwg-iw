
namespace IWorld.Model
{
    /// <summary>
    /// 默认活动的参与记录
    /// </summary>
    public class ActivityParticipateRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 参与人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 参与的活动
        /// </summary>
        public virtual Activity Activity { get; set; }

        /// <summary>
        /// 目标对象的数额
        /// </summary>
        public double Amount { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认活动的参与记录
        /// </summary>
        public ActivityParticipateRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的默认活动的参与记录
        /// </summary>
        /// <param name="owner">参与人</param>
        /// <param name="activity">参与的活动</param>
        /// <param name="amount">目标对象的数额</param>
        public ActivityParticipateRecord(Author owner, Activity activity, double amount)
        {
            this.Owner = owner;
            this.Activity = activity;
            this.Amount = amount;
        }
        
        #endregion
    }
}
