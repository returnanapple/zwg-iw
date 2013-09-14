using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 树状的数据模型的基类
    /// </summary>
    public abstract class CategoryModelBase : SolidModel
    {
        #region 属性

        /// <summary>
        /// 父祖节点集
        /// </summary>
        public virtual List<Relative> Relatives { get; set; }

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
        /// 实例化一个新的树状的数据模型
        /// </summary>
        public CategoryModelBase()
        {
        }

        /// <summary>
        /// 实例化一个新的树状的数据模型
        /// </summary>
        /// <param name="relatives">祖族</param>
        /// <param name="tree">所从属的树</param>
        public CategoryModelBase(List<Relative> relatives, string tree = "")
        {
            if (relatives == null || relatives.Count == 0)
            {
                this.Relatives = new List<Relative>();
                this.Layer = 1;
            }
            else
            {
                this.Relatives = relatives;
                this.Layer = relatives.Max(x => x.NodeId) + 1;
            }
            this.Tree = tree == "" ? Guid.NewGuid().ToString("N") : tree;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 判断当前对象是否是目标对象的父祖节点
        /// </summary>
        /// <param name="beau">所要用于判断的对象</param>
        /// <returns>返回一个布尔值 标识当前对象是否是目标对象的父祖节点</returns>
        public bool IsAncestor(CategoryModelBase beau)
        {
            return beau.Relatives.Any(x => x.NodeId == this.Id)
                && this.Tree == beau.Tree;
        }

        /// <summary>
        /// 判断当前对象是否是目标对象的子孙节点
        /// </summary>
        /// <param name="beau">所要用于判断的对象</param>
        /// <returns>返回一个布尔值 标识当前对象是否是目标对象的子孙节点</returns>
        public bool IsOffspring(CategoryModelBase beau)
        {
            return beau.IsAncestor(this);
        }

        /// <summary>
        /// 判断当前对象是否是目标对象的父节点
        /// </summary>
        /// <param name="beau">所要用于判断的对象</param>
        /// <returns>返回一个布尔值 标识当前对象是否是目标对象的父节点</returns>
        public bool IsParent(CategoryModelBase beau)
        {
            return this.IsAncestor(beau)
                && this.Layer == beau.Layer - 1;
        }

        /// <summary>
        /// 判断当前对象是否是目标对象的子节点
        /// </summary>
        /// <param name="beau">所要用于判断的对象</param>
        /// <returns>返回一个布尔值 标识当前对象是否是目标对象的子节点</returns>
        public bool IsChild(CategoryModelBase beau)
        {
            return beau.IsParent(this);
        }

        #endregion
    }
}
