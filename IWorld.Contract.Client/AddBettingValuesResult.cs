using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 添加临时彩票位的操作结果
    /// </summary>
    [DataContract]
    public class AddBettingValuesResult : OperateResult
    {
        /// <summary>
        /// 标识码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        public AddBettingValuesResult()
        {
        }

        public AddBettingValuesResult(string error)
            : base(error)
        {
        }
    }
}
