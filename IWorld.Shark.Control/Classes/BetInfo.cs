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
using IWorld.Shark.Control.JawService;
namespace IWorld.Shark.Control.Classes
{
    public class BetInfo
    {
        public IconOfJaw BetName { get; set; }
        public int BetValue { get; set; }

        public BetInfo(IconOfJaw betName, int betValue)
        {
            this.BetName = betName;
            this.BetValue = betValue;
        }
    }
}
