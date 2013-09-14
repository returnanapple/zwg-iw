
namespace IWorld.Model
{
    /// <summary>
    /// 操作记录
    /// </summary>
    public class OperateRecord : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 用户
        /// </summary>
        public virtual Administrator Owner { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Operated { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的操作记录
        /// </summary>
        public OperateRecord()
        {
        }

        /// <summary>
        /// 实例化一个新的操作记录
        /// </summary>
        /// <param name="owner">用户</param>
        /// <param name="operated">操作</param>
        public OperateRecord(Administrator owner, string operated)
        {
            this.Owner = owner;
            this.Operated = operated;
        }
        
        #endregion
    }
}
