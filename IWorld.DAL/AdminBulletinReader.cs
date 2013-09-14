using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Admin;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 公告信息的阅读者对象（后台）
    /// </summary>
    public class AdminBulletinReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的公告信息的阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminBulletinReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取公告的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回公告的分页列表</returns>
        public PaginationList<BulletinResult> ReadBulletinList(string keyword, RegularlyStatusSelectType status, int page)
        {
            Expression<Func<Bulletin, bool>> predicate1 = bulletin => bulletin.Id > 0;
            Expression<Func<Bulletin, bool>> predicate2 = bulletin => bulletin.Id > 0;
            Expression<Func<Bulletin, bool>> predicate3 = bulletin => bulletin.AutoDelete == false
                || bulletin.EndTime >= DateTime.Now;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = bulletin => kws.All(kw => bulletin.Title.Contains(kw));
            }
            switch (status)
            {
                case RegularlyStatusSelectType.未过期:
                    predicate2 = bulletin => bulletin.EndTime > DateTime.Now;
                    break;
                case RegularlyStatusSelectType.正常:
                    predicate2 = bulletin => bulletin.EndTime > DateTime.Now && bulletin.Hide == false;
                    break;
                case RegularlyStatusSelectType.暂停:
                    predicate2 = bulletin => bulletin.EndTime > DateTime.Now && bulletin.Hide == true;
                    break;
                case RegularlyStatusSelectType.已过期:
                    predicate2 = bulletin => bulletin.EndTime <= DateTime.Now;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var bSet = db.Set<Bulletin>();

            int tCount = bSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<BulletinResult> tList = bSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(bulletin => bulletin.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(bulletin => new BulletinResult(bulletin));

            return new PaginationList<BulletinResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
