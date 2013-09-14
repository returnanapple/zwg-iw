
namespace IWorld.Helper
{
    /// <summary>
    /// 控制器的帮助者对象
    /// </summary>
    public class ControllerHelper
    {
        /// <summary>
        /// 将不确切的页码转换为确切的页码
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns>返回确切的页码</returns>
        public static int CheckPage(int? page)
        {
            return page == null ? 1 : (int)page;
        }

        /// <summary>
        /// 获取当前页的初始行数
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <returns>返回当前页的初始行数</returns>
        public static int GetStartRow(int pageIndex, int pageSize)
        {
            return (pageIndex - 1) * pageSize;
        }
    }
}
