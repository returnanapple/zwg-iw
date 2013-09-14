using System.Data.Entity;
using IWorld.Model;

namespace IWorld.BLL
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class WebMapContext : DbContext
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个新的数据库上下文
        /// </summary>
        public WebMapContext()
        {
        }

        #endregion

        #region 重写上下文契约

        /// <summary>
        /// 模型创建时候将执行的契约
        /// </summary>
        /// <param name="modelBuilder">上下文模型的构建者对象</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region 用户

            #region Author的契约
            modelBuilder.Entity<Author>().ToTable("IWorld_Author");
            modelBuilder.Entity<Author>().HasKey(x => x.Id);
            modelBuilder.Entity<Author>().HasRequired(x => x.Group)
                .WithMany()
                .Map(x => x.MapKey("GroupId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region UserGroup的契约
            modelBuilder.Entity<UserGroup>().ToTable("IWorld_UserGroup");
            modelBuilder.Entity<UserGroup>().HasKey(x => x.Id);
            #endregion
            #region WithdrawalsRecord的契约
            modelBuilder.Entity<WithdrawalsRecord>().ToTable("IWorld_WithdrawalsRecord");
            modelBuilder.Entity<WithdrawalsRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<WithdrawalsRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region RechargeRecord的契约
            modelBuilder.Entity<RechargeRecord>().ToTable("IWorld_RechargeRecord");
            modelBuilder.Entity<RechargeRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<RechargeRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RechargeRecord>().HasRequired(x => x.Payer)
                .WithMany()
                .Map(x => x.MapKey("PayerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region SubordinateDynamic的契约
            modelBuilder.Entity<SubordinateDynamic>().ToTable("IWorld_SubordinateDynamic");
            modelBuilder.Entity<SubordinateDynamic>().HasKey(x => x.Id);
            modelBuilder.Entity<SubordinateDynamic>().HasRequired(x => x.From)
                .WithMany()
                .Map(x => x.MapKey("FromId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<SubordinateDynamic>().HasRequired(x => x.To)
                .WithMany()
                .Map(x => x.MapKey("ToId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region Spread的契约
            modelBuilder.Entity<Spread>().ToTable("IWorld_Spread");
            modelBuilder.Entity<Spread>().HasKey(x => x.Id);
            modelBuilder.Entity<Spread>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region UserLandingRecord的契约
            modelBuilder.Entity<UserLandingRecord>().ToTable("IWorld_UserLandingRecord");
            modelBuilder.Entity<UserLandingRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<UserLandingRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
            #region 管理员

            #region Administrator的契约
            modelBuilder.Entity<Administrator>().ToTable("IWorld_Administrator");
            modelBuilder.Entity<Administrator>().HasKey(x => x.Id);
            modelBuilder.Entity<Administrator>().HasRequired(x => x.Group)
                .WithMany()
                .Map(x => x.MapKey("GroupId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region UserGroup的契约
            modelBuilder.Entity<AdministratorGroup>().ToTable("IWorld_AdministratorGroup");
            modelBuilder.Entity<AdministratorGroup>().HasKey(x => x.Id);
            #endregion
            #region UserLandingRecord的契约
            modelBuilder.Entity<AdministratorLandingRecord>().ToTable("IWorld_AdministratorLandingRecord");
            modelBuilder.Entity<AdministratorLandingRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<AdministratorLandingRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region OperateRecord的契约
            modelBuilder.Entity<OperateRecord>().ToTable("IWorld_OperateRecord");
            modelBuilder.Entity<OperateRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<OperateRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region TransferRecord的契约
            modelBuilder.Entity<TransferRecord>().ToTable("IWorld_TransferRecord");
            modelBuilder.Entity<TransferRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<TransferRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
            #region 彩票

            #region Lottery的契约
            modelBuilder.Entity<Lottery>().ToTable("IWorld_Lottery");
            modelBuilder.Entity<Lottery>().HasKey(x => x.Id);
            modelBuilder.Entity<Lottery>().HasOptional(x => x.Operator)
                .WithMany()
                .Map(x => x.MapKey("OperatorId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Lottery>().HasRequired(x => x.Ticket)
                .WithMany()
                .Map(x => x.MapKey("TicketId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Lottery>().HasMany(x => x.Seats)
                .WithRequired()
                .Map(x => x.MapKey("LotteryId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region LotterySeat的契约
            modelBuilder.Entity<LotterySeat>().ToTable("IWorld_LotterySeat");
            modelBuilder.Entity<LotterySeat>().HasKey(x => x.Id);
            #endregion
            #region LotteryTicket的契约
            modelBuilder.Entity<LotteryTicket>().ToTable("IWorld_LotteryTicket");
            modelBuilder.Entity<LotteryTicket>().HasKey(x => x.Id);
            modelBuilder.Entity<LotteryTicket>().HasMany(x => x.Tags)
                .WithMany()
                .Map(x => x.ToTable("IWorld_LotteryTicket_Tags"));
            modelBuilder.Entity<LotteryTicket>().HasMany(x => x.Times)
                .WithRequired()
                .HasForeignKey(x => x.TicketId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<LotteryTicket>().HasMany(x => x.Seats)
                .WithRequired()
                .Map(x => x.MapKey("TicketId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region LotteryTicketSeat的契约
            modelBuilder.Entity<LotteryTicketSeat>().ToTable("IWorld_LotteryTicketSeat");
            modelBuilder.Entity<LotteryTicketSeat>().HasKey(x => x.Id);
            #endregion
            #region LotteryTime的契约
            modelBuilder.Entity<LotteryTime>().ToTable("IWorld_LotteryTime");
            modelBuilder.Entity<LotteryTime>().HasKey(x => x.Id);
            modelBuilder.Entity<LotteryTime>().Property(x => x.TimeValue)
                .IsConcurrencyToken();
            #endregion
            #region Betting的契约
            modelBuilder.Entity<Betting>().ToTable("IWorld_Betting");
            modelBuilder.Entity<Betting>().HasKey(x => x.Id);
            modelBuilder.Entity<Betting>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"));
            modelBuilder.Entity<Betting>().HasRequired(x => x.HowToPlay)
                .WithMany()
                .Map(x => x.MapKey("HowToPlayId"));
            modelBuilder.Entity<Betting>().HasMany(x => x.Seats)
                .WithRequired()
                .Map(x => x.MapKey("BettingId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region BettingSeat的契约
            modelBuilder.Entity<BettingSeat>().ToTable("IWorld_BettingSeat");
            modelBuilder.Entity<BettingSeat>().HasKey(x => x.Id);
            #endregion
            #region PlayTag的契约
            modelBuilder.Entity<PlayTag>().ToTable("IWorld_PlayTag");
            modelBuilder.Entity<PlayTag>().HasKey(x => x.Id);
            modelBuilder.Entity<PlayTag>().HasRequired(x => x.Ticket)
                .WithMany()
                .Map(x => x.MapKey("TicketId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlayTag>().HasMany(x => x.HowToPlays)
                .WithMany()
                .Map(x => x.ToTable("IWorld_PlayTag_HowToPlays"));
            #endregion
            #region HowToPlay的契约
            modelBuilder.Entity<HowToPlay>().ToTable("IWorld_HowToPlay");
            modelBuilder.Entity<HowToPlay>().HasKey(x => x.Id);
            modelBuilder.Entity<HowToPlay>().HasRequired(x => x.Tag)
                .WithMany()
                .Map(x => x.MapKey("TagId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<HowToPlay>().HasMany(x => x.Seats)
                .WithRequired()
                .Map(x => x.MapKey("HowToPlayId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region OptionalSeat的契约
            modelBuilder.Entity<OptionalSeat>().ToTable("IWorld_OptionalSeat");
            modelBuilder.Entity<OptionalSeat>().HasKey(x => x.Id);
            #endregion
            #region Chasing的契约
            modelBuilder.Entity<Chasing>().ToTable("IWorld_Chasing");
            modelBuilder.Entity<Chasing>().HasKey(x => x.Id);
            modelBuilder.Entity<Chasing>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Chasing>().HasRequired(x => x.HowToPlay)
                .WithMany()
                .Map(x => x.MapKey("HowToPlayId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Chasing>().HasMany(x => x.Seats)
                .WithRequired()
                .Map(x => x.MapKey("ChasingId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Chasing>().HasMany(x => x.Bettings)
                .WithRequired()
                .Map(x => x.MapKey("ChasingId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region ChasingSeat的契约
            modelBuilder.Entity<ChasingSeat>().ToTable("IWorld_ChasingSeat");
            modelBuilder.Entity<ChasingSeat>().HasKey(x => x.Id);
            #endregion
            #region BettingForCgasing的契约
            modelBuilder.Entity<BettingForCgasing>().ToTable("IWorld_BettingForCgasing");
            modelBuilder.Entity<BettingForCgasing>().HasKey(x => x.Id);
            modelBuilder.Entity<BettingForCgasing>().HasRequired(x => x.Chasing)
                .WithMany()
                .Map(x => x.MapKey("_ChasingId"))
                .WillCascadeOnDelete(false);

            #endregion
            #region VirtualTop的契约
            modelBuilder.Entity<VirtualTop>().ToTable("IWorld_VirtualTop");
            modelBuilder.Entity<VirtualTop>().HasKey(x => x.Id);
            modelBuilder.Entity<VirtualTop>().HasRequired(x => x.Ticket)
                .WithMany()
                .Map(x => x.MapKey("TicketId"))
                .WillCascadeOnDelete(false);

            #endregion

            #endregion
            #region 站内交流

            #region Bulletin的契约
            modelBuilder.Entity<Bulletin>().ToTable("IWorld_Bulletin");
            modelBuilder.Entity<Bulletin>().HasKey(x => x.Id);
            #endregion
            #region Notice的契约
            modelBuilder.Entity<Notice>().ToTable("IWorld_Notice");
            modelBuilder.Entity<Notice>().HasKey(x => x.Id);
            modelBuilder.Entity<Notice>().HasRequired(x => x.To)
                .WithMany()
                .Map(x => x.MapKey("ToId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region Message的契约
            modelBuilder.Entity<Message>().ToTable("IWorld_Message");
            modelBuilder.Entity<Message>().HasKey(x => x.Id);
            modelBuilder.Entity<Message>().HasOptional(x => x.From)
                .WithMany()
                .Map(x => x.MapKey("FromId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Message>().HasOptional(x => x.To)
                .WithMany()
                .Map(x => x.MapKey("ToId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region CustomerRecord的契约
            modelBuilder.Entity<CustomerRecord>().ToTable("IWorld_CustomerRecord");
            modelBuilder.Entity<CustomerRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<CustomerRecord>().HasRequired(x => x.User)
                .WithMany()
                .Map(x => x.MapKey("UserId"))
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
            #region 活动

            #region Activity的契约
            modelBuilder.Entity<Activity>().ToTable("IWorld_Activity");
            modelBuilder.Entity<Activity>().HasKey(x => x.Id);
            modelBuilder.Entity<Activity>().HasMany(x => x.Conditions)
                .WithRequired()
                .Map(x => x.MapKey("ActivityId"));
            #endregion
            #region ActivityCondition的契约
            modelBuilder.Entity<ActivityCondition>().ToTable("IWorld_ActivityCondition");
            modelBuilder.Entity<ActivityCondition>().HasKey(x => x.Id);
            #endregion
            #region ActivityParticipateRecord的契约
            modelBuilder.Entity<ActivityParticipateRecord>().ToTable("IWorld_ActivityParticipateRecord");
            modelBuilder.Entity<ActivityParticipateRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<ActivityParticipateRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ActivityParticipateRecord>().HasRequired(x => x.Activity)
                .WithMany()
                .Map(x => x.MapKey("ActivityId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region Exchange的契约
            modelBuilder.Entity<Exchange>().ToTable("IWorld_Exchange");
            modelBuilder.Entity<Exchange>().HasKey(x => x.Id);
            modelBuilder.Entity<Exchange>().HasMany(x => x.Prizes)
                .WithRequired()
                .Map(x => x.MapKey("ExchangeId"));
            modelBuilder.Entity<Exchange>().HasMany(x => x.Conditions)
                .WithRequired()
                .Map(x => x.MapKey("ExchangeId"));
            #endregion
            #region Prize的契约
            modelBuilder.Entity<Prize>().ToTable("IWorld_Prize");
            modelBuilder.Entity<Prize>().HasKey(x => x.Id);
            #endregion
            #region ExchangeCondition的契约
            modelBuilder.Entity<ExchangeCondition>().ToTable("IWorld_ExchangeCondition");
            modelBuilder.Entity<ExchangeCondition>().HasKey(x => x.Id);
            #endregion
            #region ExchangeParticipateRecord的契约
            modelBuilder.Entity<ExchangeParticipateRecord>().ToTable("IWorld_ExchangeParticipateRecord");
            modelBuilder.Entity<ExchangeParticipateRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<ExchangeParticipateRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ExchangeParticipateRecord>().HasRequired(x => x.Exchange)
                .WithMany()
                .Map(x => x.MapKey("ExchangeId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ExchangeParticipateRecord>().HasMany(x => x.Gifts)
                .WithRequired()
                .Map(x => x.MapKey("ExchangeParticipateRecordId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region GiftRecord的契约
            modelBuilder.Entity<GiftRecord>().ToTable("IWorld_GiftRecord");
            modelBuilder.Entity<GiftRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<GiftRecord>().HasRequired(x => x.Exchange)
                .WithMany()
                .Map(x => x.MapKey("ExchangeId"))
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<GiftRecord>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
            #region 系统参数

            #region BankAccount的契约
            modelBuilder.Entity<BankAccount>().ToTable("IWorld_BankAccount");
            modelBuilder.Entity<BankAccount>().HasKey(x => x.Id);
            #endregion
            #region EmailAccount的契约
            modelBuilder.Entity<EmailAccount>().ToTable("IWorld_EmailAccount");
            modelBuilder.Entity<EmailAccount>().HasKey(x => x.Id);
            modelBuilder.Entity<EmailAccount>().HasOptional(x => x.Client)
                .WithMany()
                .Map(x => x.MapKey("ClientId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region EmailClient的契约
            modelBuilder.Entity<EmailClient>().ToTable("IWorld_EmailClient");
            modelBuilder.Entity<EmailClient>().HasKey(x => x.Id);
            #endregion

            #endregion
            #region 数据统计

            #region SiteDataAtDay的契约
            modelBuilder.Entity<SiteDataAtDay>().ToTable("IWorld_SiteDataAtDay");
            modelBuilder.Entity<SiteDataAtDay>().HasKey(x => x.Id);
            #endregion
            #region SiteDataAtMonth的契约
            modelBuilder.Entity<SiteDataAtMonth>().ToTable("IWorld_SiteDataAtMonth");
            modelBuilder.Entity<SiteDataAtMonth>().HasKey(x => x.Id);
            #endregion
            #region PersonalDataAtDay的契约
            modelBuilder.Entity<PersonalDataAtDay>().ToTable("IWorld_PersonalDataAtDay");
            modelBuilder.Entity<PersonalDataAtDay>().HasKey(x => x.Id);
            modelBuilder.Entity<PersonalDataAtDay>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion
            #region PersonalDataAtMonth的契约
            modelBuilder.Entity<PersonalDataAtMonth>().ToTable("IWorld_PersonalDataAtMonth");
            modelBuilder.Entity<PersonalDataAtMonth>().HasKey(x => x.Id);
            modelBuilder.Entity<PersonalDataAtMonth>().HasRequired(x => x.Owner)
                .WithMany()
                .Map(x => x.MapKey("OwnerId"))
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
        }

        #endregion
    }
}
