using Client.Client.Control.ChatService;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Client.Client.Control.PicService;

namespace Client.Client.Control
{
    public class TalkClientViewModel : ModelBase, IChatServiceCallback
    {
        #region 网络连接

        /// <summary>
        /// 聊天服务代理
        /// </summary>
        ChatServiceClient ChatClient { get; set; }

        #endregion

        #region 私有字段

        UserOnlineStatus onlineStatus = UserOnlineStatus.在线;
        UserInfoType tarageUserType = UserInfoType.客服;
        int countOfOnlineFriends = 0;
        TalkingManModel talkingNow = null;
        string messageValue = "";
        string self = "";
        List<UserInfoModel> allUsers = new List<UserInfoModel>();
        bool haveNewMessage = false;
        bool messageToolCanSee = false;

        #endregion

        #region 属性

        /// <summary>
        /// 自己的用户名
        /// </summary>
        public string Self
        {
            get
            {
                if (self == "")
                {
                    string dataKeyOfSelf = "Client_Self";
                    self = IsolatedStorageSettings.ApplicationSettings[dataKeyOfSelf].ToString();
                }
                return self;
            }
        }

        #region 好友列表工具条

        /// <summary>
        /// 当前在线状态
        /// </summary>
        public UserOnlineStatus OnlineStatus
        {
            get { return onlineStatus; }
            set
            {
                if (onlineStatus != value)
                {
                    onlineStatus = value;
                    OnPropertyChanged("OnlineStatus");
                    ChatClient.ChangeStatusAsync(Self, onlineStatus);
                }
            }
        }

        /// <summary>
        /// 正在查看的用户的组类别
        /// </summary>
        public UserInfoType UserGroupType
        {
            get { return tarageUserType; }
            set
            {
                if (tarageUserType != value)
                {
                    tarageUserType = value;
                    OnPropertyChanged("UserGroupType");
                    ResetFriendList();
                }
            }
        }

        /// <summary>
        /// 在线好友数量
        /// </summary>
        public int CountOfOnlineFriends
        {
            get { return countOfOnlineFriends; }
            set
            {
                if (countOfOnlineFriends != value)
                {
                    countOfOnlineFriends = value;
                    OnPropertyChanged("CountOfOnlineFriends");
                }
            }
        }

        /// <summary>
        /// 一个布尔值 标识是否有新的消息
        /// </summary>
        public bool HaveNewMessage
        {
            get { return haveNewMessage; }
            set
            {
                if (haveNewMessage != value)
                {
                    haveNewMessage = value;
                    OnPropertyChanged("HaveNewMessage");
                }
            }
        }

        /// <summary>
        /// 当前显示的好友列表
        /// </summary>
        public ObservableCollection<UserInfoModel> UsersNowShow { get; set; }

        #endregion

        #region 聊天工具栏

        /// <summary>
        /// 一个布尔值 标识聊天窗口是否可见
        /// </summary>
        public bool MessageToolCanSee
        {
            get { return messageToolCanSee; }
            set
            {
                if (messageToolCanSee != value)
                {
                    messageToolCanSee = value;
                    OnPropertyChanged("MessageToolCanSee");
                }
            }
        }

        /// <summary>
        /// 当前聊天对象
        /// </summary>
        public TalkingManModel TalkingNow
        {
            get { return talkingNow; }
            set
            {
                if (talkingNow != value)
                {
                    talkingNow = value;
                    OnPropertyChanged("TalkingNow");
                    MessageValue = "";
                    Messages.Clear();
                    if (talkingNow == null)
                    {
                        ChatClient.ChangeTargetUserAsync("", Self);
                        UsersTaklingFor.ToList().ForEach(x =>
                        {
                            x.IsSelected = false;
                        });
                        return;
                    }
                    ChatClient.ChangeTargetUserAsync(talkingNow.Username, Self);
                    UsersTaklingFor.ToList().ForEach(x =>
                        {
                            x.IsSelected = talkingNow.Username == x.Username;
                        });
                    var t = allUsers.FirstOrDefault(x => x.Username == talkingNow.Username);
                    if (t == null) { return; }
                    t.CountOfNewmessages = 0;
                    ResetCountOfAllNewMessages();
                }
            }
        }

        /// <summary>
        /// 聊天框输入的信息
        /// </summary>
        public string MessageValue
        {
            get { return messageValue; }
            set
            {
                if (messageValue != value)
                {
                    messageValue = value;
                    OnPropertyChanged("MessageValue");
                }
            }
        }

