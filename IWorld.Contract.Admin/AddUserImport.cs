using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于新建用户的数据集
    /// </summary>
    [DataContract]
    public class AddUserImport
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// 代理（拥有创建下级用户的权限）
        /// </summary>
        [DataMember]
        public bool IsAgents { get; set; }

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
        /// 最多拥有直属下级数量限制
        /// </summary>
        [DataMember]
        public int MaxOfSubordinate { get; set; }
    }
}
