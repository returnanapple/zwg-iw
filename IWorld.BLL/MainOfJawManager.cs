using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IWorld.BLL
{
    /// <summary>
    /// 大白鲨游戏的主要信息的管理者对性
    /// </summary>
    public class MainOfJawManager : SimplifyManagerBase<MainOfJaw>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的大白鲨游戏的主要信息的管理者对性
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public MainOfJawManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 方法

        public void ChangeNextPhases(int mainOfJawId, string newNextPhases)
        {
            NChecker.CheckEntity<MainOfJaw>(mainOfJawId, "大白鲨游戏的主要信息", db);
            MainOfJaw moj = db.Set<MainOfJaw>().Find(mainOfJawId);
            moj.NextPhases = newNextPhases;
            db.SaveChanges();
        }

        #endregion
    }
}
