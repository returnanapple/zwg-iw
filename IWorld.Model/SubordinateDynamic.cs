
namespace IWorld.Model
{
    /// <summary>
    /// 下级用户的动态
    /// </summary>
    public class SubordinateDynamic : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 谁
        /// </summary>
        public virtual Author From { get; set; }

        /// <summary>
        /// 做了什么
        /// </summary>
        public string Done { get; set; }

        /// <summary>
        /// 给谁
        /// </summary>
        public virtual Author To { get; set; }

        /// <summary>
        /// 给予了多少
        /// </summary>
        public double Give { get; set; }

        /// <summary>
        /// 货币单位
        /// </summary>
        public Currency Currency { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的下级用户的动态
        /// </summary>
        public SubordinateDynamic()
        {
        }

        /// <summary>
        /// 实例化一个新的下级用户的动态
        /// </summary>
        /// <param name="_from">谁</param>
        /// <param name="done">做了什么</param>
        /// <param name="_to">给谁</param>
        /// <param name="give">给予了多少</param>
        /// <param name="currency">货币单位</param>
        public SubordinateDynamic(Author _from, string done, Author _to, double give, Currency currency)
        {
            this.From = _from;
            this.Done = done;
            this.To = _to;
            this.Give = give;
            this.Currency = currency;
        }

        #endregion
    }
}
