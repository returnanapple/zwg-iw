using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 管理用户组的信息（后台）
    /// </summary>
    [DataContract]
    public class ManagerGroupResult
    {
        /// <summary>
        /// 存储指针
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

        /// <summary>
        /// 允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息
        /// </summary>
        [DataMember]
        public bool CanViewAndEditManagers { get; set; }

        /// <summary>
        /// 实例化一个新的管理用户组的信息（后台）
        /// </summary>
        /// <param name="group">管理员用户组</param>
        public ManagerGroupResult(AdministratorGroup group)
        {
            this.GroupId = group.Id;
            this.Name = group.Name;
            this.Grade = group.Grade;
            this.CanViewUsers = group.CanViewUsers;
            this.CanEditUsers = group.CanEditUsers;
            this.CanViewTickets = group.CanViewTickets;
            this.CanEditTickets = group.CanEditTickets;
            this.CanViewActivities = group.CanViewActivities;
            this.CanEditActivities = group.CanEditActivities;
            this.CanSettingSite = group.CanSettingSite;
            this.CanViewDataReports = group.CanViewDataReports;
            this.CanViewAndAddFundsReports = group.CanViewAndAddFundsReports;
            this.CanViewAndEditMessageBox = group.CanViewAndEditMessageBox;
            this.CanViewAndEditManagers = group.CanViewAndEditManagers;
        }
    }
}