        /// <summary>
        /// 显示在“正在聊天”列表的用户
        /// </summary>
        public ObservableCollection<TalkingManModel> UsersTaklingFor { get; set; }

        /// <summary>
        /// 聊天信息列表
        /// </summary>
        public ObservableCollection<MessageResult> Messages { get; set; }

        #endregion

        #region 命令

        /// <summary>
        /// 打开聊天窗口的命令
        /// </summary>
        public UniversalCommand OpenTalkWindowCommand { get; set; }

        /// <summary>
        /// 选择在线状态的命令
        /// </summary>
        public UniversalCommand ChooseOnlineStatusCommand { get; set; }

        /// <summary>
        /// 将用户移出“正在聊天”列表的命令
        /// </summary>
        public UniversalCommand RemoveTalkingUserCommand { get; set; }

        /// <summary>
        /// 选择目标聊天用户的命令
        /// </summary>
        public UniversalCommand ChooseTalkingUserCommand { get; set; }

        /// <summary>
        /// 关闭聊天窗口的命令
        /// </summary>
        public UniversalCommand CloseTalkWindowCommand { get; set; }

        /// <summary>
        /// 发送新消息的命令
        /// </summary>
        public UniversalCommand SendMessageCommand { get; set; }

        /// <summary>
        /// 打开表情选择窗口的命令
        /// </summary>
        public UniversalCommand ShowChooseIconWindowCommand { get; set; }

        /// <summary>
        /// 打开图片上传窗口的命令
        /// </summary>
        public UniversalCommand ShowUploadPicWindowCommand { get; set; }

        /// <summary>
        /// 打开截屏工具的命令
        /// </summary>
        public UniversalCommand ShowScreenshotWindowCommand { get; set; }

        #endregion

        #endregion

        #region 构造方法

        public TalkClientViewModel()
        {
            UsersNowShow = new ObservableCollection<UserInfoModel>();
            UsersTaklingFor = new ObservableCollection<TalkingManModel>();
            Messages = new ObservableCollection<MessageResult>();

            OpenTalkWindowCommand = new UniversalCommand(new Action<object>(OpenTalkWindow));
            ChooseOnlineStatusCommand = new UniversalCommand(new Action<object>(ChooseOnlineStatus));
            RemoveTalkingUserCommand = new UniversalCommand(new Action<object>(RemoveTalkingUser));
            ChooseTalkingUserCommand = new UniversalCommand(new Action<object>(ChooseTalkingUser));
            CloseTalkWindowCommand = new UniversalCommand(new Action<object>(CloseTalkWindow));
            SendMessageCommand = new UniversalCommand(new Action<object>(SendMessage));
            ShowChooseIconWindowCommand = new UniversalCommand(new Action<object>(ShowChooseIconWindow));
            ShowUploadPicWindowCommand = new UniversalCommand(new Action<object>(ShowUploadPicWindow));
            ShowScreenshotWindowCommand = new UniversalCommand(new Action<object>(ShowScreenshotWindow));

            ChatClient = new ChatServiceClient(new InstanceContext(this));
            ChatClient.RegisterAndGetFriendListCompleted += WriteFriendList;
            ChatClient.ChangeTargetUserCompleted += WriteUnreadMessages;

            ChatClient.RegisterAndGetFriendListAsync(Self, false);
        }

        #endregion

        #region 私有方法

        #region 命令

        #region 用户列表

        #region 打开聊天窗口

        /// <summary>
        /// 打开聊天窗口
        /// </summary>
        /// <param name="parameter"></param>
        void OpenTalkWindow(object parameter)
        {
            var tUser = allUsers.FirstOrDefault(x => x.Username == parameter.ToString());
            if (tUser == null) { return; }
            TalkingManModel tmm = UsersTaklingFor.FirstOrDefault(x => x.Username == tUser.Username);
            if (tmm == null)
            {
                tmm = new TalkingManModel
                {
                    Username = tUser.Username,
                    Online = tUser.Status != UserShowStatus.离线,
                    ChooseTalkingUserCommand = this.ChooseTalkingUserCommand,
                    RemoveTalkingUserCommand = this.RemoveTalkingUserCommand
                };
                UsersTaklingFor.Add(tmm);
            }
            MessageToolCanSee = true;
            TalkingNow = tmm;
        }

