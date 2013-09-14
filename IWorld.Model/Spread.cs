using System;

namespace IWorld.Model
{
    /// <summary>
    /// 推广记录
    /// </summary>
    public class Spread : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 推广人
        /// </summary>
        public virtual Author Owner { get; set; }

        /// <summary>
        /// 标识码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 正常返点数
        /// </summary>
        public double NormalReturnPoints { get; set; }

        /// <summary>
        /// 不定位返点数
        /// </summary>
        public double UncertainReturnPoints { get; set; }

        /// <summary>
        /// 已经被使用
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的推广记录
        /// </summary>
        public Spread()
        {
        }

        /// <summary>
        /// 实例化一个新的推广记录
        /// </summary>
        /// <param name="owner">推广人</param>
        /// <param name="normalReturnPoints">正常返点数</param>
        /// <param name="uncertainReturnPoints">不定位返点数</param>
        /// <param name="expiredTime">过期时间</param>
        public Spread(Author owner, double normalReturnPoints, double uncertainReturnPoints, DateTime expiredTime)
        {
            this.Owner = owner;
            this.Code = Guid.NewGuid().ToString("N");
            this.NormalReturnPoints = normalReturnPoints;
            this.UncertainReturnPoints = uncertainReturnPoints;
            this.Used = false;
            this.ExpiredTime = expiredTime;
        }

        #endregion
    }
}
