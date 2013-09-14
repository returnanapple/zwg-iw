using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Client;

namespace IWorld.DAL
{
    /// <summary>
    /// 公告信息的阅读者对象（前台）
    /// </summary>
    public class ClientBulletinReader : ReaderBase
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的公告信息的阅读者对象（前台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public ClientBulletinReader(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 读取公告列表
        /// </summary>
        /// <returns>返回公告列表</returns>
        public NormalList<BulletinResult> ReadBulletins()
        {
            List<BulletinResult> tList = db.Set<Bulletin>()
                .Where(x => x.BeginTime < DateTime.Now
                    && x.Hide == false
                    && x.EndTime > DateTime.Now)
                    .OrderByDescending(x => x.CreatedTime)
                    .ToList()
                    .ConvertAll(x => new BulletinResult(x));

            return new NormalList<BulletinResult>(tList);
        }

        public NoticeResult ReadNotice(int userId)
        {
            bool hadNoticeNoReaded = db.Set<Notice>().Any(x => x.To.Id == userId && x.IsReaded == false);
            if (!hadNoticeNoReaded)
            {
                return null;
            }
            Notice notice = db.Set<Notice>()
                .OrderBy(x => x.CreatedTime)
                .FirstOrDefault(x => x.To.Id == userId && x.IsReaded == false);
            switch (notice.Type)
            {
                case NoticeType.提现反馈:
                    return new NoticeResult(notice);
                case NoticeType.充值反馈:
                    RechargeRecord rr = db.Set<RechargeRecord>().Find(notice.TargetId);
                    return new NoticeResult(notice, rr);
                case NoticeType.开奖提醒:
                    Betting betting = db.Set<Betting>().Find(notice.TargetId);
                    Lottery lottery = db.Set<Lottery>().FirstOrDefault(x => x.Phases == betting.Phases
                        && x.Ticket.Id == betting.HowToPlay.Tag.Ticket.Id);
                    return new NoticeResult(notice, betting, lottery);
                default:
                    return null;
            }
        }

        #endregion
    }
}
