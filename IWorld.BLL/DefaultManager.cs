using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 默认数据的管理者对象
    /// </summary>
    public class DefaultManager
    {
        #region 保护字段

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected DbContext db;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个新的默认数据的管理者对象
        /// </summary>
        public DefaultManager(DbContext db)
        {
            this.db = db;
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 添加初始数据
        /// </summary>
        public void Initialize()
        {
            #region 管理员用户组及系统管理员账号

            AdministratorGroup administratorGroup1
                = new AdministratorGroup("系统管理员", 255, true, true, true, true, true, true, true, true, true, true, true);
            AdministratorGroup administratorGroup2
                = new AdministratorGroup("管理员", 200, true, true, true, false, true, true, false, true, true, true, false);
            var agSet = db.Set<AdministratorGroup>();
            agSet.Add(administratorGroup1);
            agSet.Add(administratorGroup2);
            db.SaveChanges();

            Administrator administrator = new Administrator("admin", EncryptHelper.EncryptByMd5("admin"), administratorGroup1);
            db.Set<Administrator>().Add(administrator);
            db.SaveChanges();

            #endregion

            #region 系统邮箱及其服务器

            EmailClient emailClient = new EmailClient("gmail", "smtp.gmail.com", 586, "谷歌邮箱", false);
            db.Set<EmailClient>().Add(emailClient);
            db.SaveChanges();

            EmailAccount emailAccount = new EmailAccount("测试账号", "magicvrtest@gmail.com", "testmagicvr"
                , "公用的测试账号，请勿用于正常运营", emailClient, false);
            db.Set<EmailAccount>().Add(emailAccount);
            db.SaveChanges();

            #endregion

            #region 用户组

            List<string> userGroupNames = new List<string> { "见习会员", "正式会员", "高级会员", "VIP会员" };
            List<int> userGroupGrades = new List<int> { 1, 2, 3, 4 };
            List<int> userGroupLimit = new List<int> { 0, 1000, 100000, 10000000 };
            List<int> userGroupUpper = new List<int> { 999, 99999, 9999999, 999999999 };
            UserGroupManager userGroupManager = new UserGroupManager(db);
            for (int i = 0; i < userGroupNames.Count; i++)
            {
                ICreatePackage<UserGroup> userGroup = UserGroupManager.Factory
                    .CreatePackageForCreate(userGroupNames[i], userGroupGrades[i], userGroupLimit[i], userGroupUpper[i]);
                userGroupManager.Create(userGroup);
            }

            #endregion

            #region 彩票/玩法标签/玩法

            XElement _doc = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "/Content/Xml/Tickets.xml");
            _doc.Elements("ticket").ToList().ForEach(_ticket =>
                {
                    int t = 0;
                    List<int> valuesOfAll = new List<int>();
                    List<int> valuesOfLarge = new List<int>();
                    List<int> valuesOfSmall = new List<int>();
                    List<int> valuesOfSingle = new List<int>();
                    List<int> valuesOfDouble = new List<int>();
                    #region 加载位的初始化数值
                    int valueOfFirst = Convert.ToInt32(_ticket.Element("valueOfFirst").Value);
                    int countOfValues = Convert.ToInt32(_ticket.Element("values").Value);
                    int halfOfValues = countOfValues % 2 == 0 
                        ? countOfValues / 2 + valueOfFirst : countOfValues / 2 + (valueOfFirst + 1);
                    for (int i = valueOfFirst; i < valueOfFirst + countOfValues; i++)
                    {
                        valuesOfAll.Add(i);
                        if (i >= halfOfValues)
                        {
                            valuesOfLarge.Add(i);
                        }
                        else
                        {
                            valuesOfSmall.Add(i);
                        }
                        if (i % 2 == 0)
                        {
                            valuesOfDouble.Add(i);
                        }
                        else
                        {
                            valuesOfSingle.Add(i);
                        }
                    }
                    #endregion
                    List<LotteryTicketManager.Factory.IPackageForSeat> ticketSeats = new List<LotteryTicketManager.Factory.IPackageForSeat>();
                    #region 加载位
                    _ticket.Element("seats").Elements("seat").ToList().ForEach(_seat =>
                        {
                            ticketSeats.Add(LotteryTicketManager.Factory.CreatePackageForSeat(_seat.Element("name").Value
                                , Convert.ToBoolean(_seat.Element("isSpecial").Value)
                                , string.Join(",", valuesOfAll)
                                , Convert.ToInt32(_seat.Element("order").Value)));
                        });

                    #endregion
                    List<LotteryTicketManager.Factory.IPackageForTime> ticketTimes = new List<LotteryTicketManager.Factory.IPackageForTime>();
                    #region 加载开奖时间
                    t = 1;
                    _ticket.Element("time").Value.Split(new char[] { ',' }).ToList().ForEach(_time =>
                        {
                            ticketTimes.Add(LotteryTicketManager.Factory.CreatePackageForTime(t, _time));
                            t++;
                        });
                    #endregion
                    ICreatePackage<LotteryTicket> pfcTicket = LotteryTicketManager.Factory
                        .CreatePackageForCreate(_ticket.Element("name").Value, ticketTimes, ticketSeats
                        , Convert.ToInt32(_ticket.Element("order").Value));
                    LotteryTicket ticket = new LotteryTicketManager(db).Create(pfcTicket);

                    #region 加载玩法标签
                    _ticket.Element("tags").Elements("tag").ToList().ForEach(_tag =>
                        {
                            ICreatePackage<PlayTag> pfcTag = PlayTagManager.Fantory
                                .CreatePackageForCreate(_tag.Element("name").Value, ticket.Id, Convert.ToInt32(_tag.Element("order").Value));
                            PlayTag tag = new PlayTagManager(db).Create(pfcTag);

                            #region 加载玩法
                            _tag.Element("plays").Elements("play").ToList().ForEach(_play =>
                                {
                                    List<HowToPlayManager.Factory.IPackageForSeat> playSeats
                                        = new List<HowToPlayManager.Factory.IPackageForSeat>();
                                    #region 加载可选位
                                    _play.Element("seats").Elements("seat").ToList().ForEach(_seat =>
                                        {
                                            playSeats.Add(HowToPlayManager.Factory.CreatePackageForSeat(_seat.Element("name").Value
                                                , Convert.ToBoolean(_seat.Element("isRequired").Value)
                                                , string.Join(",", valuesOfAll)
                                                , string.Join(",", valuesOfLarge)
                                                , string.Join(",", valuesOfSmall)
                                                , string.Join(",", valuesOfSingle)
                                                , string.Join(",", valuesOfDouble)
                                                , Convert.ToInt32(_seat.Element("limitOfPick").Value)
                                                , Convert.ToInt32(_seat.Element("upperOfPick").Value)));
                                        });
                                    #endregion
                                    ICreatePackage<HowToPlay> pfcPlay = HowToPlayManager.Factory
                                        .CreatePackageForCreate(_play.Element("name").Value
                                        , _play.Element("description").Value
                                        , _play.Element("rule").Value
                                        , tag.Id
                                        , Convert.ToInt32(_play.Element("lowerseats").Value)
                                        , Convert.ToInt32(_play.Element("upperseats").Value)
                                        , 0
                                        , 0
                                        , 0
                                        , _play.Element("interface").Value
                                        , Convert.ToBoolean(_play.Element("isStackedBit").Value)
                                        , false
                                        , Convert.ToInt32(_play.Element("order").Value)
                                        , playSeats
                                        , Convert.ToInt32(_play.Element("parameter1").Value)
                                        , Convert.ToInt32(_play.Element("parameter2").Value)
                                        , Convert.ToInt32(_play.Element("parameter3").Value));
                                    new HowToPlayManager(db).Create(pfcPlay);
                                });
                            #endregion
                        });
                    #endregion
                });

            #endregion

            #region 银行账号

            BankAccount ba = new BankAccount("测试", "大厨卖当牛", "6214453595666458236", Bank.中国工商银行, "", 1, true);
            db.Set<BankAccount>().Add(ba);
            db.SaveChanges();

            #endregion
        }

        #endregion
    }
}
