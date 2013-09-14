using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IWorld.Client.Class;

namespace IWorld.Client
{
    /// <summary>
    /// 表格攻击
    /// </summary>
    public partial class TableTool : UserControl
    {
        #region 内置参数

        /// <summary>
        /// 当前显示页
        /// </summary>
        private int page = 0;

        /// <summary>
        /// 总宽
        /// </summary>
        private double mainWidth = 0;

        /// <summary>
        /// 总高
        /// </summary>
        private double mainHeight = 0;

        /// <summary>
        /// 标题高
        /// </summary>
        private double titleHeight = 35;

        /// <summary>
        /// 行高
        /// </summary>
        private double rowHeight = 20;

        /// <summary>
        /// 显示列名
        /// </summary>
        private bool haveColumnName = true;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的表格工具
        /// </summary>
        /// <param name="_title">标题</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="columns">列信息</param>
        /// <param name="rows">行信息</param>
        /// <param name="_haveColumnName">显示列名</param>
        public TableTool(string _title, int pageIndex, int totalPage, List<TableToolColumnImport> columns
            , List<ITableToolRow> rows, bool _haveColumnName = true)
        {
            InitializeComponent();
            this.page = pageIndex;
            this.mainWidth = columns.Sum(x => x.Width);
            this.mainHeight = titleHeight + rowHeight * rows.Count;
            this.haveColumnName = _haveColumnName;
            title.Text = _title;
            ShowTableBg(columns, rows);
            ShowBody(rows);
            if (pageIndex > 0 && totalPage > 0 && pageIndex <= totalPage)
            {
                ShowPageTool(pageIndex, totalPage);
            }
        }

        #endregion

        #region 描绘表格背景

        /// <summary>
        /// 行号 - 背景元素 对照
        /// </summary>
        private Dictionary<int, Border> _body_bgs = new Dictionary<int, Border>();

