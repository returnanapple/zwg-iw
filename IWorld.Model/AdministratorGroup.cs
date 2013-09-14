
namespace IWorld.Model
{
    /// <summary>
    /// 管理员用户组
    /// </summary>
    public class AdministratorGroup : ModelBase
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Grade { get; set; }

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

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的管理员用户组
        /// </summary>
        public AdministratorGroup()
        {
        }

        /// <summary>
        /// 实例化一个新的管理员用户组
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="grade">等级</param>
        /// <param name="canViewUsers">允许查看前台用户列表</param>
        /// <param name="canEditUsers">允许创建和修改用户信息</param>
        /// <param name="canViewTickets">允许查看彩票信息</param>
        /// <param name="canEditTickets">允许修改彩票信息</param>
        /// <param name="canViewActivities">允许查看活动信息</param>
        /// <param name="canEditActivities">允许修改活动信息</param>
        /// <param name="canSettingSite">允许查看和修改系统设置</param>
        /// <param name="canViewDataReports">允许查看数据报表</param>
        /// <param name="canViewAndAddFundsReports">允许查看并添加资金支取记录</param>
        /// <param name="canViewAndEditMessageBox">允许参看消息盒子</param>
        /// <param name="canViewAndEditManagers">允许查看和管理“管理员”以其用户组并查看相关登陆、操作信息</param>
        public AdministratorGroup(string name, int grade, bool canViewUsers, bool canEditUsers, bool canViewTickets, bool canEditTickets
            , bool canViewActivities, bool canEditActivities, bool canSettingSite, bool canViewDataReports, bool canViewAndAddFundsReports
            , bool canViewAndEditMessageBox, bool canViewAndEditManagers)
        {
            this.Name = name;
            this.Grade = grade;
            this.CanViewUsers = canViewUsers;
            this.CanEditUsers = canEditUsers;
            this.CanViewTickets = canViewTickets;
            this.CanEditTickets = canEditTickets;
            this.CanViewActivities = canViewActivities;
            this.CanEditActivities = canEditActivities;
            this.CanSettingSite = canSettingSite;
            this.CanViewDataReports = canViewDataReports;
            this.CanViewAndAddFundsReports = canViewAndAddFundsReports;
            this.CanViewAndEditMessageBox = canViewAndEditMessageBox;
            this.CanViewAndEditManagers = canViewAndEditManagers;
        }

        #endregion
    }
}
