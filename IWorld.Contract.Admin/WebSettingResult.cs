using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using IWorld.Setting;

namespace IWorld.Contract.Admin
{
    /// <summary>
    /// 系统设置信息
    /// </summary>
    [DataContract]
    public class WebSettingResult : OperateResult
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 网站标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [DataMember]
        public string Rootpath { get; set; }

        /// <summary>
        /// 站点介绍
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 站点关键字
        /// </summary>
        [DataMember]
        public string Keywords { get; set; }

        /// <summary>
        /// 前台页面大小（条）
        /// </summary>
        [DataMember]
        public int PageSizeForClient { get; set; }

        /// <summary>
        /// 后台页面大小（条）
        /// </summary>
        [DataMember]
        public int PageSizeForAdmin { get; set; }

        /// <summary>
        /// 心跳间隔（秒）
        /// </summary>
        [DataMember]
        public int HeartbeatInterval { get; set; }

        /// <summary>
        /// 推广记录保持时间（小时）
        /// </summary>
        [DataMember]
        public int SpreadKeepTime { get; set; }

        /// <summary>
        /// 允许取款时间 - 开始
        /// </summary>
        [DataMember]
        public string WorkingHour_Begin { get; set; }

        /// <summary>
        /// 允许取款时间 - 结束
        /// </summary>
        [DataMember]
        public string WorkingHour_End { get; set; }

        /// <summary>
        /// 取款虚拟排队
        /// </summary>
        [DataMember]
        public int VirtualQueuing { get; set; }

        /// <summary>
        /// 通知显示时间（秒）
        /// </summary>
        [DataMember]
        public int TimeOfNoticeShow { get; set; }

        /// <summary>
        /// 每日允许提现次数
        /// </summary>
        [DataMember]
        public int Withdrawals { get; set; }

        /// <summary>
        /// 运行采集程序
        /// </summary>
        [DataMember]
        public bool CollectionRunning { get; set; }

        /// <summary>
        /// 用户登录状态保持的时间（分钟）
        /// </summary>
        [DataMember]
        public int UserInTime { get; set; }

        /// <summary>
        /// 最小充值额度
        /// </summary>
        [DataMember]
        public double MinimumRechargeAmount { get; set; }

        /// <summary>
        /// 最大充值额度
        /// </summary>
        [DataMember]
        public double MaximumRechargeAmount { get; set; }

        /// <summary>
        /// 允许用户创建直属下级数量（个）
        /// </summary>
        [DataMember]
        public int Subordinate { get; set; }

        /// <summary>
        /// 单笔最低取款金额
        /// </summary>
        [DataMember]
        public double MinimumWithdrawalAmount { get; set; }

        /// <summary>
        /// 单笔最高取款金额
        /// </summary>
        [DataMember]
        public double MaximumWithdrawalAmount { get; set; }

        /// <summary>
        /// 取款说明
        /// </summary>
        [DataMember]
        public string WithdrawalInstructions { get; set; }

        /// <summary>
        /// 最大返点数
        /// </summary>
        [DataMember]
        public double MaximumReturnPoints { get; set; }

        /// <summary>
        /// 最小返点数
        /// </summary>
        [DataMember]
        public double MinimumReturnPoints { get; set; }

        /// <summary>
        /// 上下级之间的最小返点数差额
        /// </summary>
        [DataMember]
        public double ReturnPointsDifference { get; set; }

        /// <summary>
        /// 单注价格
        /// </summary>
        [DataMember]
        public double UnitPrice { get; set; }

        /// <summary>
        /// 返奖基数
        /// （2 : n）
        /// </summary>
        [DataMember]
        public int PayoutBase { get; set; }

        /// <summary>
        /// 禁止投注的基数线
        /// </summary>
        [DataMember]
        public int LineForProhibitBetting { get; set; }

        /// <summary>
        /// 最大奖金模式
        /// </summary>
        [DataMember]
        public int MaximumBonusMode { get; set; }

        /// <summary>
        /// 基准奖金模式
        /// </summary>
        [DataMember]
        public int ReferenceBonusMode { get; set; }

        /// <summary>
        /// 最小奖金模式
        /// </summary>
        [DataMember]
        public int MinimumBonusMode { get; set; }

        /// <summary>
        /// 返奖金额上限
        /// </summary>
        [DataMember]
        public int MaximumPayout { get; set; }

        /// <summary>
        /// 返点数刻度
        /// </summary>
        [DataMember]
        public double ReturnPointsScale { get; set; }

        /// <summary>
        /// 奖金模式刻度
        /// </summary>
        [DataMember]
        public double BonusModeScale { get; set; }

        /// <summary>
        /// 奖金 - 返点换算率
        /// </summary>
        [DataMember]
        public double ConversionRates { get; set; }

        /// <summary>
        /// 最大投注倍数
        /// </summary>
        [DataMember]
        public int MaximumBetsNumber { get; set; }

        /// <summary>
        /// 封单时间（分钟）
        /// </summary>
        [DataMember]
        public int ClosureSingleTime { get; set; }

        /// <summary>
        /// 当前支持的付款/提现银行
        /// </summary>
        [DataMember]
        public string Banks { get; set; }

        /// <summary>
        /// 属性名对照
        /// </summary>
        [DataMember]
        public Dictionary<string, string> TheContrast { get; set; }

        /// <summary>
        /// 实例化一个新的系统设置信息（成功）
        /// </summary>
        public WebSettingResult()
        {
            WebSetting webSetting = new WebSetting();
            this.TheContrast = webSetting.GetContrast();
            System.Type type = typeof(WebSetting);
            typeof(WebSettingResult).GetProperties().ToList()
                .ForEach(x =>
                    {
                        List<string> ignore = new List<string> { "Success", "Error", "TheContrast" };
                        if (!ignore.Contains(x.Name))
                        {
                            object val = type.GetProperty(x.Name).GetValue(webSetting);
                            x.SetValue(this, val);
                        }
                    });
        }

        /// <summary>
        /// 实例化一个新的系统设置信息（失败）
        /// </summary>
        /// <param name="error">错误信息</param>
        public WebSettingResult(string error)
            : base(error)
        {
        }
    }
}
