using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 用户登陆记录的管理者对象
    /// </summary>
    public class UserLandingRecordManager
    {
        /// <summary>
        /// 创建用户登陆记录
        /// </summary>
        /// <param name="sender">监视对象</param>
        /// <param name="e">触发对象</param>
        public static void CreateLandingRecord(object sender, AuthorManager.LoginEventArgs e)
        {
            Author user = (Author)e.State;
            UserLandingRecord landingRecord = new UserLandingRecord(user, e.LoginIp);
            e.Db.Set<UserLandingRecord>().Add(landingRecord);
            e.Db.SaveChanges();
        }
    }
}
