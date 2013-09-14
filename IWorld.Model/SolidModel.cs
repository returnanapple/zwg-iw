using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 实体数据模型的基类
    /// </summary>
    public abstract class SolidModel : ModelBase
    {
        #region 属性

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
        /// 实例化一个新的实体数据模型
        /// </summary>
        public SolidModel()
        {
            this.CreatedTime = DateTime.Now;
            this.ModifiedTime = DateTime.Now;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 声明数据模型已经被修改
        /// </summary>
        public void OnModify()
        {
            this.ModifiedTime = DateTime.Now;
        }

        #endregion
    }
}
