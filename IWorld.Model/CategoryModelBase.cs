using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    ///  类目相关的数据模型的基类
    /// </summary>
    public class CategoryModelBase : RecordingTimeModelBase
    {
        #region 属性

        /// <summary>
        /// 父祖节点
        /// </summary>
        public List<Relative> Relative { get; set; }

        /// <summary>
        /// 所从属的树
        /// </summary>
        public string Tree { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的树状结构的数据模型
        /// </summary>
        public CategoryModelBase()
        {
        }

        /// <summary>
        /// 实例化一个新的树状结构的数据模型
        /// </summary>
        /// <param name="relative">父祖节点</param>
        /// <param name="tree">所从属的树</param>
        public CategoryModelBase(List<Relative> relative, string tree = "")
            : base(DateTime.Now)
        {
            this.Relative = relative == null ? new List<Relative>() : relative;
            this.Tree = tree == "" ? Guid.NewGuid().ToString("N") : tree;
        }

        #endregion
    }
}
