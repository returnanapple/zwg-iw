using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 数据采集状态
    /// </summary>
    [DataContract]
    public class CollectionStatusResult : OperateResult
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        [DataMember]
        public bool Running { get; set; }

        /// <summary>
        /// 实例化一个新的数据采集状态（成功）
        /// </summary>
        /// <param name="running">正在运行</param>
        public CollectionStatusResult(bool running)
        {
            this.Running = running;
        }

        /// <summary>
        /// 实例化一个新的数据采集状态（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public CollectionStatusResult(string error)
            : base(error)
        {
        }
    }
}
