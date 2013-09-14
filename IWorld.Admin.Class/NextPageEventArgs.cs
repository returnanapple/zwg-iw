using System;

namespace IWorld.Admin.Class
{
    /// <summary>
    /// 翻页动作的监视者对象
    /// </summary>
    public class NextPageEventArgs : EventArgs
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// 目标页
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// 实例化一个新的翻页动作的监视者对象
        /// </summary>
        /// <param name="_to">目标页</param>
        public NextPageEventArgs(int _to)
        {
            this.From = 0;
            this.To = _to;
        }

        /// <summary>
        /// 实例化一个新的翻页动作的监视者对象
        /// </summary>
        /// <param name="_from">当前页</param>
        /// <param name="_to">目标页</param>
        public NextPageEventArgs(int _from, int _to)
        {
            this.From = _from;
            this.To = _to;
        }
    }
}
