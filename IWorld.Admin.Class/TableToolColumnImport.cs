
namespace IWorld.Admin.Class
{
    /// <summary>
    /// 表格工具的列信息封装
    /// </summary>
    public class TableToolColumnImport
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 实例化一个新的表格工具的列信息封装
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="width">列宽</param>
        public TableToolColumnImport(string name, double width)
        {
            this.Name = name;
            this.Width = width;
        }
    }
}
