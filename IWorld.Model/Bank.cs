using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IWorld.Model
{
    /// <summary>
    /// 银行
    /// </summary>
    [DataContract]
    public enum Bank
    {
        //无
        [EnumMember]
        无 = 0,
        //大型商业银行
        [EnumMember]
        中国工商银行 = 101,
        [EnumMember]
        中国农业银行 = 102,
        [EnumMember]
        中国银行 = 103,
        [EnumMember]
        中国建设银行 = 104,
        [EnumMember]
        交通银行 = 105,
        [EnumMember]
        招商银行 = 106,
        [EnumMember]
        民生银行 = 107,
        [EnumMember]
        邮政存储 = 108,
        //网络支付
        [EnumMember]
        财付通 = 201
    }
}
