using System.Collections.Generic;
using System.Xml.Linq;

namespace IWorld.Setting
{
    /// <summary>
    /// 站点配置信息（该类不能继承）
    /// </summary>
    public sealed class WebSetting : SettingBase<WebSetting>
    {
        #region 保护方法

        /// <summary>
        /// 创建新的配置文件
        /// </summary>
        /// <param name="path">所要放置新的配置文件的路径</param>
        protected override void SetFile(string path)
        {
            XElement _e = new XElement("Websetting"
                , new XElement("Name", "")
                , new XElement("Title", "")
                , new XElement("Rootpath", "")
                , new XElement("Description", "")
                , new XElement("Keywords", "")
                , new XElement("PageSizeForClient", 12)
                , new XElement("PageSizeForAdmin", 20)
                , new XElement("HeartbeatInterval", 60)
                , new XElement("SpreadKeepTime", 48)
                , new XElement("WorkingHour_Begin", "8:00")
                , new XElement("WorkingHour_End", "17:00")
                , new XElement("VirtualQueuing", 3)
                , new XElement("TimeOfNoticeShow", 5)
                , new XElement("Withdrawals", 3)
                , new XElement("CollectionRunning", true)
                , new XElement("UserInTime", 10)
                , new XElement("MinimumRechargeAmount", 10)
                , new XElement("MaximumRechargeAmount", 1000000)
                , new XElement("Subordinate", 4)
                , new XElement("MinimumWithdrawalAmount", 10)
                , new XElement("MaximumWithdrawalAmount", 100000)
                , new XElement("WithdrawalInstructions", "取款时间从每日8:00-17:00 单次取款限额为10-100000元")
                , new XElement("MaximumReturnPoints", 13)
                , new XElement("MinimumReturnPoints", 4.5)
                , new XElement("ReturnPointsDifference", 0.5)
                , new XElement("UnitPrice", 2)
                , new XElement("PayoutBase", 1700)
                , new XElement("LineForProhibitBetting", 1950)
                , new XElement("MaximumBonusMode", 1950)
                , new XElement("ReferenceBonusMode", 1700)
                , new XElement("MinimumBonusMode", 1700)
                , new XElement("MaximumPayout", 100000)
                , new XElement("ReturnPointsScale", 0.1)
                , new XElement("BonusModeScale", 2)
                , new XElement("ConversionRates", 20)
                , new XElement("MaximumBetsNumber", 12)
                , new XElement("ClosureSingleTime", 30)
                , new XElement("Banks", "中国工商银行,中国农业银行,中国银行,中国建设银行,交通银行,财付通")
                , new XElement("ProfitabilityOfAllDayLottery", 50));
            _e.Save(path);
        }

        #endregion

        #region 公开属性

        #region 系统设置

        /// <summary>
        /// 网站名称
        /// </summary>
        public string Name
        {
            get { return GetStringValue("Name", ""); }
            set { SetValue("Name", value); }
        }

