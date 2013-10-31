using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IWorld.BLL
{
    /// <summary>
    /// 大白鲨游戏的投注记录的管理对象
    /// </summary>
    public class BettingOfJawManager : SimplifyManagerBase<BettingOfJaw>
    {
        #region 构造方法

        /// <summary>
        /// 大白鲨游戏的投注记录的管理对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public BettingOfJawManager(DbContext db)
            : base(db)
        {
        }

        #endregion
    }
}
