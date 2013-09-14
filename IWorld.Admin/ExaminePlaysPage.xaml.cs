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
    public partial class ExaminePlaysPage : UserControl
    {
        #region 私有变量

        int ticketId = 0;

        int tagId = 0;

        int pageIndex = 1;

        string keyword = "";

        HideOrNotSelectType selectType = HideOrNotSelectType.全部;

        #endregion

        public ExaminePlaysPage(int ticketId = 0, int tagId = 0)
        {
            InitializeComponent();
            this.ticketId = ticketId;
            this.tagId = tagId;

            InsertTable();
        }

        void InsertTable()
        {
            List<TableToolColumnImport> columns = new List<TableToolColumnImport>();
            columns.Add(new TableToolColumnImport("名称", 152));
            columns.Add(new TableToolColumnImport("所属彩票", 152));
            columns.Add(new TableToolColumnImport("所属标签", 152));
            columns.Add(new TableToolColumnImport("返奖基数", 152));
            columns.Add(new TableToolColumnImport("排序", 152));
            columns.Add(new TableToolColumnImport("操作", 152));
            List<ITableToolRow> rows = new List<ITableToolRow>();

            LotteryTicketServiceClient client = new LotteryTicketServiceClient();
            client.GetHowToPlayListCompleted += (sender, e) =>
            {
                if (e.Result.Success)
                {
                    int t = 0;
                    e.Result.Content.ForEach(x =>
                    {
                        ExaminePlaysPage_TableRow row = new ExaminePlaysPage_TableRow(x, t);
                        row.SelectForTicketEventHandler += SelectForTicket;
                        row.SelectForTagEventHandler += SelectForTag;
                        row.RefreshEventHandler += Refresh;
                        rows.Add(row);
                        t++;
                    });
                    TableTool tool = new TableTool("查看玩法信息", e.Result.PageIndex, e.Result.TotalOfPage
                        , columns, rows);
                    tool.NextPageEventHandler += GoNextPage;
                    tableBody.Children.Clear();
                    tableBody.Children.Add(tool);
                }
            };
            client.GetHowToPlayListAsync(ticketId, tagId, keyword, selectType, pageIndex, App.Token);
        }

        void SelectForTicket(object sender, EventArgs e)
        {
            ExaminePlaysPage_TableRow row = (ExaminePlaysPage_TableRow)sender;
            ticketId = row.HowToPlay.TicketId;
            pageIndex = 1;

            InsertTable();
        }

        void SelectForTag(object sender, EventArgs e)
        {
            ExaminePlaysPage_TableRow row = (ExaminePlaysPage_TableRow)sender;
            tagId = row.HowToPlay.TagId;
            pageIndex = 1;

            InsertTable();
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
            tagId = 0;
            keyword = "";
            pageIndex = 1;
            selectType = HideOrNotSelectType.全部;

            InsertTable();
        }
    }
}
