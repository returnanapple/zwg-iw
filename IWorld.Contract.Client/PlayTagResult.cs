using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IWorld.Model;

namespace IWorld.Contract.Client
{
    /// <summary>
    /// 玩法标签信息
    /// </summary>
    [DataContract]
    public class PlayTagResult
    {
        #region 公开属性

        /// <summary>
        /// 玩法标签的数据库存储指针
        /// </summary>
        [DataMember]
        public int PlayTagId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 下辖玩法玩法
        /// </summary>
        [DataMember]
        public List<HowToPlayResult> HowToPlays { get; set; }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法标签信息
        /// </summary>
        /// <param name="tag">玩法标签信息的数据封装</param>
        public PlayTagResult(PlayTag tag)
        {
            this.PlayTagId = tag.Id;
            this.Name = tag.Name;
            this.HowToPlays = tag.HowToPlays.Where(x => x.Hide == false).ToList().ConvertAll(x => new HowToPlayResult(x));
        }

        #endregion
    }
}
