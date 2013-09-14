using System.Runtime.Serialization;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 用于修改管理员用户组信息的数据集
    /// </summary>
    [DataContract]
    public class EditManagerGroupImport
    {
        /// <summary>
        /// 目标用户组的存储指针
        /// </summary>
        [DataMember]
        public int GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [DataMember]
        public int Grade { get; set; }

        /// <summary>
        /// 允许查看前台用户列表
        /// </summary>
        [DataMember]
        public bool CanViewUsers { get; set; }

        /// <summary>
        /// 允许创建和修改用户信息
        /// </summary>
        [DataMember]
        public bool CanEditUsers { get; set; }

        /// <summary>
        /// 允许查看彩票信息
        /// </summary>
        [DataMember]
        public bool CanViewTickets { get; set; }

        /// <summary>
        /// 允许修改彩票信息
        /// </summary>
        [DataMember]
        public bool CanEditTickets { get; set; }

        /// <summary>
        /// 允许查看活动信息
        /// </summary>
        [DataMember]
        public bool CanViewActivities { get; set; }

        /// <summary>
        /// 允许修改活动信息
        /// </summary>
        [DataMember]
        public bool CanEditActivities { get; set; }

        /// <summary>
        /// 允许查看和修改系统设置
        /// </summary>
        [DataMember]
        public bool CanSettingSite { get; set; }

        /// <summary>
        /// 允许查看数据报表
        /// </summary>
        [DataMember]
        public bool CanViewDataReports { get; set; }

        /// <summary>
        /// 允许查看并添加资金支取记录
        /// </summary>
        [DataMember]
        public bool CanViewAndAddFundsReports { get; set; }

        /// <summary>
        /// 允许参看消息盒子
        /// </summary>
        [DataMember]
        public bool CanViewAndEditMessageBox { get; set; }
    }
}
