using System;

namespace IWorld.Model
{
    /// <summary>
    /// 用于继承的类目相关的模型基础类
    /// </summary>
    public abstract class CategoryBase : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 左键
        /// </summary>
        public int LeftKey { get; set; }

        /// <summary>
        /// 右键
        /// </summary>
        public int RightKey { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 所从属的树
        /// </summary>
        public string Tree { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的类目相关的模型
        /// </summary>
        public CategoryBase()
        {
            this.LeftKey = 1;
            this.RightKey = 2;
            this.Layer = 1;
            this.Tree = Guid.NewGuid().ToString("N");
        }

        #endregion
    }
}
