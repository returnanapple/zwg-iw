using System.Timers;

namespace IWorld.BLL
{
    /// <summary>
    /// 时间线的管理者对象
    /// </summary>
    public class TimeLineManager
    {
        #region 静态方法

        /// <summary>
        /// 初始化时间线任务
        /// </summary>
        public static void Initialize()
        {
            System.Threading.Thread thread = new System.Threading.Thread((obj) =>
                {
                    #region 时间线任务

                    Timer timer = new Timer(20000);
                    timer.Elapsed += (sender, e) =>
                    {
                        if (Interval20SecondEventHandler != null)
                        {
                            WebMapContext db = new WebMapContext();
                            Interval20SecondEventHandler(null, new NEventArgs(db, null));
                        }
                    };
                    timer.Start();

                    #endregion
                });
            thread.Start();
        }

        #endregion

        #region 有关事件

        /// <summary>
        /// 间隔20秒
        /// </summary>
        public static event NDelegate Interval20SecondEventHandler;

        #endregion
    }
}
