using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    [DataContract]
    public class CreateUserImport
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 正常返点数
        /// </summary>
        [DataMember]
        public double NormalReturnPoints { get; set; }

        /// <summary>
        /// 不定位返点数
        /// </summary>
        [DataMember]
        public double UncertainReturnPoints { get; set; }

        /// <summary>
        /// 直属下级配额
        /// </summary>
        [DataMember]
        public int Quota { get; set; }
    }
}
