using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace IWorld.Shark.Control.Classes
{
    public class ResultNote : INotifyPropertyChanged
    {
        #region 私有字段
        /// <summary>
        /// 历史结果
        /// </summary>
        IconOfJaw resultNoteName;
        /// <summary>
        /// 历史结果期号
        /// </summary>
        string resultNoteNumber;
        #endregion

        #region 属性
        /// <summary>
        /// 历史结果
        /// </summary>
        public IconOfJaw ResultNoteName
        {
            get
            { return resultNoteName; }
            set
            {
                resultNoteName = value;
                OnPropertyChanged(this, "ResultNoteName");
            }
        }
        /// <summary>
        /// 历史结果期号
        /// </summary>
        public string ResultNoteNumber
        {
            get
            { return resultNoteNumber; }
            set
            {
                resultNoteNumber = value;
                OnPropertyChanged(this, "ResultNoteNumber");
            }
        }
        #endregion

        #region 构造函数
        public ResultNote(IconOfJaw resultNoteName, string resultNoteNumber)
        {
            this.ResultNoteName = resultNoteName;
            this.ResultNoteNumber = resultNoteNumber;
        }
        #endregion

        #region 属性变更事件
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(object sender, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
