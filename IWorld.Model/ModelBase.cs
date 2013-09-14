using System;

namespace IWorld.Model
{
    /// <summary>
    /// 用于继承的模型基础类
    /// </summary>
    public abstract class ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 存储指针
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的模型
        /// </summary>
        public ModelBase()
        {
            this.CreatedTime = DateTime.Now;
            this.ModifiedTime = DateTime.Now;
        }

        #endregion
    }
}
