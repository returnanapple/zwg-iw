using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 彩票管理的数据服务（后台）
    /// </summary>
    public class LotteryTicketService : ILotteryTicketService
    {
        /// <summary>
        /// 获取彩票信息的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回彩票信息的分页列表</returns>
        public PaginationList<TicketResult> GetTicketList(string keyword, HideOrNotSelectType isHide, int page
            , string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<TicketResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<TicketResult>("该用户所属的用户组无权查看彩票信息");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadTicketList(keyword, isHide, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<TicketResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取开奖时间的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖时间的分页列表</returns>
        public PaginationList<LotteryTimeResult> GetLotteryTimeList(int ticketId, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<LotteryTimeResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<LotteryTimeResult>("该用户所属的用户组无权查看开奖时间");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadLotteryTimeList(ticketId, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<LotteryTimeResult>(e.Message);
            }
        }

        /// <summary>
        /// 获取开奖记录的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="sources">来源</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回开奖记录的分页列表</returns>
        public PaginationList<LotteryResult> GetLotteryList(int ticketId, LotterySourcesSelectType sources, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<LotteryResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<LotteryResult>("该用户所属的用户组无权查看开奖记录");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadLotteryList(ticketId, sources, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<LotteryResult>(e.Message);
            }
        }

        /// <summary>
        /// 修改彩票信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EdotTicket(EditTicketImport import, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权修改彩票信息");
                    }

                    IUpdatePackage<LotteryTicket> pfu = LotteryTicketManager.Factory
                        .CreatePackageForUpdateName(import.TicketId, import.Name, import.Order);
                    new LotteryTicketManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 修改开奖时间集
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="imports">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditLotteryTime(int ticketId, List<EditLotteryTimeImport> imports, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权修改开奖时间集");
                    }

                    List<LotteryTicketManager.Factory.IPackageForTime> times
                        = imports.ConvertAll(x => LotteryTicketManager.Factory.CreatePackageForTime(x.Phases, x.TimeValue));
                    new LotteryTicketManager(db).EditLotteryTimes(ticketId, times);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HideTicket(int ticketId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权隐藏彩票");
                    }

                    new LotteryTicketManager(db).Hide(ticketId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示彩票
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowTicket(int ticketId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权显示彩票");
                    }

                    new LotteryTicketManager(db).Show(ticketId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取玩法标签信息的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回玩法标签信息的分页列表</returns>
        public PaginationList<PlayTagResult> GetPlayTagList(int ticketId, string keyword, HideOrNotSelectType isHide
            , int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<PlayTagResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<PlayTagResult>("该用户所属的用户组无权查看玩法标签信息");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadPlayTagList(ticketId, keyword, isHide, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<PlayTagResult>(e.Message);
            }
        }

        /// <summary>
        /// 修改玩法标签
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditPlayTag(EditPlayTagImport import, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权显示彩票");
                    }

                    IUpdatePackage<PlayTag> pfu = PlayTagManager.Fantory
                        .CreatePackageForUpdate(import.PlayTagId, import.Name, import.Order);
                    new PlayTagManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏玩法标签
        /// </summary>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HidePlayTag(int playTagId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权隐藏玩法标签");
                    }

                    new PlayTagManager(db).Hide(playTagId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示玩法标签
        /// </summary>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowPlayTag(int playTagId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权显示玩法标签");
                    }

                    new PlayTagManager(db).Show(playTagId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取玩法的分页列表
        /// </summary>
        /// <param name="ticketId">目标彩票的存储指针</param>
        /// <param name="playTagId">目标玩法标签的存储指针</param>
        /// <param name="keyword">关键字</param>
        /// <param name="isHide">隐藏</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回玩法的分页列表</returns>
        public PaginationList<HowToPlayResult> GetHowToPlayList(int ticketId, int playTagId, string keyword
            , HideOrNotSelectType isHide, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<HowToPlayResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<HowToPlayResult>("该用户所属的用户组无权查看玩法信息");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadHowToPlayList(ticketId, playTagId, keyword, isHide, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<HowToPlayResult>(e.Message);
            }
        }

        /// <summary>
        /// 修改玩法信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditHowToPlay(EditHowToPlayImport import, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权隐藏玩法");
                    }

                    IUpdatePackage<HowToPlay> pfu = HowToPlayManager.Factory
                        .CreatePackageForUpdate_Basic(import.HowToPlayId, import.Name, import.Description, import.Rule, import.Odds, import.Order);
                    new HowToPlayManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HideHowToPlay(int howToPlayId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权隐藏玩法");
                    }

                    new HowToPlayManager(db).Hide(howToPlayId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowHowToPlay(int howToPlayId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权显示玩法");
                    }

                    new HowToPlayManager(db).Show(howToPlayId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取虚拟排行信息的分页列表
        /// </summary>
        /// <param name="ticketId">对应的彩票的存储指针</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回虚拟排行信息的分页列表</returns>
        public PaginationList<VirtualTopResult> GetVirtualTopList(int ticketId, int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<VirtualTopResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanViewTickets)
                    {
                        return new PaginationList<VirtualTopResult>("该用户所属的用户组无权查看虚拟排行信息");
                    }

                    AdminLotteryTicketReader reader = new AdminLotteryTicketReader(db);
                    return reader.ReadVirtualTopList(ticketId, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<VirtualTopResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加虚拟排行信息
        /// </summary>
        /// <param name="import"数据集></param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddVirtualTop(AddVirtualTopImport import, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权添加虚拟排行信息");
                    }
                    int ticketId = db.Set<LotteryTicket>().Where(x => x.Name == import.Ticket)
                        .Select(x => x.Id)
                        .First();
                    ICreatePackage<VirtualTop> pfc = VirtualTopManager.Factory.CreatePackageForCreate(ticketId
                        , import.Sum);
                    new VirtualTopManager(db).Create(pfc);

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑虚拟排行信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns><返回操作结果/returns>
        public OperateResult EditVirtualTop(EditVirtualTopImport import, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑虚拟排行信息");
                    }
                    int ticketId = db.Set<LotteryTicket>().Where(x => x.Name == import.Ticket)
                        .Select(x => x.Id)
                        .First();
                    IUpdatePackage<VirtualTop> pfu = VirtualTopManager.Factory.CreatePackageForUpdate(import.VirtualTopId
                        , ticketId, import.Sum);
                    new VirtualTopManager(db).Update(pfu);

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 移除虚拟排行
        /// </summary>
        /// <param name="virtualTopId">对应的虚拟排行信息</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveVirtualTop(int virtualTopId, string token)
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
                    if (!administrator.Group.CanEditTickets)
                    {
                        return new OperateResult("该用户所属的用户组无权移除虚拟排行");
                    }
                    new VirtualTopManager(db).Remove(virtualTopId);

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
