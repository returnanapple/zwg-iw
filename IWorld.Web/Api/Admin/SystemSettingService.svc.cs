using System;
using System.Linq;
using IWorld.Contract.Admin;
using IWorld.Model;
using IWorld.BLL;
using IWorld.Setting;
using IWorld.DAL;

namespace IWorld.Web.Api.Admin
{
    /// <summary>
    /// 系统设置管理的数据服务（后台）
    /// </summary>
    public class SystemSettingService : ISystemSettingService
    {
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回站点设置</returns>
        public WebSettingResult GetWebSetting(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new WebSettingResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new WebSettingResult("该用户所属的用户组无权查看站点设置");
                    }

                    return new WebSettingResult();
                }
            }
            catch (Exception e)
            {
                return new WebSettingResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑站点设置
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditWebSetting(EditWebSettingImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑站点设置");
                    }

                    WebSetting webSetting = new WebSetting();
                    System.Type _type = typeof(WebSetting);
                    typeof(EditWebSettingImport).GetProperties().ToList()
                        .ForEach(x =>
                            {
                                object val = x.GetValue(import);
                                _type.GetProperty(x.Name).SetValue(webSetting, val);
                            });
                    webSetting.Save();
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取银行账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回银行账户的分页列表</returns>
        public PaginationList<BankAccountResult> GetBankAccountList(int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<BankAccountResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new PaginationList<BankAccountResult>("该用户所属的用户组无权查看银行账户");
                    }

                    AdminSystemSettingReader reader = new AdminSystemSettingReader(db);
                    return reader.ReadBankAccountList(page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<BankAccountResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加银行账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddBankAccount(AddBankAccountImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权添加银行账户");
                    }

                    ICreatePackage<BankAccount> pfc = BankAccountManager.Factory
                        .CreatePackageForCreate(import.Key, import.Name, import.Card, import.Bank.ToString(), import.Remark, import.Order);
                    new BankAccountManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑银行账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditBankAccount(EditBankAccountImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑银行账户");
                    }

                    IUpdatePackage<BankAccount> pfc = BankAccountManager.Factory
                        .CreatePackageForUpdate(import.BankAccountId, import.Key, import.Name, import.Card, import.Bank.ToString()
                        , import.Remark, import.Order);
                    new BankAccountManager(db).Update(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除银行账户
        /// </summary>
        /// <param name="bankAccountId">目标银行账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveBankAccount(int bankAccountId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权删除银行账户");
                    }

                    new BankAccountManager(db).Remove(bankAccountId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取系统邮件账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回系统邮件账户的分页列表</returns>
        public PaginationList<EmailAccountResult> GetEmailAccountList(int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<EmailAccountResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new PaginationList<EmailAccountResult>("该用户所属的用户组无权查看系统邮件账户");
                    }

                    AdminSystemSettingReader reader = new AdminSystemSettingReader(db);
                    return reader.ReadEmailAccountList(page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<EmailAccountResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加系统邮件账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddEmailAccount(AddEmailAccountImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权添加系统邮件账户");
                    }
                    bool hadKey = db.Set<EmailClient>().Any(x => x.Key == import.ClientKey);
                    if (!hadKey)
                    {
                        throw new Exception(string.Format("制定的索引字{0}并没有找到对应的服务器", import.ClientKey));
                    }
                    int clientId = db.Set<EmailClient>().FirstOrDefault(x => x.Key == import.ClientKey).Id;

                    ICreatePackage<EmailAccount> pfc = EmailAccountManager.Factory
                        .CreatePackageForCreate(import.Key, import.Account, import.Password, import.Remark, clientId);
                    new EmailAccountManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑系统邮件账户
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditEmailAccount(EditEmailAccountImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑系统邮件账户");
                    }
                    bool hadKey = db.Set<EmailClient>().Any(x => x.Key == import.ClientKey);
                    if (!hadKey)
                    {
                        throw new Exception(string.Format("制定的索引字{0}并没有找到对应的服务器", import.ClientKey));
                    }
                    int clientId = db.Set<EmailClient>().FirstOrDefault(x => x.Key == import.ClientKey).Id;

                    IUpdatePackage<EmailAccount> pfu = EmailAccountManager.Factory
                        .CreatePackageForUpdate(import.EmailAccountId, import.Key, import.Account, import.Password, import.Remark
                        , clientId);
                    new EmailAccountManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除系统邮件账户
        /// </summary>
        /// <param name="emailAccountId">目标系统邮件账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveEmailAccount(int emailAccountId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权删除系统邮件账户");
                    }

                    new EmailAccountManager(db).Remove(emailAccountId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取邮件服务地址的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回邮件服务地址的分页列表</returns>
        public PaginationList<EmailClientResult> GetEmailClientList(int page, string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new PaginationList<EmailClientResult>("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new PaginationList<EmailClientResult>("该用户所属的用户组无权查看邮件服务地址");
                    }

                    AdminSystemSettingReader reader = new AdminSystemSettingReader(db);
                    return reader.ReadEmailClientList(page);
                }
            }
            catch (Exception e)
            {
                return new PaginationList<EmailClientResult>(e.Message);
            }
        }

        /// <summary>
        /// 添加邮件服务地址
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult AddEmailClient(AddEmailClientImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权添加邮件服务地址");
                    }

                    ICreatePackage<EmailClient> pfc = EmailClientManager.Factory
                        .CreatePackageForCreate(import.Key, import.Host, import.Port, import.Remark);
                    new EmailClientManager(db).Create(pfc);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 编辑邮件服务地址
        /// </summary>
        /// <param name="import">数据集</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult EditEmailClient(EditEmailClientImport import, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑邮件服务地址");
                    }

                    IUpdatePackage<EmailClient> pfu = EmailClientManager.Factory
                        .CreatePackageForUpdate(import.EmailClientId, import.Key, import.Host, import.Port, import.Remark);
                    new EmailClientManager(db).Update(pfu);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 删除邮件服务地址
        /// </summary>
        /// <param name="emailClientId">目标邮件服务地址的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult RemoveEmailClient(int emailClientId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权编辑邮件服务地址");
                    }

                    new EmailClientManager(db).Remove(emailClientId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 获取采集程序运行状态
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回采集程序运行状态</returns>
        public CollectionStatusResult GetCollectionStatusResult(string token)
        {
            try
            {
                int administratorId = WebHepler.GetAdministratorId(token);
                if (administratorId == -1)
                {
                    return new CollectionStatusResult("未登陆");
                }
                using (WebMapContext db = new WebMapContext())
                {
                    Administrator administrator = db.Set<Administrator>().Find(administratorId);
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new CollectionStatusResult("该用户所属的用户组无权获取采集程序运行状态");
                    }

                    WebSetting websetting = new WebSetting();
                    return new CollectionStatusResult(websetting.CollectionRunning);
                }
            }
            catch (Exception e)
            {
                return new CollectionStatusResult(e.Message);
            }
        }

        /// <summary>
        /// 关闭或打开数据采集
        /// </summary>
        /// <param name="close">标识 | 关闭</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult CloseOrOpenCollection(bool close, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权关闭或打开数据采集");
                    }

                    WebSetting webSetting = new WebSetting();
                    webSetting.CollectionRunning = !close;
                    webSetting.Save();

                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 重置缓存池
        /// </summary>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult ClearCachePond(string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权重置缓存车");
                    }

                    CacheManager.ClearCache();
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 设置默认银行账户
        /// </summary>
        /// <param name="bankAccountId">目标银行账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult SetDefaultBankAccount(int bankAccountId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权设置默认银行账户");
                    }

                    new BankAccountManager(db).SetDefault(bankAccountId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 设置默认邮件账户
        /// </summary>
        /// <param name="emailAccountId">目标邮件账户的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult SetDefaultEmailAccount(int emailAccountId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权设置默认邮件账户");
                    }

                    new EmailAccountManager(db).SetDefault(emailAccountId);
                    return new OperateResult();
                }
            }
            catch (Exception e)
            {
                return new OperateResult(e.Message);
            }
        }

        /// <summary>
        /// 设置默认邮件服务地址
        /// </summary>
        /// <param name="emailAccountId">目标邮件服务地址的存储指针</param>
        /// <param name="token">身份标识</param>
        /// <returns>返回操作结果</returns>
        public OperateResult SetDefaultEmailClient(int emailClientId, string token)
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
                    if (!administrator.Group.CanSettingSite)
                    {
                        return new OperateResult("该用户所属的用户组无权设置默认邮件服务地址");
                    }

                    new EmailClientManager(db).SetDefault(emailClientId);
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
