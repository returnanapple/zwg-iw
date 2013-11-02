using IWorld.BLL;
using IWorld.Contract.Client;
using IWorld.DAL;
using IWorld.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IWorld.Web.Api
{
    /// <summary>
    /// 大白鲨游戏的服务
    /// </summary>
    public class JawService : IJawService
    {
        /// <summary>
        /// 获取大白鲨游戏的主显示信息
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回大白鲨游戏的主显示信息</returns>
        public MainOfJawResult GetMainOfJaw(string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new MainOfJawResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientJawReader reader = new ClientJawReader(db);
                    return reader.ReadMainOfJaw(userId);
                }
            }
            catch (Exception ex)
            {
                return new MainOfJawResult(ex.Message);
            }
        }

        /// <summary>
        /// 获取大白鲨游戏的开奖记录（前十期）
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回大白鲨游戏的开奖记录（前十期）</returns>
        public List<LotteryOfJawResult> GetLotterys(string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new List<LotteryOfJawResult>();
                }

                using (WebMapContext db = new WebMapContext())
                {
                    ClientJawReader reader = new ClientJawReader(db);
                    return reader.GetLotterys();
                }
            }
            catch (Exception ex)
            {
                return new List<LotteryOfJawResult>();
            }
        }

        /// <summary>
        /// 投注
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Bet(BettingOfJawImport import, string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    var dPackages = import.Details.ConvertAll(x =>
                        {
                            return BettingOfJawManager.Factory
                                .CreatePackageForCreateDetail(x.Icon, x.Sum);
                        });
                    var packageForCreate = BettingOfJawManager.Factory
                        .CreatePackageForCreate(userId, import.Issue, dPackages);
                    new BettingOfJawManager(db).Create(packageForCreate);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }

        /// <summary>
        /// 撤销投注
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult Revocation(string token)
        {
            try
            {
                int userId = CacheManager.GetUserId(token);
                if (userId <= 0)
                {
                    return new OperateResult("未登录");
                }

                using (WebMapContext db = new WebMapContext())
                {
                    var moj = db.Set<MainOfJaw>().First();
                    var b = db.Set<BettingOfJaw>().FirstOrDefault(x => x.Owner.Id == userId
                        && x.Issue == moj.NextPhases
                        && x.Status == BettingStatus.等待开奖);
                    if (b == null)
                    {
                        throw new Exception("没有符合要求的投注记录");
                    }
                    new BettingOfJawManager(db).Remove(b.Id);

                    return new OperateResult();
                }
            }
            catch (Exception ex)
            {
                return new OperateResult(ex.Message);
            }
        }
    }
}
