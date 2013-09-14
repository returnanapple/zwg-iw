using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 树状结构的父祖亲族
    /// </summary>
    public class Relative : ModelBase
    {
        #region 属性

        /// <summary>
        /// 目标节点的存储指针
        /// </summary>
        public long NodeId { get; set; }

        /// <summary>
        /// 节点层级
        /// </summary>
        public int NodeLayer { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的树状结构的父祖亲族
        /// </summary>
        public Relative()
        {
        }

        /// <summary>
        /// 实例化一个新的树状结构的父祖亲族
        /// </summary>
        /// <param name="beau">所要用于生产亲族信息的树状的数据模型对象</param>
        public Relative(CategoryModelBase beau)
        {
            this.NodeId = beau.Id;
            this.NodeLayer = beau.Layer;
        }

        #endregion
    }
}
