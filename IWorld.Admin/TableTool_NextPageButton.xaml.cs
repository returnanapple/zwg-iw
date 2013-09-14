using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IWorld.Admin.Class;

namespace IWorld.Admin
{
    /// <summary>
    /// 表格工具的分页按键
    /// </summary>
    public partial class TableTool_NextPageButton : UserControl
    {
        #region 缓存字段/事件

        /// <summary>
        /// 标识 | 允许点击
        /// </summary>
        private bool canClick = true;

        /// <summary>
        /// 翻页按键被触发时候将触发的事件
        /// </summary>
        public event NextPageDelegate NextPageEventHandler;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的表格工具的分页按键
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="column">行</param>
        /// <param name="isSelected">标识 | 是否已经被选中（默认为否）</param>
        /// <param name="text">显示文本（可空 如果为空或""则显示当前页码）</param>
        public TableTool_NextPageButton(int pageIndex, int column, bool isSelected = false, string text = "")
        {
            InitializeComponent();
            this.Margin = new Thickness(0, 0, 5, 0);
            this.SetValue(Grid.ColumnProperty, column);
            text_pageIndex.Text = text == "" ? pageIndex.ToString() : text;
            if (isSelected)
            {
                root.Cursor = Cursors.Arrow;
                bg.Style = (Style)this.Resources["border_selected"];
                text_pageIndex.Style = (Style)this.Resources["text_selected"];
                this.canClick = false;
            }
            #region 鼠标事件
            if (canClick)
            {
                root.MouseEnter += (sender, e) =>
                    {
                        bg.Style = (Style)this.Resources["border_hover"];
                    };
                root.MouseLeave += (sender, e) =>
                    {
                        bg.Style = (Style)this.Resources["border_normal"];
                    };
                root.MouseLeftButtonDown += (sender, e) =>
                    {
                        if (NextPageEventHandler != null)
                        {
                            NextPageEventHandler(this, new NextPageEventArgs(pageIndex));
                        }
                    };
            }
            #endregion
        }

        #endregion
    }
}
