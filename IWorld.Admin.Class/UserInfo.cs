using System;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoCaChe
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 所属用户组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 所属用户组等级
        /// </summary>
        public int GroupGrade { get; set; }

        /// <summary>
        /// 允许查看前台用户列表
        /// </summary>
        public bool CanViewUsers { get; set; }

        /// <summary>
        /// 允许创建和修改用户信息
        /// </summary>
        public bool CanEditUsers { get; set; }

        /// <summary>
        /// 允许查看彩票信息
        /// </summary>
        public bool CanViewTickets { get; set; }

        /// <summary>
        /// 允许修改彩票信息
        /// </summary>
        public bool CanEditTickets { get; set; }

        /// <summary>
        /// 允许查看活动信息
        /// </summary>
        public bool CanViewActivities { get; set; }

        /// <summary>
        /// 允许修改活动信息
        /// </summary>
        public bool CanEditActivities { get; set; }

        /// <summary>
        /// 允许查看和修改系统设置
        /// </summary>
        public bool CanSettingSite { get; set; }

        /// <summary>
        /// 允许查看数据报表
        /// </summary>
        public bool CanViewDataReports { get; set; }

        /// <summary>
        /// 允许查看并添加资金支取记录
        /// </summary>
        public bool CanViewAndAddFundsReports { get; set; }

        /// <summary>
        /// 允许参看消息盒子
        /// </summary>
        public bool CanViewAndEditMessageBox { get; set; }

        /// <summary>
        /// 允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息
        /// </summary>
        public bool CanViewAndEditManagers { get; set; }
    }
}
