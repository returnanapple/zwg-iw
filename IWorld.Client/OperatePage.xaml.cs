using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IWorld.Client.Class;
using IWorld.Client.GamingService;
using IWorld.Client.UsersService;
using IWorld.Client.BulletinService;

namespace IWorld.Client
{
    public partial class OperatePage : UserControl
    {
        int _refreshTime = 1;
        int _updateTime = 30;

        public OperatePage()
        {
            InitializeComponent();
            InitializeKeys();
            WriteUserInfo();
            ShowBulletins();
            CycleShowTicketMain();
            CycleWriteUserInfo();
            CycleReadUserInfo();
            CycleReadTickets();
            CycleShowBulletins();
            InsertNoticeToolEvent();
            ShowNotice();
        }

        #region 快捷功能

        /// <summary>
        /// 顶部功能按键触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXHover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = new SolidColorBrush(Colors.Gray);
        }

        /// <summary>
        /// 顶部功能按键恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonXUnhover(object sender, EventArgs e)
        {
            ((Grid)sender).Background = null;
        }

        /// <summary>
        /// 最小化窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeMainWindow(object sender, EventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMainWindow(object sender, EventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        #endregion

        #region 写出用户信息

        void WriteUserInfo()
        {
            text_username.Text = App.UserInfo.Username;
            text_userGroup.Text = App.UserInfo.Group.Name;
            text_money.Text = App.UserInfo.Money.ToString("0.00");
            text_freeze.Text = App.UserInfo.MoneyBeFrozen.ToString("0.00");
            text_integral.Text = App.UserInfo.Integral.ToString();
        }

