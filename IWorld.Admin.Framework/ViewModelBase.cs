using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IWorld.Admin.Framework
{
    /// <summary>
    /// 视图模型的基类
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region 私有参数

        bool isBusy = false;

        #endregion

        #region 公开属性

        /// <summary>
        /// 一个布尔值 标识线程正忙
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 当目标属性的值被改变的时候将触发的事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region 保护方法

        /// <summary>
        /// 触发目标属性被改变的事件
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
