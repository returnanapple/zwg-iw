using System;
using System.Xml.Linq;
using System.IO;

namespace IWorld.Setting
{
    /// <summary>
    /// 用于继承的配置对象的基类
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class SettingBase<T> : XmlOperator
        where T : class ,new()
    {
        #region 保护字段

        /// <summary>
        /// 数据存储对象所存储的路径
        /// </summary>
        protected string path;

        #endregion

        #region 保护方法

        /// <summary>
        /// 创建新的配置文件
        /// </summary>
        /// <param name="path">所要放置新的配置文件的路径</param>
        protected virtual void SetFile(string path)
        {
            XElement _e = new XElement("root");
            _e.Save(path);
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的用于继承的配置对象的基类
        /// </summary>
        public SettingBase()
        {
            CheckDirectory();
            this.path = string.Format("{0}/Content/Xml/{1}.xml", AppDomain.CurrentDomain.BaseDirectory, typeof(T).Name);
            if (!File.Exists(this.path))
            {
                this.SetFile(this.path);
            }
            this.e = XElement.Load(this.path);
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 保存对数据集的修改
        /// </summary>
        public virtual void Save()
        {
            e.Save(path);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 检查用于存储配置文件的文件夹是否存储（如果不存在则创建文件夹）
        /// </summary>
        private void CheckDirectory()
        {
            string _path = string.Format("{0}/Content", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            _path = string.Format("{0}/Content/Xml", AppDomain.CurrentDomain.BaseDirectory);
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        #endregion
    }
}
