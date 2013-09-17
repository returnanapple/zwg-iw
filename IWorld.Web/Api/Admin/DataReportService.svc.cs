using System;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 报表的数据服务（后台）
    /// </summary>
    public class DataReportService : IDataReportService
    {
        /// <summary>
        /// 获取站点综合信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点综合信息</returns>
        public ComprehensiveInformationResult GetComprehensiveInformation(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new ComprehensiveInformationResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new ComprehensiveInformationResult("该用户所属的用户组无权查看站点综合信息");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadComprehensiveInformation();
                }
            }
            catch (Exception e)
            {
                return new ComprehensiveInformationResult(e.Message);
            }
        }

        /// <summary>
        /// 获取站点信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点信息统计的分页列表</returns>
        public PaginationList<SiteDataResult> GetSiteDataList(string beginTime, string endTime, TimePeriodSelectType timePeriod
            , int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<SiteDataResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<SiteDataResult>("该用户所属的用户组无权查看站点信息统计的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadSiteDataList(beginTime, endTime, timePeriod, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<SiteDataResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取个人信息统计的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="timePeriod">时间段</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="username">目标用户的用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回个人信息统计的分页列表</returns>
        public PaginationList<PersonalDataResult> GetPersonalDataList(string beginTime, string endTime
            , TimePeriodSelectType timePeriod, int userId, string username, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<PersonalDataResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<PersonalDataResult>("该用户所属的用户组无权查看个人信息统计的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadPersonalDataList(beginTime, endTime, timePeriod, userId, username, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<PersonalDataResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取投注信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="usermae">目标用户的用户名</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注信息的分页列表</returns>
        public PaginationList<BettingResult> GetBettingList(string beginTime, string endTime, int ownerId, string usermae
            , int ticketId, int tagId, int howToPlatId, BettingStatusSelectType status, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<BettingResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<BettingResult>("该用户所属的用户组无权查看投注信息的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadBettingList(beginTime, endTime, ownerId, usermae, ticketId, tagId, howToPlatId
                        , status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<BettingResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取追号信息的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="ownerId">目标用户的存储指针</param>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="tagId">目标玩法标签的存储指针</param>
        /// <param name="howToPlatId">目标玩法的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回追号信息的分页列表</returns>
        public PaginationList<ChasingResult> GetChasingList(string beginTime, string endTime, int ownerId, int ticketId, int tagId
            , int howToPlatId, ChasingStatusSelectType status, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<ChasingResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<ChasingResult>("该用户所属的用户组无权查看追号信息的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadChasingList(beginTime, endTime, ownerId, ticketId, tagId, howToPlatId, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<ChasingResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取投注（追号）信息的分页列表
        /// </summary>
        /// <param name="chasingId">目标追号记录的存储指针</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回投注（追号）信息的分页列表</returns>
        public PaginationList<BettingForChasingResult> GetBettingForChasingList(int chasingId, BettingStatusSelectType status
            , int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<BettingForChasingResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<BettingForChasingResult>("该用户所属的用户组无权查看投注（追号）信息的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadBettingForChasingList(chasingId, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<BettingForChasingResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取充值申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="username">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回充值申请的分页列表</returns>
        public PaginationList<RechargeResult> GetRechargeList(string beginTime, string endTime, RechargeStatusSelectType status
            , int userId, string username, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<RechargeResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<RechargeResult>("该用户所属的用户组无权查看充值申请的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadRechargeList(beginTime, endTime, status, userId, username, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<RechargeResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取未处理的充值申请的数量
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回未处理的充值申请的数量</returns>
        public UntreatedRecharCountResult GetUntreatedRecharCount(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new UntreatedRecharCountResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new UntreatedRecharCountResult("该用户所属的用户组无权查看未处理的充值申请的数量");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadUntreatedRecharCount();
                }
            }
            catch (Exception e)
            {
                return new UntreatedRecharCountResult(e.Message);
            }
        }

        /// <summary>
        /// 确认充值申请
        /// </summary>
        /// <param name="rechargeId">目标充值申请的存储指针</param>
        /// <param name="card">来源卡号</param>
        /// <param name="name">来源卡的开户人</param>
        /// <param name="bank">来源卡的开户银行</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult DeterminetRecharge(int rechargeId, string card, string name, Bank bank, string token)
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
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new OperateResult("该用户所属的用户组无权确认充值申请");
                    }

                    new RechargeRecordManager(db).ChangeStatus(rechargeId, RechargeStatus.充值成功, "", card, name, bank);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 否决充值申请
        /// </summary>
        /// <param name="rechargeId">目标充值申请的存储指针</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult NegativeRecharge(int rechargeId, string remark, string token)
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
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new OperateResult("该用户所属的用户组无权否决充值申请");
                    }

                    new RechargeRecordManager(db).ChangeStatus(rechargeId, RechargeStatus.失败, remark);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取提现申请的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="username">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回提现申请的分页列表</returns>
        public PaginationList<WithdrawalResult> GetWithdrawalList(string beginTime, string endTime
            , WithdrawalsStatusSelectType status, int userId, string username, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<WithdrawalResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<WithdrawalResult>("该用户所属的用户组无权查看提现申请的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadWithdrawalList(beginTime, endTime, status, userId, username, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<WithdrawalResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取未处理的提现申请的数量
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public UntreatedWithdrawalCountResult GetUntreatedWithdrawalCount(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new UntreatedWithdrawalCountResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new UntreatedWithdrawalCountResult("该用户所属的用户组无权查看未处理的提现申请的数量");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadUntreatedWithdrawalCount();
                }
            }
            catch (Exception e)
            {
                return new UntreatedWithdrawalCountResult(e.Message);
            }
        }

        /// <summary>
        /// 确认提现申请
        /// </summary>
        /// <param name="withdrawalId">目标提现申请的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult DeterminetWithdrawal(int withdrawalId, string token)
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
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new OperateResult("该用户所属的用户组无权确认提现申请");
                    }

                    new WithdrawalsRecordManager(db).ChangeStatus(withdrawalId, WithdrawalsStatus.提现成功);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 否决提现申请
        /// </summary>
        /// <param name="withdrawalId">目标提现申请的存储指针</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult NegativeWithdrawal(int withdrawalId, string remark, string token)
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
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new OperateResult("该用户所属的用户组无权否决提现申请");
                    }

                    new WithdrawalsRecordManager(db).ChangeStatus(withdrawalId, WithdrawalsStatus.失败);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取支取记录的分页列表
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="userId">目标用户的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回支取记录的分页列表</returns>
        public PaginationList<TransferResult> GetTransferList(string beginTime, string endTime, int userId, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<TransferResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new PaginationList<TransferResult>("该用户所属的用户组无权查看支取记录的分页列表");
                    }

                    AdminDataReportReader reader = new AdminDataReportReader(db);
                    return reader.ReadTransferList(beginTime, endTime, userId, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<TransferResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加支取记录
        /// </summary>
        /// <param name="sum">数额</param>
        /// <param name="remark">备注</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddTransfer(double sum, string remark, string token)
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
                    if (!administrator.Group.CanViewDataReports)
                    {
                        return new OperateResult("该用户所属的用户组无权添加支取记录");
                    }

                    ICreatePackage<TransferRecord> pfc = TransferRecordManager.Factory
                        .CreatePackageForCreate(administratorId, sum, remark);
                    new TransferRecordManager(db).Create(pfc);
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
