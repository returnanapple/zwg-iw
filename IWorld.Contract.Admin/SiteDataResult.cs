using System.Linq;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 站点信息统计
    /// </summary>
    [DataContract]
    public class SiteDataResult
    {
        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
        public string Date { get; set; }

        /// <summary>
        /// 登陆人数
        /// </summary>
        [DataMember]
        public int CountOfSetIn { get; set; }

        /// <summary>
        /// 注册人数
        /// </summary>
        [DataMember]
        public int CountOfSetUp { get; set; }

        /// <summary>
        /// 投注额
        /// </summary>
        [DataMember]
        public double AmountOfBets { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 活动返还
        /// </summary>
        [DataMember]
        public double Expenditures { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        [DataMember]
        public double GainsAndLosses { get; set; }

        /// <summary>
        /// 充值
        /// </summary>
        [DataMember]
        public double Recharge { get; set; }

        /// <summary>
        /// 提现
        /// </summary>
        [DataMember]
        public double Withdrawal { get; set; }

        /// <summary>
        /// 支取
        /// </summary>
        [DataMember]
        public double Transfer { get; set; }

        /// <summary>
        /// 现金流
        /// </summary>
        [DataMember]
        public double Cash { get; set; }

        /// <summary>
        /// 实例化一个新的站点信息统计
        /// </summary>
        /// <param name="siteDataAtDay">站点信息统计的数据封装</param>
        public SiteDataResult(SiteDataAtDay siteDataAtDay)
        {
            this.Date = string.Format("{0}-{1}-{2}", siteDataAtDay.Year, siteDataAtDay.Month, siteDataAtDay.Day);
            System.Type type = typeof(SiteDataAtDay);
            typeof(SiteDataResult).GetProperties().ToList()
                .ForEach(x =>
                {
                    if (x.Name != "Date")
                    {
                        object val = type.GetProperty(x.Name).GetValue(siteDataAtDay);
                        x.SetValue(this, val);
                    }
                });
        }

        /// <summary>
        /// 实例化一个新的站点信息统计
        /// </summary>
        /// <param name="siteDataAtDay">站点信息统计的数据封装</param>
        public SiteDataResult(SiteDataAtMonth siteDataAtMonth)
        {
            this.Date = string.Format("{0}-{1}", siteDataAtMonth.Year, siteDataAtMonth.Month);
            System.Type type = typeof(SiteDataAtMonth);
            typeof(SiteDataResult).GetProperties().ToList()
                .ForEach(x =>
                {
                    if (x.Name != "Date")
                    {
                        object val = type.GetProperty(x.Name).GetValue(siteDataAtMonth);
                        x.SetValue(this, val);
                    }
                });
        }
    }
}
