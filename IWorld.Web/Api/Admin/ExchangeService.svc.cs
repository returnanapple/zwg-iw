using System;
using System.Collections.Generic;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 兑换活动管理的数据服务（后台）
    /// </summary>
    public class ExchangeService : IExchangeService
    {
        /// <summary>
        /// 获取兑换活动的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的分页列表</returns>
        public PaginationList<ExchangeResult> GetExchangeList(string keyword, RegularlyStatusSelectType status, int page
            , string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ExchangeResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewActivities)
                    {
                        return new PaginationList<ExchangeResult>("该用户所属的用户组无权查看兑换活动");
                    }

                    AdminExchangeReader reader = new AdminExchangeReader(db);
                    return reader.ReadExchangeList(keyword, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ExchangeResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取兑换活动的参与记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回兑换活动的参与记录的分页列表</returns>
        public PaginationList<ExchangeParticipateRecordResult> GetParticipateRecordList(int exchangeId, int ownerId, string beginTime
            , string endTime, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ExchangeParticipateRecordResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewActivities)
                    {
                        return new PaginationList<ExchangeParticipateRecordResult>("该用户所属的用户组无权查看兑换活动的参与记录");
                    }

                    AdminExchangeReader reader = new AdminExchangeReader(db);
                    return reader.ReadParticipateRecordList(exchangeId, ownerId, beginTime, endTime, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ExchangeParticipateRecordResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加兑换活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddExchange(AddExchangeImport import, string token)
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

                    List<ExchangeManager.Factory.IPackageForCondition> conditions = import.Conditions
                        .ConvertAll(x => ExchangeManager.Factory.CreatePackageForCondition(x.Type.ToString(), x.Limit, x.Upper));
                    List<ExchangeManager.Factory.IPackageForPrize> prizes = import.Prizes
                        .ConvertAll(x => ExchangeManager.Factory.CreatePackageForPrize(x.Name, x.Description, x.Sum, x.Type.ToString()
                            , x.Price, x.Remark));
                    ICreatePackage<Exchange> pfc = ExchangeManager.Factory
                        .CreatePackageForCreate(import.Name, import.Places, import.UnitPrice, import.EachPersonCanExchangeTheNumberOfTimes
                        , import.EachPersonCanExchangeTheTimesOfDays, import.EachPersonCanExchangeTheNumberOfDays
                        , import.EachPersonCanExchangeTheTimesOfAll, import.EachPersonCanExchangeTheNumberOfAll, prizes, conditions
                        , import.BeginTime, import.Days, import.AutoDelete);
                    new ExchangeManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑兑换活动的基本信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditExchange_Basic(EditExchangeImport_Basic import, string token)
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

                    IUpdatePackage<Exchange> pfu = ExchangeManager.Factory
                        .CreatePackageForUpdate(import.ExchangeId, import.Days, import.Hide, import.AutoDelete);
                    new ExchangeManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑兑换活动
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditExchange(EditExchangeImport import, string token)
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
                        return new OperateResult("该用户所属的用户组无权隐藏兑换活动");
                    }
                    ExchangeManager exchangeManager = new ExchangeManager(db);

                    List<ExchangeManager.Factory.IPackageForCondition> conditions = import.Conditions
                        .ConvertAll(x => ExchangeManager.Factory.CreatePackageForCondition(x.Type.ToString(), x.Limit, x.Upper));
                    exchangeManager.EditCondition(import.ExchangeId, conditions);

                    List<ExchangeManager.Factory.IPackageForPrize> prizes = import.Prizes
                        .ConvertAll(x => ExchangeManager.Factory.CreatePackageForPrize(x.Name, x.Description, x.Sum, x.Type.ToString()
                            , x.Price, x.Remark));
                    exchangeManager.EditPrize(import.ExchangeId, prizes);

                    IUpdatePackage<Exchange> pfu = ExchangeManager.Factory
                        .CreatePackageForUpdate(import.ExchangeId, import.Name, import.Places, import.UnitPrice
                        , import.EachPersonCanExchangeTheNumberOfTimes, import.EachPersonCanExchangeTheTimesOfDays
                        , import.EachPersonCanExchangeTheNumberOfDays, import.EachPersonCanExchangeTheTimesOfAll
                        , import.EachPersonCanExchangeTheNumberOfAll, import.Days, import.Hide, import.AutoDelete);
                    exchangeManager.Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveExchange(int exchangeId, string token)
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
                        return new OperateResult("该用户所属的用户组无权删除兑换活动");
                    }

                    new ExchangeManager(db).Remove(exchangeId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HideExchange(int exchangeId, string token)
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
                        return new OperateResult("该用户所属的用户组无权隐藏兑换活动");
                    }

                    IUpdatePackage<Exchange> pfu = ExchangeManager.Factory
                        .CreatePackageForUpdate(exchangeId, true);
                    new ExchangeManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示兑换活动
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowExchange(int exchangeId, string token)
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
                        return new OperateResult("该用户所属的用户组无权显示兑换活动");
                    }

                    IUpdatePackage<Exchange> pfu = ExchangeManager.Factory
                        .CreatePackageForUpdate(exchangeId, false);
                    new ExchangeManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取实体奖品赠送记录的分页列表
        /// </summary>
        /// <param name="exchangeId">目标活动的存储指针</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="status">赠送状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回实体奖品赠送记录的分页列表</returns>
        public PaginationList<GiftResult> GetGiftList(int exchangeId, int ownerId, GiftStatusSelectType status, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<GiftResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewActivities)
                    {
                        return new PaginationList<GiftResult>("该用户所属的用户组无权查看实体奖品赠送记录");
                    }

                    AdminExchangeReader reader = new AdminExchangeReader(db);
                    return reader.ReadGiftList(exchangeId, ownerId, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<GiftResult>(e.Message);
            }
        }

        /// <summary>
        /// 赠送实体奖品
        /// </summary>
        /// <param name="giftId">实体奖品赠送记录的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HandselGift(int giftId, string token)
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
                        return new OperateResult("该用户所属的用户组无权赠送实体奖品");
                    }

                    new GiftManager(db).Hndsel(giftId);
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
