using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Data.Enums;
using Microsoft.EntityFrameworkCore;


namespace CRM.Data.Database
{
    public partial class CRMDbContext : DbContext
    {
        public CRMDbContext()
        {
        }

        public CRMDbContext(DbContextOptions<CRMDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            CheckForInvalidSystemTagModifications();

            foreach (var entry in ChangeTracker.Entries<Deal>())
            {
                if (entry.State == EntityState.Modified && entry.Property(e => e.DealStatus).IsModified)
                {
                    DealHistory.Add(new Data.Database.DealHistory
                    {
                        ChangedValue = entry.Entity.DealStatus.ToString(),
                        ChangeType = (short)DealChangeType.Status,
                        DealGuid = entry.Entity.DealGuid,
                        RegisteredDate = DateTime.Now,
                        UserGuid = entry.Entity.CurrentUserGuid
                    });
                }

                if (entry.State == EntityState.Modified && entry.Property(e => e.UnderwriterUserGuid).IsModified)
                {
                    DealHistory.Add(new Data.Database.DealHistory
                    {
                        ChangedValue = entry.Entity.UnderwriterUserGuid?.ToString() ?? "N/A",
                        ChangeType = (short)DealChangeType.ResponsiblePerson,
                        DealGuid = entry.Entity.DealGuid,
                        RegisteredDate = DateTime.Now,
                        UserGuid = entry.Entity.CurrentUserGuid
                    });
                }

                if (entry.State == EntityState.Modified && entry.Property(e => e.ResponsiblePersonType).IsModified)
                {
                    if (entry.Entity.DealStatus == (int)DealStatus.SurveySent
                            || entry.Entity.DealStatus == (int)DealStatus.Linked
                            || entry.Entity.DealStatus == (int)DealStatus.Agreed)
                    {
                        DealHistory.Add(new Data.Database.DealHistory
                        {
                            ChangedValue = entry.Entity.CreatedByUserGuid.ToString() ?? "N/A",
                            ChangeType = (short)DealChangeType.ResponsiblePerson,
                            DealGuid = entry.Entity.DealGuid,
                            RegisteredDate = DateTime.Now,
                            UserGuid = entry.Entity.CurrentUserGuid
                        });
                    }
                    else
                    {
                        DealHistory.Add(new Data.Database.DealHistory
                        {
                            ChangedValue = entry.Entity.Discussion?.LastOrDefault()?.ReceiverGuid?.ToString() ?? "N/A",
                            ChangeType = (short)DealChangeType.ResponsiblePerson,
                            DealGuid = entry.Entity.DealGuid,
                            RegisteredDate = DateTime.Now,
                            UserGuid = entry.Entity.CurrentUserGuid
                        });
                    }
                }

                if (entry.Entity is Deal deal && entry.Collection(e => e.Discussion).IsModified)
                {
                    var discussions = deal.Discussion; // Get the discussions navigation property
                    var lastDiscussion = discussions?.LastOrDefault(); // Get the last discussion in the list (if available)

                    if (lastDiscussion != null)
                    {
                        // Check the original and current values of ReceiverGuid for the last discussion
                        var originalReceiverGuid = Entry(lastDiscussion).Property("ReceiverGuid").OriginalValue?.ToString();
                        var currentReceiverGuid = Entry(lastDiscussion).Property("ReceiverGuid").CurrentValue.ToString();

                        if (originalReceiverGuid != currentReceiverGuid)
                        {
                            DealHistory.Add(new Data.Database.DealHistory
                            {
                                ChangedValue = currentReceiverGuid?.ToString() ?? "N/A",
                                ChangeType = (short)DealChangeType.InnerResponsibleChange,
                                DealGuid = deal.DealGuid,
                                RegisteredDate = DateTime.Now,
                                UserGuid = deal.CurrentUserGuid
                            });
                        }
                    }
                }



            }

            return base.SaveChanges();
        }
        public virtual DbSet<ClientHistory> ClientHistories { get; set; }
        public virtual DbSet<AssetDetails> AssetDetails { get; set; }
        public virtual DbSet<DealHistory> DealHistory { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<CallHistory> CallHistories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientContactInfoComp> ClientContactInfoComps { get; set; }
        public virtual DbSet<ClientRef> ClientRefs { get; set; }
        public virtual DbSet<ClientRelationComp> ClientRelationComps { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<CodeType> CodeTypes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentComp> CommentComps { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Naming> Namings { get; set; }
        public virtual DbSet<OtherAsset> OtherAssets { get; set; }
        public virtual DbSet<PhysicalPerson> PhysicalPeople { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<PropertyAsset> PropertyAssets { get; set; }
        public virtual DbSet<Queue> Queues { get; set; }
        public virtual DbSet<Relation> Relations { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagComp> TagComps { get; set; }
        public virtual DbSet<UserClientComp> UserClientComps { get; set; }
        public virtual DbSet<VehicleAsset> VehicleAssets { get; set; }
        public virtual DbSet<Deal> Deal { get; set; }
        public virtual DbSet<Discussion> Discussion { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<VehicleInsurance> VehicleInsurance { get; set; }
        public virtual DbSet<VehicleInfoForInsurance> VehiclesForInsurance { get; set; }
        public virtual DbSet<CarAndEquipmentInfoForInsurance> CarNEquipForInsurance { get; set; }
        public virtual DbSet<ClassifierCounter> ClassifierCounter { get; set; }
        public virtual DbSet<DealPolicyLink> DealPolicyLink { get; set; }
        public virtual DbSet<OtherTypeOffer> OtherTypeOffer { get; set; }
        public virtual DbSet<PropertyInsurance> PropertyInsurance { get; set; }
        public virtual DbSet<PersonalAccident> PersonalAccident { get; set; }
        public virtual DbSet<LifeInsurance> LifeInsurance { get; set; }
        public virtual DbSet<VoluntaryHealthInsurance> VoluntaryHealthInsurance { get; set; }
        public virtual DbSet<VoluntaryHealthInsuranceEmployeeGroup> VoluntaryHealthInsurancePeople { get; set; }
        public virtual DbSet<CarAndEquipmentInsurance> CarAndEquipmentInsurance { get; set; }
        public virtual DbSet<LiabilityInsurance> LiabilityInsurance { get; set; }
        public virtual DbSet<CargoInsurance> CargoInsurance { get; set; }
        public virtual DbSet<PropertyLiability> PropertyLiabilityOffers { get; set; }
        public virtual DbSet<ProductLiabilityOffer> ProductLiabilityOffers { get; set; }
        public virtual DbSet<HiringLiability> HiringLiabilityOffers { get; set; }
        public virtual DbSet<ElectronicsInsuranceOffer> ElectronicsInsuranceOffers { get; set; }
        public virtual DbSet<BenchOffer> BenchOffers { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<TerrorLog> TerrorLog { get; set; }
        public virtual DbSet<IgnoredDoc> IgnoredDocs { get; set; }

        public virtual DbSet<BirthdayClientsView> BirthdayClientsView { get; set; }
        public virtual DbSet<Tender> Tenders { get; set; }

        public virtual DbSet<LeadObject> LeadObjects { get; set; }
        public virtual DbSet<PushNotificationModel> PushNotificationModels { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deal>()
             .Ignore(e => e.CurrentUserGuid);
            modelBuilder.Entity<Deal>()
           .Ignore(e => e.CurrentUserName);

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInfo)
                    .HasMaxLength(50)
                    .HasColumnName("additional_info");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountryQl).HasColumnName("country_ql");

                entity.Property(e => e.DistrictOrStreet)
                    .HasMaxLength(50)
                    .HasColumnName("district_or_street");

                entity.Property(e => e.DistrictOrStreetQl).HasColumnName("district_or_street_ql");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.RegOrCityId).HasColumnName("reg_or_city_id");

                entity.Property(e => e.RegOrCityQl).HasColumnName("reg_or_city_ql");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_ClientRef");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.CommentsAfterAppointent)
                   .HasMaxLength(500)
                   .HasColumnName("commentsAfterAppointent");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .HasColumnName("location");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.ULogonName).HasColumnName("u_logon_name");

                entity.HasOne(d => d.InnNavigation)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointment_ClientRef");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Assets", "Composition");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AssetDetailId).HasColumnName("asset_details_id");

                entity.Property(e => e.Inn).HasColumnName("inn");

                //entity.Property(e => e.ObjectIdentifier).HasColumnName("object_identifier");

                //entity.Property(e => e.ObjectIdentifierSourceType).HasColumnName("object_identifier_source_type");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.AssetInfo)
                    .HasMaxLength(50)
                    .HasColumnName("asset_info");
                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assets_ClientRef");


                entity.HasOne(d => d.AssetDetail)
                   .WithMany(p => p.Assets)
                   .HasForeignKey(x => x.AssetDetailId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Assets_AssetDetails");
            });

            modelBuilder.Entity<AssetDetails>(entity =>
            {
                entity.ToTable("Assets", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InsuredObjectGuid).HasColumnName("Insured_Object_Guid");

                //entity.Property(e => e.ObjectIdentifier).HasColumnName("object_identifier");

                //entity.Property(e => e.ObjectIdentifierSourceType).HasColumnName("object_identifier_source_type");

                entity.Property(e => e.AssetInfo)
                    .HasMaxLength(50)
                    .HasColumnName("Asset_Info");

                entity.Property(e => e.AssetType)
                .HasColumnName("Asset_Type");

                entity.Property(e => e.AssetDescription)
                .HasColumnName("Asset_Description")
                   .HasMaxLength(250);


            });


            modelBuilder.Entity<CallHistory>(entity =>
            {
                entity.ToTable("CallHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CallTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("call_timestamp")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ContactInfoId).HasColumnName("contact_info_id");

                entity.Property(e => e.Direction)
                    .HasMaxLength(15)
                    .HasColumnName("direction");

                entity.Property(e => e.ResponsibleNumner)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("responsible_numner");

                entity.HasOne(d => d.ContactInfo)
                    .WithMany(p => p.CallHistories)
                    .HasForeignKey(x => x.ContactInfoId)
                    .HasConstraintName("FK_CallHistory_ContactInfo");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientType).HasColumnName("client_type");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.IsHidden).HasColumnName("is_hidden");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_Client");
            });

