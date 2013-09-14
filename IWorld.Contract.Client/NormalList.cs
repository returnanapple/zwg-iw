using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    [DataContract]
    public class NormalList<T> : OperateResult
        where T : class
    {
        /// <summary>
        /// 主体
        /// </summary>
        [DataMember]
        public List<T> Content { get; set; }

        /// <summary>
        /// 实例化一个新的列表（成功）
        /// </summary>
        /// <param name="content">主体</param>
        public NormalList(List<T> content)
        {
            this.Content = new List<T>();
            this.Content.AddRange(content);
        }

        /// <summary>
        /// 实例化一个新的列表（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public NormalList(string error)
            : base(error)
        {
        }
    }
}