        /// <summary>
        /// 网站标题
        /// </summary>
        public string Title
        {
            get { return GetStringValue("Title", ""); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public string Rootpath
        {
            get { return GetStringValue("Rootpath", ""); }
            set { SetValue("Rootpath", value); }
        }

        /// <summary>
        /// 站点介绍
        /// </summary>
        public string Description
        {
            get { return GetStringValue("Description", ""); }
            set { SetValue("Description", value); }
        }

        /// <summary>
        /// 站点关键字
        /// </summary>
        public string Keywords
        {
            get { return GetStringValue("Keywords", ""); }
            set { SetValue("Keywords", value); }
        }

        /// <summary>
        /// 前台页面大小（条）
        /// </summary>
        public int PageSizeForClient
        {
            get { return GetIntValue("PageSizeForClient", 16); }
            set { SetValue("PageSizeForClient", value); }
        }

        /// <summary>
        /// 后台页面大小（条）
        /// </summary>
        public int PageSizeForAdmin
        {
            get { return GetIntValue("PageSizeForAdmin", 20); }
            set { SetValue("PageSizeForAdmin", value); }
        }

        /// <summary>
        /// 心跳间隔（秒）
        /// </summary>
        public int HeartbeatInterval
        {
            get { return GetIntValue("HeartbeatInterval", 60); }
            set { SetValue("HeartbeatInterval", value); }
        }

        /// <summary>
        /// 推广记录保持时间（小时）
        /// </summary>
        public int SpreadKeepTime
        {
            get { return GetIntValue("SpreadKeepTime", 48); }
            set { SetValue("SpreadKeepTime", value); }
        }

        /// <summary>
        /// 允许取款时间 - 开始
        /// </summary>
        public string WorkingHour_Begin
        {
            get { return GetStringValue("WorkingHour_Begin", "8:00"); }
            set { SetValue("WorkingHour_Begin", value); }
        }

        /// <summary>
        /// 允许取款时间 - 结束
        /// </summary>
        public string WorkingHour_End
        {
            get { return GetStringValue("WorkingHour_End", "17:00"); }
            set { SetValue("WorkingHour_End", value); }
        }

        /// <summary>
        /// 取款虚拟排队
        /// </summary>
        public int VirtualQueuing
        {
            get { return GetIntValue("VirtualQueuing", 3); }
            set { SetValue("VirtualQueuing", value); }
        }

        /// <summary>
        /// 通知显示时间（秒）
        /// </summary>
        public int TimeOfNoticeShow
        {
            get { return GetIntValue("TimeOfNoticeShow", 5); }
            set { SetValue("TimeOfNoticeShow", value); }
        }

        /// <summary>
        /// 每日允许提现次数
        /// </summary>
        public int Withdrawals
        {
            get { return GetIntValue("Withdrawals", 3); }
            set { SetValue("Withdrawals", value); }
        }

        /// <summary>
        /// 运行采集程序
        /// </summary>
        public bool CollectionRunning
        {
            get { return GetBooleanValue("CollectionRunning", true); }
            set { SetValue("CollectionRunning", value); }
        }

        #endregion

        #region 用户相关

        /// <summary>
        /// 用户登录状态保持的时间（分钟）
        /// </summary>
        public int UserInTime
        {
            get { return GetIntValue("UserInTime", 10); }
            set { SetValue("UserInTime", value); }
        }

        /// <summary>
        /// 最小充值额度
        /// </summary>
        public double MinimumRechargeAmount
        {
            get { return GetDoubleValue("MinimumRechargeAmount", 10); }
            set { SetValue("MinimumRechargeAmount", value); }
        }

        /// <summary>
        /// 最大充值额度
        /// </summary>
        public double MaximumRechargeAmount
        {
            get { return GetDoubleValue("MaximumRechargeAmount", 1000000); }
            set { SetValue("MaximumRechargeAmount", value); }
        }

        /// <summary>
        /// 允许用户创建直属下级数量（个）
        /// </summary>
        public int Subordinate
        {
            get { return GetIntValue("Subordinate", 4); }
            set { SetValue("Subordinate", value); }
        }

        /// <summary>
        /// 单笔最低取款金额
        /// </summary>
        public double MinimumWithdrawalAmount
        {
            get { return GetDoubleValue("MinimumWithdrawalAmount", 10); }
            set { SetValue("MinimumWithdrawalAmount", value); }
        }

        /// <summary>
        /// 单笔最高取款金额
        /// </summary>
        public double MaximumWithdrawalAmount
        {
            get { return GetDoubleValue("MaximumWithdrawalAmount", 100000); }
            set { SetValue("MaximumWithdrawalAmount", value); }
        }

        /// <summary>
        /// 取款说明
        /// </summary>
        public string WithdrawalInstructions
        {
            get { return GetStringValue("WithdrawalInstructions", "取款时间从每日8:00-17:00 单次取款限额为10-100000元"); }
            set { SetValue("WithdrawalInstructions", value); }
        }

        /// <summary>
        /// 最大返点数
        /// </summary>
        public double MaximumReturnPoints
        {
            get { return GetDoubleValue("MaximumReturnPoints", 13); }
            set { SetValue("MaximumReturnPoints", value); }
        }

        /// <summary>
        /// 最小返点数
        /// </summary>
        public double MinimumReturnPoints
        {
            get { return GetDoubleValue("MinimumReturnPoints", 4.5); }
            set { SetValue("MinimumReturnPoints", value); }
        }

        /// <summary>
        /// 上下级之间的最小返点数差额
        /// </summary>
        public double ReturnPointsDifference
        {
            get { return GetDoubleValue("ReturnPointsDifference", 0.5); }
            set { SetValue("ReturnPointsDifference", value); }
        }

        #endregion

        #region 彩票相关

        /// <summary>
        /// 单注价格
        /// </summary>
        public double UnitPrice
        {
            get { return GetDoubleValue("UnitPrice", 2); }
            set { SetValue("UnitPrice", value); }
        }

        /// <summary>
        /// 返奖基数
        /// （2 : n）
        /// </summary>
        public int PayoutBase
        {
            get { return GetIntValue("PayoutBase", 1700); }
            set { SetValue("PayoutBase", value); }
        }

        /// <summary>
        /// 禁止投注的基数线
        /// </summary>
        public int LineForProhibitBetting
        {
            get { return GetIntValue("LineForProhibitBetting", 1950); }
            set { SetValue("LineForProhibitBetting", value); }
        }

        /// <summary>
        /// 最大奖金模式
        /// </summary>
        public int MaximumBonusMode
        {
            get { return GetIntValue("MaximumBonusMode", 1950); }
            set { SetValue("MaximumBonusMode", value); }
        }

        /// <summary>
        /// 基准奖金模式
        /// </summary>
        public int ReferenceBonusMode
        {
            get { return GetIntValue("ReferenceBonusMode", 1700); }
            set { SetValue("ReferenceBonusMode", value); }
        }

        /// <summary>
        /// 最小奖金模式
        /// </summary>
        public int MinimumBonusMode
        {
            get { return GetIntValue("MinimumBonusMode", 1700); }
            set { SetValue("MinimumBonusMode", value); }
        }

        /// <summary>
        /// 返奖金额上限
        /// </summary>
        public int MaximumPayout
        {
            get { return GetIntValue("MaximumPayout", 100000); }
            set { SetValue("MaximumPayout", value); }
        }

        /// <summary>
        /// 返点数刻度
        /// </summary>
        public double ReturnPointsScale
        {
            get { return GetDoubleValue("ReturnPointsScale", 0.1); }
            set { SetValue("ReturnPointsScale", value); }
        }

        /// <summary>
        /// 奖金模式刻度
        /// </summary>
        public double BonusModeScale
        {
            get { return GetDoubleValue("BonusModeScale", 2); }
            set { SetValue("BonusModeScale", value); }
        }

        /// <summary>
        /// 奖金 - 返点换算率
        /// </summary>
        public double ConversionRates
        {
            get { return GetDoubleValue("ConversionRates", 20); }
            set { SetValue("ConversionRates", value); }
        }

        /// <summary>
        /// 最大投注倍数
        /// </summary>
        public int MaximumBetsNumber
        {
            get { return GetIntValue("MaximumBetsNumber", 12); }
            set { SetValue("MaximumBetsNumber", value); }
        }

        /// <summary>
        /// 封单时间（秒）
        /// </summary>
        public int ClosureSingleTime
        {
            get { return GetIntValue("ClosureSingleTime", 30); }
            set { SetValue("ClosureSingleTime", value); }
        }

        /// <summary>
        /// 当前支持的付款/提现银行
        /// </summary>
        public string Banks
        {
            get { return GetStringValue("Banks", "中国工商银行,中国农业银行,中国银行,中国建设银行,交通银行,财付通"); }
            set { SetValue("Banks", value); }
        }

        /// <summary>
        /// 全天彩盈利率
        /// </summary>
        public double ProfitabilityOfAllDayLottery
        {
            get { return GetDoubleValue("ProfitabilityOfAllDayLottery", 50); }
            set { SetValue("ProfitabilityOfAllDayLottery", value); }
        }

        /// <summary>
        /// 大白鲨盈利率
        /// </summary>
        public double ProfitabilityOfJaw
        {
            get { return GetDoubleValue("ProfitabilityOfJaw", 50); }
            set { SetValue("ProfitabilityOfJaw", value); }
        }

        #endregion

        #endregion

        #region 实例方法

        /// <summary>
        /// 获取属性的中-英文对照
        /// </summary>
        /// <returns>返回属性的中-英文对照</returns>
        public Dictionary<string, string> GetContrast()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add("Name", "网站名称");
            result.Add("Title", "网站标题");
            result.Add("Rootpath", "域名");
            result.Add("Description", "站点介绍");
            result.Add("Keywords", "站点关键字");
            result.Add("PageSizeForClient", "前台页面大小（条）");
            result.Add("PageSizeForAdmin", "后台页面大小（条）");
            result.Add("HeartbeatInterval", "心跳间隔（秒）");
            result.Add("SpreadKeepTime", "推广记录保持时间（小时）");
            result.Add("WorkingHour_Begin", "允许取款时间 - 开始");
            result.Add("WorkingHour_End", "允许取款时间 - 结束");
            result.Add("VirtualQueuing", "取款虚拟排队");
            result.Add("TimeOfNoticeShow", "通知显示时间（秒）");
            result.Add("Withdrawals", "每日允许提现次数");
            result.Add("CollectionRunning", "运行采集程序");

            result.Add("UserInTime", "用户登录状态保持的时间（分钟）");
            result.Add("MinimumRechargeAmount", "最小充值额度");
            result.Add("MaximumRechargeAmount", "最大充值额度");
            result.Add("Subordinate", "允许用户创建直属下级数量（个）");
            result.Add("MinimumWithdrawalAmount", "单笔最低取款金额");
            result.Add("MaximumWithdrawalAmount", "单笔最高取款金额");
            result.Add("WithdrawalInstructions", "取款说明");
            result.Add("MaximumReturnPoints", "最大返点数");
            result.Add("MinimumReturnPoints", "最小返点数");
            result.Add("ReturnPointsDifference", "上下级之间的最小返点数差额");

            result.Add("UnitPrice", "单注价格");
            result.Add("PayoutBase", "返奖基数（2 : n）");
            result.Add("LineForProhibitBetting", "禁止投注的基数线");
            result.Add("MaximumBonusMode", "最大奖金模式");
            result.Add("ReferenceBonusMode", "基准奖金模式");
            result.Add("MinimumBonusMode", "最小奖金模式");
            result.Add("MaximumPayout", "返奖金额上限");
            result.Add("ReturnPointsScale", "返点数刻度");
            result.Add("BonusModeScale", "奖金模式刻度");
            result.Add("ConversionRates", "奖金 - 返点换算率");
            result.Add("MaximumBetsNumber", "最大投注倍数");
            result.Add("ClosureSingleTime", "封单时间（秒）");
            result.Add("Banks", "当前支持的付款/提现银行");
            result.Add("ProfitabilityOfAllDayLottery", "全天彩盈利率");

            return result;
        }

        #endregion
    }
}
