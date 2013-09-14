using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 可选位信息
    /// </summary>
    [DataContract]
    public class OptionalSeatResult
    {
        #region 公开属性

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 对应的号码的集合的列表（只读）
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }

        /// <summary>
        /// “大”选项所选号码集合的列表（只读）
        /// </summary>
        [DataMember]
        public List<string> ValuesForLarge { get; set; }

        /// <summary>
        /// “小”选项所选号码集合的列表（只读）
        /// </summary>
        [DataMember]
        public List<string> ValuesForSmall { get; set; }

        /// <summary>
        /// “单”选项所选号码集合的列表（只读）
        /// </summary>
        [DataMember]
        public List<string> ValuesForSingle { get; set; }

        /// <summary>
        /// “双”选项所选号码集合的列表（只读）
        /// </summary>
        [DataMember]
        public List<string> ValuesForDouble { get; set; }

        /// <summary>
        /// 排序系数（继承于对应彩种的同名系数）
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// 最少选号
        /// </summary>
        [DataMember]
        public int LimitOfPick { get; set; }

        /// <summary>
        /// 最多选号
        /// </summary>
        [DataMember]
        public int UpperOfPick { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的可选位信息
        /// </summary>
        /// <param name="seat">可选位信息的数据封装</param>
        public OptionalSeatResult(OptionalSeat seat)
        {
            this.Name = seat.Name;
            this.Values = seat.ValueList;
            this.ValuesForLarge = seat.ValuesForLargeList;
            this.ValuesForSmall = seat.ValuesForSmallList;
            this.ValuesForSingle = seat.ValuesForSingleList;
            this.ValuesForDouble = seat.ValuesForDoubleList;
            this.Order = seat.Order;
            this.LimitOfPick = seat.LimitOfPick;
            this.UpperOfPick = seat.UpperOfPick;
        }

        #endregion
    }
}
