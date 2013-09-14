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
using IWorld.Admin.SystemSettingService;

namespace IWorld.Admin
{
    public partial class WebSiteSettingPage : UserControl
    {
        public WebSiteSettingPage()
        {
            InitializeComponent();
            InsertBody();
        }

        void InsertBody()
        {
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.GetWebSettingCompleted += ShowWebSetting;
            client.GetWebSettingAsync(App.Token);
        }

        void ShowWebSetting(object sender, GetWebSettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                int t = 0;
                List<string> ignore = new List<string> { "Success", "Error", "TheContrast" };
                typeof(WebSettingResult).GetProperties().ToList()
                    .ForEach(x =>
                        {
                            if (!ignore.Contains(x.Name))
                            {
                                RowDefinition rd = new RowDefinition();
                                rd.Height = new GridLength(30);
                                body.RowDefinitions.Add(rd);

                                TextBlock tb = new TextBlock();
                                tb.Text = e.Result.TheContrast[x.Name] + ":";
                                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                tb.SetValue(Grid.RowProperty, t);
                                tb.SetValue(Grid.ColumnProperty, 0);
                                body.Children.Add(tb);

                                TextBox box = new TextBox();
                                box.Name = "input_" + x.Name;
                                box.Width = 200;
                                box.Text = x.GetValue(e.Result, null).ToString();
                                box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                                box.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                box.SetValue(Grid.RowProperty, t);
                                box.SetValue(Grid.ColumnProperty, 2);
                                body.Children.Add(box);

                                t++;
                            }
                        });
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            EditWebSettingImport import = new EditWebSettingImport();
            typeof(EditWebSettingImport).GetProperties().ToList()
                .ForEach(x =>
                    {
                        TextBox box = (TextBox)FindName("input_" + x.Name);
                        if (x.PropertyType == typeof(int))
                        {
                            x.SetValue(import, Convert.ToInt32(box.Text), null);
                        }
                        else if (x.PropertyType == typeof(double))
                        {
                            x.SetValue(import, Convert.ToDouble(box.Text), null);
                        }
                        else if (x.PropertyType == typeof(bool))
                        {
                            x.SetValue(import, Convert.ToBoolean(box.Text), null);
                        }
                        else
                        {
                            x.SetValue(import, box.Text, null);
                        }
                    });
            SystemSettingServiceClient client = new SystemSettingServiceClient();
            client.EditWebSettingCompleted += ShowEditResult;
            client.EditWebSettingAsync(import, App.Token);
        }
        #region 编辑

        void ShowEditResult(object sender, EditWebSettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ErrorPrompt ep = new ErrorPrompt("修改成功");
                ep.Closed += Refresh;
                ep.Show();
            }
            else
            {
                ErrorPrompt ep = new ErrorPrompt(e.Result.Error);
                ep.Closed += Refresh;
                ep.Show();
            }
        }

        void Refresh(object sender, EventArgs e)
        {
            InsertBody();
        }

        #endregion

        private void BackToIndex(object sender, EventArgs e)
        {
            if (BackToIndexEventHandler != null)
            {
                BackToIndexEventHandler(this, new EventArgs());
            }
        }

        public event NDelegate BackToIndexEventHandler;
    }
}
