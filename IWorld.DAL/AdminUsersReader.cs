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
    /// 用户信息的阅读者对象（后台）
    /// </summary>
    public class AdminUsersReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的用户信息的阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminUsersReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取用户信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="groupId">目标用户组的存储指针</param>
        /// <param name="teamForUser">目标用户的存储指针（指向该用户的团队）</param>
        /// <param name="page">页码</param>
        /// <returns>返回用户信息的分页列表</returns>
        public PaginationList<UserInfoResult> ReadUserList(string keyword, int groupId, int teamForUser, int page)
        {
            Expression<Func<Author, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Author, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Author, bool>> predicate3 = x => x.Id > 0;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Username.Contains(kw));
            }
            if (groupId > 0)
            {
                predicate2 = x => x.Group.Id == groupId;
            }
            var aSet = db.Set<Author>();
            if (teamForUser > 0)
            {
                Author tUser = aSet.Find(teamForUser);
                predicate3 = x => x.LeftKey >= tUser.LeftKey && x.RightKey <= tUser.RightKey && x.Tree == tUser.Tree;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);

            int tCount = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<UserInfoResult> tList = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderBy(x => x.Username)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new UserInfoResult(x));

            return new PaginationList<UserInfoResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取用户登录记录的分页列表
        /// </summary>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间（xxxx-xx-xx 格式）</param>
        /// <param name="endTime">结束时间（xxxx-xx-xx 格式）</param>
        /// <param name="page">页码</param>
        /// <returns>返回用户登录记录的分页列表</returns>
        public PaginationList<LandingRecordResult> ReadLandingRecordList(int userId, string beginTime, string endTime, int page)
        {
            Expression<Func<UserLandingRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<UserLandingRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<UserLandingRecord, bool>> predicate3 = x => x.Id > 0;
            if (userId != 0)
            {
                predicate1 = x => x.Owner.Id == userId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate2 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime <= tTime;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var ulrSet = db.Set<UserLandingRecord>();

            int tCount = ulrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<LandingRecordResult> tList = ulrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new LandingRecordResult(x));

            return new PaginationList<LandingRecordResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取用户组信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <returns>返回用户组信息的分页列表</returns>
        public PaginationList<UserGroupResult> ReadGroupList(string keyword, int page)
        {
            Expression<Func<UserGroup, bool>> predicate1 = x => x.Id > 0;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Name.Contains(kw));
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var ugSet = db.Set<UserGroup>();

            int tCount = ugSet
                .Where(predicate1)
                .Count();
            List<UserGroupResult> tList = ugSet
                .Where(predicate1)
                .OrderBy(x => x.Name)
                .OrderBy(x => x.Grade)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new UserGroupResult(x));

            return new PaginationList<UserGroupResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