        #endregion

        #region 改变在线状态

        /// <summary>
        /// 改变在线状态
        /// </summary>
        /// <param name="parameter"></param>
        void ChooseOnlineStatus(object parameter)
        {
            OnlineStatus = OnlineStatus == UserOnlineStatus.在线
                ? UserOnlineStatus.隐身
                : UserOnlineStatus.在线;
        }

        #endregion

        #endregion

        #region 聊天窗口

        #region 将用户移出“正在聊天”列表

        /// <summary>
        /// 将用户移出“正在聊天”列表
        /// </summary>
        /// <param name="parameter"></param>
        void RemoveTalkingUser(object parameter)
        {
            var tUser = UsersTaklingFor.FirstOrDefault(x => x.Username == parameter.ToString());
            if (tUser == null) { return; }
            UsersTaklingFor.Remove(tUser);
            if (UsersTaklingFor.Count <= 0)
            {
                TalkingNow = null;
                MessageToolCanSee = false;
                return;
            }
            if (TalkingNow.Username == tUser.Username)
            {
                TalkingNow = UsersTaklingFor.First();
            }
        }

        #endregion

        #region 选择目标聊天用户

        /// <summary>
        /// 选择目标聊天用户
        /// </summary>
        /// <param name="parameter"></param>
        void ChooseTalkingUser(object parameter)
        {
            var tUser = UsersTaklingFor.FirstOrDefault(x => x.Username == parameter.ToString());
            TalkingNow = tUser;
        }

        #endregion

        #region 关闭聊天窗口

        /// <summary>
        /// 关闭聊天窗口
        /// </summary>
        /// <param name="parameter"></param>
        void CloseTalkWindow(object parameter)
        {
            TalkingNow = null;
            UsersTaklingFor.Clear();
            MessageToolCanSee = false;
        }

        #endregion

        #endregion

        #region 聊天工具栏

        #region 发送新消息

        /// <summary>
        /// 发送新消息
        /// </summary>
        /// <param name="parameter"></param>
        void SendMessage(object parameter)
        {
            if (TalkingNow == null || !allUsers.Any(x => x.Username == TalkingNow.Username))
            {
                return;
            }
            if (MessageValue == "") { return; }
            SendMessageImport import = new SendMessageImport
            {
                From = Self,
                To = TalkingNow.Username,
                Content = MessageValue
            };
            ChatClient.SendMessageAsync(import);
            MessageValue = "";
        }

        #endregion

        #region 打开表情选择窗口

        /// <summary>
        /// 打开表情选择窗口
        /// </summary>
        /// <param name="parameter"></param>
        void ShowChooseIconWindow(object parameter)
        {
            ChooseIconWindow ciw = new ChooseIconWindow();
            ciw.Closed += (send, e) =>
                {
                    ChooseIconWindow cw = (ChooseIconWindow)send;
                    if (cw.DialogResult != true) { return; }
                    string t = string.Format("[^icon]{0}[$icon]", cw.State);
                    MessageValue += t;
                };
            ciw.Show();
        }

        #endregion

        #region 打开图片上传窗口

        /// <summary>
        /// 打开图片上传窗口
        /// </summary>
        /// <param name="parameter"></param>
        void ShowUploadPicWindow(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "图片 (*.jpg)|*.jpg",
                Multiselect = false  //不允许多选 
            };
            bool chooseFile = dialog.ShowDialog() == true;
            if (!chooseFile) { return; }

            Stream tStream = dialog.File.OpenRead();
            byte[] t = new byte[tStream.Length];
            tStream.Read(t, 0, (int)tStream.Length);
            PicServiceClient client = new PicServiceClient();
            client.UploadCompleted += (_sender, _e) =>
            {
                PicServiceClient c = (PicServiceClient)_sender;
                MessageValue += string.Format("[^pic]{0}[$pic]", _e.Result);
                c.CloseAsync();
            };
            client.UploadAsync(t);
        }

        #endregion

        #region 打开截屏工具