        /// <summary>
        /// 显示表格背景
        /// </summary>
        /// <param name="columns">列信息列表</param>
        /// <param name="rows">行数</param>
        public void ShowTableBg(List<TableToolColumnImport> columns, List<ITableToolRow> rows)
        {
            double firstRdHeight = haveColumnName ? titleHeight : 0;
            RowDefinition rd1 = new RowDefinition();
            rd1.Height = new GridLength(firstRdHeight);
            body_bg.RowDefinitions.Add(rd1);
            RowDefinition rd2 = new RowDefinition();
            rd2.Height = new GridLength(firstRdHeight);
            body.RowDefinitions.Add(rd2);
            #region 外框

            bg.Height = mainHeight;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(CreateLine(0, 0, mainWidth, 0));
            group.Children.Add(CreateLine(0, 0, 0, mainHeight));
            group.Children.Add(CreateLine(mainWidth, 0, mainWidth, mainHeight));
            if (haveColumnName)
            {
                group.Children.Add(CreateLine(0, titleHeight, mainWidth, titleHeight));
            }
            if (rows.Count != 0)
            {
                group.Children.Add(CreateLine(0, mainHeight, mainWidth, mainHeight));
            }

            #endregion
            #region 列名、内部分割线和背景
            Grid grid = new Grid();
            grid.Height = titleHeight;
            grid.SetValue(Grid.RowProperty, 0);
            body_bg.Children.Add(grid);

            double tx = 0;
            int t = 0;
            columns.ForEach(x =>
            {
                tx += x.Width;
                if (tx > 0 && tx < mainWidth)
                {
                    group.Children.Add(CreateLine(tx, 0, tx, mainHeight));
                }
                if (haveColumnName)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.Width = new GridLength(x.Width);
                    grid.ColumnDefinitions.Add(cd);

                    TextBlock tb = new TextBlock();
                    tb.Text = x.Name;
                    tb.Style = (Style)this.Resources["titleStyle"];
                    tb.SetValue(Grid.ColumnProperty, t);
                    grid.Children.Add(tb);
                }

                t++;
            });
            #endregion
            #region 行背景
            t = 1;
            rows.ForEach(x =>
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(rowHeight);
                body_bg.RowDefinitions.Add(rd);

                string styleName = t % 2 == 0 ? "bg_double" : "bg_single";
                Border border = new Border();
                border.Style = (Style)this.Resources[styleName];
                border.SetValue(Grid.RowProperty, t);
                body_bg.Children.Add(border);
                _body_bgs.Add(t, border);

                t++;
            });
            #endregion
            bg.Data = group;
        }

        /// <summary>
        /// 创建一条线段
        /// </summary>
        /// <param name="sx">起点x坐标</param>
        /// <param name="sy">起点y坐标</param>
        /// <param name="ex">终点x坐标</param>
        /// <param name="ey">终点y坐标</param>
        /// <returns></returns>
        private LineGeometry CreateLine(double sx, double sy, double ex, double ey)
        {
            LineGeometry result = new LineGeometry();
            result.StartPoint = new Point(sx, sy);
            result.EndPoint = new Point(ex, ey);
            return result;
        }

        /// <summary>
        /// 将被触发的行的背景改变为相关颜色
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void ChangeRowBgToHover(object sender, TableToolBodyHoverEventArgs e)
        {
            _body_bgs[e.Row + 1].Style = (Style)this.Resources["bg_hover"];
        }

        /// <summary>
        /// 将取消触发的行的背景改变为原来的颜色
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        private void ChangeRowBgToNormal(object sender, TableToolBodyHoverEventArgs e)
        {
            string _style = e.Row % 2 == 0 ? "bg_double" : "bg_single";
            _body_bgs[e.Row + 1].Style = (Style)this.Resources[_style];
        }

        #endregion

        #region 插入内容

        /// <summary>
        /// 显示表格主体
        /// </summary>
        /// <param name="rows">行信息</param>
        private void ShowBody(List<ITableToolRow> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(rowHeight);
                body.RowDefinitions.Add(rd);

                rows[i].HoverEventHandler += ChangeRowBgToHover;
                rows[i].UnhoverEventHandler += ChangeRowBgToNormal;

                FrameworkElement element = rows[i].GetElement();
                element.SetValue(Grid.RowProperty, i + 1);
                body.Children.Add(element);
            }
        }

        #endregion

        #region 分页和翻页

        /// <summary>
        /// 翻页按键被触发时候将触发的事件
        /// </summary>
        public event NextPageDelegate NextPageEventHandler;

        /// <summary>
        /// 触发翻页事件
        /// </summary>
        /// <param name="sender">触发对象</param>
        /// <param name="e">监视对象</param>
        public void GoToNextPage(object sender, NextPageEventArgs e)
        {
            if (NextPageEventHandler != null)
            {
                NextPageEventHandler(this, new NextPageEventArgs(this.page, e.To));
            }
        }

        /// <summary>
        /// 显示分页工具栏
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="totalPage"></param>
        private void ShowPageTool(int pageIndex, int totalPage)
        {
            pageTool.Children.Clear();
            pageTool.ColumnDefinitions.Clear();
            int range = 5;  //分页显示范围
            List<int> pages = new List<int>();
            #region 加载显示列表
            for (int i = pageIndex - range; i <= pageIndex + range; i++)
            {
                if (i > 0 && i <= totalPage)
                {
                    pages.Add(i);
                }
            }
            int all = 2 * range + 1;
            if (pages.Count < all)
            {
                int difference = all - pages.Count;
                if (pages.Count(x => x < pageIndex) < range)
                {
                    int max = pages.Max();
                    int limit = pages.Max() + difference;
                    for (int i = max + 1; i <= limit; i++)
                    {
                        if (i > 0 && i <= totalPage)
                        {
                            pages.Add(i);
                        }
                    }
                }
                else
                {
                    int min = pages.Min();
                    int limit = min - difference;
                    for (int i = limit; i < min; i++)
                    {
                        if (i > 0 && i <= totalPage)
                        {
                            pages.Add(i);
                        }
                    }
                }
            }
            pages = pages.OrderBy(x => x).ToList();
            #endregion
            for (int i = 0; i < pages.Count + 4; i++)
            {
                pageTool.ColumnDefinitions.Add(new ColumnDefinition());
                if (i == 0)
                {
                    if (pageIndex > 1)
                    {
                        TableTool_NextPageButton button = new TableTool_NextPageButton(1, i, false, "frist");
                        button.NextPageEventHandler += GoToNextPage;
                        pageTool.Children.Add(button);
                    }
                }
                else if (i == 1)
                {
                    if (pageIndex > 1)
                    {
                        TableTool_NextPageButton button = new TableTool_NextPageButton(pageIndex - 1, i, false, "before");
                        button.NextPageEventHandler += GoToNextPage;
                        pageTool.Children.Add(button);
                    }
                }
                else if (i == pages.Count + 2)
                {
                    if (pageIndex < totalPage)
                    {
                        TableTool_NextPageButton button = new TableTool_NextPageButton(pageIndex + 1, i, false, "next");
                        button.NextPageEventHandler += GoToNextPage;
                        pageTool.Children.Add(button);
                    }
                }
                else if (i == pages.Count + 3)
                {
                    if (pageIndex < totalPage)
                    {
                        TableTool_NextPageButton button = new TableTool_NextPageButton(totalPage, i, false, "last");
                        button.NextPageEventHandler += GoToNextPage;
                        pageTool.Children.Add(button);
                    }
                }
                else
                {
                    bool isSelected = pages[i - 2] == pageIndex;
                    TableTool_NextPageButton button = new TableTool_NextPageButton(pages[i - 2], i, isSelected);
                    button.NextPageEventHandler += GoToNextPage;
                    pageTool.Children.Add(button);
                }
            }
        }

        #endregion
    }
}
