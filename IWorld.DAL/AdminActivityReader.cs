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
    /// 默认活动的阅读者对象（后台）
    /// </summary>
    public class AdminActivityReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的默认活动的阅读者对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminActivityReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取默认活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">活动类型</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <returns>返回默认活动的分页列表</returns>
        public PaginationList<ActivityResult> ReadActivityList(string keyword, ActivityTypeSelectType type
            , RegularlyStatusSelectType status, int page)
        {
            Expression<Func<Activity, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<Activity, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<Activity, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<Activity, bool>> predicate4 = x => x.AutoDelete == false
                || x.EndTime >= DateTime.Now;
            if (keyword != "")
            {
                keyword = TextHelper.EliminateSpaces(keyword);
                string[] kws = keyword.Split(new char[] { ' ' });
                predicate1 = x => kws.All(kw => x.Title.Contains(kw));
            }
            switch (type)
            {
                case ActivityTypeSelectType.注册返点:
                    predicate2 = x => x.Type == ActivityType.注册返点;
                    break;
                case ActivityTypeSelectType.充值返点:
                    predicate2 = x => x.Type == ActivityType.充值返点;
                    break;
                case ActivityTypeSelectType.下级用户充值返点:
                    predicate2 = x => x.Type == ActivityType.下级用户充值返点;
                    break;
                case ActivityTypeSelectType.消费返点:
                    predicate2 = x => x.Type == ActivityType.消费返点;
                    break;
                case ActivityTypeSelectType.下级用户消费返点:
                    predicate2 = x => x.Type == ActivityType.下级用户消费返点;
                    break;
            }
            switch (status)
            {
                case RegularlyStatusSelectType.未过期:
                    predicate3 = x => x.EndTime > DateTime.Now;
                    break;
                case RegularlyStatusSelectType.正常:
                    predicate3 = x => x.EndTime > DateTime.Now && x.Hide == false;
                    break;
                case RegularlyStatusSelectType.暂停:
                    predicate3 = x => x.EndTime > DateTime.Now && x.Hide == true;
                    break;
                case RegularlyStatusSelectType.已过期:
                    predicate3 = x => x.EndTime <= DateTime.Now;
                    break;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var aSet = db.Set<Activity>();

            int tCount = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<ActivityResult> tList = aSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderByDescending(x => x.CreatedTime)
                .OrderBy(x => x.Title)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ActivityResult(x));

            return new PaginationList<ActivityResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取默认活动的参与记录的分页列表
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <returns>返回默认活动的参与记录的分页列表</returns>
        public PaginationList<ActivityParticipateRecordResult> ReadParticipateRecordList(int activityId, int ownerId, string beginTime
            , string endTime, int page)
        {
            Expression<Func<ActivityParticipateRecord, bool>> predicate1 = x => x.Id > 0;
            Expression<Func<ActivityParticipateRecord, bool>> predicate2 = x => x.Id > 0;
            Expression<Func<ActivityParticipateRecord, bool>> predicate3 = x => x.Id > 0;
            Expression<Func<ActivityParticipateRecord, bool>> predicate4 = x => x.Id > 0;
            if (activityId != 0)
            {
                predicate1 = x => x.Activity.Id == activityId;
            }
            if (ownerId != 0)
            {
                predicate2 = x => x.Owner.Id == ownerId;
            }
            if (beginTime != "")
            {
                string[] tTimeStr = beginTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate3 = x => x.CreatedTime >= tTime;
            }
            if (endTime != "")
            {
                string[] tTimeStr = endTime.Split(new char[] { '-' });
                DateTime tTime = new DateTime(Convert.ToInt32(tTimeStr[0]), Convert.ToInt32(tTimeStr[1]), Convert.ToInt32(tTimeStr[2]));
                predicate4 = x => x.CreatedTime <= tTime;
            }
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var aprSet = db.Set<ActivityParticipateRecord>();

            int tCount = aprSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .Count();
            List<ActivityParticipateRecordResult> tList = aprSet
                .Where(predicate1)
                .Where(predicate2)
                .Where(predicate3)
                .Where(predicate4)
                .OrderByDescending(x => x.CreatedTime)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new ActivityParticipateRecordResult(x));

            return new PaginationList<ActivityParticipateRecordResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
