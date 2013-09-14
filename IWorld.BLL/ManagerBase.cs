using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用于继承的管理者对象的基类
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class ManagerBase<T> : IManager<T>
        where T : ModelBase
    {
        #region 保护字段

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbContext db;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理者对象的基类
        /// </summary>
        public ManagerBase(DbContext db)
        {
            this.db = db;
        }

        #endregion

        #region 保护方法

        /// <summary>
        /// 触发将实例添加到数据库前的事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        protected void CreatingTouchOff(object sender, NEventArgs e)
        {
            if (CreatingEventHandler != null)
            {
                CreatingEventHandler(sender, e);
            }
        }

        /// <summary>
        /// 触发将实例添加到数据库后的事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        protected void CreatedTouchOff(object sender, NEventArgs e)
        {
            if (CreatedEventHandler != null)
            {
                CreatedEventHandler(sender, e);
            }
        }

        /// <summary>
        /// 触发将实例从数据库中移除前的事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        protected void RemovingTouchOff(object sender, NEventArgs e)
        {
            if (RemovingEventHandler != null)
            {
                RemovingEventHandler(sender, e);
            }
        }

        /// <summary>
        /// 将实例从数据库中移除后的事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        protected void RemovedTouchOff(object sender, NEventArgs e)
        {
            if (RemovedEventHandler != null)
            {
                RemovedEventHandler(sender, e);
            }
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 将实例添加到数据库
        /// </summary>
        /// <param name="package">所要用于添加实例的数据集</param>
        /// <returns>返回被添加的实例的副本</returns>
        public virtual T Create(ICreatePackage<T> package)
        {
            package.CheckData(db);//验证数据
            T t = package.GetEntity(db);

            CreatingTouchOff(this, new NEventArgs(db, t));//触发前置事件
            db.Set<T>().Add(t);
            db.SaveChanges();
            CreatedTouchOff(this, new NEventArgs(db, t));//触发后置事件

            return t;
        }

        /// <summary>
        /// 将实例的改变更新到数据库
        /// </summary>
        /// <param name="package">所要用于修改实例的数据集</param>
        public virtual void Update(IUpdatePackage<T> package)
        {
            package.CheckData(db);//验证数据
            T t = package.GetEntity(db);

            Type entityType = typeof(T);
            Dictionary<string, object> properties = package.GetPropertieList();
            Dictionary<string, object> _properties = new Dictionary<string, object>();
            properties.Keys.ToList().ForEach(x =>
            {
                if (entityType.GetProperty(x).GetValue(t) != properties[x])
                {
                    entityType.GetProperty(x)
                        .SetValue(t, properties[x], null);
                    _properties.Add(x, properties[x]);
                }
            });
            t.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 将指定的实例从数据库中移除
        /// </summary>
        /// <param name="package">所要用于移除实例的数据集</param>
        public virtual void Remove(IRemovePackage<T> package)
        {
            package.CheckData(db);//验证数据
            T t = package.GetEntity(db);

            RemovingTouchOff(this, new NEventArgs(db, t));//触发前置事件
            db.Set<T>().Remove(t);
            db.SaveChanges();
            RemovedTouchOff(this, new NEventArgs(db, t));//触发后置事件
        }

        #endregion

        #region 有关事件

        /// <summary>
        /// 将实例添加到数据库前将触发的事件
        /// </summary>
        public static event NDelegate CreatingEventHandler;

        /// <summary>
        /// 将实例添加到数据库后将触发的事件
        /// </summary>
        public static event NDelegate CreatedEventHandler;

        /// <summary>
        /// 将实例从数据库中移除前将触发的事件
        /// </summary>
        public static event NDelegate RemovingEventHandler;

        /// <summary>
        /// 将实例从数据库中移除后将触发的事件
        /// </summary>
        public static event NDelegate RemovedEventHandler;

        #endregion
    }
}
