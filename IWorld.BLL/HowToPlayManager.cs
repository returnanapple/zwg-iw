using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using IWorld.Model;
using IWorld.Setting;
using IWorld.Helper;

namespace IWorld.BLL
{
    /// <summary>
    /// 玩法的管理者对象
    /// </summary>
    public class HowToPlayManager : SimplifyManagerBase<HowToPlay>, IManager<HowToPlay>, ISimplify<HowToPlay>
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的玩法的管理者对象
        /// </summary>
        /// <param name="db">数据库连接对象</param>
        public HowToPlayManager(DbContext db)
            : base(db)
        {
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// 编辑玩法的可选位
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        /// <param name="seats">位的集合</param>
        public void EditSeats(int howToPlayId, List<Factory.IPackageForSeat> seats)
        {
            NChecker.CheckEntity<HowToPlay>(howToPlayId, "玩法", db);
            HowToPlay howToPlay = db.Set<HowToPlay>().Find(howToPlayId);
            List<OptionalSeat> _seats = seats.ConvertAll(x => x.GetSeat(howToPlay.Tag.Ticket));
            var osSet = db.Set<OptionalSeat>();

            List<OptionalSeat> rSeates = howToPlay.Seats.Where(x => !_seats.Any(t => t.Name == x.Name)).ToList();
            howToPlay.Seats.RemoveAll(x => rSeates.Contains(x));
            rSeates.ForEach(x =>
                {
                    osSet.Remove(x);
                });
            howToPlay.Seats.ForEach(x =>
                {
                    var tSeat = _seats.FirstOrDefault(t => t.Name == x.Name);
                    x.IsRequired = tSeat.IsRequired;
                    x.Values = tSeat.Values;
                    x.ValuesForLarge = tSeat.ValuesForLarge;
                    x.ValuesForSmall = tSeat.ValuesForSmall;
                    x.ValuesForSingle = tSeat.ValuesForSingle;
                    x.ValuesForDouble = tSeat.ValuesForDouble;
                    x.Order = tSeat.Order;
                    x.LimitOfPick = tSeat.LimitOfPick;
                    x.UpperOfPick = tSeat.UpperOfPick;
                    x.ModifiedTime = DateTime.Now;
                });
            _seats.Where(x => !howToPlay.Seats.Any(t => t.Name == x.Name)).ToList()
                .ForEach(x =>
                    {
                        howToPlay.Seats.Add(x);
                    });
            db.SaveChanges();
        }

        /// <summary>
        /// 显示玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        public void Show(int howToPlayId)
        {
            NChecker.CheckEntity<HowToPlay>(howToPlayId, "玩法", db);
            HowToPlay howToPlay = db.Set<HowToPlay>().Find(howToPlayId);
            if (!howToPlay.Hide)
            {
                throw new Exception("该玩法没有被隐藏");
            }

            howToPlay.Hide = false;
            howToPlay.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// 隐藏玩法
        /// </summary>
        /// <param name="howToPlayId">目标玩法的存储指针</param>
        public void Hide(int howToPlayId)
        {
            NChecker.CheckEntity<HowToPlay>(howToPlayId, "玩法", db);
            HowToPlay howToPlay = db.Set<HowToPlay>().Find(howToPlayId);
            if (howToPlay.Hide)
            {
                throw new Exception("该玩法已经被隐藏");
            }

            howToPlay.Hide = true;
            howToPlay.ModifiedTime = DateTime.Now;
            db.SaveChanges();
        }

        #endregion

        #region 内嵌类型

        /// <summary>
        /// 内部工厂
        /// </summary>
        public class Factory
        {
            #region 静态方法

            /// <summary>
            /// 创建一个用于新建玩法的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="description">描述</param>
            /// <param name="rule">规则</param>
            /// <param name="tagId">所属的玩法标签的存储指针</param>
            /// <param name="lowerSeats">选位下限</param>
            /// <param name="upperSeats">选位上限</param>
            /// <param name="odds">赔率（如为0则采用系统参数）</param>
            /// <param name="conversionRates">赔率/返点数转化率（如为0则采用系统参数）</param>
            /// <param name="cardinalNumber">返奖基数（如为0则采用系统参数）</param>
            /// <param name="_interface">所采用的返奖接口</param>
            /// <param name="isStackedBit">叠位</param>
            /// <param name="allowFreeSeats">允许自选位</param>
            /// <param name="order">排序系数</param>
            /// <param name="seats">位</param>
            /// <param name="parameter1">可选参数1</param>
            /// <param name="parameter2">可选参数2</param>
            /// <param name="parameter3">可选参数3</param>
            /// <returns>返回用于新建玩法的数据集</returns>
            public static ICreatePackage<HowToPlay> CreatePackageForCreate(string name, string description, string rule, int tagId
                , int lowerSeats, int upperSeats, double odds, double conversionRates, double cardinalNumber, string _interface
                , bool isStackedBit, bool allowFreeSeats, int order, List<IPackageForSeat> seats, int parameter1 = 0
                , int parameter2 = 0, int parameter3 = 0)
            {
                LotteryInterface __interface = EnumHelper.Parse<LotteryInterface>(_interface);

                return new PackageForCreate(name, description, rule, tagId, lowerSeats, upperSeats, odds, conversionRates
                    , cardinalNumber, __interface, isStackedBit, allowFreeSeats, parameter1, parameter2, parameter3, order, seats);
            }

            /// <summary>
            /// 创建一个用于更新玩法信息的数据集
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="description">描述</param>
            /// <param name="rule">规则</param>
            /// <param name="lowerSeats">选位下限</param>
            /// <param name="upperSeats">选位上限</param>
            /// <param name="odds">赔率（如为0则采用系统参数）</param>
            /// <param name="conversionRates">赔率/返点数转化率（如为0则采用系统参数）</param>
            /// <param name="cardinalNumber">返奖基数（如为0则采用系统参数）</param>
            /// <param name="_interface">所采用的返奖接口</param>
            /// <param name="isStackedBit">叠位</param>
            /// <param name="allowFreeSeats">允许自选位</param>
            /// <param name="order">排序系数</param>
            /// <param name="parameter1">可选参数1</param>
            /// <param name="parameter2">可选参数2</param>
            /// <param name="parameter3">可选参数3</param>
            /// <returns>返回用于更新玩法信息的数据集</returns>
            public static IUpdatePackage<HowToPlay> CreatePackageForUpdate(int id, string name, string description, string rule
                , int lowerSeats, int upperSeats, double odds, double conversionRates, double cardinalNumber, string _interface
                , bool isStackedBit, bool allowFreeSeats, int order, int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
            {
                LotteryInterface __interface = EnumHelper.Parse<LotteryInterface>(_interface);

                return new PackageForUpdate(id, name, description, rule, lowerSeats, upperSeats, odds, conversionRates
                    , cardinalNumber, __interface, isStackedBit, allowFreeSeats, parameter1, parameter2, parameter3, order);
            }

            /// <summary>
            /// 创建一个用于更新玩法信息的数据集（基础）
            /// </summary>
            /// <param name="id">存储指针</param>
            /// <param name="name">名称</param>
            /// <param name="description">描述</param>
            /// <param name="rule">规则</param>
            /// <param name="odds">赔率</param>
            /// <param name="order">排序系数</param>
            /// <returns>返回用于更新玩法信息的数据集（基础）</returns>
            public static IUpdatePackage<HowToPlay> CreatePackageForUpdate_Basic(int id, string name, string description, string rule
                , double odds, int order)
            {
                return new PackageForUpdate_Basic(id, name, description, rule, odds, order);
            }

            /// <summary>
            /// 创建一个新建位的数据集
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="isRequired">必选</param>
            /// <param name="values">对应的号码集合</param>
            /// <param name="valuesForLarge">“大”选项所选号码集合</param>
            /// <param name="valuesForSmall">“小”选项所选号码集合</param>
            /// <param name="valuesForSingle">“单”选项所选号码集合</param>
            /// <param name="valuesForDouble">“双”选项所选号码集合</param>
            /// <param name="limitOfPick">最少选号</param>
            /// <param name="upperOfPick">最多选号</param>
            /// <returns>返回新建位的数据集</returns>
            public static IPackageForSeat CreatePackageForSeat(string name, bool isRequired, string values, string valuesForLarge
                , string valuesForSmall, string valuesForSingle, string valuesForDouble, int limitOfPick, int upperOfPick)
            {
                return new PackageForSeat(name, isRequired, values, valuesForLarge, valuesForSmall, valuesForSingle
                    , valuesForDouble, limitOfPick, upperOfPick);
            }

            #endregion

            #region 内嵌类型

            /// <summary>
            /// 用于新建玩法的数据集
            /// </summary>
            private class PackageForCreate : IPackage<HowToPlay>, ICreatePackage<HowToPlay>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 描述
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 规则
                /// </summary>
                public string Rule { get; set; }

                /// <summary>
                /// 所属的玩法标签的存储指针
                /// </summary>
                public int TagId { get; set; }

                /// <summary>
                /// 选位下限
                /// </summary>
                public int LowerSeats { get; set; }

                /// <summary>
                /// 选位上限
                /// </summary>
                public int UpperSeats { get; set; }

                /// <summary>
                /// 赔率（如为0则采用系统参数）
                /// </summary>
                public double Odds { get; set; }

                /// <summary>
                /// 赔率/返点数转化率（如为0则采用系统参数）
                /// </summary>
                public double ConversionRates { get; set; }

                /// <summary>
                /// 返奖基数（如为0则采用系统参数）
                /// </summary>
                public double CardinalNumber { get; set; }

                /// <summary>
                /// 所采用的返奖接口
                /// </summary>
                public LotteryInterface Interface { get; set; }

                /// <summary>
                /// 叠位
                /// </summary>
                public bool IsStackedBit { get; set; }

                /// <summary>
                /// 允许自选位
                /// </summary>
                public bool AllowFreeSeats { get; set; }

                /// <summary>
                /// 可选参数1
                /// </summary>
                public int Parameter1 { get; set; }

                /// <summary>
                /// 可选参数2
                /// </summary>
                public int Parameter2 { get; set; }

                /// <summary>
                /// 可选参数3
                /// </summary>
                public int Parameter3 { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                /// <summary>
                /// 位
                /// </summary>
                public List<IPackageForSeat> Seats { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于新建玩法的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="description">描述</param>
                /// <param name="rule">规则</param>
                /// <param name="tagId">所属的玩法标签的存储指针</param>
                /// <param name="lowerSeats">选位下限</param>
                /// <param name="upperSeats">选位上限</param>
                /// <param name="odds">赔率（如为0则采用系统参数）</param>
                /// <param name="conversionRates">赔率/返点数转化率（如为0则采用系统参数）</param>
                /// <param name="cardinalNumber">返奖基数（如为0则采用系统参数）</param>
                /// <param name="_interface">所采用的返奖接口</param>
                /// <param name="isStackedBit">叠位</param>
                /// <param name="allowFreeSeats">允许自选位</param>
                /// <param name="parameter1">可选参数1</param>
                /// <param name="parameter2">可选参数2</param>
                /// <param name="parameter3">可选参数3</param>
                /// <param name="order">排序系数</param>
                /// <param name="seats">位</param>
                public PackageForCreate(string name, string description, string rule, int tagId, int lowerSeats, int upperSeats
                    , double odds, double conversionRates, double cardinalNumber, LotteryInterface _interface, bool isStackedBit
                    , bool allowFreeSeats, int parameter1, int parameter2, int parameter3, int order, List<IPackageForSeat> seats)
                {
                    this.Name = name;
                    this.Description = description;
                    this.Rule = rule;
                    this.TagId = tagId;
                    this.LowerSeats = lowerSeats;
                    this.UpperSeats = upperSeats;
                    this.Odds = odds;
                    this.ConversionRates = conversionRates;
                    this.CardinalNumber = cardinalNumber;
                    this.Interface = _interface;
                    this.IsStackedBit = isStackedBit;
                    this.AllowFreeSeats = allowFreeSeats;
                    this.Parameter1 = parameter1;
                    this.Parameter2 = parameter2;
                    this.Parameter3 = parameter3;
                    this.Order = order;
                    this.Seats = seats;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public void CheckData(DbContext db)
                {
                    WebSetting webSetting = new WebSetting();
                    NChecker.CheckEntity<PlayTag>(this.TagId, "标签", db);
                    if (db.Set<HowToPlay>().Any(x => x.Name == this.Name
                        && x.Tag.Id == this.TagId))
                    {
                        throw new Exception("已经存在同名的玩法");
                    }
                    int seatCount = db.Set<PlayTag>().Where(x => x.Id == this.TagId)
                        .Select(x => x.Ticket.Seats)
                        .FirstOrDefault().Count();
                    if (this.LowerSeats < 1
                        || this.LowerSeats > seatCount)
                    {
                        throw new Exception(string.Format("选位下限（{0}）超出合法范围（{1}-{2}）", this.LowerSeats, 1, seatCount));
                    }
                    if (this.UpperSeats < 1
                        || this.UpperSeats > seatCount)
                    {
                        throw new Exception(string.Format("选位上限（{0}）超出合法范围（{1}-{2}）", this.UpperSeats, 1, seatCount));
                    }
                    if (this.LowerSeats > this.UpperSeats)
                    {
                        throw new Exception("选位下限不得大于选位上限");
                    }
                    if (this.Odds < 0)
                    {
                        throw new Exception("赔率不能小于0");
                    }
                    if (this.ConversionRates < 0)
                    {
                        throw new Exception("赔率/返点数转化率不能小于0");
                    }
                    if (this.CardinalNumber < webSetting.MinimumBonusMode
                        && this.CardinalNumber != 0)
                    {
                        throw new Exception(string.Format("返奖基数不能小于{0}", webSetting.MinimumBonusMode));
                    }
                    LotteryTicket ticket = db.Set<PlayTag>().Where(x => x.Id == this.TagId)
                        .Select(x => x.Ticket).FirstOrDefault();
                    this.Seats.ForEach(x =>
                        {
                            x.CheckSeat(ticket);
                        });
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public HowToPlay GetEntity(DbContext db)
                {
                    PlayTag tag = db.Set<PlayTag>().Find(this.TagId);
                    List<OptionalSeat> seats = new List<OptionalSeat>();
                    this.Seats.ForEach(x =>
                        {
                            seats.Add(x.GetSeat(tag.Ticket));
                        });

                    return new HowToPlay(this.Name, this.Description, this.Rule, tag, this.LowerSeats, this.UpperSeats, this.Odds
                        , this.ConversionRates, this.CardinalNumber, this.Interface, this.IsStackedBit, this.AllowFreeSeats
                        , seats, this.Parameter1, this.Parameter2, this.Parameter3, this.Order);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新玩法信息的数据集（基础）
            /// </summary>
            private class PackageForUpdate_Basic : PackageForUpdateBase<HowToPlay>, IPackage<HowToPlay>, IUpdatePackage<HowToPlay>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 描述
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 规则
                /// </summary>
                public string Rule { get; set; }

                /// <summary>
                /// 赔率
                /// </summary>
                public double Odds { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新玩法信息的数据集（基础）
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="description">描述</param>
                /// <param name="rule">规则</param>
                /// <param name="odds">赔率</param>
                /// <param name="order">排序系数</param>
                public PackageForUpdate_Basic(int id, string name, string description, string rule, double odds, int order)
                    : base(id)
                {
                    this.Name = name;
                    this.Description = description;
                    this.Rule = rule;
                    this.Odds = odds;
                    this.Order = order;
                }

                #endregion

                #region  实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    base.CheckData(db);
                    int tagId = (from c in db.Set<HowToPlay>()
                                 where c.Id == this.Id
                                 select c.Tag.Id)
                                    .FirstOrDefault();
                    if (db.Set<HowToPlay>().Any(x => x.Name == this.Name
                        && x.Tag.Id == tagId
                        && x.Id != this.Id))
                    {
                        throw new Exception("已经存在同名的玩法");
                    }
                    if (this.Odds < 0)
                    {
                        throw new Exception("赔率不能小于0");
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override HowToPlay GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Description", this.Description);
                    this.AddToUpdating("Rule", this.Rule);
                    this.AddToUpdating("Odds", this.Odds);
                    this.AddToUpdating("Order", this.Order);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 用于更新玩法信息的数据集
            /// </summary>
            private class PackageForUpdate : PackageForUpdateBase<HowToPlay>, IPackage<HowToPlay>, IUpdatePackage<HowToPlay>
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 描述
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 规则
                /// </summary>
                public string Rule { get; set; }

                /// <summary>
                /// 选位下限
                /// </summary>
                public int LowerSeats { get; set; }

                /// <summary>
                /// 选位上限
                /// </summary>
                public int UpperSeats { get; set; }

                /// <summary>
                /// 赔率（如为0则采用系统参数）
                /// </summary>
                public double Odds { get; set; }

                /// <summary>
                /// 赔率/返点数转化率（如为0则采用系统参数）
                /// </summary>
                public double ConversionRates { get; set; }

                /// <summary>
                /// 返奖基数（如为0则采用系统参数）
                /// </summary>
                public double CardinalNumber { get; set; }

                /// <summary>
                /// 所采用的返奖接口
                /// </summary>
                public LotteryInterface Interface { get; set; }

                /// <summary>
                /// 叠位
                /// </summary>
                public bool IsStackedBit { get; set; }

                /// <summary>
                /// 允许自选位
                /// </summary>
                public bool AllowFreeSeats { get; set; }

                /// <summary>
                /// 可选参数1
                /// </summary>
                public int Parameter1 { get; set; }

                /// <summary>
                /// 可选参数2
                /// </summary>
                public int Parameter2 { get; set; }

                /// <summary>
                /// 可选参数3
                /// </summary>
                public int Parameter3 { get; set; }

                /// <summary>
                /// 排序系数
                /// </summary>
                public int Order { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的用于更新玩法信息的数据集
                /// </summary>
                /// <param name="id">存储指针</param>
                /// <param name="name">名称</param>
                /// <param name="description">描述</param>
                /// <param name="rule">规则</param>
                /// <param name="lowerSeats">选位下限</param>
                /// <param name="upperSeats">选位上限</param>
                /// <param name="odds">赔率（如为0则采用系统参数）</param>
                /// <param name="conversionRates">赔率/返点数转化率（如为0则采用系统参数）</param>
                /// <param name="cardinalNumber">返奖基数（如为0则采用系统参数）</param>
                /// <param name="_interface">所采用的返奖接口</param>
                /// <param name="isStackedBit">叠位</param>
                /// <param name="allowFreeSeats">允许自选位</param>
                /// <param name="parameter1">可选参数1</param>
                /// <param name="parameter2">可选参数2</param>
                /// <param name="parameter3">可选参数3</param>
                /// <param name="order">排序系数</param>
                public PackageForUpdate(int id, string name, string description, string rule, int lowerSeats, int upperSeats
                    , double odds, double conversionRates, double cardinalNumber, LotteryInterface _interface, bool isStackedBit
                    , bool allowFreeSeats, int parameter1, int parameter2, int parameter3, int order)
                    : base(id)
                {
                    this.Name = name;
                    this.Description = description;
                    this.Rule = rule;
                    this.LowerSeats = lowerSeats;
                    this.UpperSeats = upperSeats;
                    this.Odds = odds;
                    this.ConversionRates = conversionRates;
                    this.CardinalNumber = cardinalNumber;
                    this.Interface = _interface;
                    this.IsStackedBit = isStackedBit;
                    this.AllowFreeSeats = allowFreeSeats;
                    this.Parameter1 = parameter1;
                    this.Parameter2 = parameter2;
                    this.Parameter3 = parameter3;
                    this.Order = order;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查数据集的内容是否符合定义
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                public override void CheckData(DbContext db)
                {
                    WebSetting webSetting = new WebSetting();
                    base.CheckData(db);
                    var tag = db.Set<HowToPlay>().Where(x => x.Id == this.Id)
                        .Select(x => x.Tag).FirstOrDefault();
                    if (db.Set<HowToPlay>().Any(x => x.Name == this.Name
                        && x.Tag.Id == tag.Id
                        && x.Id != this.Id))
                    {
                        throw new Exception("已经存在同名的玩法");
                    }
                    List<LotteryTicketSeat> seats = tag.Ticket.Seats;
                    if (this.LowerSeats < 1
                        || this.LowerSeats > seats.Count)
                    {
                        throw new Exception(string.Format("选位下限（{0}）超出合法范围（{1}-{2}）", this.LowerSeats, 1, seats.Count));
                    }
                    if (this.UpperSeats < 1
                        || this.UpperSeats > seats.Count)
                    {
                        throw new Exception(string.Format("选位上限（{0}）超出合法范围（{1}-{2}）", this.UpperSeats, 1, seats.Count));
                    }
                    if (this.LowerSeats > this.UpperSeats)
                    {
                        throw new Exception("选位下限不得大于选位上限");
                    }
                    if (this.Odds < 0)
                    {
                        throw new Exception("赔率不能小于0");
                    }
                    if (this.ConversionRates < 0)
                    {
                        throw new Exception("赔率/返点数转化率不能小于0");
                    }
                    if (this.CardinalNumber < webSetting.MinimumBonusMode
                        && this.CardinalNumber != 0)
                    {
                        throw new Exception(string.Format("返奖基数不能小于{0}", webSetting.MinimumBonusMode));
                    }
                }

                /// <summary>
                /// 获取实体对象
                /// </summary>
                /// <param name="db">数据库连接对象</param>
                /// <returns>返回泛型状态所规定的实体类</returns>
                public override HowToPlay GetEntity(DbContext db)
                {
                    this.AddToUpdating("Name", this.Name);
                    this.AddToUpdating("Description", this.Description);
                    this.AddToUpdating("Rule", this.Rule);
                    this.AddToUpdating("LowerSeats", this.LowerSeats);
                    this.AddToUpdating("UpperSeats", this.UpperSeats);
                    this.AddToUpdating("Odds", this.Odds);
                    this.AddToUpdating("ConversionRates", this.ConversionRates);
                    this.AddToUpdating("CardinalNumber", this.CardinalNumber);
                    this.AddToUpdating("Interface", this.Interface);
                    this.AddToUpdating("IsStackedBit", this.IsStackedBit);
                    this.AddToUpdating("AllowFreeSeats", this.AllowFreeSeats);
                    this.AddToUpdating("Parameter1", this.Parameter1);
                    this.AddToUpdating("Parameter2", this.Parameter2);
                    this.AddToUpdating("Parameter3", this.Parameter3);
                    this.AddToUpdating("Order", this.Order);

                    return base.GetEntity(db);
                }

                #endregion
            }

            /// <summary>
            /// 新建位的数据集
            /// </summary>
            private class PackageForSeat : IPackageForSeat
            {
                #region 公开属性

                /// <summary>
                /// 名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 必选
                /// </summary>
                public bool IsRequired { get; set; }

                /// <summary>
                /// 对应的号码集合
                /// </summary>
                public string Values { get; set; }

                /// <summary>
                /// “大”选项所选号码集合
                /// </summary>
                public string ValuesForLarge { get; set; }

                /// <summary>
                /// “小”选项所选号码集合
                /// </summary>
                public string ValuesForSmall { get; set; }

                /// <summary>
                /// “单”选项所选号码集合
                /// </summary>
                public string ValuesForSingle { get; set; }

                /// <summary>
                /// “双”选项所选号码集合
                /// </summary>
                public string ValuesForDouble { get; set; }

                /// <summary>
                /// 最少选号
                /// </summary>
                public int LimitOfPick { get; set; }

                /// <summary>
                /// 最多选号
                /// </summary>
                public int UpperOfPick { get; set; }

                #endregion

                #region 构造方法

                /// <summary>
                /// 实例化一个新的新建位的数据集
                /// </summary>
                /// <param name="name">名称</param>
                /// <param name="isRequired">必选</param>
                /// <param name="values">对应的号码集合</param>
                /// <param name="valuesForLarge">“大”选项所选号码集合</param>
                /// <param name="valuesForSmall">“小”选项所选号码集合</param>
                /// <param name="valuesForSingle">“单”选项所选号码集合</param>
                /// <param name="valuesForDouble">“双”选项所选号码集合</param>
                /// <param name="limitOfPick">最少选号</param>
                /// <param name="upperOfPick">最多选号</param>
                public PackageForSeat(string name, bool isRequired, string values, string valuesForLarge, string valuesForSmall
                    , string valuesForSingle, string valuesForDouble, int limitOfPick, int upperOfPick)
                {
                    this.Name = name;
                    this.IsRequired = isRequired;
                    this.Values = values;
                    this.ValuesForLarge = valuesForLarge;
                    this.ValuesForSmall = valuesForSmall;
                    this.ValuesForSingle = valuesForSingle;
                    this.ValuesForDouble = valuesForDouble;
                    this.LimitOfPick = limitOfPick;
                    this.UpperOfPick = upperOfPick;
                }

                #endregion

                #region 实例方法

                /// <summary>
                /// 检查位信息的合法性
                /// </summary>
                /// <param name="ticket">彩票</param>
                public void CheckSeat(LotteryTicket ticket)
                {
                    if (!ticket.Seats.Any(x => x.Name == this.Name))
                    {
                        throw new Exception(string.Format("位：{0} 并不属于彩票：{1}", this.Name, ticket.Name));
                    }
                    if (this.UpperOfPick < this.LimitOfPick)
                    {
                        throw new Exception("最大选位不能小于最小选位");
                    }
                    var seat = ticket.Seats.FirstOrDefault(x => x.Name == this.Name);
                    if (this.LimitOfPick < 0 || this.LimitOfPick > seat.ValueList.Count)
                    {
                        throw new Exception(string.Format("最小选位不能超出合法范围：{0} - {1}", 0, seat.ValueList.Count));
                    }
                    if (this.UpperOfPick < 0 || this.UpperOfPick > seat.ValueList.Count)
                    {
                        throw new Exception(string.Format("最大选位不能超出合法范围：{0} - {1}", 0, seat.ValueList.Count));
                    }
                    List<string> tValues = this.Values.Split(new char[] { ',' }).ToList();
                    List<string> tValuesForLarge = this.ValuesForLarge.Split(new char[] { ',' }).ToList();
                    List<string> tValuesForSmall = this.ValuesForSmall.Split(new char[] { ',' }).ToList();
                    List<string> tValuesForSingle = this.ValuesForSingle.Split(new char[] { ',' }).ToList();
                    List<string> tValuesForDouble = this.ValuesForDouble.Split(new char[] { ',' }).ToList();
                    tValues.ForEach(value =>
                        {
                            if (!seat.ValueList.Any(x => x == value))
                            {
                                throw new Exception(string.Format("位：{0} 中并不包括值：{1}", this.Name, value));
                            }
                        });
                    if (tValuesForLarge.Any(value => !tValues.Contains(value)))
                    {
                        throw new Exception("“大”选项所选号码集合中有号码不属于主集合");
                    }
                    if (tValuesForSmall.Any(value => !tValues.Contains(value)))
                    {
                        throw new Exception("“小”选项所选号码集合中有号码不属于主集合");
                    }
                    if (tValuesForSingle.Any(value => !tValues.Contains(value)))
                    {
                        throw new Exception("“单”选项所选号码集合中有号码不属于主集合");
                    }
                    if (tValuesForDouble.Any(value => !tValues.Contains(value)))
                    {
                        throw new Exception("“双”选项所选号码集合中有号码不属于主集合");
                    }
                }

                /// <summary>
                /// 获取可选位
                /// </summary>
                /// <param name="ticket">彩票</param>
                /// <returns>返回可选位的封装</returns>
                public OptionalSeat GetSeat(LotteryTicket ticket)
                {
                    var seat = ticket.Seats.FirstOrDefault(x => x.Name == this.Name);

                    return new OptionalSeat(this.Name, this.IsRequired, this.Values, this.ValuesForLarge, this.ValuesForSmall
                        , this.ValuesForSingle, this.ValuesForDouble, seat.Order, this.LimitOfPick, this.UpperOfPick);
                }

                #endregion
            }

            #endregion

            #region 内嵌接口

            /// <summary>
            /// 定义用于新建位的数据集
            /// </summary>
            public interface IPackageForSeat
            {
                /// <summary>
                /// 检查位信息的合法性
                /// </summary>
                /// <param name="ticket">彩票</param>
                void CheckSeat(LotteryTicket ticket);

                /// <summary>
                /// 获取可选位
                /// </summary>
                /// <param name="ticket">彩票</param>
                /// <returns>返回可选位的封装</returns>
                OptionalSeat GetSeat(LotteryTicket ticket);
            }

            #endregion
        }

        #endregion
    }
}
