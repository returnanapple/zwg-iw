using System;
using System.Collections.Generic;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 默认活动管理的数据服务（后台）
    /// </summary>
    public class ActivityService : IActivityService
    {
        /// <summary>
        /// 获取默认活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="type">活动类型</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的分页列表</returns>
        public PaginationList<ActivityResult> GetActivityList(string keyword, ActivityTypeSelectType type
            , RegularlyStatusSelectType status, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ActivityResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewActivities)
                    {
                        return new PaginationList<ActivityResult>("该用户所属的用户组无权查看默认活动");
                    }

                    AdminActivityReader reader = new AdminActivityReader(db);
                    return reader.ReadActivityList(keyword, type, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ActivityResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取默认活动的参与记录的分页列表
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回默认活动的参与记录的分页列表</returns>
        public PaginationList<ActivityParticipateRecordResult> GetParticipateRecordList(int activityId, int ownerId, string beginTime
            , string endTime, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ActivityParticipateRecordResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewActivities)
                    {
                        return new PaginationList<ActivityParticipateRecordResult>("该用户所属的用户组无权查看默认活动的参与记录");
                    }

                    AdminActivityReader reader = new AdminActivityReader(db);
                    return reader.ReadParticipateRecordList(activityId, ownerId, beginTime, endTime, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ActivityParticipateRecordResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加默认活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddActivity(AddActivityImport import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权添加默认活动");
                    }

                    List<ActivityManager.Factory.IPackageForCondition> conditions = import.Conditions
                        .ConvertAll(x => ActivityManager.Factory.CreatePackageForCondition(x.Type.ToString(), x.Limit, x.Upper));
                    ICreatePackage<Activity> pfc = ActivityManager.Factory
                        .CreatePackageForCreate(import.Title, import.Type.ToString(), import.MinRestrictionValue, import.MaxRestrictionValues
                        , import.RewardType.ToString(), import.RewardValueIsAbsolute, import.Reward, conditions, import.BeginTime, import.Days
                        , import.AutoDelete);
                    new ActivityManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑默认活动的基本信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditActivity_Basic(EditActivityImport_Basic import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权添加默认活动");
                    }

                    IUpdatePackage<Activity> pfu = ActivityManager.Factory
                        .CreatePackageForUpdate(import.ActivityId, import.Days, import.Hide, import.AutoDelete);
                    new ActivityManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑默认活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditActivity(EditActivityImport import, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权添加默认活动");
                    }

                    ActivityManager activityManager = new ActivityManager(db);

                    IUpdatePackage<Activity> pfu = ActivityManager.Factory
                        .CreatePackageForUpdate(import.ActivityId, import.Title, import.Type.ToString(), import.MinRestrictionValue
                        , import.MaxRestrictionValues, import.RewardType.ToString(), import.RewardValueIsAbsolute, import.Reward
                        , import.Days, import.Hide, import.AutoDelete);
                    activityManager.Update(pfu);

                    List<ActivityManager.Factory.IPackageForCondition> conditions = import.Conditions
                        .ConvertAll(x => ActivityManager.Factory.CreatePackageForCondition(x.Type.ToString(), x.Limit, x.Upper));
                    activityManager.EditCondition(import.ActivityId, conditions);

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveActivity(int activityId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权删除默认活动");
                    }

                    new ActivityManager(db).Remove(activityId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HideActivity(int activityId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权隐藏默认活动");
                    }

                    IUpdatePackage<Activity> pfu = ActivityManager.Factory
                        .CreatePackageForUpdate(activityId, true);
                    new ActivityManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示默认活动
        /// </summary>
        /// <param name="activityId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowActivity(int activityId, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new OperateResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanEditActivities)
                    {
                        return new OperateResult("该用户所属的用户组无权显示默认活动");
                    }

                    IUpdatePackage<Activity> pfu = ActivityManager.Factory
                        .CreatePackageForUpdate(activityId, false);
                    new ActivityManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }
    }
}
