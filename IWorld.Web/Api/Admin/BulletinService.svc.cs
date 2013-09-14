using System;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 公告管理的数据服务（后台）
    /// </summary>
    public class BulletinService : IBulletinService
    {
        /// <summary>
        /// 获取公告的分页列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回公告的分页列表</returns>
        public PaginationList<BulletinResult> GetBulletinList(string keyword, RegularlyStatusSelectType status, int page
            , string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<BulletinResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    AdminBulletinReader reader = new AdminBulletinReader(db);
                    return reader.ReadBulletinList(keyword, status, page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<BulletinResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddBulletin(AddBulletinImport import, string token)
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
                    ICreatePackage<Bulletin> pfc = BulletinManager.Factory
                        .CreatePackageForCreate(import.Title, import.Context, import.BeginTime, import.Days, import.AutoDelete);
                    new BulletinManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑公告信息
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditBulletin(EditBulletinImport import, string token)
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
                    IUpdatePackage<Bulletin> pfu = BulletinManager.Factory
                        .CreatePackageForUpdate(import.BulletinId, import.Title, import.Context, import.Days, import.Hide, import.AutoDelete);
                    new BulletinManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveBulletin(int bulletinId, string token)
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
                    new BulletinManager(db).Remove(bulletinId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 隐藏公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult HideBulletin(int bulletinId, string token)
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
                    IUpdatePackage<Bulletin> pfu = BulletinManager.Factory
                        .CreatePackageForUpdate(bulletinId, true);
                    new BulletinManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 显示公告
        /// </summary>
        /// <param name="bulletinId">目标公告的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ShowBulletin(int bulletinId, string token)
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
                    IUpdatePackage<Bulletin> pfu = BulletinManager.Factory
                        .CreatePackageForUpdate(bulletinId, false);
                    new BulletinManager(db).Update(pfu);
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
