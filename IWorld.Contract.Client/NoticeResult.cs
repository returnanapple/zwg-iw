using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 通知
    /// </summary>
    [DataContract]
    public class NoticeResult
    {
        /// <summary>
        /// 通知的存储指针
        /// </summary>
        [DataMember]
        public int NoticeId { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        [DataMember]
        public NoticeType Type { get; set; }

        /// <summary>
        /// 投注明细（如果有的话）
        /// </summary>
        [DataMember]
        public BettingDetailsResult BettingDetails { get; set; }

        /// <summary>
        /// 充值明细（如果有的话）
        /// </summary>
        [DataMember]
        public RechargeDetailsResult RechargeDetails { get; set; }

        public NoticeResult(Notice notice)
        {
            this.NoticeId = notice.Id;
            this.Context = notice.Context;
            this.Type = notice.Type;
        }

        public NoticeResult(Notice notice, RechargeRecord rr)
        {
            this.NoticeId = notice.Id;
            this.Context = notice.Context;
            this.Type = notice.Type;
            this.RechargeDetails = new RechargeDetailsResult(rr);
        }

        public NoticeResult(Notice notice, Betting betting, Lottery lottery)
        {
            this.NoticeId = notice.Id;
            this.Context = notice.Context;
            this.Type = notice.Type;
            this.BettingDetails = new BettingDetailsResult(betting, lottery);
        }
    }
}
