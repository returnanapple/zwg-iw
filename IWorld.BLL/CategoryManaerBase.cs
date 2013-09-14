using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用于继承的类目相关的管理者对象的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CategoryManaerBase<T> : ManagerBase<T>, IManager<T>, ICategoryManager<T>
        where T : CategoryBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的类目相关的管理者对象的基类
        /// </summary>
        /// <param name="db"></param>
        public CategoryManaerBase(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 操作对象

        /// <summary>
        /// 将实例添加到数据库（警告：此重载已过时，请尽量不要使用）
        /// </summary>
        /// <param name="package">所要用于添加实例的数据集</param>
        /// <returns>返回被添加的实例的副本</returns>
        public override T Create(ICreatePackage<T> package)
        {
            if (!(package is ICreateCategoryPackagr<T>))
            {
                throw new Exception("用于新建类目相关实例的数据集必须实现 ICreateCategoryPackagr 接口");
            }
            ICreateCategoryPackagr<T> _package = package as ICreateCategoryPackagr<T>;

            return this.Create(_package);
        }

        /// <summary>
        /// 将实例添加到数据库
        /// </summary>
        /// <param name="package">所要用于添加实例的数据集</param>
        /// <returns>返回被添加的实例的副本</returns>
        public T Create(ICreateCategoryPackagr<T> package)
        {
            package.CheckData(db);//检查数据合法性
            var tSet = db.Set<T>();//获取实例操作对象
            T t = package.GetEntity(db);
            T f = package.GetParent(db);

            CreatingTouchOff(this, new NEventArgs(db, t));//触发前置事件
            if (f != null)
            {
                t.LeftKey = f.RightKey;
                t.RightKey = t.LeftKey + 1;//设置实例本身的左右键
                t.Layer = f.Layer + 1;
                t.Tree = f.Tree;

                /* 开始重做树的左右键 */
                tSet.Where(x => x.LeftKey > t.LeftKey
                    || x.RightKey >= t.LeftKey)
                    .Where(x => x.Tree == f.Tree)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x.LeftKey > t.LeftKey) { x.LeftKey += 2; }
                        if (x.RightKey >= t.LeftKey) { x.RightKey += 2; }
                    });
                /* 重做树的左右键完毕 */
            }
            tSet.Add(t);
            db.SaveChanges(); ;
            CreatedTouchOff(this, new NEventArgs(db, t));//触发后置事件

            return t;
        }

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="package">所要用于移除实例的数据集</param>
        public override void Remove(IRemovePackage<T> package)
        {
            package.CheckData(db);//检查数据合法性
            T t = package.GetEntity(db);

            RemovingTouchOff(this, new NEventArgs(db, t));//触发前置事件
            /* 开始删除类目树 */
            db.Set<T>().Where(x => x.LeftKey >= t.LeftKey
                && x.RightKey <= t.RightKey
                && x.Tree == t.Tree)
                .ToList()
                .ForEach(x =>
                {
                    db.Set<T>().Remove(x);
                });
            /* 删除类目树完毕 */
            /* 开始重做树的左右键 */
            int tNum = t.RightKey - t.LeftKey + 1;
            db.Set<T>().Where(x => x.RightKey > t.RightKey
                && x.Tree == t.Tree)
                .ToList()
                .ForEach(x =>
                {
                    if (x.LeftKey > t.LeftKey) { x.LeftKey -= tNum; }
                    x.RightKey -= tNum;
                });
            /* 重做树的左右键完毕 */
            db.SaveChanges();
            RemovedTouchOff(this, new NEventArgs(db, t));//触发后置事件
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取父类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回父类目的实例</returns>
        public T GetParent(CategoryBase entity)
        {
            return db.Set<T>().FirstOrDefault(x => x.LeftKey < entity.LeftKey
                && x.RightKey > entity.RightKey
                && x.Layer == entity.Layer - 1
                && x.Tree == entity.Tree);
        }

        /// <summary>
        /// 返回所有的上级类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回所有的上级类目的列表</returns>
        public List<T> GetElders(CategoryBase entity)
        {
            return db.Set<T>().Where(x => x.LeftKey < entity.LeftKey
                && x.RightKey > entity.RightKey
                && x.Tree == entity.Tree)
                .OrderBy(x => x.Layer)
                .ToList();
        }

        /// <summary>
        /// 获取子类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回子类目的列表</returns>
        public List<T> GetChildren(CategoryBase entity)
        {
            return db.Set<T>().Where(x => x.LeftKey > entity.LeftKey
                && x.RightKey < entity.RightKey
                && x.Layer == entity.Layer + 1
                && x.Tree == entity.Tree)
                .OrderBy(x => x.LeftKey)
                .ToList();
        }

        /// <summary>
        /// 获取所有子孙类目
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns></returns>
        public List<T> GetOffspring(CategoryBase entity)
        {
            return db.Set<T>().Where(x => x.LeftKey > entity.LeftKey
                && x.RightKey < entity.RightKey
                && x.Tree == entity.Tree)
                .OrderBy(x => x.LeftKey)
                .ToList();
        }

        /// <summary>
        /// 获取整个家族类目树
        /// </summary>
        /// <param name="entity">目标类目</param>
        /// <returns>返回家族树的列表</returns>
        public List<T> GetClan(CategoryBase entity)
        {
            return db.Set<T>().Where(x => (x.LeftKey < entity.LeftKey
                && x.RightKey > entity.RightKey
                && x.Tree == entity.Tree)
                || (x.LeftKey >= entity.LeftKey
                && x.RightKey <= entity.RightKey
                && x.Tree == entity.Tree))
                .OrderBy(x => x.LeftKey)
                .ToList();
        }

        #endregion
    }
}
