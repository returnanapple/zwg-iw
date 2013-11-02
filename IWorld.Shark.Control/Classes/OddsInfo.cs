using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IWorld.Shark.Control
{
    /// <summary>
    /// 赔率信息
    /// </summary>
    public class OddsInfo
    {
        /// <summary>
        /// 下注名称
        /// </summary>
        public IconOfJaw OddsName { get; set; }
        /// <summary>
        /// 赔率
        /// </summary>
        public int OddsValue { get; set; }

        public OddsInfo(IconOfJaw oddsName,int oddsValue)
        {
            this.OddsName = oddsName;
            this.OddsValue = oddsValue;
        }
    }
}
