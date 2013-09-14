using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Contract.Admin;
using IWorld.Helper;
using IWorld.Setting;

namespace IWorld.DAL
{
    /// <summary>
    /// 站点设置信息的阅读着对象（后台）
    /// </summary>
    public class AdminSystemSettingReader : ReaderBase
    {
        /// <summary>
        /// 实例化一个新的站点设置信息的阅读着对象（后台）
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public AdminSystemSettingReader(DbContext db)
            : base(db)
        {
        }

        /// <summary>
        /// 读取银行账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回银行账户的分页列表</returns>
        public PaginationList<BankAccountResult> ReadBankAccountList(int page)
        {
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var baSet = db.Set<BankAccount>();

            int tCount = baSet
                .Count();
            List<BankAccountResult> tList = baSet
                .OrderBy(x => x.Order)
                .OrderBy(x => x.Key)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new BankAccountResult(x));

            return new PaginationList<BankAccountResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取系统邮件账户的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回系统邮件账户的分页列表</returns>
        public PaginationList<EmailAccountResult> ReadEmailAccountList(int page)
        {
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var eaSet = db.Set<EmailAccount>();

            int tCount = eaSet
                .Count();
            List<EmailAccountResult> tList = eaSet
                .OrderBy(x => x.Key)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new EmailAccountResult(x));

            return new PaginationList<EmailAccountResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }

        /// <summary>
        /// 读取邮件服务地址的分页列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回邮件服务地址的分页列表</returns>
        public PaginationList<EmailClientResult> ReadEmailClientList(int page)
        {
            WebSetting webSetting = new WebSetting();
            int startRow = ControllerHelper.GetStartRow(page, webSetting.PageSizeForAdmin);
            var ecSet = db.Set<EmailClient>();

            int tCount = ecSet
                .Count();
            List<EmailClientResult> tList = ecSet
                .OrderBy(x => x.Key)
                .Skip(startRow)
                .Take(webSetting.PageSizeForAdmin)
                .ToList()
                .ConvertAll(x => new EmailClientResult(x));

            return new PaginationList<EmailClientResult>(page, webSetting.PageSizeForAdmin, tCount, tList);
        }
    }
}
