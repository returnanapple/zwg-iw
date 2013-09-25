using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IWorld.Model;
using IWorld.BLL;

namespace IWorld.DataCall
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        static void SaveToXaml()
        {
            XElement doc = new XElement("users");
            using (WebMapContext db = new WebMapContext())
            {
                db.Set<Author>().ToList().ForEach(user =>
                {
                    XElement e = new XElement("user"
                        , new XElement("Username", user.Username)
                        , new XElement("Password", user.Password)
                        , new XElement("SafeCode", user.SafeCode)
                        , new XElement("Email", user.Email)
                        , new XElement("BindingEmail", user.BindingEmail)
                        , new XElement("NormalReturnPoints", user.NormalReturnPoints)
                        , new XElement("UncertainReturnPoints", user.UncertainReturnPoints)
                        , new XElement("Money", user.Money)
                        , new XElement("MoneyBeFrozen", user.MoneyBeFrozen)
                        , new XElement("Holder", user.Holder)
                        , new XElement("Card", user.Card)
                        , new XElement("Bank", user.Bank.ToString())
                        , new XElement("BindingCard", user.BindingCard)
                        , new XElement("Status", user.Status.ToString())
                        , new XElement("MaxOfSubordinate", user.MaxOfSubordinate)
                        , new XElement("Subordinate", user.Subordinate)
                        , new XElement("LeftKey", user.LeftKey)
                        , new XElement("RightKey", user.RightKey)
                        , new XElement("Layer", user.Layer)
                        , new XElement("Tree", user.Tree)
                        );
                    doc.Add(e);
                });
            }
            doc.Save("E:/a.xml");
        }

        static void SaveToData()
        {
            XElement doc = XElement.Load("E:/a.xml");
            using (WebMapContext db = new WebMapContext())
            {
                doc.Elements().ToList().ForEach(e =>
                    {
                        Author user = new Author();
                        user.Username = e.Element("Username").Value;
                        user.Password = e.Element("Password").Value;
                        user.SafeCode = e.Element("SafeCode").Value;
                        user.Email = e.Element("Email").Value;
                        user.BindingEmail = Convert.ToBoolean(e.Element("BindingEmail").Value);
                        user.IsAgents = true;
                        user.NormalReturnPoints = Convert.ToDouble(e.Element("NormalReturnPoints").Value);
                        user.UncertainReturnPoints = Convert.ToDouble(e.Element("UncertainReturnPoints").Value);
                        user.Group = db.Set<UserGroup>().Find(1);
                        user.Money = 0;
                        user.MoneyBeFrozen = 0;
                        user.Consumption = 0;
                        user.Integral = 0;
                        user.Holder = e.Element("Holder").Value;
                        user.Card = e.Element("Card").Value;
                        user.Bank = (Bank)Enum.Parse(typeof(Bank), e.Element("Bank").Value);
                        user.BindingCard = Convert.ToBoolean(e.Element("BindingCard").Value);
                        user.Status = (UserStatus)Enum.Parse(typeof(UserStatus), e.Element("Status").Value);
                        user.MaxOfSubordinate = Convert.ToInt32(e.Element("MaxOfSubordinate").Value);
                        user.Subordinate = Convert.ToInt32(e.Element("Subordinate").Value);
                        user.LastLoginTime = DateTime.Now;
                        user.LastLoginIp = "";
                        user.LeftKey = Convert.ToInt32(e.Element("LeftKey").Value);
                        user.RightKey = Convert.ToInt32(e.Element("RightKey").Value);
                        user.Layer = Convert.ToInt32(e.Element("Layer").Value);
                        user.Tree = e.Element("Tree").Value;
                        user.CreatedTime = DateTime.Now;
                        user.ModifiedTime = DateTime.Now;

                        db.Set<Author>().Add(user);

                        //double money = Convert.ToDouble(e.Element("Money").Value);
                        //if (money > 0)
                        //{
                        //    RechargeRecord record = new RechargeRecord();
                        //    record.Owner = user;
                        //    record.Payer = user;
                        //    record.Sum = money;
                        //    record.Card = "";
                        //    record.Name = "";
                        //    record.Bank = Bank.无;
                        //    record.Remark = "";
                        //    record.CreatedTime = DateTime.Now;
                        //    record.ModifiedTime = DateTime.Now;

                        //    db.Set<RechargeRecord>().Add(record);
                        //}
                    });
                db.SaveChanges();
            }
        }

        static void SaveToRecord()
        {
            XElement doc = new XElement("records");
            using (WebMapContext db = new WebMapContext())
            {
                db.Set<WithdrawalsRecord>().ToList().ForEach(x =>
                    {
                        XElement e = new XElement("record"
                            , new XElement("申请人", x.Owner.Username)
                            , new XElement("金额", x.Sum.ToString("0.00"))
                            , new XElement("目标卡号", x.Card)
                            , new XElement("目标卡的开户人", x.Name)
                            , new XElement("目标卡的开户银行", x.Bank.ToString())
                            , new XElement("状态", x.Status.ToString())
                            , new XElement("备注", x.Remark)
                            , new XElement("时间", string.Format("{0} {1}"
                                , x.CreatedTime.ToLongDateString()
                                , x.CreatedTime.ToLongTimeString()
                                    )
                                )
                            );
                        doc.Add(e);
                    });
            }
            doc.Save("E:/b.xml");
        }

        static void ClearMoney()
        {
            using (WebMapContext db = new WebMapContext())
            {
                db.Set<Author>().ToList().ForEach(x =>
                    {
                        x.Money = 0;
                        x.MoneyBeFrozen = 0;
                    });
                db.SaveChanges();
            }
        }

        static void SaveToDataRecord()
        {
            XElement doc = new XElement("records");
            using (WebMapContext db = new WebMapContext())
            {
                db.Set<SiteDataAtDay>().ToList().ForEach(x =>
                    {
                        db.Set<SiteDataAtDay>().Remove(x);
                    });
                db.Set<SiteDataAtMonth>().ToList().ForEach(x =>
                    {
                        db.Set<SiteDataAtMonth>().Remove(x);
                    });
                db.Set<PersonalDataAtDay>().ToList().ForEach(x =>
                    {
                        db.Set<PersonalDataAtDay>().Remove(x);
                    });
                db.Set<PersonalDataAtMonth>().ToList().ForEach(x =>
                    {
                        db.Set<PersonalDataAtMonth>().Remove(x);
                    });
                SiteDataAtDay _SiteDataAtDay = new SiteDataAtDay(DateTime.Now);
                SiteDataAtMonth _SiteDataAtMonth = new SiteDataAtMonth(DateTime.Now);

                db.Set<Author>().Where(x => x.Status == UserStatus.正常).ToList().ForEach(user =>
                    {
                        XElement e = new XElement("record", new XElement("用户名", user.Username));
                        PersonalDataAtDay _PersonalDataAtDay = new PersonalDataAtDay(user);
                        PersonalDataAtMonth _PersonalDataAtMonth = new PersonalDataAtMonth(user);

                        #region 充值

                        double _recharge = 0;
                        if (db.Set<RechargeRecord>().Any(x => x.Status == RechargeStatus.充值成功 && x.Owner.Id == user.Id))
                        {
                            _recharge = db.Set<RechargeRecord>()
                                .Where(x => x.Status == RechargeStatus.充值成功 && x.Owner.Id == user.Id)
                                .Sum(x => x.Sum);
                        }
                        e.Add(new XElement("充值", _recharge));
                        _SiteDataAtDay.Recharge += _recharge;
                        _SiteDataAtMonth.Recharge += _recharge;
                        _PersonalDataAtDay.Recharge = _recharge;
                        _PersonalDataAtMonth.Recharge = _recharge;

                        #endregion

                        #region 提现

                        double _withdraw = 0;
                        if (db.Set<WithdrawalsRecord>().Any(x => x.Status == WithdrawalsStatus.提现成功 && x.Owner.Id == user.Id))
                        {
                            _withdraw = db.Set<WithdrawalsRecord>()
                                .Where(x => x.Status == WithdrawalsStatus.提现成功 && x.Owner.Id == user.Id)
                                .Sum(x => x.Sum);
                        }
                        e.Add(new XElement("提现", _withdraw));
                        _SiteDataAtDay.Withdrawal += _withdraw;
                        _SiteDataAtMonth.Withdrawal += _withdraw;
                        _PersonalDataAtDay.Withdrawal = _withdraw;
                        _PersonalDataAtMonth.Withdrawal = _withdraw;

                        #endregion

                        #region 投注

                        double _bet = 0;
                        List<BettingStatus> _bettingStatus = new List<BettingStatus> { BettingStatus.即将开奖, BettingStatus.未中奖, BettingStatus.中奖 };
                        if (db.Set<Betting>().Any(x => x.Owner.Id == user.Id && _bettingStatus.Contains(x.Status)))
                        {
                            _bet = db.Set<Betting>()
                                .Where(x => x.Owner.Id == user.Id && _bettingStatus.Contains(x.Status))
                                .Sum(x => x.Pay);
                        }
                        e.Add(new XElement("投注", _bet));
                        _SiteDataAtDay.AmountOfBets += _bet;
                        _SiteDataAtMonth.AmountOfBets += _bet;
                        _PersonalDataAtDay.AmountOfBets = _bet;
                        _PersonalDataAtMonth.AmountOfBets = _bet;

                        #endregion

                        #region 中奖

                        double _bonus = 0;
                        if (db.Set<Betting>().Any(x => x.Owner.Id == user.Id && x.Status == BettingStatus.中奖))
                        {
                            _bonus = db.Set<Betting>()
                                .Where(x => x.Owner.Id == user.Id && x.Status == BettingStatus.中奖)
                                .Sum(x => x.Bonus);
                        }
                        e.Add(new XElement("中奖", _bonus));
                        _SiteDataAtDay.Bonus += _bonus;
                        _SiteDataAtMonth.Bonus += _bonus;
                        _PersonalDataAtDay.Bonus = _bonus;
                        _PersonalDataAtMonth.Bonus = _bonus;

                        #endregion

                        #region 返点

                        double _rebate = 0;
                        if (db.Set<SubordinateDynamic>().Any(x => x.To.Id == user.Id && x.Currency == Currency.元))
                        {
                            _rebate = db.Set<SubordinateDynamic>()
                                .Where(x => x.To.Id == user.Id && x.Currency == Currency.元)
                                .Sum(x => x.Give);
                        }
                        e.Add(new XElement("返点", _rebate));
                        _SiteDataAtDay.ReturnPoints += _rebate;
                        _SiteDataAtMonth.ReturnPoints += _rebate;
                        _PersonalDataAtDay.ReturnPoints = _rebate;
                        _PersonalDataAtMonth.ReturnPoints = _rebate;

                        #endregion

                        #region 活动返还

                        double _return = 0;
                        if (db.Set<ActivityParticipateRecord>().Any(x => x.Owner.Id == user.Id
                            && x.Activity.RewardType == ActivityRewardType.人民币))
                        {
                            _return = db.Set<ActivityParticipateRecord>().Where(x => x.Owner.Id == user.Id
                                    && x.Activity.RewardType == ActivityRewardType.人民币)
                                .Sum(x => x.Activity.Reward);
                        }
                        e.Add(new XElement("活动返还", _return));
                        _SiteDataAtDay.Expenditures += _return;
                        _SiteDataAtMonth.Expenditures += _return;
                        _PersonalDataAtDay.Expenditures = _return;
                        _PersonalDataAtMonth.Expenditures = _return;

                        #endregion

                        double profit = _bonus + _rebate + _return - _bet;
                        e.Add(new XElement("盈利", profit));

                        double money = profit + _recharge - _withdraw;
                        e.Add(new XElement("理论余额", money));
                        _PersonalDataAtDay.Money = money;
                        _PersonalDataAtMonth.Money = money;

                        e.Add(new XElement("实际余额", user.Money));
                        user.Money = money;

                        doc.Add(e);
                        db.Set<PersonalDataAtDay>().Add(_PersonalDataAtDay);
                        db.Set<PersonalDataAtMonth>().Add(_PersonalDataAtMonth);
                        Console.WriteLine(string.Format("统计完毕，当前用户：{0}，Id：{1}", user.Username, user.Id));
                    });

                db.Set<SiteDataAtDay>().Add(_SiteDataAtDay);
                db.Set<SiteDataAtMonth>().Add(_SiteDataAtMonth);

                doc.Save("E:/用户统计数据报表.xml");
                db.SaveChanges();
            }
        }
    }
}