        void CycleWriteUserInfo()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _refreshTime);
            timer.Tick += (sender, e) =>
                {
                    WriteUserInfo();
                };
            timer.Start();
        }

        #endregion

        #region 功能键

        List<FunctionKey> FunctionKeys { get; set; }
        List<TicketButton> TicketButtons { get; set; }

        /// <summary>
        /// 初始化功能键状态
        /// </summary>
        void InitializeKeys()
        {
            FunctionKeys = new List<FunctionKey>();
            FunctionKeys.Add(functionKey_users);
            FunctionKeys.Add(functionKey_money);
            FunctionKeys.Add(functionKey_betting);
            FunctionKeys.Add(functionKey_dataReport);
            FunctionKeys.Add(functionKey_center);
            FunctionKeys.Add(functionKey_exc);

            FunctionKeys.ForEach(x =>
                {
                    x.ClickEventHandler += FunctionKeysClick;
                });

            TicketButtons = new List<TicketButton>();
            titleTool.Children.Clear();
            int t = 0;
            App.Ticktes.OrderBy(x => x.Order).Take(7).ToList().ForEach(x =>
                {
                    TicketButton button = new TicketButton();
                    button.Text = x.Name;
                    button.SetValue(Grid.ColumnProperty, t);
                    if (t == 0)
                    {
                        TicketButtonsClieck(button, new EventArgs());
                    }
                    button.ClickEventHandler += TicketButtonsClieck;
                    titleTool.Children.Add(button);
                    TicketButtons.Add(button);

                    t++;
                });
        }
        #region 功能键点击事件

        void FunctionKeysClick(object sender, EventArgs e)
        {
            ClearKeysSelected();
            FunctionKey button = (FunctionKey)sender;
            button.IsSelected = true;
            switch (button.Text)
            {
                case "会员管理":
                    GoToUsersPage();
                    break;
                case "资金管理":
                    GoToFundsPage();
                    break;
                case "投注明细":
                    GoToBettingDetailsPage();
                    break;
                case "数据报表":
                    GoToDataReportsPage();
                    break;
                case "个人中心":
                    GoToUserCenterPage();
                    break;
                case "积分兑换":
                    GoToExchangesPage();
                    break;
            }
        }

        void TicketButtonsClieck(object sender, EventArgs e)
        {
            ClearKeysSelected();
            TicketButton button = (TicketButton)sender;
            button.IsSelected = true;
            InsertTicketPage(button.Text);
        }

        #endregion

        /// <summary>
        /// 清理功能键选中状态
        /// </summary>
        void ClearKeysSelected()
        {
            FunctionKeys.ForEach(x =>
                {
                    x.IsSelected = false;
                });
            TicketButtons.ForEach(x =>
                {
                    x.IsSelected = false;
                });
        }

        #endregion

        #region 投注界面

        #region 参数

        bool showingTicket = false;
        bool showingColon = true;
        string ticketNameNowShow = "";
        string playTagNameNowShow = "";
        string howToPlayNameNowShow = "";
        int ticketIdNowShow = 0;
        int playTagIdNowShow = 0;
        int howToPlayIdNowShow = 0;
        double usedPoints = 0;
        List<PlayTagButton> PlayTagButtons { get; set; }
        List<HowToPlayButton> HowToPlayButtons { get; set; }
        List<Ball> Balls { get; set; }
        List<BettingRes> bRess { get; set; }
        List<NumButton> NumButtons { get; set; }
        List<BettingValuesImport> BettingValuesImports { get; set; }

        #endregion

        #region 参数处理

        private void SeletePoints(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            usedPoints = Math.Round(e.NewValue, 1);
            ShowPoints();
        }

        private void MultipleChanged(object sender, TextChangedEventArgs e)
        {
            if (BettingValuesImports != null)
            {
                if (BettingValuesImports.Count > 0)
                {
                    ShowBettingInfo();
                }
            }
        }

        private void BettingReady(object sender, RoutedEventArgs e)
        {
            HowToPlayResult howToPlay = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow);
            List<BettingValuesImport> bvImports = new List<BettingValuesImport>();
            if (howToPlay.IsSingle)
            {
                #region 单式

                string _text = input_insertBox.Text;
                #region 移除/替换冗余字符
                _text = new Regex(@"^ {1,}| {1,}$|^,{1,}|,{1,}$|^，{1,}|，{1,}$|^;{1,}|;{1,}$|^；{1,}|；{1,}$|^\r{1,}|\r{1,}$|^\|{1,}|\|{1,}$").Replace(_text, "");
                _text = new Regex(@" {1,}").Replace(_text, " ");
                _text = new Regex(@";{1,}").Replace(_text, " ");
                _text = new Regex(@"；{1,}").Replace(_text, " ");
                _text = new Regex(@",{1,}").Replace(_text, " ");
                _text = new Regex(@"，{1,}").Replace(_text, " ");
                _text = new Regex(@"\|{1,}").Replace(_text, " ");
                _text = new Regex(@"\r{1,}").Replace(_text, " ");
                #endregion
                List<List<string>> _texts = new List<List<string>>();
                _text.Split(new char[] { ' ' }).ToList().ForEach(x =>
                    {
                        List<string> t = new List<string>();
                        x.ToArray().ToList().ForEach(y =>
                            {
                                t.Add(y.ToString());
                            });
                        _texts.Add(t);
                    });
                bool error = _texts.Any(x => x.Count != howToPlay.Seats.Count);
                if (!error)
                {
                    int index = 0;
                    howToPlay.Seats.ForEach(x =>
                        {
                            BettingValuesImport import = new BettingValuesImport
                            {
                                Seat = x.Name,
                                Values = new List<string>()
                            };
                            _texts.ForEach(t =>
                                {
                                    import.Values.Add(string.Join("", t));
                                });
                            bvImports.Add(import);

                            index++;
                        });
                }

                #endregion
            }
            else
            {
                if (howToPlay.IsStackedBit)
                {
                    #region 复式叠位

                    howToPlay.Seats.ForEach(x =>
                        {
                            BettingValuesImport import = new BettingValuesImport();
                            import.Seat = x.Name;
                            import.Values = new List<string>();
                            NumButtons.Where(button => button.IsSelected)
                                .OrderBy(button => button.Num)
                                .ToList()
                                .ForEach(button =>
                                    {
                                        import.Values.Add(button.Num.ToString());
                                    });
                            bvImports.Add(import);
                        });

                    #endregion
                }
                else
                {
                    #region 复式不叠位

                    howToPlay.Seats.ForEach(x =>
                        {
                            BettingValuesImport import = new BettingValuesImport();
                            import.Seat = x.Name;
                            import.Values = new List<string>();
                            NumButtons.Where(button => button.SeatName == x.Name
                                && button.IsSelected)
                                .OrderBy(button => button.Num)
                                .ToList()
                                .ForEach(button =>
                                    {
                                        import.Values.Add(button.Num.ToString());
                                    });
                            bvImports.Add(import);
                        });

                    #endregion
                }
            }
            BettingValuesImports = bvImports;
            ShowBettingInfo();
        }
        #region 显示投注信息

        void ShowBettingInfo()
        {
            if (BettingValuesImports.Count != 0)
            {
                HowToPlayResult _howToPlay = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                    .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                    .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow);
                int _sum = 1;
                #region 注数

                switch (_howToPlay.Interface)
                {
                    case LotteryInterface.任N组选:
                        #region 组选
                        int _tNum = BettingValuesImports[0].Values.Count;
                        if (_howToPlay.Name.Contains("组三"))
                        {
                            #region 组三
                            _sum = _tNum * (_tNum - 1);
                            #endregion
                        }
                        else if (_howToPlay.Name.Contains("组六"))
                        {
                            #region 组六
                            _sum *= _tNum < 3 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                / (DigitalHelper.GetFactorialIn0To12(3) * DigitalHelper.GetFactorialIn0To12(_tNum - 3));
                            #endregion
                        }
                        else if (_howToPlay.Name.Contains("组选"))
                        {
                            #region 二星组选
                            _sum *= _tNum < 2 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                                / (DigitalHelper.GetFactorialIn0To12(2) * DigitalHelper.GetFactorialIn0To12(_tNum - 2));
                            #endregion
                        }
                        #endregion
                        break;
                    case LotteryInterface.任N定位胆:
                        #region 定位胆
                        _sum = 0;
                        BettingValuesImports.ForEach(x =>
                        {
                            _sum += x.Values.Count;
                        });
                        #endregion
                        break;
                    case LotteryInterface.任N直选:
                        #region 任N直选
                        if (_howToPlay.IsSingle)
                        {
                            _sum = BettingValuesImports.First().Values.Count;
                        }
                        else
                        {
                            BettingValuesImports.ForEach(x =>
                                {
                                    _sum *= x.Values.Count;
                                });
                        }
                        #endregion
                        break;
                    case LotteryInterface.任N不定位:
                        #region 不定位
                        _sum = BettingValuesImports.FirstOrDefault().Values.Count;
                        #endregion
                        break;
                }

                #endregion

                if (_sum > 0)
                {
                    input_insertBox.Text = "";
                    showTool_sum.Text = string.Format("总注数：{0}", _sum);
                    double _money = Math.Round(_sum * Convert.ToDouble(input_multiple.Text) * App.Websetting.UnitPrice, 2);
                    showTool_money.Text = string.Format("总金额：{0}", _money);
                    switch (_howToPlay.Interface)
                    {
                        case LotteryInterface.任N不定位:
                        case LotteryInterface.任N组选:
                            showTool_num.Text = string.Join(",", BettingValuesImports.First().Values);
                            break;
                        case LotteryInterface.任N直选:
                            if (_howToPlay.IsSingle)
                            {
                                showTool_num.Text = string.Join(" ", BettingValuesImports.First().Values);
                            }
                            else
                            {
                                List<string> ts = new List<string>();
                                BettingValuesImports.ForEach(x =>
                                    {
                                        ts.Add(string.Join("", x.Values));
                                    });
                                showTool_num.Text = string.Join(",", ts);
                            }
                            break;
                        default:
                            List<string> _ts = new List<string>();
                            BettingValuesImports.ForEach(x =>
                                {
                                    _ts.Add(string.Join("", x.Values));
                                });
                            showTool_num.Text = string.Join(",", _ts);
                            break;
                    }
                }
                else
                {
                    showTool_money.Text = "总金额：0";
                    showTool_num.Text = "请选择号码";
                    showTool_sum.Text = "总注数：0";
                }
            }
            else
            {
                input_insertBox.Text = "";
                input_multiple.Text = "1";
                showTool_money.Text = "总金额：0";
                showTool_num.Text = "请选择号码";
                showTool_sum.Text = "总注数：0";
            }
        }

        #endregion

        void ShowBettingGoTool(object sender, EventArgs e)
        {
            if (BettingValuesImports == null)
            {
                return;
            }
            if (BettingValuesImports.Count == 0)
            {
                return;
            }
            LotteryTicketResult ticket = App.Ticktes.First(x => x.Name == ticketNameNowShow);
            var t = showTool_odds.Text.Split(new char[] { ' ' });
            BettingPrompt bp = new BettingPrompt();
            bp._Values = showTool_num.Text;
            bp._Ticket = ticketNameNowShow;
            bp._PlayTag = playTagNameNowShow;
            bp._HowToPlay = howToPlayNameNowShow;
            bp._Phases = ticket.NextPhases;
            bp._Odds = t[0];
            bp._Points = t[2];
            bp._Price = App.Websetting.UnitPrice.ToString();
            bp._Sum = showTool_sum.Text.Split(new char[] { ':', '：' })[1];
            bp._Multiple = input_multiple.Text;
            bp._Money = showTool_money.Text.Split(new char[] { ':', '：' })[1];

            bp.Closed += BettingGo;
            bp.Show();
        }

        private void BettingGo(object sender, EventArgs e)
        {
            BettingPrompt bp = (BettingPrompt)sender;
            if (bp.DialogResult == false)
            {
                return;
            }
            NumButtons.ForEach(x =>
                {
                    x.IsSelected = false;
                });
            LotteryTicketResult ticket = App.Ticktes.First(x => x.Name == ticketNameNowShow);
            HowToPlayResult howToPlay = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow);
            BettingImport import = new BettingImport
            {
                BettingInfo = new BettingInfoImport
                {
                    HowToPlayId = howToPlayIdNowShow,
                    Multiple = Math.Round(Convert.ToDouble(input_multiple.Text), 2),
                    Phases = ticket.NextPhases,
                    Points = usedPoints
                },
                ChasingInfo = new ChasingInfoImport { IsChasing = false },
                Values = BettingValuesImports
            };

            GamingServiceClient client = new GamingServiceClient();
            client.BettingCompleted += ShowBettingResult;
            if (howToPlay.IsSingle)
            {
                int over = 0;
                var values = import.Values.First().Values;
                int count = values.Count;
                int pageSize = 250;
                int t = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
                import.Codes = new List<string>();

                for (int i = 0; i < t; i++)
                {
                    int startIndex = i * pageSize;
                    List<string> tValues = values.Skip(startIndex).Take(pageSize).ToList();
                    client.AddBettingValuesCompleted += (_sender, _e) =>
                        {
                            if (!import.Codes.Contains(_e.Result.Code))
                            {
                                import.Codes.Add(_e.Result.Code);
                                over++;
                                if (over == t)
                                {
                                    import.Values = new List<BettingValuesImport>();
                                    client.BettingAsync(import, App.Token);
                                }
                            }
                        };
                    client.AddBettingValuesAsync(tValues, App.Token);
                }
            }
            else
            {
                client.BettingAsync(import, App.Token);
            }

        }
        #region 投注

        void ShowBettingResult(object sender, BettingCompletedEventArgs e)
        {
            BettingValuesImports.Clear();
            ShowBettingInfo();
            if (e.Result.Success)
            {
                ReadUserInfo();
                BettingRes res = new BettingRes
                {
                    Id = e.Result.BettingId,
                    HowToPlay = e.Result.HowToPlay,
                    Money = e.Result.Money,
                    Multiple = e.Result.Multiple,
                    Values = e.Result.Values
                };
                bRess.Add(res);
                ShowBetting();
            }
            else
            {
                ErrorPromt ep = new ErrorPromt(e.Result.Error);
                ep.Show();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 初始化投注界面
        /// </summary>
        /// <param name="ticketName">彩票名</param>
        void InsertTicketPage(string ticketName)
        {
            #region 初始化参数
            PlayTagButtons = new List<PlayTagButton>();
            HowToPlayButtons = new List<HowToPlayButton>();
            bRess = new List<BettingRes>();
            NumButtons = new List<NumButton>();
            Balls = new List<Ball> { ball_red, ball_yellow, ball_grean, ball_blue, ball_purple };
            ticketNameNowShow = ticketName;
            ticketIdNowShow = App.Ticktes.FirstOrDefault(x => x.Name == ticketNameNowShow).TicketId;
            #endregion
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Visible;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Visible;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Visible;
            body.Visibility = System.Windows.Visibility.Collapsed;
            showingTicket = true;
            #endregion

            ShowBetting();
            ShowTicketMain();
            ShowPlayTagButtons();
            ShowHistory();
            ShowTop();
        }

        void ShowTicketMain()
        {
            LotteryTicketResult ticket = App.Ticktes.First(x => x.Name == ticketNameNowShow);
            DateTime sTime = App.TicketsRefreshTime.Add(ticket.SurplusTime - ticket.TimeAtServer);
            ShowTime(sTime);
            ShowBalls(ticket.Values.Count);
            text_phases_next.Text = string.Format("第 {0} 期", ticket.NextPhases);
            #region 上期开奖

            int t = 0;
            if (sTime > DateTime.Now)
            {
                text_phases.Text = string.Format("第 {0} 期", ticket.Phases);
                ticket.Values.Take(5).ToList().ForEach(x =>
                    {
                        Balls[t].Text = x;
                        t++;
                    });
            }
            else
            {
                text_phases.Text = "正在努力开奖中……";
                ticket.Values.Take(5).ToList().ForEach(x =>
                    {
                        Balls[t].Text = "-";
                        t++;
                    });
            }

            #endregion

        }
        #region 辅助

        void ShowBalls(int count)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < count)
                {
                    Balls[i].Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    Balls[i].Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        void ShowTime(DateTime time)
        {
            TimeSpan ts = time <= DateTime.Now ? new TimeSpan() : time - DateTime.Now;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (ts.Hours > 0)
            {
                num1 = ts.Hours / 10;
                num2 = ts.Hours % 10;
                num3 = ts.Minutes / 10;
                num4 = ts.Minutes % 10;
            }
            else
            {
                num1 = ts.Minutes / 10;
                num2 = ts.Minutes % 10;
                num3 = ts.Seconds / 10;
                num4 = ts.Seconds % 10;
            }
            time_1.Text = num1.ToString();
            time_2.Text = num2.ToString();
            time_3.Text = num3.ToString();
            time_4.Text = num4.ToString();
        }

        #endregion

        void CycleShowTicketMain()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _refreshTime);
            timer.Tick += (sender, e) =>
                {
                    if (showingTicket)
                    {
                        ShowTicketMain();
                        time_t.Opacity = showingColon ? 0 : 1;
                        showingColon = !showingColon;
                    }
                };
            timer.Start();
        }

        void ShowPlayTagButtons()
        {
            LotteryTicketResult ticket = App.Ticktes.First(x => x.Name == ticketNameNowShow);
            playTagTool.Children.Clear();
            PlayTagButtons.Clear();
            int t = 0;
            ticket.Tags.ForEach(x =>
                {
                    PlayTagButton button = new PlayTagButton();
                    button.Text = x.Name;
                    button.SetValue(Grid.ColumnProperty, t);
                    if (t == 0)
                    {
                        button.SetValue(Grid.ColumnProperty, t);
                        PlayTagButtonsClieck(button, new EventArgs());
                    }
                    button.ClickEventHandler += PlayTagButtonsClieck;
                    playTagTool.Children.Add(button);
                    PlayTagButtons.Add(button);
                    t++;
                });
        }

        void ShowHowToPlayButtons()
        {
            LotteryTicketResult ticket = App.Ticktes.First(x => x.Name == ticketNameNowShow);
            PlayTagResult tag = ticket.Tags.FirstOrDefault(x => x.Name == playTagNameNowShow);
            howToPlayTool.Children.Clear();
            HowToPlayButtons.Clear();
            int t = 0;
            tag.HowToPlays.ForEach(y =>
                {
                    HowToPlayButton button = new HowToPlayButton();
                    button.Text = y.Name;
                    button.SetValue(Grid.ColumnProperty, t);
                    if (t == 0)
                    {
                        button.IsSelected = true;
                        HowToPlayButtonsClick(button, new EventArgs());
                    }
                    button.ClickEventHandler += HowToPlayButtonsClick;
                    howToPlayTool.Children.Add(button);
                    HowToPlayButtons.Add(button);
                    t++;
                });
        }

        #region 按键点击

        void PlayTagButtonsClieck(object sender, EventArgs e)
        {
            ClearPlayTagButtons();
            PlayTagButton button = (PlayTagButton)sender;
            button.IsSelected = true;
            playTagNameNowShow = button.Text;
            playTagIdNowShow = App.Ticktes.FirstOrDefault(x => x.Name == ticketNameNowShow).Tags
                .FirstOrDefault(x => x.Name == playTagNameNowShow).PlayTagId;
            ShowHowToPlayButtons();
        }

        void HowToPlayButtonsClick(object sender, EventArgs e)
        {
            ClearHowToPlayButtons();
            HowToPlayButton button = (HowToPlayButton)sender;
            button.IsSelected = true;
            howToPlayNameNowShow = button.Text;
            howToPlayIdNowShow = App.Ticktes.FirstOrDefault(x => x.Name == ticketNameNowShow).Tags
                .FirstOrDefault(x => x.Name == playTagNameNowShow).HowToPlays
                .FirstOrDefault(x => x.Name == howToPlayNameNowShow).HowToPlayId;
            ShowInsertTool();
        }

        void ClearPlayTagButtons()
        {
            this.PlayTagButtons.ForEach(x =>
            {
                x.IsSelected = false;
            });
        }

        void ClearHowToPlayButtons()
        {
            this.HowToPlayButtons.ForEach(x =>
            {
                x.IsSelected = false;
            });
        }

        #endregion

        void ShowInsertTool()
        {
            BettingValuesImports = new List<BettingValuesImport>();
            HowToPlayResult howToPlay = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow);
            usedPoints = 0;
            input_point.Minimum = 0;
            if (howToPlay.Interface == LotteryInterface.任N不定位)
            {
                input_point.Maximum = App.UserInfo.UncertainReturnPoints;
            }
            else
            {
                input_point.Maximum = App.UserInfo.NormalReturnPoints;
            }
            input_point.Value = 0;
            input_multiple.Text = "1";
            ShowBettingInfo();
            ShowPoints();
            if (howToPlay.IsSingle)
            {
                insertBox_single.Visibility = System.Windows.Visibility.Visible;
                insertBox_double.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                insertBox_single.Visibility = System.Windows.Visibility.Collapsed;
                insertBox_double.Visibility = System.Windows.Visibility.Visible;
                insertBox_double.Children.Clear();
                NumButtons.Clear();
                int r = 0;
                if (howToPlay.IsStackedBit)
                {
                    #region 叠号

                    var _seat = howToPlay.Seats.First();

                    TextBlock tb = new TextBlock();
                    tb.Text = "选号";
                    tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb.SetValue(Grid.RowProperty, r);
                    tb.SetValue(Grid.ColumnProperty, 0);
                    insertBox_double.Children.Add(tb);

                    #region 快捷组选

                    NumGroupButton button1 = new NumGroupButton();
                    button1.Text = "大";
                    button1.SeatName = _seat.Name;
                    button1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button1.SetValue(Grid.RowProperty, r);
                    button1.SetValue(Grid.ColumnProperty, 13);
                    button1.ClickEventHandler += SelectNumOfLarge;
                    insertBox_double.Children.Add(button1);

                    NumGroupButton button2 = new NumGroupButton();
                    button2.Text = "小";
                    button2.SeatName = _seat.Name;
                    button2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button2.SetValue(Grid.RowProperty, r);
                    button2.SetValue(Grid.ColumnProperty, 14);
                    button2.ClickEventHandler += SelectNumOfSmall;
                    insertBox_double.Children.Add(button2);

                    NumGroupButton button3 = new NumGroupButton();
                    button3.Text = "单";
                    button3.SeatName = _seat.Name;
                    button3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button3.SetValue(Grid.RowProperty, r);
                    button3.SetValue(Grid.ColumnProperty, 15);
                    button3.ClickEventHandler += SelectNumOfSingle;
                    insertBox_double.Children.Add(button3);

                    NumGroupButton button4 = new NumGroupButton();
                    button4.Text = "双";
                    button4.SeatName = _seat.Name;
                    button4.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button4.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button4.SetValue(Grid.RowProperty, r);
                    button4.SetValue(Grid.ColumnProperty, 16);
                    button4.ClickEventHandler += SelectNumOfDouble;
                    insertBox_double.Children.Add(button4);

                    NumGroupButton button5 = new NumGroupButton();
                    button5.Text = "全";
                    button5.SeatName = _seat.Name;
                    button5.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button5.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button5.SetValue(Grid.RowProperty, r);
                    button5.SetValue(Grid.ColumnProperty, 17);
                    button5.ClickEventHandler += SelectNumOfAll;
                    insertBox_double.Children.Add(button5);

                    NumGroupButton button6 = new NumGroupButton();
                    button6.Text = "清";
                    button6.SeatName = _seat.Name;
                    button6.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button6.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button6.SetValue(Grid.RowProperty, r);
                    button6.SetValue(Grid.ColumnProperty, 18);
                    button6.ClickEventHandler += SelectNumOfNone;
                    insertBox_double.Children.Add(button6);

                    #endregion

                    #region 选号

                    int c = 1;
                    _seat.Values.ForEach(value =>
                        {
                            NumButton button = new NumButton();
                            button.Num = Convert.ToInt32(value);
                            button.SeatName = _seat.Name;
                            button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button.SetValue(Grid.RowProperty, r);
                            button.SetValue(Grid.ColumnProperty, c);
                            insertBox_double.Children.Add(button);
                            NumButtons.Add(button);

                            c++;
                            if (c > 12)
                            {
                                c = 1;
                                r++;
                            }
                        });

                    #endregion

                    #endregion
                }
                else
                {
                    #region 非叠号

                    howToPlay.Seats.ForEach(x =>
                        {
                            TextBlock tb = new TextBlock();
                            tb.Text = x.Name;
                            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            tb.SetValue(Grid.RowProperty, r);
                            tb.SetValue(Grid.ColumnProperty, 0);
                            insertBox_double.Children.Add(tb);

                            #region 快捷组选

                            NumGroupButton button1 = new NumGroupButton();
                            button1.Text = "大";
                            button1.SeatName = x.Name;
                            button1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button1.SetValue(Grid.RowProperty, r);
                            button1.SetValue(Grid.ColumnProperty, 13);
                            button1.ClickEventHandler += SelectNumOfLarge;
                            insertBox_double.Children.Add(button1);

                            NumGroupButton button2 = new NumGroupButton();
                            button2.Text = "小";
                            button2.SeatName = x.Name;
                            button2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button2.SetValue(Grid.RowProperty, r);
                            button2.SetValue(Grid.ColumnProperty, 14);
                            button2.ClickEventHandler += SelectNumOfSmall;
                            insertBox_double.Children.Add(button2);

                            NumGroupButton button3 = new NumGroupButton();
                            button3.Text = "单";
                            button3.SeatName = x.Name;
                            button3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button3.SetValue(Grid.RowProperty, r);
                            button3.SetValue(Grid.ColumnProperty, 15);
                            button3.ClickEventHandler += SelectNumOfSingle;
                            insertBox_double.Children.Add(button3);

                            NumGroupButton button4 = new NumGroupButton();
                            button4.Text = "双";
                            button4.SeatName = x.Name;
                            button4.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button4.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button4.SetValue(Grid.RowProperty, r);
                            button4.SetValue(Grid.ColumnProperty, 16);
                            button4.ClickEventHandler += SelectNumOfDouble;
                            insertBox_double.Children.Add(button4);

                            NumGroupButton button5 = new NumGroupButton();
                            button5.Text = "全";
                            button5.SeatName = x.Name;
                            button5.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button5.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button5.SetValue(Grid.RowProperty, r);
                            button5.SetValue(Grid.ColumnProperty, 17);
                            button5.ClickEventHandler += SelectNumOfAll;
                            insertBox_double.Children.Add(button5);

                            NumGroupButton button6 = new NumGroupButton();
                            button6.Text = "清";
                            button6.SeatName = x.Name;
                            button6.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            button6.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            button6.SetValue(Grid.RowProperty, r);
                            button6.SetValue(Grid.ColumnProperty, 18);
                            button6.ClickEventHandler += SelectNumOfNone;
                            insertBox_double.Children.Add(button6);

                            #endregion

                            #region 选号

                            int c = 1;
                            x.Values.ForEach(value =>
                                {
                                    NumButton button = new NumButton();
                                    button.Num = Convert.ToInt32(value);
                                    button.SeatName = x.Name;
                                    button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                                    button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                                    button.SetValue(Grid.RowProperty, r);
                                    button.SetValue(Grid.ColumnProperty, c);
                                    insertBox_double.Children.Add(button);
                                    NumButtons.Add(button);

                                    c++;
                                    if (c > 12)
                                    {
                                        c = 1;
                                        r++;
                                    }
                                });

                            #endregion

                            r++;
                        });

                    #endregion
                }
            }
        }
        #region 快捷组选

        void SelectNumOfLarge(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            var _seat = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow)
                .Seats.First(x => x.Name == button.SeatName);
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = _seat.ValuesForLarge.Contains(x.Num.ToString());
                    });
        }

        void SelectNumOfSmall(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            var _seat = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow)
                .Seats.First(x => x.Name == button.SeatName);
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = _seat.ValuesForSmall.Contains(x.Num.ToString());
                    });
        }

        void SelectNumOfSingle(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            var _seat = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow)
                .Seats.First(x => x.Name == button.SeatName);
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = _seat.ValuesForSingle.Contains(x.Num.ToString());
                    });
        }

        void SelectNumOfDouble(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            var _seat = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow)
                .Seats.First(x => x.Name == button.SeatName);
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = _seat.ValuesForDouble.Contains(x.Num.ToString());
                    });
        }

        void SelectNumOfAll(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = true;
                    });
        }

        void SelectNumOfNone(object sender, EventArgs e)
        {
            NumGroupButton button = (NumGroupButton)sender;
            NumButtons.Where(x => x.SeatName == button.SeatName).ToList()
                .ForEach(x =>
                    {
                        x.IsSelected = false;
                    });
        }

        #endregion
        #region 赔率显示

        void ShowPoints()
        {
            HowToPlayResult howToPlay = App.Ticktes.First(x => x.TicketId == ticketIdNowShow)
                .Tags.First(x => x.PlayTagId == playTagIdNowShow)
                .HowToPlays.First(x => x.HowToPlayId == howToPlayIdNowShow);
            double odds = 0;
            #region 赔率
            double tSum = 1000;
            double _tSum = 1;
            switch (howToPlay.Interface)
            {
                case LotteryInterface.任N组选:
                    #region 组选
                    int _tNum = howToPlay.Seats.FirstOrDefault().Values.Count;
                    if (howToPlay.Name.Contains("组三"))
                    {
                        #region 组三
                        _tSum = _tNum * (_tNum - 1);
                        #endregion
                    }
                    else if (howToPlay.Name.Contains("组六"))
                    {
                        #region 组六
                        _tSum *= _tNum < 3 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                            / (DigitalHelper.GetFactorialIn0To12(3) * DigitalHelper.GetFactorialIn0To12(_tNum - 3));
                        #endregion
                    }
                    else if (howToPlay.Name.Contains("组选"))
                    {
                        #region 二星组选
                        _tSum *= _tNum < 2 ? 0 : DigitalHelper.GetFactorialIn0To12(_tNum)
                            / (DigitalHelper.GetFactorialIn0To12(2) * DigitalHelper.GetFactorialIn0To12(_tNum - 2));
                        #endregion
                    }
                    #endregion
                    break;
                case LotteryInterface.任N直选:
                    #region 直选
                    howToPlay.Seats.ForEach(x =>
                    {
                        _tSum *= x.Values.Count;
                    });

                    #endregion
                    break;
                case LotteryInterface.任N不定位:
                    #region 不定位
                    _tSum = howToPlay.Seats.FirstOrDefault().Values.Count;
                    #endregion
                    break;
                case LotteryInterface.任N定位胆:
                    #region 定位胆
                    _tSum = howToPlay.Seats.First().Values.Count();
                    #endregion
                    break;
            }
            odds = App.Websetting.ReferenceBonusMode * _tSum / tSum;
            odds = howToPlay.Odds == 0 ? odds : howToPlay.Odds;
            double coefficient = (App.Websetting.PayoutBase + usedPoints * App.Websetting.ConversionRates)
                    / App.Websetting.PayoutBase;
            odds = odds * coefficient;
            #endregion

            double rp = howToPlay.Interface == LotteryInterface.任N不定位 ? App.UserInfo.UncertainReturnPoints
                : App.UserInfo.NormalReturnPoints;
            showTool_odds.Text = string.Format("{0} / {1}%", odds, Math.Round(rp - usedPoints, 1));
        }

        #endregion

        void ShowHistory()
        {
            historyTool.Children.Clear();
            GamingServiceClient client = new GamingServiceClient();
            client.GetHistoryOfLotteryCompleted += ShowHistory_do;
            client.GetHistoryOfLotteryAsync(ticketIdNowShow);
        }
        #region 显示开奖历史

        void ShowHistory_do(object sender, GetHistoryOfLotteryCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                int t = 0;
                e.Result.Content.ForEach(x =>
                {
                    TextBlock tb1 = new TextBlock();
                    tb1.Text = string.Format("{0}期", x.Phases);
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.RowProperty, t);
                    historyTool.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.Text = string.Join(",", x.Values);
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.RowProperty, t);
                    historyTool.Children.Add(tb2);

                    t++;
                });
            }
        }

        #endregion

        void ShowTop()
        {
            topTool.Children.Clear();
            GamingServiceClient client = new GamingServiceClient();
            client.GetRankingCompleted += ShowTop_do;
            client.GetRankingAsync(ticketIdNowShow);
        }
        #region 显示中奖排行
        void ShowTop_do(object sender, GetRankingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                int t = 0;
                e.Result.Content.ForEach(x =>
                    {
                        TextBlock tb1 = new TextBlock();
                        long _t = x.IsRealBetting ? 10727110010 : 10727210010;
                        tb1.Text = string.Format("{0}", _t + x.Id);
                        tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        tb1.SetValue(Grid.RowProperty, t);
                        topTool.Children.Add(tb1);

                        TextBlock tb2 = new TextBlock();
                        tb2.Text = string.Format("￥{0}", x.Bonus);
                        tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        tb2.SetValue(Grid.RowProperty, t);
                        topTool.Children.Add(tb2);

                        t++;
                    });
            }
        }
        #endregion

        void ShowBetting()
        {
            if (bRess.Count > 3)
            {
                bRess.RemoveRange(0, bRess.Count - 3);
            }
            buttomTool.Children.Clear();
            int t = 0;
            bRess.ForEach(x =>
                {
                    TextBlock tb1 = new TextBlock();
                    tb1.Text = string.Format("{0}", x.Id + 10727110010);
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.SetValue(Grid.RowProperty, t);
                    tb1.SetValue(Grid.ColumnProperty, 0);
                    buttomTool.Children.Add(tb1);

                    TextBlock tb2 = new TextBlock();
                    tb2.Text = x.HowToPlay;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.SetValue(Grid.RowProperty, t);
                    tb2.SetValue(Grid.ColumnProperty, 1);
                    buttomTool.Children.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.Text = x.Values;
                    tb3.MaxWidth = 210;
                    tb3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tb3.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb3.SetValue(Grid.RowProperty, t);
                    tb3.SetValue(Grid.ColumnProperty, 2);
                    buttomTool.Children.Add(tb3);

                    TextBlock tb4 = new TextBlock();
                    tb4.Text = x.Multiple.ToString();
                    tb4.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb4.SetValue(Grid.RowProperty, t);
                    tb4.SetValue(Grid.ColumnProperty, 3);
                    buttomTool.Children.Add(tb4);

                    TextBlock tb5 = new TextBlock();
                    tb5.Text = x.Money.ToString("0.00");
                    tb5.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb5.SetValue(Grid.RowProperty, t);
                    tb5.SetValue(Grid.ColumnProperty, 4);
                    buttomTool.Children.Add(tb5);

                    TextBlock tb6 = new TextBlock();
                    tb6.Text = "撤单";
                    tb6.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb6.Style = (Style)this.Resources["LinkStyle"];
                    tb6.SetValue(Grid.RowProperty, t);
                    tb6.SetValue(Grid.ColumnProperty, 5);
                    NButtonFeedbackTrigger trigger = new NButtonFeedbackTrigger();
                    trigger.Attach(tb6);
                    tb6.MouseLeftButtonDown += (sender, e) =>
                        {
                            RevokeBetting(x.Id);
                        };
                    buttomTool.Children.Add(tb6);

                    t++;
                });
        }

        void RevokeBetting(int bettingId)
        {
            var br = bRess.First(x => x.Id == bettingId);
            RemoveBettingPrompt rbp = new RemoveBettingPrompt();
            rbp._Id = br.Id.ToString("000000");
            rbp._Values = br.Values;
            rbp._Ticket = ticketNameNowShow;
            rbp._PlayTag = playTagNameNowShow;
            rbp._HowToPlay = br.HowToPlay;
            rbp._Money = br.Money.ToString("0.00");
            rbp.Closed += (sender, e) =>
                {
                    RemoveBettingPrompt rdb = (RemoveBettingPrompt)sender;
                    if (rbp.DialogResult == true)
                    {
                        bRess.RemoveAll(x => x.Id == bettingId);
                        ShowBetting();
                        GamingServiceClient client = new GamingServiceClient();
                        client.RemoveBettingCompleted += ShowRevokeBettingResult;
                        client.RemoveBettingAsync(bettingId, App.Token);
                    }
                };
            rbp.Show();
        }
        #region 撤单

        void ShowRevokeBettingResult(object sender, RemoveBettingCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                ReadUserInfo();
            }
            string message = e.Result.Success ? "撤单成功" : e.Result.Error;
            ErrorPromt ep = new ErrorPromt(message);
            ep.Show();
        }

        #endregion

        #endregion

        #region 读取网络信息

        void CycleReadTickets()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _updateTime);
            timer.Tick += (sender, e) =>
                {
                    GamingServiceClient client = new GamingServiceClient();
                    client.GetLotteryTicketsCompleted += UpdateLotteryTickets;
                    client.GetLotteryTicketsAsync();
                };
            timer.Start();
        }
        #region 更新彩票信息

        void UpdateLotteryTickets(object sender, GetLotteryTicketsCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Ticktes.ForEach(x =>
                {
                    LotteryTicketResult ticket = e.Result.Content.FirstOrDefault(y => y.TicketId == x.TicketId);
                    x.Name = ticket.Name;
                    x.Order = ticket.Order;
                    x.Phases = ticket.Phases;
                    x.Values = ticket.Values;
                    x.NextPhases = ticket.NextPhases;
                    x.SurplusTime = ticket.SurplusTime;
                    x.TimeAtServer = ticket.TimeAtServer;
                });
                App.TicketsRefreshTime = DateTime.Now;
            }
            else
            {
                App.GoToLoginPage();
            }
        }

        #endregion

        void ReadUserInfo()
        {
            UsersServiceClient client = new UsersServiceClient();
            client.GetUserInfoCompleted += UpdateUserInfo;
            client.GetUserInfoAsync(App.Token);
        }
        #region 更新用户信息

        void UpdateUserInfo(object sender, GetUserInfoCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.UserInfo = e.Result;
            }
            else
            {
                App.GoToLoginPage();
            }
        }

        #endregion

        void CycleReadUserInfo()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _updateTime);
            timer.Tick += (sender, e) =>
                {
                    ReadUserInfo();
                };
            timer.Start();
        }

        void CycleReadBulletins()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _updateTime);
            timer.Tick += (sender, e) =>
                {
                    BulletinServiceClient client = new BulletinServiceClient();
                    client.GetBulletinsCompleted += UpdateBulletins;
                    client.GetBulletinsAsync();
                };
            timer.Start();
        }
        #region 更新公告信息
        void UpdateBulletins(object sender, GetBulletinsCompletedEventArgs e)
        {
            if (e.Result.Success)
            {
                App.Bulletins = e.Result.Content;
            }
            else
            {
                App.GoToLoginPage();
            }
        }
        #endregion

        #endregion

        #region 充值/提现

        private void OpenRechargeTool(object sender, EventArgs e)
        {
            RechargeTool rt = new RechargeTool(App.UserInfo.UserId, App.UserInfo.Username);
            rt.Closed += ShowRechargeResult;
            rt.Show();
        }
        #region 充值

        void ShowRechargeResult(object sender, EventArgs e)
        {
            RechargeTool rt = (RechargeTool)sender;
            if (rt.DialogResult == true)
            {
                if (rt.ShowError)
                {
                    ErrorPromt ep = new ErrorPromt(rt.Result.Error);
                    ep.Show();
                }
                else
                {
                    RechargeResultTool rrt = new RechargeResultTool(rt.Result);
                    rrt.Closed += ShowRechargeResultList;
                    rrt.Show();
                }
            }
        }

        void ShowRechargeResultList(object sender, EventArgs e)
        {
            GoToFundsPage();
        }

        #endregion

        private void OpenWithdrawTool(object sender, EventArgs e)
        {
            WithdrawTool wt = new WithdrawTool();
            wt.Closed += ShowWithdrawResult;
            wt.Show();
        }
        #region 提现

        void ShowWithdrawResult(object sender, EventArgs e)
        {
            WithdrawTool wt = (WithdrawTool)sender;
            if (wt.DialogResult == true)
            {
                if (wt.ShowError)
                {
                    ErrorPromt ep = new ErrorPromt(wt.Error);
                    ep.Show();
                }
                else
                {
                    ErrorPromt ep = new ErrorPromt("您的提现申请已经提交");
                    ep.Closed += ShowWithdrawResultList;
                    ep.Show();
                }
            }
        }

        void ShowWithdrawResultList(object sender, EventArgs e)
        {
            GoToFundsPage(true);
        }

        #endregion

        #endregion

        #region 信息管理

        void GoToUsersPage()
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            UsersPage page = new UsersPage();
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToFundsPage(bool SeeW = false)
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            FundsPage page = new FundsPage(SeeW);
            page.OpenRToolEventHacnler += OpenRechargeTool;
            page.OpenWToolEventHacnler += OpenWithdrawTool;
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToBettingDetailsPage()
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            BettingDetailsPage page = new BettingDetailsPage();
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToDataReportsPage()
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            DataReportsPage page = new DataReportsPage();
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToUserCenterPage()
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            UserCenterPage page = new UserCenterPage();
            body.Children.Clear();
            body.Children.Add(page);
        }

        void GoToExchangesPage()
        {
            #region 显示框架
            ticketPageBody.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_right.Visibility = System.Windows.Visibility.Collapsed;
            ticketPageBody_buttom.Visibility = System.Windows.Visibility.Collapsed;
            body.Visibility = System.Windows.Visibility.Visible;
            showingTicket = false;
            #endregion
            ExchangesPage page = new ExchangesPage();
            body.Children.Clear();
            body.Children.Add(page);
        }

        #endregion

        #region 公告/通知

        int BulletinIdNowShow = 0;
        void ShowBulletins()
        {
            if (App.Bulletins.Count > 0)
            {
                Random r = new Random();
                int index = r.Next(0, App.Bulletins.Count - 1);
                BulletinResult _bulletin = App.Bulletins[index];
                BulletinIdNowShow = _bulletin.BulletinId;
                text_bulletin.Text = _bulletin.Title;

                bulletinBody.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                bulletinBody.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void CycleShowBulletins()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, _refreshTime * 5);
            timer.Tick += (sender, e) =>
                {

                };
            timer.Start();
        }

        private void ShowBulletin(object sender, MouseButtonEventArgs e)
        {
            BulletinResult _bulletin = App.Bulletins.FirstOrDefault(x => x.BulletinId == BulletinIdNowShow);
            if (_bulletin != null)
            {
                BulletinFullWindows fw = new BulletinFullWindows(_bulletin);
                fw.Show();
            }
        }

        void InsertNoticeToolEvent()
        {
            noticeTool.ClickEventHandler += NoticeToolClick;
            noticeTool.CallMusicEventHandler += noticeTool_CallMusicEventHandler;
        }

        void noticeTool_CallMusicEventHandler(object sender, EventArgs e)
        {
            bg_music.Stop();
            bg_music.Play();
        }
        #region 通知栏点击事件

        void NoticeToolClick(object sender, EventArgs e)
        {
            NoticeWindow nw = (NoticeWindow)sender;
            if (nw.Notice != null)
            {
                switch (nw.Notice.Type)
                {
                    case NoticeType.充值反馈:
                        IWorld.Client.FundsService.RechargeDetailsResult _rdResult = new IWorld.Client.FundsService.RechargeDetailsResult();
                        _rdResult.RechargeId = nw.Notice.RechargeDetails.RechargeId;
                        _rdResult.Sum = nw.Notice.RechargeDetails.Sum;
                        _rdResult.Time = nw.Notice.RechargeDetails.Time;
                        _rdResult.Status = (FundsService.RechargeStatus)Enum.Parse(typeof(FundsService.RechargeStatus), nw.Notice.RechargeDetails.Status.ToString(), false);
                        _rdResult.To = nw.Notice.RechargeDetails.To;
                        _rdResult.From = nw.Notice.RechargeDetails.From;
                        _rdResult.Code = nw.Notice.RechargeDetails.Code;
                        _rdResult.Remark = nw.Notice.RechargeDetails.Remark;
                        FundsPage_FullWindow fw = new FundsPage_FullWindow(_rdResult);
                        fw.Show();
                        break;
                    case NoticeType.开奖提醒:
                        IWorld.Client.GamingService.BettingDetailsResult _bettingResult = new IWorld.Client.GamingService.BettingDetailsResult();
                        _bettingResult.BettingId = nw.Notice.BettingDetails.BettingId;
                        _bettingResult.Ticket = nw.Notice.BettingDetails.Ticket;
                        _bettingResult.PlayTag = nw.Notice.BettingDetails.PlayTag;
                        _bettingResult.HowToPlay = nw.Notice.BettingDetails.HowToPlay;
                        _bettingResult.Phases = nw.Notice.BettingDetails.Phases;
                        _bettingResult.Time = nw.Notice.BettingDetails.Time;
                        _bettingResult.Owner = nw.Notice.BettingDetails.Owner;
                        _bettingResult.Pay = nw.Notice.BettingDetails.Pay;
                        _bettingResult.Bonus = nw.Notice.BettingDetails.Bonus;
                        _bettingResult.Status = (GamingService.BettingStatus)Enum.Parse(typeof(GamingService.BettingStatus), nw.Notice.BettingDetails.Status.ToString(), false);
                        _bettingResult.Sum = nw.Notice.BettingDetails.Sum;
                        _bettingResult.Multiple = nw.Notice.BettingDetails.Multiple;
                        _bettingResult.RetutnPoints = nw.Notice.BettingDetails.RetutnPoints;
                        _bettingResult.Values = nw.Notice.BettingDetails.Values;
                        _bettingResult.LotteryValues = nw.Notice.BettingDetails.LotteryValues;
                        BettingDetailsPage_FullWindow _fw = new BettingDetailsPage_FullWindow(_bettingResult);
                        _fw.Show();
                        break;
                    case NoticeType.提现反馈:
                        GoToFundsPage(true);
                        break;
                }
            }
        }

        #endregion

        void ShowNotice()
        {
            BulletinServiceClient client = new BulletinServiceClient();
            client.GetNoticeCompleted += WriteNotice;
            client.GetNoticeAsync(App.Token);
        }
        #region 显示通知

        void WriteNotice(object sender, GetNoticeCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                Storyboard sb = new Storyboard();
                sb.Duration = new Duration(new TimeSpan(0, 0, 10));
                sb.Completed += ShowNoticeAgain;
                sb.Begin();
            }
            else
            {
                ReadUserInfo();
                noticeTool.Notice = e.Result;
                ShowNotice();
            }
        }

        void ShowNoticeAgain(object sender, EventArgs e)
        {
            ShowNotice();
        }

        #endregion

        #endregion

        #region 呼叫客服

        private void CallCustomerService(object sender, EventArgs e)
        {
            OOBHyperLinkButton.OpenWebPage(new Uri("http://wpa.qq.com/msgrd?V=1&Uin=2621762252", UriKind.Absolute), "_blank");
        }

        #endregion
    }

    public delegate void XDelegate();
}
