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
    /// 管理员信息的阅读者对象（后台）
    /// </summary>
    public class AdminManagersReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的管理员信息的阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminManagersReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取管理员信息的分页数据
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="groupId">指定的用户组的存储指针</param>
        /// <param name="page">页码</param>
        /// <returns>返回管理员信息的分页数据</returns>
        public PaginationList<ManagerInfoResult> ReadManagerList(string keyword, int groupId, int page)
        {
            Expression<Func<Administrator, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Administrator, bool>> predicate2 = x => x.Id > 0;
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
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var aSet = db.Set<Administrator>();

            int tCount = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Count();
            List<ManagerInfoResult> tList = aSet
                .Where(predicate1)
                .Where(predicate2)
                .OrderByDescending(x => x.Group.Grade)
                .OrderBy(x => x.Username)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ManagerInfoResult(x));

            return new PaginationList<ManagerInfoResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取管理用户组的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <returns>返回管理用户组的分页列表</returns>
        public PaginationList<ManagerGroupResult> ReadGroupList(string keyword, int page)
        {
            Expression<Func<AdministratorGroup, bool>> predicate1 = x => x.Id > 0;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Name.Contains(kw));
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var agSet = db.Set<AdministratorGroup>();

            int tCount = agSet
                .Where(predicate1)
                .Count();
            List<ManagerGroupResult> tList = agSet
                .Where(predicate1)
                .OrderByDescending(x => x.Grade)
                .OrderBy(x => x.Name)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ManagerGroupResult(x));

            return new PaginationList<ManagerGroupResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取管理员登陆记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <returns>返回管理员登陆记录的分页列表</returns>
        public PaginationList<LandingRecordResult> ReadLandingRecordList(int userId, string beginTime, string endTime, int page)
        {
            Expression<Func<AdministratorLandingRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<AdministratorLandingRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<AdministratorLandingRecord, bool>> predicate3 = x => x.Id > 0;
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
            var alrSet = db.Set<AdministratorLandingRecord>();

            int tCount = alrSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<LandingRecordResult> tList = alrSet
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
        /// 读取管理记录的分页列表
        /// </summary>
        /// <param name="userId">用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <returns>返回管理记录的分页列表</returns>
        public PaginationList<OperatedRecordResult> ReadOperatedRecordList(int userId, string beginTime, string endTime, int page)
        {
            Expression<Func<OperateRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<OperateRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<OperateRecord, bool>> predicate3 = x => x.Id > 0;
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
            var prSet = db.Set<OperateRecord>();

            int tCount = prSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Count();
            List<OperatedRecordResult> tList = prSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new OperatedRecordResult(x));

            return new PaginationList<OperatedRecordResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
