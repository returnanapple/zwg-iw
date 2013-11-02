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
            BankAccount ba2 = new BankAccount("测试2", "李南瓜", "3838438", Bank.财付通, "", 2, true);
            db.Set<BankAccount>().Add(ba2);
            db.SaveChanges();

            #endregion

            InsertJawInfo();
        }

        void InsertJawInfo()
        {
            #region 主要信息
            List<LotteryTimeOfJaw> times = new List<LotteryTimeOfJaw>();
            List<int> k = new List<int> { 0, 0, 1 };
            while (k[0] < 24)
            {
                string timeValue = string.Format("{0}:{1}", k[0], k[1]);
                LotteryTimeOfJaw ltoj = new LotteryTimeOfJaw(k[2], timeValue);
                times.Add(ltoj);
                k[1] += 5;
                if (k[1] >= 60)
                {
                    k[1] -= 60;
                    k[0]++;
                }
                k[2]++;
            }
            MainOfJaw moj = new MainOfJaw(times);
            db.Set<MainOfJaw>().Add(moj);
            #endregion
            #region 标记
            List<IconOfJaw> icons = new List<IconOfJaw>
            {
                IconOfJaw.金色鲨鱼,
                IconOfJaw.燕子,
                IconOfJaw.鸽子,
                IconOfJaw.通杀,
                IconOfJaw.孔雀,
                IconOfJaw.老鹰,
                IconOfJaw.蓝色鲨鱼,
                IconOfJaw.狮子,
                IconOfJaw.熊猫,
                IconOfJaw.通赔,
                IconOfJaw.猴子,
                IconOfJaw.兔子,
                IconOfJaw.飞禽,
                IconOfJaw.走兽
            };
            List<string> touchOffs = new List<string>
            {
                "0",//金色鲨鱼
                "1,2,3",//燕子
                "4,5,6",//鸽子
                "7",//通杀
                "8,9,10",//孔雀
                "11,12,13",//老鹰
                "14",//蓝色鲨鱼
                "15,16,17",//狮子
                "18,19,20",//熊猫
                "21",//通赔
                "22,23,24",//猴子
                "25,26,27",//兔子
                "",//飞禽
                ""//走兽
            };
            List<string> openUps = new List<string>
            {
                "0,21",//金色鲨鱼
                "1,2,3,21",//燕子
                "4,5,6,21",//鸽子
                "",//通杀
                "8,9,10,21",//孔雀
                "11,12,13,21",//老鹰
                "14,21",//蓝色鲨鱼
                "15,16,17,21",//狮子
                "18,19,20,21",//熊猫
                "",//通赔
                "21,22,23,24",//猴子
                "21,25,26,27",//兔子
                "1,2,3,4,5,6,8,9,10,11,12,13,21",//飞禽
                "15,16,17,18,19,20,21,22,23,24,25,26,27"//走兽
            };
            List<int> probabilities = new List<int>
            {
                108,//金色鲨鱼
                1200,//燕子
                900,//鸽子
                180,//通杀
                900,//孔雀
                600,//老鹰
                300,//蓝色鲨鱼
                600,//狮子
                900,//熊猫
                72,//通赔
                900,//猴子
                1200,//兔子
                0,//飞禽
                0//走兽

            };
            List<double> odds = new List<double>
            {
                100,//金色鲨鱼
                6,//燕子
                8,//鸽子
                0,//通杀
                8,//孔雀
                12,//老鹰
                24,//蓝色鲨鱼
                12,//狮子
                8,//熊猫
                0,//通赔
                8,//猴子
                6,//兔子
                2,//飞禽
                2//走兽
            };
            for (int i = 0; i < icons.Count; i++)
            {
                MarkOfJaw mark = new MarkOfJaw(icons[i], touchOffs[i], openUps[i], probabilities[i], odds[i]);
                db.Set<MarkOfJaw>().Add(mark);
            }
            #endregion
            db.SaveChanges();
        }

        #endregion
    }
}