        /// <summary>
        /// 打开截屏工具
        /// </summary>
        /// <param name="parameter"></param>
        void ShowScreenshotWindow(object parameter)
        {
            //TalkClient.Myself.Visibility = Visibility.Collapsed;
            UIElement ui = Application.Current.RootVisual;
            WriteableBitmap wb = new WriteableBitmap(ui, null);
           // TalkClient.Myself.Visibility = Visibility.Visible;
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bands = 3;
            byte[][,] raster = new byte[bands][,];
            for (int i = 0; i < bands; i++)
            {
                raster[i] = new byte[width, height];
            }
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int pixel = wb.Pixels[width * row + column];
                    raster[0][column, row] = (byte)(pixel >> 16);
                    raster[1][column, row] = (byte)(pixel >> 8);
                    raster[2][column, row] = (byte)pixel;
                }
            }
            FluxJpeg.Core.ColorModel model = new FluxJpeg.Core.ColorModel { colorspace = FluxJpeg.Core.ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);
            MemoryStream stream = new MemoryStream();
            FluxJpeg.Core.Encoder.JpegEncoder encoder = new FluxJpeg.Core.Encoder.JpegEncoder(img, 100, stream);
            encoder.Encode();
            byte[] binaryData = stream.ToArray();

            PicServiceClient client = new PicServiceClient();
            client.UploadCompleted += (_sender, _e) =>
            {
                PicServiceClient c = (PicServiceClient)_sender;
                MessageValue += string.Format("[^pic]{0}[$pic]", _e.Result);
                c.CloseAsync();
            };
            client.UploadAsync(binaryData);
        }

        #endregion

        #endregion

        #endregion

        #region 后台方法

        /// <summary>
        /// 写用户列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WriteFriendList(object sender, RegisterAndGetFriendListCompletedEventArgs e)
        {
            e.Result.Users.ForEach(x =>
            {
                UserInfoModel uim = new UserInfoModel
                {
                    Username = x.Username,
                    Status = x.Type == UserInfoType.客服
                         ? UserShowStatus.客服
                         : x.OnlineStatus == UserOnlineStatus.离线
                            ? UserShowStatus.离线
                            : UserShowStatus.在线,
                    CountOfNewmessages = 0,
                    UserType = x.Type,
                    OpenTalkWindowCommand = this.OpenTalkWindowCommand
                };
                allUsers.Add(uim);
            });
            e.Result.UnreadMessageCounts.ForEach(x =>
            {
                var u = allUsers.FirstOrDefault(t => t.Username == x.Username);
                if (u == null) { return; }
                u.CountOfNewmessages = x.Count;
            });
            CountOfOnlineFriends = allUsers.Count(x => x.UserType != UserInfoType.客服 && x.Status == UserShowStatus.在线);
            ResetFriendList();
            ResetCountOfAllNewMessages();
        }

        /// <summary>
        /// 刷新用户列表显示
        /// </summary>
        void ResetFriendList()
        {
            UsersNowShow.Clear();
            allUsers.Where(x => x.UserType == UserGroupType).OrderBy(x => x.Status).ToList().ForEach(x =>
                {
                    UsersNowShow.Add(x);
                });
        }

        /// <summary>
        /// 刷新新消息状态
        /// </summary>
        void ResetCountOfAllNewMessages()
        {
            int c = allUsers.Count == 0 ? 0 : allUsers.Sum(x => x.CountOfNewmessages);
            HaveNewMessage = c > 0;
            if (HaveNewMessage)
            {
                //TalkClient.Myself._music.Stop();
                //TalkClient.Myself._music.Play();
            }
        }

        /// <summary>
        /// 写未读信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WriteUnreadMessages(object sender, ChangeTargetUserCompletedEventArgs e)
        {
            e.Result.ForEach(x =>
                {
                    Messages.Add(x);
                });
        }

        #endregion

        #endregion

        #region 回调方法

        public void AddTheCountOfNewMessageForSomeone(string username)
        {
            var t = allUsers.FirstOrDefault(x => x.Username == username);
            if (t == null) { return; }
            t.CountOfNewmessages++;
            ResetCountOfAllNewMessages();
        }

        public void WriteMessage(MessageResult message)
        {
            Messages.Add(message);
        }

        public void ChangeOnlineStatus(string username, UserOnlineStatus onlineStatus, bool isOfficial)
        {
            var t = allUsers.FirstOrDefault(x => x.Username == username);
            if (t == null) { return; }
            t.Status = isOfficial ? UserShowStatus.客服
                : onlineStatus == UserOnlineStatus.离线 ? UserShowStatus.离线 : UserShowStatus.在线;
            ResetFriendList();
        }

        #endregion
    }
}
