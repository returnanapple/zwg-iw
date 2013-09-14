using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 管理员登陆记录的管理者对象
    /// </summary>
    public class AdministratorLandingRecordManager
    {
        /// <summary>
        /// 创建管理员登陆记录
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public static void CreateLandingRecord(object sender, AdministratorManager.LoginEventArgs e)
        {
            Administrator administrator = (Administrator)e.State;
            AdministratorLandingRecord landingRecord = new AdministratorLandingRecord(administrator, e.LoginIp);
            e.Db.Set<AdministratorLandingRecord>().Add(landingRecord);
            e.Db.SaveChanges();
        }
    }
}
