using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using AutoMapper;
using AutoMapper.Extensions.EnumMapping;

using CRM.Data.Database;
using CRM.Data.Enums;
using CRM.Operation.Enums;
using CRM.Operation.Models;
using CRM.Operation.Models.DealOfferModels;
using CRM.Operation.Models.RequestModels;
using Zircon.Core.Extensions;

namespace AutoMapperOdata
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //ClientContract
            CreateMap<Code, CodeViewModel>().ReverseMap();
            CreateMap<ClientRef, ClientContract>().
                ForMember(dest => dest.INN,
            opt =>
            {
                opt.MapFrom(y => y.Inn);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.Documents,
            opt =>
            {
                opt.MapFrom(y => y.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.DocumentType!=13));
                //opt.ExplicitExpansion();
            })
            .ForMember(dest => dest.ContactsInfo,
            opt =>
            {
                opt.MapFrom(y => y.ClientContactInfoComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.AdditionalInfo,
            opt =>
            {
                opt.MapFrom(y => y.Addresses.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).AdditionalInfo);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.Relations,
            opt =>
            {
                opt.MapFrom(y => y.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.ClientType,
            opt =>
            {
                opt.MapFrom(y => y.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.Tags,
            opt =>
            {
                opt.MapFrom(y => y.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.CompanyName,
            opt =>
            {
                opt.MapFrom(y => y.Companies.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).CompanyName);

            }).
            ForMember(dest => dest.FirstName,
            opt =>
            {
                opt.MapFrom(y => y.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FirstName);

            })
            .
            ForMember(dest => dest.LastName,
            opt =>
            {
                opt.MapFrom(y => y.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).LastName);

            })
            .
            ForMember(dest => dest.FullName,
            opt =>
            {
                opt.MapFrom(y => y.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FullName);

            })
            .
            ForMember(dest => dest.PinNumber,
            opt =>
            {
                opt.MapFrom(y => y.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).Pin);

            })
            .
            ForMember(dest => dest.FatherName,
            opt =>
            {
                opt.MapFrom(y => y.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FatherName);

            })
            .
            ForMember(dest => dest.Assets,
            opt =>
            {
                opt.MapFrom(y => y.Assets);

            })
            .
            ForMember(dest => dest.LeadObjects,
            opt =>
            {
                opt.MapFrom(y => y.LeadObjects);

            })
           .
            ForMember(dest => dest.TinNumber,
            opt =>
            {
                opt.MapFrom(y => y.Documents.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo > DateTime.Now
                  && x.DocumentType == (int)DocumentTypeEnum.Tin).DocumentNumber);
            })
             .
            ForMember(dest => dest.IdNumber,
            opt =>
            {
                opt.MapFrom(y => y.Documents.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo > DateTime.Now
                  && x.DocumentType == (int)DocumentTypeEnum.IdCard).DocumentNumber);
            })
            ;


            CreateMap<ClientRelationComp, ClientRelationModel>().
                 //IncludeBase<ClientRef, ClientContract>().
                 ForMember(desc => desc.RelationTypeId,
                 opt =>
                 {
                     opt.MapFrom(y => y.RelationId);
                 })
                 .
                 ForMember(dest => dest.FirstName,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2Navigation.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FirstName);

                 })
                 .
                 ForMember(dest => dest.LastName,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2Navigation.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).LastName);

                 })
                 .
                 ForMember(dest => dest.FatherName,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2Navigation.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FatherName);

                 })
                  .
                 ForMember(dest => dest.FullName,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2Navigation.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).FullName);

                 })
                 .
                 ForMember(dest => dest.CompanyName,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2Navigation.Companies.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).CompanyName);

                 })
                 .
                 ForMember(dest => dest.RelationalINN,
                 opt =>
                 {
                     opt.MapFrom(y => y.Client2);
                 })
                 .
                 ForMember(dest => dest.PinNumber,
                 opt =>
                 {
                     //here I used Client_1 (ClientRef) for going to PhysicalPeople. Because here pin is used for searching by pin for affliate person
                     //api/clientrelation?$select=relationTypeId&$filter=pinnumber eq '2egnmx3'
                     opt.MapFrom(y => y.ClientRef.PhysicalPeople.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).Pin);
                 })
                 ;



            CreateMap<PhysicalPerson, ClientContract>().
            // IncludeBase<ClientRef, ClientContract>().
            ForMember(dest => dest.ClientType,
            opt =>
            {
                opt.MapFrom(y => ClientType.Pyhsical); //opt.MapFrom(y => y.ClientRef.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.FirstName,
            opt =>
            {
                opt.MapFrom(y => y.FirstName);

            }).
            ForMember(dest => dest.LastName,
            opt =>
            {
                opt.MapFrom(y => y.LastName);

            })
            .
            ForMember(dest => dest.FullName,
            opt =>
            {
                opt.MapFrom(y => y.FullName);

            }).
            ForMember(dest => dest.FatherName,
            opt =>
            {
                opt.MapFrom(y => y.FatherName);

            });


            //Contact
            //Contact
            CreateMap<PhysicalPerson, ContactListItemModel>().
            // IncludeBase<ClientRef, ClientContract>().
            ForMember(dest => dest.ClientType,
            opt =>
            {
                opt.MapFrom(y => ClientType.Pyhsical); //opt.MapFrom(y => y.ClientRef.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.PositionName,
            opt =>
            {
                opt.MapFrom(y => y.
                Position.PositionName);
            }).
            ForMember(dest => dest.FullName,
            opt =>
            {
                opt.MapFrom(y => y.FullName);

            }).
            ForMember(dest => dest.FirstName,
            opt =>
            {
                opt.MapFrom(y => y.FirstName);

            }).
             ForMember(dest => dest.FullName,
            opt =>
            {
                opt.MapFrom(y => y.FullName);

            }).
            ForMember(dest => dest.LastName,
            opt =>
            {
                opt.MapFrom(y => y.LastName);

            }).
            ForMember(dest => dest.FatherName,
            opt =>
            {
                opt.MapFrom(y => y.FatherName);

            }).
            ForMember(dest => dest.Position,
            opt =>
            {
                opt.MapFrom(y => y.PositionId);

            }).
            ForMember(dest => dest.MonthlyIncome,
            opt =>
            {
                opt.MapFrom(y => y.MonthlyIncome);

            }).
            ForMember(dest => dest.BirthDate,
            opt =>
            {
                opt.MapFrom(y => y.BirthDate);
            })

            .
            ForMember(dest => dest.PinNumber,
            opt =>
            {
                opt.MapFrom(y => y.Pin);

            })
            .ForMember(dest => dest.GenderType,
            opt =>
            {
                opt.MapFrom(y => y.Gender.HasValue ?
               ((GenderTypeEnum)y.Gender.Value).ToString().ToLower() : null);
            })
            .ForMember(dest => dest.Documents,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
            })
            .ForMember(dest=>dest.TinNumber,
            opt =>
            {
                opt.MapFrom(src => src.ClientRef.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now && x.DocumentType == (int)DocumentTypeEnum.Tin).FirstOrDefault().DocumentNumber);
            })
            .ForMember(dest => dest.ContactsInfo,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.ClientContactInfoComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
            })
            .ForMember(dest => dest.ClientComments,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.CommentComps.Where(x => x.ContactInfo == null).Select(x => x.Comment));
            })
            .
            ForMember(dest => dest.Tags,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            }).
             ForMember(dest => dest.Relations,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            })
            .ForMember(dest => dest.Assets,
             opt =>
             {
                 opt.MapFrom(y => y.ClientRef.Assets);
                 //opt.ExplicitExpansion();
             })
            .ForMember(dest => dest.LeadObjects,
             opt =>
             {
                 opt.MapFrom(y => y.ClientRef.LeadObjects.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
             })
            ;

            //Contact
            //Contact
            CreateMap<BirthdayClientsView, ContactListItemModel>().
                // IncludeBase<ClientRef, ClientContract>().
                ForMember(dest => dest.ClientType,
                    opt =>
                    {
                        opt.MapFrom(y => ClientType.Pyhsical); //opt.MapFrom(y => y.ClientRef.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                        //opt.ExplicitExpansion();
                    }).ForMember(dest => dest.FullName,
                    opt =>
                    {
                        opt.MapFrom(y => y.FullName);

                    }).ForMember(dest => dest.FirstName,
                    opt =>
                    {
                        opt.MapFrom(y => y.FirstName);

                    }).ForMember(dest => dest.LastName,
                    opt =>
                    {
                        opt.MapFrom(y => y.LastName);

                    }).ForMember(dest => dest.FatherName,
                    opt =>
                    {
                        opt.MapFrom(y => y.FatherName);

                    }).ForMember(dest => dest.Position,
                    opt =>
                    {
                        opt.MapFrom(y => y.PositionId);

                    }).ForMember(dest => dest.MonthlyIncome,
                    opt =>
                    {
                        opt.MapFrom(y => y.MonthlyIncome);

                    }).ForMember(dest => dest.BirthDate,
                    opt =>
                    {
                        opt.MapFrom(y => y.BirthDate);
                    })

                .ForMember(dest => dest.PinNumber,
                    opt =>
                    {
                        opt.MapFrom(y => y.Pin);

                    })
                .ForMember(dest => dest.GenderType,
                    opt =>
                    {
                        opt.MapFrom(y => y.Gender.HasValue ? ((GenderTypeEnum)y.Gender.Value).ToString().ToLower() : null);
                    });


            CreateMap<Company, ClientContract>().
            //IncludeBase<ClientRef, ClientContract>().
            ForMember(dest => dest.ClientType,
            opt =>
            {
                opt.MapFrom(y => ClientType.Company); //opt.MapFrom(y => y.ClientRef.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.CompanyName,
            opt =>
            {
                opt.MapFrom(y => y.CompanyName);
            });


            //Company
            CreateMap<Company, CompanyListItemModel>().
            //IncludeBase<ClientRef, ClientContract>().
            ForMember(dest => dest.ClientType,
            opt =>
           {
                opt.MapFrom(y => ClientType.Company); //opt.MapFrom(y => y.ClientRef.Clients.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).ClientType);
                //opt.ExplicitExpansion();
            }).
            ForMember(dest => dest.TinNumber,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.Documents.FirstOrDefault(x => x.ValidFrom <= DateTime.Now && x.ValidTo > DateTime.Now
                  && x.DocumentType == (int)DocumentTypeEnum.Tin).DocumentNumber);
            }).
            ForMember(dest => dest.CompanyName,
            opt =>
            {
                opt.MapFrom(y => y.CompanyName);

            })
            .ForMember(dest => dest.Documents,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.Documents.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
            })
            .ForMember(dest => dest.ContactsInfo,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.ClientContactInfoComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
            })
            .ForMember(dest => dest.ClientComments,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.CommentComps.Where(x => x.ContactInfo == null).Select(x => x.Comment));
            })
            .
            ForMember(dest => dest.Tags,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.TagComps.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            })
            .
            ForMember(dest => dest.Relations,
            opt =>
            {
                opt.MapFrom(y => y.ClientRef.ClientRelationCompClient1Navigations.Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now));
                //opt.ExplicitExpansion();
            })
            ;

            CreateMap<Document, DocumentContract>();

            CreateMap<ContactInfo, ContactInfoContract>().
            ForMember(dest => dest.ContactComments,
            opt =>
            {
                opt.MapFrom(y => y.CommentComps.Select(x => x.Comment));
            });


            CreateMap<LeadObject, LeadObjectModel>().ReverseMap();

            CreateMap<ClientContactInfoComp, ContactInfoContract>().
                ForMember(dest => dest.Type,
                opt =>
                {
                    opt.MapFrom(y => y.ContactInfo.Type);
                }).ForMember(dest => dest.Value,
                opt =>
                {
                    opt.MapFrom(y => y.ContactInfo.Value);
                }).
                ForMember(dest => dest.ContactComments,

                opt =>
                {
                    opt.MapFrom(y => y.ContactInfo.CommentComps.Select(x => x.Comment));
                }).
                ForMember(dest => dest.Calls,

                opt =>
                {
                    opt.MapFrom(y => y.ContactInfo.CallHistories);
                })
                .ForMember(dest=>dest.Reason,
                opt=> 
                {
                    opt.MapFrom(x => x.ReasonId);
                });

            CreateMap<ClientRelationComp, RelationContract>().
                ForMember(dest => dest.ClientINN,
                opt =>
                {
                    opt.MapFrom(x => x.Client2);
                }).
                ForMember(dest => dest.RelationType,
                opt =>
                {
                    opt.MapFrom(x => x.RelationId);
                }).
                ForMember(dest => dest.Code,
                opt =>
                {
                    opt.MapFrom(x => x.Client2Navigation.Documents.FirstOrDefault(x=>x.DocumentType == (int)DocumentTypeEnum.Tin && x.ValidFrom <= DateTime.Now && x.ValidTo >= DateTime.Now).DocumentNumber);
                })
                .
                ForMember(dest => dest.ClientName,
                opt =>
                {
                    opt.MapFrom(x => x.Client2Navigation.Companies.FirstOrDefault(y => y.ValidFrom <= DateTime.Now && y.ValidTo >= DateTime.Now).CompanyName);
                });

            CreateMap<Comment, CommentContract>();

            CreateMap<CommentComp, CommentContract>().
                ForMember(dest => dest.Text,
                opt =>
                {
                    opt.MapFrom(y => y.Comment.Text);
                }).
                ForMember(dest => dest.Creator,
                opt =>
                {
                    opt.MapFrom(y => y.Comment.Creator);
                })
                .
                ForMember(dest => dest.FullName,
                opt =>
                {
                    opt.MapFrom(y => y.Comment.FullName);
                })
                 .
                ForMember(dest => dest.CreateTimeStamp,
                opt =>
                {
                    opt.MapFrom(y => y.Comment.CreateTimestamp);
                });

            CreateMap<TagComp, TagContract>().
                ForMember(dest => dest.Name,
                opt =>
                {
                    opt.MapFrom(y => y.Tag.Name);
                }).
                ForMember(dest => dest.Id,
                opt =>
                {
                    opt.MapFrom(y => y.Tag.Id);
                });
            CreateMap<AssetContract,Asset>().ReverseMap();


            CreateMap<AssetViewModel, AssetViewModel>().
             ForMember(dest => dest.AssetType,
             opt =>
             {
                 opt.MapFrom(y => y.AssetType);
             }).
             ForMember(dest => dest.AssetDescription,
             opt =>
             {
                 opt.MapFrom(y => y.AssetDescription);
             })
             .
             ForMember(dest => dest.AssetInfo,
             opt =>
             {
                 opt.MapFrom(y => y.AssetInfo);
             })
             ;

            CreateMap<Address, AddressContract>().
               ForMember(dest => dest.CountryId,
               opt =>
               {
                   opt.MapFrom(y => y.CountryId);
               }).
               ForMember(dest => dest.RegOrCityId,
               opt =>
               {
                   opt.MapFrom(y => y.RegOrCityId);
               }).
               ForMember(dest => dest.AdditionalInfo,
               opt =>
               {
                   opt.MapFrom(y => y.AdditionalInfo);
               }).
               ForMember(dest => dest.DistrictOrStreet,
               opt =>
               {
                   opt.MapFrom(y => y.DistrictOrStreet);
               });



            CreateMap<CallHistory, CallContract>().
               ForMember(dest => dest.CallTimestamp,
               opt =>
               {
                   opt.MapFrom(y => y.CallTimestamp);
               }).
               ForMember(dest => dest.ResponsibleNumber,
               opt =>
               {
                   opt.MapFrom(y => y.ResponsibleNumner);
               }).
               ForMember(dest => dest.Direction,
               opt =>
               {
                   opt.MapFrom(y => y.Direction);
               });

            //DealModel
            CreateMap<DealModel, Deal>()
                .ForMember(dest => dest.ClientInn, opt => opt.MapFrom(x => x.Client.INN))
                .ForMember(dest => dest.CreatedByUserGuid, opt => opt.MapFrom(x => x.CreatedByUser.UserGuid))
                .ForMember(dest => dest.CreatedByUserFullName, opt => opt.MapFrom(x => x.CreatedByUser.FullName))
                .ForMember(dest => dest.UnderwriterUserGuid, opt => opt.MapFrom(x=>x.UnderwriterUser == null ? (Guid?)null : x.UnderwriterUser.UserGuid))
                .ForMember(dest => dest.UnderwriterUserFullName, opt => opt.MapFrom(x=>x.UnderwriterUser == null ? null : x.UnderwriterUser.FullName))
                .ForMember(dest => dest.DealGuid, opt => opt.MapFrom(x => x.DealGuid))
                .ForMember(dest => dest.DealStatus, opt => opt.MapFrom(x => (int?) x.DealStatus))
                .ForMember(dest => dest.DealType, opt => opt.MapFrom(x => x.SelectedOfferType))
                .ForMember(dest => dest.ResponsiblePersonType, opt => opt.MapFrom(x => (int)x.ResponsiblePersonType))
                .ForMember(dest => dest.GeneralNote, opt => opt.MapFrom(x => x.GeneralNote))
                .ForMember(dest => dest.GeneralNote, opt => opt.MapFrom(x => x.GeneralNote))
                .ForMember(dest => dest.DealLanguageOid, opt => opt.MapFrom(x => x.OfferLanguageOid));

            CreateMap<Deal, DealModel>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(x => x.ClientInnNavigation))
                .ForMember(dest => dest.DealGuid, opt => opt.MapFrom(x => x.DealGuid))
                .ForMember(dest => dest.DealStatus, opt => opt.MapFrom(x => (DealStatus) x.DealStatus))
                .ForMember(dest => dest.OfferLanguageOid, opt => opt.MapFrom(x => x.DealLanguageOid))
                .ForMember(dest => dest.SelectedOfferType, opt => opt.MapFrom(x => x.DealType))
                .ForMember(dest => dest.ResponsiblePersonType, opt => opt.MapFrom(x => (int)x.ResponsiblePersonType))
                .ForMember(dest => dest.Offers, opt => opt.MapFrom(x => x.Offer))
                .ForMember(dest => dest.GeneralNote, opt => opt.MapFrom(x => x.GeneralNote))
                .ForMember(dest => dest.DealPolicyLinks, opt => opt.MapFrom(x => x.DealPolicyLink))
                .ForMember(dest => dest.FormComponentModel, opt =>opt.Ignore());


            //CreateMap<Deal, DealListItem>()
            //      .ForMember(destination => destination.DealStatus,
            //                 opt => opt.MapFrom(source => Enum.GetName(typeof(DealStatus), source.DealStatus)));

            //CreateMap<DealListItem, Deal>()
            //     .ForMember(destination => destination.DealStatus,
            //                opt => opt.MapFrom(source => Enum.GetName(typeof(DealStatus), source.DealStatus)));

            CreateMap<Deal, DealListItem>()
               .ForMember(dest => dest.DealStatus, opt => opt.MapFrom(x => (DealStatus)x.DealStatus))
               .ForMember(dest => dest.DealType, opt => opt.MapFrom(x => (OfferTypeEnum)x.DealType))
               .ReverseMap();

            CreateMap<VehicleInsuranceOfferModel, VehicleInsurance>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<VehicleInsurance,VehicleInsuranceOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));

            CreateMap<InsuredVehicleModel, VehicleInfoForInsurance>().ReverseMap();
            CreateMap<Tender, TenderModel>().ReverseMap();
            CreateMap<OtherTypeOfferModel,OtherTypeOffer>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<OtherTypeOffer,OtherTypeOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));

            CreateMap<PropertyInsuranceOfferModel,PropertyInsurance>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<PropertyInsurance,PropertyInsuranceOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));

            CreateMap<PersonalAccidentOfferModel, PersonalAccident>().ReverseMap();

            CreateMap<LifeInsuranceOfferModel, LifeInsurance>().ReverseMap();

            CreateMap<VoluntaryHealthInsuranceOfferModel,VoluntaryHealthInsurance>()
                .ForMember(dest=>dest.VoluntaryHealthInsuranceEmployeeGroup, opt=>opt.MapFrom(y=>y.VoluntaryHealthInsuranceEmployeeGroups));

            CreateMap<VoluntaryHealthInsurance,VoluntaryHealthInsuranceOfferModel>()
                .ForMember(dest=>dest.VoluntaryHealthInsuranceEmployeeGroups, opt=>opt.MapFrom(y=>y.VoluntaryHealthInsuranceEmployeeGroup));

            CreateMap<VoluntaryHealthInsuranceEmployeeGroupViewModel, VoluntaryHealthInsuranceEmployeeGroup>()
                .ReverseMap();

            CreateMap<CarAndEquipmentInsuranceOfferModel,CarAndEquipmentInsurance>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<CarAndEquipmentInsurance,CarAndEquipmentInsuranceOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));
            CreateMap<InsuredVehicleModel, CarAndEquipmentInsurance>().ReverseMap();
            CreateMap<InsuredVehicleModel, CarAndEquipmentInfoForInsurance>()
                .ReverseMap();


            CreateMap<LiabilityInsuranceOfferModel,LiabilityInsurance>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<LiabilityInsurance,LiabilityInsuranceOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));

            CreateMap<CargoInsuranceOfferModel,CargoInsurance>()
                .ForMember(dest=>dest.BeneficiaryClientInn, opt=>opt.MapFrom(x=>x.Beneficiary.INN));

            CreateMap<CargoInsurance,CargoInsuranceOfferModel>()
                .ForMember(dest=>dest.Beneficiary, opt=>opt.MapFrom(y=>y.BeneficiaryClientInnNavigation));



            CreateMap<HiringLiabilityInsuranceOfferModel,HiringLiability>()
               .ForMember(dest => dest.BeneficiaryClientInn, opt => opt.MapFrom(x => x.Beneficiary.INN));

            CreateMap<HiringLiability, HiringLiabilityInsuranceOfferModel>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(y => y.BeneficiaryClientInnNavigation));

            CreateMap<PropertyLiabilityInsuranceOfferModel, PropertyLiability>()
               .ForMember(dest => dest.BeneficiaryClientInn, opt => opt.MapFrom(x => x.Beneficiary.INN));

            CreateMap<PropertyLiability, PropertyLiabilityInsuranceOfferModel>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(y => y.BeneficiaryClientInnNavigation));

            CreateMap<ProductLiabilityInsuranceOfferModel, ProductLiabilityOffer>()
               .ForMember(dest => dest.BeneficiaryClientInn, opt => opt.MapFrom(x => x.Beneficiary.INN));

            CreateMap<ProductLiabilityOffer, ProductLiabilityInsuranceOfferModel>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(y => y.BeneficiaryClientInnNavigation));

            CreateMap<ElectronicsInsuranceOfferModel, ElectronicsInsuranceOffer>()
               .ForMember(dest => dest.BeneficiaryClientInn, opt => opt.MapFrom(x => x.Beneficiary.INN));

            CreateMap<ElectronicsInsuranceOffer, ElectronicsInsuranceOfferModel>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(y => y.BeneficiaryClientInnNavigation));

            CreateMap<BenchInsuranceOfferModel, BenchOffer>()
               .ForMember(dest => dest.BeneficiaryClientInn, opt => opt.MapFrom(x => x.Beneficiary.INN));

            CreateMap<BenchOffer, BenchInsuranceOfferModel>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(y => y.BeneficiaryClientInnNavigation));








            CreateMap<DiscussionViewModel, Discussion>()
                .ForMember(dest => dest.SenderGuid, opt => opt.MapFrom(x => x.Sender.UserGuid))
                .ForMember(dest => dest.ReceiverGuid, opt => opt.MapFrom(x => x.Receiver.UserGuid))
                .ReverseMap();

            CreateMap<OfferViewModel, Offer>().ReverseMap();

            CreateMap<DealPolicyLink, DealPolicyLinkViewModel>().ReverseMap();

            CreateMap<Survey, SurveyModel>().ReverseMap();
            CreateMap<Survey, UpdateSurveyRequestModel>().ReverseMap();
            CreateMap<ClientRef, ClientRefModel>()
                .ForMember(opt=>opt.Companies, x=>x.MapFrom(y=>y.Companies.Where(y => y.ValidFrom <= DateTime.Now && y.ValidTo >= DateTime.Now)))
                .ForMember(opt=>opt.PhysicalPeople, x=>x.MapFrom(y=>y.PhysicalPeople.Where(y => y.ValidFrom <= DateTime.Now && y.ValidTo >= DateTime.Now)))
                .ReverseMap();
            CreateMap<Company, DealListClientContract>()
                .ForMember(opt => opt.CompanyName, x => x.MapFrom(y => y.CompanyName));
            CreateMap<PhysicalPerson, DealListClientContract>().ReverseMap();
        }
    }
}
