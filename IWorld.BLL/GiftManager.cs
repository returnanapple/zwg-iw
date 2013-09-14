using System;
using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 实体奖品赠送记录的管理者对象
    /// </summary>
    public class GiftManager
    {
        #region 保护字段

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbContext db;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的实体奖品赠送记录的管理者对象
        /// </summary>
        public GiftManager(DbContext db)
        {
            this.db = db;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 将奖品标记为”已赠送“
        /// </summary>
        /// <param name="giftId">目标奖品的存储指针</param>
        public void Hndsel(int giftId)
        {
            NChecker.CheckEntity<GiftRecord>(giftId, "奖品", db);
            GiftRecord gift = db.Set<GiftRecord>().Find(giftId);
            if (gift.Status == GiftStatus.已赠送)
            {
                throw new Exception("该奖品已经赠送");
            }

            gift.Status = GiftStatus.已赠送;
            gift.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        #endregion
    }
}
