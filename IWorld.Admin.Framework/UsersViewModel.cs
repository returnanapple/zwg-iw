using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IWorld.Admin.Framework
{
    public class UsersViewModel : ManagerViewModelBase
    {
        #region 构造方法

        public UsersViewModel()
            : base("用户管理", "查看用户列表")
        {
            
        }

        #endregion
    }
}
