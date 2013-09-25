using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IWorld.Model;
using IWorld.Setting;

namespace IWorld.BLL
{
    public class UserLoginToken : ModelBase
    {
        public string Code { get; set; }

        public int UserId { get; set; }

        public DateTime ExpiredTime { get; set; }

        public UserLoginToken()
        {
        }

        public UserLoginToken(int userId)
        {
            this.Code = Guid.NewGuid().ToString("N");
            this.UserId = userId;
            this.ExpiredTime = DateTime.Now.AddMinutes(new WebSetting().UserInTime);
        }
    }
}
