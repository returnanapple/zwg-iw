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
            SaveToData();
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
    }
}
