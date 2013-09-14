using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IWorld.Admin.Class;
using IWorld.Admin.LotteryTicketService;

namespace IWorld.Admin
{
    public partial class ExamineTagsPage : UserControl
    {
        #region 私有变量

        int ticketId = 0;

        int pageIndex = 1;

        string keyword = "";

        HideOrNotSelectType selectType = HideOrNotSelectType.全部;

        #endregion

        public ExamineTagsPage(int ticketId = 0)
        {
            InitializeComponent();
            this.ticketId = ticketId;
            InsertTable();
        }

        #region 事件

        public event NDelegate ViewPlaysEventHandler;

        #endregion

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("名称", 180));
            columns.Add(new TableToolColumnImport("所属彩票", 180));
            columns.Add(new TableToolColumnImport("玩法", 180));
            columns.Add(new TableToolColumnImport("排序", 180));
            columns.Add(new TableToolColumnImport("操作", 192));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.GetPlayTagListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        ExamineTagsPage_TableRow row = new ExamineTagsPage_TableRow(x, t);
                        row.SelectForTicketEventHandler += SelectForTicket;
                        row.ViewPlaysEventHandler += ViewPlays;
                        row.RefreshEventHandler += Refresh;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看玩法标签信息", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetPlayTagListAsync(ticketId, keyword, selectType, pageIndex, App.Token);
        }

        void SelectForTicket(object sender, EventArgs e)
        {
            ExamineTagsPage_TableRow row = (ExamineTagsPage_TableRow)sender;
            ticketId = row.PlayTag.TicketId;
            pageIndex = 1;

            InsertTable();
        }

        void ViewPlays(object sender, EventArgs e)
        {
            if (ViewPlaysEventHandler != null)
            {
                ViewPlaysEventHandler(sender, e);
            }
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertTable();
        }

        void GoNextPage(object sender, NextPageEventArgs e)
        {
            pageIndex = e.To;
            InsertTable();
        }

        private void SelectForIsS(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            this.selectType = (HideOrNotSelectType)Enum.Parse(typeof(HideOrNotSelectType), (string)button.Content, false);
            pageIndex = 1;
            InsertTable();
        }

        private void SelectForKeyword(object sender, EventArgs e)
        {
            keyword = input_keyword.Text;
            pageIndex = 1;
            InsertTable();
        }

        private void Reset(object sender, EventArgs e)
        {
            input_keyword.Text = "";
            input_selecetType_all.IsChecked = true;
            input_selecetType_show.IsChecked = false;
            input_selecetType_show.IsChecked = false;

            ticketId = 0;
            keyword = "";
            pageIndex = 1;
            selectType = HideOrNotSelectType.全部;

            InsertTable();
        }
    }
}