            modelBuilder.Entity<ClientContactInfoComp>(entity =>
            {
                entity.ToTable("ClientContactInfoComp", "Composition");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContactInfoId).HasColumnName("contact_info_id");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.ReasonId).HasColumnName("reason_id");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ContactInfo)
                    .WithMany(p => p.ClientContactInfoComps)
                    .HasForeignKey(x => x.ContactInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactInfo_ContactInfo1");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.ClientContactInfoComps)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientContactInfoComp_ClientRef");
            });

            modelBuilder.Entity<ClientRef>(entity =>
            {
                entity.HasKey(x => x.Inn);

                entity.ToTable("ClientRef");

                entity.Property(e => e.Inn).HasColumnName("inn");
            });

            modelBuilder.Entity<ClientRelationComp>(entity =>
            {
                entity.ToTable("ClientRelationComp", "Composition");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client1).HasColumnName("client_1");

                entity.Property(e => e.Client2).HasColumnName("client_2");

                entity.Property(e => e.RelationId).HasColumnName("relation_id");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.ClientRelationCompClient1Navigations)
                    .HasForeignKey(x => x.Client1)
                    .HasConstraintName("FK_ClientRelationComp_ClientRef");

                entity.HasOne(d => d.Client2Navigation)
                    .WithMany(p => p.ClientRelationCompClient2Navigations)
                    .HasForeignKey(x => x.Client2)
                    .HasConstraintName("FK_ClientRelationComp_ClientRef1");
            });

            modelBuilder.Entity<Code>(entity =>
            {
                entity.ToTable("Code", "Classifier");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CodeOid).HasColumnName("code_oid");

                entity.Property(e => e.TypeOid).HasColumnName("type_oid");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .HasColumnName("value");

                entity.HasOne(d => d.CodeType)
                    .WithMany(p => p.Codes)
                    .HasForeignKey(x => x.TypeOid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Code_CodeType");


                entity.HasMany(d => d.Tags)
                    .WithOne(p => p.CodeNavigation)
                    .HasForeignKey(x => x.CodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_Code");
            });

            modelBuilder.Entity<CodeType>(entity =>
            {
                entity.ToTable("CodeType", "Classifier");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description).HasMaxLength(150);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("create_timestamp")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Creator)
                    .HasMaxLength(34)
                    .HasColumnName("creator");

                entity.Property(e => e.FullName)
                 .HasMaxLength(50)
                 .HasColumnName("FullName");

                entity.Property(e => e.Text)
                    .HasMaxLength(500)
                    .HasColumnName("text");
            });

            modelBuilder.Entity<CommentComp>(entity =>
            {
                entity.ToTable("CommentComp", "Composition");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentComps)
                    .HasForeignKey(x => x.CommentId)
                    .HasConstraintName("FK_CommentComp_Comment");

                entity.HasOne(d => d.ContactInfo)
                    .WithMany(p => p.CommentComps)
                    .HasForeignKey(x => x.ContactId)
                    .HasConstraintName("FK_CommentComp_ContactInfo");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.CommentComps)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentComp_ClientRef");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("company_name");

                entity.Property(e => e.CompanyNameQl).HasColumnName("company_name_ql");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_ClientRef");
            });

            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Value)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("value");
            });


            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DocumentExpireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("document_expire_date")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.DocumentNumber)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("document_number");

                entity.Property(e => e.DocumentType).HasColumnName("document_type");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Document_ClientRef");
            });


            modelBuilder.Entity<Naming>(entity =>
            {
                entity.ToTable("Naming");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DefaultValue)
                    .HasMaxLength(34)
                    .HasColumnName("default_value");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.ValueEn)
                    .HasMaxLength(34)
                    .IsUnicode(false)
                    .HasColumnName("value_en");

                entity.Property(e => e.ValueRu)
                    .HasMaxLength(34)
                    .HasColumnName("value_ru");
            });

            modelBuilder.Entity<OtherAsset>(entity =>
            {
                entity.ToTable("OtherAsset");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<PhysicalPerson>(entity =>
            {
                entity.ToTable("PhysicalPerson");

                entity.Property(e => e.Id).HasColumnName("id");


                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birth_date")
                    .HasAnnotation("Relational:ColumnType", "datetime")
                    .HasConversion(v => v, v => DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));


                entity.Property(e => e.BirthDateQl).HasColumnName("birth_date_ql");

                entity.Property(e => e.FatherName)
                    .HasMaxLength(34)
                    .HasColumnName("father_name");

                entity.Property(e => e.ImageBase64).HasColumnName("imageBase64");

                entity.Property(e => e.FatherNameQl).HasColumnName("father_name_ql");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(34)
                    .HasColumnName("first_name");

                entity.Property(e => e.FirstNameQl).HasColumnName("first_name_ql");

                entity.Property(e => e.FullName)
                    .HasMaxLength(108)
                    .HasColumnName("full_name");

                entity.Property(e => e.FullNameQl).HasColumnName("full_name_ql");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.LastName)
                    .HasMaxLength(34)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastNameQl).HasColumnName("last_name_ql");
                entity.Property(e => e.ImageBase64Ql).HasColumnName("image_base64_ql");

                entity.Property(e => e.MonthlyIncome)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("monthly_income")
                    .HasAnnotation("Relational:ColumnType", "decimal(12, 2)");

                entity.Property(e => e.MonthlyIncomeQl).HasColumnName("monthly_income_ql");

                entity.Property(e => e.Pin)
                    .HasMaxLength(10)
                    .HasColumnName("pin")
                    .IsFixedLength(true);

                entity.Property(e => e.PinQl).HasColumnName("pin_ql");


                entity.Property(e => e.PositionCustom)
                    .HasMaxLength(250)
                    .HasColumnName("position_custom");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.PositionQl).HasColumnName("position_ql");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.PhysicalPeople)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhysicalPerson_Position");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PhysicalPeople)
                    .HasForeignKey(x => x.PositionId)
                    .HasConstraintName("FK_PhysicalPerson_Position1");
            });

            modelBuilder.Entity<BirthdayClientsView>(entity =>
            {
                entity.ToView("BirthdayClientsView");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birth_date");

                entity.Property(e => e.BirthDateQl).HasColumnName("birth_date_ql");

                entity.Property(e => e.FatherName)
                    .HasMaxLength(34)
                    .HasColumnName("father_name");

                entity.Property(e => e.FatherNameQl).HasColumnName("father_name_ql");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(34)
                    .HasColumnName("first_name");

                entity.Property(e => e.FirstNameQl).HasColumnName("first_name_ql");

                entity.Property(e => e.FullName)
                    .HasMaxLength(108)
                    .HasColumnName("full_name");

                entity.Property(e => e.FullNameQl).HasColumnName("full_name_ql");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.DaysLeft).HasColumnName("days_left");

                entity.Property(e => e.LastName)
                    .HasMaxLength(34)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastNameQl).HasColumnName("last_name_ql");

                entity.Property(e => e.MonthlyIncome)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("monthly_income");

                entity.Property(e => e.MonthlyIncomeQl).HasColumnName("monthly_income_ql");

                entity.Property(e => e.Pin)
                    .HasMaxLength(10)
                    .HasColumnName("pin");

                entity.Property(e => e.PinQl).HasColumnName("pin_ql");

                entity.Property(e => e.PositionCustom)
                    .HasMaxLength(250)
                    .HasColumnName("position_custom");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.PositionQl).HasColumnName("position_ql");

                entity.Property(e => e.ULogonName)
                    .HasMaxLength(255)
                    .HasColumnName("u_logon_name");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Position", "Classifier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasMaxLength(25)
                    .HasColumnName("category")
                    .IsFixedLength(true);

                entity.Property(e => e.PositionName)
                    .HasMaxLength(250)
                    .HasColumnName("position_name")
                    .IsFixedLength(true);

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");
            });


            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region", "Classifier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.ValidTill)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_till")
                    .HasAnnotation("Relational:ColumnType", "datetime");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "Classifier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name")
                    .IsFixedLength(true);

                entity.Property(e => e.Seq).HasColumnName("seq");

                entity.Property(e => e.ValidTill)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_till")
                    .HasAnnotation("Relational:ColumnType", "datetime");
            });

            modelBuilder.Entity<PropertyAsset>(entity =>
            {
                entity.ToTable("PropertyAsset");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Queue>(entity =>
            {
                entity.ToTable("Queue");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("created_timestamp")
                    .HasDefaultValueSql("(getdate())")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.LastErrorText).HasColumnName("last_error_text");

                entity.Property(e => e.LastProcessedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("last_processed_on")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Payload)
                    .HasColumnType("xml")
                    .HasColumnName("payload")
                    .HasAnnotation("Relational:ColumnType", "xml");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.ProcessAfter)
                    .HasColumnType("datetime")
                    .HasColumnName("process_after")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.Recipient)
                    .HasMaxLength(256)
                    .HasColumnName("recipient");

                entity.Property(e => e.RelatedObjectId).HasColumnName("related_object_id");

                entity.Property(e => e.RetryCount).HasColumnName("retry_count");

                entity.Property(e => e.StatusOid).HasColumnName("status_oid");

                entity.Property(e => e.SubtypeOid).HasColumnName("subtype_oid");

                entity.Property(e => e.SystemOid).HasColumnName("system_oid");

                entity.Property(e => e.TypeOid).HasColumnName("type_oid");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.ToTable("Relation", "Classifier");

                entity.HasAnnotation("Relational:IsTableExcludedFromMigrations", false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Reverse)
                    .HasMaxLength(50)
                    .HasColumnName("reverse");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("text");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.Relations)
                    .HasForeignKey(x => x.TagId)
                    .HasConstraintName("FK_Relation_Tag");
            });


            modelBuilder.Entity<Source>(entity =>
            {
                entity.ToTable("Source", "Classifier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.ParamsJson).HasColumnName("params_json");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");
            });


            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag", "Classifier");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodeId)
                       .HasColumnName("code_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(e => e.CodeNavigation)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.CodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_Code");

            });


            modelBuilder.Entity<TagComp>(entity =>
            {
                entity.ToTable("TagComp", "Composition");

                entity.HasAnnotation("Relational:IsTableExcludedFromMigrations", false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagComps)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagComp_Tag");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.TagComps)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagComp_TagComp");
            });

            modelBuilder.Entity<UserClientComp>(entity =>
            {
                entity.HasKey(x => new { x.Id });

                entity.ToTable("UserClientComp", "Composition");

                entity.HasAnnotation("Relational:IsTableExcludedFromMigrations", false);

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.ULogonName).HasColumnName("u_logon_name");
                entity.Property(e => e.LobOid);
                entity.Property(e => e.ValidFrom).HasColumnName("Valid_From");
                entity.Property(e => e.ValidTo).HasColumnName("Valid_To");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.UserClientComps)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserClientComp_ClientRef");
            });

            modelBuilder.Entity<VehicleAsset>(entity =>
            {
                entity.ToTable("VehicleAsset");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Deal>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Deal__320B85397CEED946");

                entity.ToTable("Deal", "Deals");

                entity.HasIndex(e => e.DealNumber)
                    .HasName("UQ__Deal__DBB98F1C66219EA3")
                    .IsUnique();

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.SumInsured).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ClientPhoneNumber)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTimeStamp).HasColumnType("datetime");

                entity.Property(e => e.DealNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.GeneralNote).HasMaxLength(1000);
                entity.HasOne(d => d.ClientInnNavigation)
                    .WithMany(p => p.Deal)
                    .HasForeignKey(d => d.ClientInn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Deal__ClientInn__68336F3E");
            });

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.ToTable("Discussion", "Deals");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.Discussion)
                    .HasForeignKey(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Discussio__DealG__6EE06CCD");
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("Offer", "Offers");

                entity.HasIndex(e => e.OfferNumber)
                    .HasName("UQ__Offer__330D784D2D778CEC")
                    .IsUnique();

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.OfferNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Offer__DealGuid__72B0FDB1");
            });




            modelBuilder.Entity<CarAndEquipmentInfoForInsurance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("CarAndEquipmentInfoForInsurance", "Offers");

                entity.HasOne(d => d.CarAndEquipment)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(d => d.DealGuid)
                .HasConstraintName("CarNEquipment_CarNEquipmentInfo");


            });

            modelBuilder.Entity<CarAndEquipmentInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__CarAndEq__320B85398F55EE5D");

                entity.ToTable("CarAndEquipmentInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.AnyInfoForInsuranceCompany);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.LastFiveYearLossHistory);
                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.UnconditionalExemptionAmount);
                entity.Property(e => e.InsuranceValue).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.CarAndEquipmentInsurance)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__CarAndEqu__Benef__02B25B50");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.CarAndEquipmentInsurance)
                    .HasForeignKey<CarAndEquipmentInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CarAndEqu__DealG__01BE3717");
            });


            modelBuilder.Entity<VehicleInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__VehicleI__320B8539B9F977FC");

                entity.ToTable("VehicleInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceArea);



                entity.Property(e => e.LastFiveYearLossHistory).HasColumnName("LastFiveYearLossHistory").HasMaxLength(1000);
                entity.Property(e => e.CompanyActivityType).HasColumnName("CompanyActivityType").HasMaxLength(250);
                entity.Property(e => e.UnconditionalExemptionAmount).HasColumnName("UnconditionalExemptionAmount");

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.VehicleInsurance)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__VehicleIn__Benef__6C040022");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.VehicleInsurance)
                    .HasForeignKey<VehicleInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VehicleIn__DealG__6B0FDBE9");
            });

            modelBuilder.Entity<VehicleInfoForInsurance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("VehicleInfoForInsurance", "Offers");

                entity.HasOne(d => d.VehicleInfo)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(d => d.DealGuid)
                .HasConstraintName("VehicleIn_VehicleInfo");


            });

            modelBuilder.Entity<DealHistory>(entity =>
           {
               entity.HasKey(e => e.Id);

               entity.ToTable("DealHistory", "Offers");

               entity.Property(e => e.UserGuid)
                  .HasColumnName("user_guid");


               entity.Property(e => e.DealGuid)
                  .HasColumnName("deal_guid");


               entity.Property(e => e.ChangedValue)
                  .HasColumnName("changed_value");


               entity.Property(e => e.ChangeType)
                  .HasColumnName("change_type");

               entity.Property(e => e.RegisteredDate)
                  .HasColumnName("registered_date");
           });


            modelBuilder.Entity<ClassifierCounter>(entity =>
            {
                entity.HasKey(e => e.CounterOid)
                    .HasName("Counter_PK");

                entity.ToTable("ClassifierCounter", "Classifier");

                entity.Property(e => e.CounterOid)
                    .HasColumnName("counter_oid")
                    .ValueGeneratedNever();

                entity.Property(e => e.NextValue).HasColumnName("next_value");

                entity.Property(e => e.Prefix)
                    .HasColumnName("prefix")
                    .HasMaxLength(20);

                entity.Property(e => e.Usage)
                    .IsRequired()
                    .HasColumnName("usage")
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DealPolicyLink>(entity =>
            {
                entity.ToTable("DealPolicyLink", "Deals");

                entity.Property(e => e.LinkDate).HasColumnType("datetime");

                entity.Property(e => e.PolicyNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Deal)
                    .WithMany(p => p.DealPolicyLink)
                    .HasForeignKey(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DealPolic__DealG__7C3A67EB");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.DealPolicyLink)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DealPolic__Offer__7D2E8C24");
            });

            modelBuilder.Entity<OtherTypeOffer>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__OtherTyp__320B8539A335435A");

                entity.ToTable("OtherTypeOffer", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment);

                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.LastFiveYearsInjuryKnowledge);
                entity.Property(e => e.AnyInfo);

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.OtherTypeOffer)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__OtherType__Benef__07AC1A97");



                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.OtherTypeOffer)
                    .HasForeignKey<OtherTypeOffer>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OtherType__DealG__06B7F65E");
            });
            modelBuilder.Entity<PropertyLiability>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Property__320B8539A6FD213F");

                entity.ToTable("PropertyLiabilityOffer", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment);
                entity.Property(e => e.TempWorkersUsage);
                entity.Property(e => e.AnyInfo);
                entity.Property(e => e.ShowAcids);
                entity.Property(e => e.LossHistory);

                entity.Property(e => e.UnconditionalExemptionAmount);
                entity.Property(e => e.CurrentYearSales);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.ExplainDangerousProduct);
                entity.Property(e => e.AnnualTurnoverOfCompany).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.LimitPerAccident).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.AnnualGeneralLimit).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.IsAnyDirtAround);
                entity.Property(e => e.IsSituationGood);
                entity.Property(e => e.NumberOfEmployees);
                entity.Property(e => e.IsAnyProductDangerous);
                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.ActivityFeatures);
                entity.Property(e => e.LocalCompanyFoundDate).HasColumnType("datetime");



                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.PropertyLiabilityOffers)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__PropertyL__Benef__02133CD2");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.PropertyLiability)
                    .HasForeignKey<PropertyLiability>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PropertyL__DealG__0307610B");
            });

            modelBuilder.Entity<ProductLiabilityOffer>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__ProductL__320B85392FF1CCF0");

                entity.ToTable("ProductLiabilityOffer", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment);
                entity.Property(e => e.ExistingPolicyNumber);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.LocalCompanyFoundDate);
                entity.Property(e => e.ActivityFeatures);
                entity.Property(e => e.AllCompanyNames);
                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.IsSituationGood);
                entity.Property(e => e.ExplainProducts);
                entity.Property(e => e.QualityCheck);
                entity.Property(e => e.IsAnyProductDangerous);
                entity.Property(e => e.ExplainDangerousProduct);
                entity.Property(e => e.NumberOfEmployees);
                entity.Property(e => e.IsAnyDirtAround);
                entity.Property(e => e.LimitPerAccident);
                entity.Property(e => e.AnnualGeneralLimit);
                entity.Property(e => e.AnnualTurnoverOfCompany);
                entity.Property(e => e.CurrentYearSales);
                entity.Property(e => e.LossHistory);
                entity.Property(e => e.AnyInfo);
                entity.Property(e => e.ShowAcids);
                entity.Property(e => e.UnconditionalExemptionAmount);




                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.ProductLiabilityOffers)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__ProductLi__Benef__7889D298");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.ProductLiabilityOffer)
                    .HasForeignKey<ProductLiabilityOffer>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductLi__DealG__797DF6D1");
            });

            modelBuilder.Entity<HiringLiability>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__HiringLi__320B853992403336");

                entity.ToTable("HiringLiability", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExistingPolicyNumber);
                entity.Property(e => e.Comment);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.AnnualGeneralLimit);
                entity.Property(e => e.LimitPerAccident);
                entity.Property(e => e.ServantQuantity);
                entity.Property(e => e.LaborQuantity);
                entity.Property(e => e.ServantAnnualSalaryFond);
                entity.Property(e => e.LaborAnnualSalaryFond);
                entity.Property(e => e.SeaWorkerAnnualSalaryFond);
                entity.Property(e => e.SeaWorkerQuantity);
                entity.Property(e => e.DistanceFromSea);
                entity.Property(e => e.MaxTransportedEmployees);
                entity.Property(e => e.MaxEmployeesInSea);
                entity.Property(e => e.DaysPassedInSea);
                entity.Property(e => e.ShowAcids);

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.HiringLiabilityOffers)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__HiringLia__Benef__6C23FBB3");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.HiringLiability)
                    .HasForeignKey<HiringLiability>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HiringLia__DealG__6D181FEC");
            });

            modelBuilder.Entity<BenchOffer>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__BenchOff__320B85396544BACE");

                entity.ToTable("BenchOffer", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment);
                entity.Property(e => e.CompanyActivityType);


                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.InsurancePredmetAddress);
                entity.Property(e => e.InsuredBenchValue);
                entity.Property(e => e.ReplacementCost);
                entity.Property(e => e.InsuredBenchAmount);
                entity.Property(e => e.Unit);
                entity.Property(e => e.Injuries);
                entity.Property(e => e.Year).HasColumnName("ManufactureYear");
                entity.Property(e => e.ManufacturerName).HasColumnName("ManufacturerName");
                entity.Property(e => e.Series).HasColumnName("Series");
                entity.Property(e => e.Type).HasColumnName("Type");
                entity.Property(e => e.Voltage).HasColumnName("Voltage");
                entity.Property(e => e.Power).HasColumnName("Power");
                entity.Property(e => e.CustomsExpenses).HasColumnName("CustomsExpenses");
                entity.Property(e => e.PackagingExpenses).HasColumnName("PackagingExpenses");
                entity.Property(e => e.CurrentPrice).HasColumnName("CurrentPrice");
                entity.Property(e => e.AnyInjuryKnowledge).HasColumnName("AnyInjuryKnowledge");
                entity.Property(e => e.LastThreeYearsInjuryKnowledge).HasColumnName("LastThreeYearsInjuryKnowledge");
                entity.Property(e => e.ImmovablePropertyTypeOid).HasColumnName("ImmovablePropertyTypeOid");
                entity.Property(e => e.MovablePropertyTypeOid).HasColumnName("MovablePropertyTypeOid");
                entity.Property(e => e.InsuranceAreaType).HasColumnName("InsuranceAreaType");
                entity.Property(e => e.InsuranceArea).HasColumnName("InsuranceArea");
                entity.Property(e => e.ImmovablePropertyTypeOther).HasColumnName("ImmovablePropertyTypeOther");
                entity.Property(e => e.MovablePropertyTypeOther).HasColumnName("MovablePropertyTypeOther");
                entity.Property(e => e.SecurityAndFirefightingSystemInfo).HasColumnName("SecurityAndFirefightingSystemInfo");
                entity.Property(e => e.AnyUsefulInfoForInsuranceCompany).HasColumnName("AnyUsefulInfoForInsuranceCompany");
                entity.Property(e => e.SpecialConditions).HasColumnName("SpecialConditions");
                entity.Property(e => e.SpecialConditionsYes).HasColumnName("SpecialConditionsYes");
                entity.Property(e => e.ExistingPolicyNumber).HasColumnName("ExistingPolicyNumber");

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.BenchOffers)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__BenchOffe__Benef__629A9179");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.BenchOffer)
                    .HasForeignKey<BenchOffer>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BenchOffe__DealG__638EB5B2");
            });


            modelBuilder.Entity<ElectronicsInsuranceOffer>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Electron__320B853920070A89");

                entity.ToTable("ElectronicsInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment);
                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.InsurancePredmetAddress);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.InsuranceArea);
                entity.Property(e => e.SpecialConditions);
                entity.Property(e => e.LastFiveYearsInjuryKnowledge);
                entity.Property(e => e.SecurityAndFirefightingSystemInfo);
                entity.Property(e => e.SpecialConditionsYes);
                entity.Property(e => e.DeviceAddress);
                entity.Property(e => e.DeviceIsNew);
                entity.Property(e => e.TakeCare);
                entity.Property(e => e.Training);
                entity.Property(e => e.DangerousChemicals);

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.ElectronicsInsuranceOffers)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__Electroni__Benef__675F4696");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.ElectronicsInsuranceOffer)
                    .HasForeignKey<ElectronicsInsuranceOffer>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Electroni__DealG__68536ACF");
            });






            modelBuilder.Entity<PropertyInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Property__320B85397EEFD485");

                entity.ToTable("PropertyInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImmovablePropertyInsuranceAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.InsuranceArea);

                entity.Property(e => e.LostHistoryForTheLastFiveYears);

                entity.Property(e => e.MovablePropertyDescription);

                entity.Property(e => e.MovablePropertyInsuranceAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SecurityAndFirefightingSystemInfo);
                entity.Property(e => e.SpecialConditionsYes).HasColumnName("SpecialConditionsYes").HasMaxLength(250);
                entity.Property(e => e.SpecialConditions);



                entity.Property(e => e.ImmovablePropertyValue);
                entity.Property(e => e.MovablePropertyValue);
                // entity.Property(e => e.InsurancePredmet);
                entity.Property(e => e.InsurancePredmetAddress);
                entity.Property(e => e.InsuranceAreaType);

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.PropertyInsurance)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__PropertyI__Benef__16EE5E27");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.PropertyInsurance)
                    .HasForeignKey<PropertyInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PropertyI__DealG__15FA39EE");
            });

            modelBuilder.Entity<PersonalAccident>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Personal__320B853945F74622");

                entity.ToTable("PersonalAccident", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CompanyActivityType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.PersonalAccident)
                    .HasForeignKey<PersonalAccident>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PersonalA__DealG__253C7D7E");
            });

            modelBuilder.Entity<LifeInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__LifeInsu__320B8539E6AA4F43");

                entity.ToTable("LifeInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.LifeInsurance)
                    .HasForeignKey<LifeInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LifeInsur__DealG__290D0E62");
            });

            modelBuilder.Entity<VoluntaryHealthInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Voluntar__320B8539020A241F");

                entity.ToTable("VoluntaryHealthInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceArea).HasMaxLength(50);

                entity.Property(e => e.IsJobContainsHarmfulFactors).HasMaxLength(80);

                entity.Property(e => e.TypeOfActivity).HasMaxLength(50);

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.VoluntaryHealthInsurance)
                    .HasForeignKey<VoluntaryHealthInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Voluntary__DealG__642DD430");
            });

            modelBuilder.Entity<VoluntaryHealthInsuranceEmployeeGroup>(entity =>
            {
                entity.ToTable("VoluntaryHealthInsuranceEmployeeGroup", "Offers");

                entity.HasOne(d => d.VoluntaryHealthInsurance)
                    .WithMany(p => p.VoluntaryHealthInsuranceEmployeeGroup)
                    .HasForeignKey(d => d.VoluntaryHealthInsuranceGuid)
                    .HasConstraintName("FK__Voluntary__Volun__6DB73E6A");
            });


            modelBuilder.Entity<LiabilityInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Liabilit__320B8539B3499BBA");

                entity.ToTable("LiabilityInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.AnnualTurnoverOfCompany).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BodyInjuryAnnualAggregateLimit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BodyInjuryLimitPerAccident).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BodyInjuryLimitPerPerson).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceArea).HasMaxLength(50);

                entity.Property(e => e.PropertyDamageAnnualAggregateLimit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PropertyDamageLimitPerAccident).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.LiabilityInsurance)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__Liability__Benef__0682EC34");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.LiabilityInsurance)
                    .HasForeignKey<LiabilityInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Liability__DealG__058EC7FB");
            });

            modelBuilder.Entity<CargoInsurance>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__CargoIns__320B8539638820AC");

                entity.ToTable("CargoInsurance", "Offers");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DestinationPoint)
                    .HasMaxLength(50);

                entity.Property(e => e.EmailToNotify)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsuranceArea);

                entity.Property(e => e.InvoiceAmountOfCargo).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber);

                entity.Property(e => e.Packaging);
                entity.Property(e => e.PackagingOther);

                entity.Property(e => e.ProbableDeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.ProbableShippingDate).HasColumnType("datetime");

                entity.Property(e => e.ShippingCosts).HasColumnType("decimal(18, 0)");



                entity.Property(e => e.CompanyActivityType);
                entity.Property(e => e.CargoDescription);
                entity.Property(e => e.InsuranceAreaType);
                entity.Property(e => e.LostHistoryForTheLastFiveYears);
                entity.Property(e => e.CargoFeatures);
                entity.Property(e => e.AnyInfo);
                entity.Property(e => e.Route);

                entity.Property(e => e.StartPoint);

                entity.HasOne(d => d.BeneficiaryClientInnNavigation)
                    .WithMany(p => p.CargoInsurance)
                    .HasForeignKey(d => d.BeneficiaryClientInn)
                    .HasConstraintName("FK__CargoInsu__Benef__12E8C319");

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.CargoInsurance)
                    .HasForeignKey<CargoInsurance>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CargoInsu__DealG__11F49EE0");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.HasKey(e => e.DealGuid)
                    .HasName("PK__Survey__320B85392B645EF0");

                entity.ToTable("Survey", "Deals");

                entity.Property(e => e.DealGuid).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.SurveyLink)
                    .IsUnicode(false);

                entity.HasOne(d => d.Deal)
                    .WithOne(p => p.Survey)
                    .HasForeignKey<Survey>(d => d.DealGuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Survey__DealGuid__0CFADF99");
            });

            modelBuilder.Entity<Tender>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.ToTable("Tender", "Deals");

                entity.HasOne(d => d.Deal)
                .WithOne(p => p.Tender)
                .HasForeignKey<Tender>(d => d.DealGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Deal_Tender");

            });






            modelBuilder.Entity<TerrorLog>(entity =>
            {
                entity.ToTable("TerrorLog");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Inn).HasColumnName("inn").IsRequired(true);
                entity.Property(e => e.OperationType).HasColumnName("operation_type").HasMaxLength(255).IsUnicode(true);
                entity.Property(e => e.OperationDescription).HasColumnName("operation_description").HasMaxLength(255).IsUnicode(true);
                entity.Property(e => e.JsonResult).HasColumnName("json_result");
                entity.Property(e => e.RegisteredDateTime).HasColumnName("registered_datetime").HasColumnType("datetime");
            });

            modelBuilder.Entity<ClientHistory>(entity =>
            {
                entity.ToTable("ClientHistory", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Inn).HasColumnName("inn").IsRequired(true);
                entity.Property(e => e.OperationType).HasColumnName("operation_type").HasMaxLength(255).IsUnicode(true);
                entity.Property(e => e.OperationDescription).HasColumnName("operation_description").HasMaxLength(255).IsUnicode(true);
                entity.Property(e => e.JsonResult).HasColumnName("json_result");
                entity.Property(e => e.RegisteredDateTime).HasColumnName("registered_datetime").HasColumnType("datetime");
                entity.HasOne(d => d.ClientRef)
                 .WithMany(p => p.ClientHistories)
                 .HasForeignKey(x => x.Inn)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_ClientRef_ClientHistories");
            });


            modelBuilder.Entity<LeadObject>(entity =>
            {
                entity.ToTable("LeadObject", "dbo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DiscountPercentage)
                    .HasColumnName("discount_percentage").
                     HasColumnType("decimal(12, 2)")
                    .HasAnnotation("Relational:ColumnType", "decimal(12, 2)");

                entity.Property(e => e.CampaignName).HasColumnName("campaign_name");

                entity.Property(e => e.Payload).HasColumnName("payload");

                entity.Property(e => e.ObejctId).HasColumnName("object_id").HasMaxLength(50);

                entity.Property(e => e.UserGuid).HasColumnName("user_guid").HasColumnType("uniqueidentifier");

                entity.Property(e => e.UniqueKey).HasColumnName("unique_key").IsRequired();

                entity.Property(e => e.Inn).HasColumnName("inn");

                entity.Property(e => e.ValidFrom)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_from")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.Property(e => e.ValidTo)
                    .HasColumnType("datetime")
                    .HasColumnName("valid_to")
                    .HasAnnotation("Relational:ColumnType", "datetime");

                entity.HasOne(d => d.ClientRef)
                    .WithMany(p => p.LeadObjects)
                    .HasForeignKey(x => x.Inn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeadObject_ClientRef");
            });




            modelBuilder.Entity<PushNotificationModel>(entity =>
            {
                entity.ToTable("PushNotificationModel", "dbo");


                entity.Property(e => e.Id).HasColumnName("Id");
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Pin).HasColumnName("Pin").IsRequired();

                entity.Property(e => e.PlatformType).HasColumnName("Platform").IsRequired();

                entity.Property(e => e.DeviceId).HasColumnName("DeviceId").IsRequired();

                entity.Property(e => e.Token).HasColumnName("Token").IsRequired();
            });



            modelBuilder.Seed();
            OnModelCreatingPartial(modelBuilder);
        }



        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void CheckForInvalidSystemTagModifications()
        {
            var ValidateSystemTagModifications = ChangeTracker.Entries<Code>()
                  .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted)
                  .Any(e => e.GetDatabaseValues()?.GetValue<string>("Value") == ClassifiersCodeEnum.System.ToString());

            if (ValidateSystemTagModifications)
                throw new InvalidOperationException("Tags categorized as 'System' cannot be modified or deleted.");
        }
    }
}