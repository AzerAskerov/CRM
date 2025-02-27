using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Login;
using CRM.Operation.Models;
using CRM.Operation.Enums;
using CRM.Data.Enums;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class CompanyCardView
    {
        [CascadingParameter]
        public CRM.Client.Shared.ComponentInstance BaseCompo { get; set; }

        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract CompanyContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }
        private ClaimsPrincipal User;


        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }
        public List<CuratorProductModel> CuratorProducts { get; set; } = new List<CuratorProductModel>();
        private List<ProductBaseModel> InforceProductList = new List<ProductBaseModel>();
        private List<ProductBaseModel> InOtherStatusProductList = new List<ProductBaseModel>();

        public CompanyListItemModel CompanyListItemModel { get; set; }
        public bool IsEdit = false;
        public bool ClientInfoOpenState;
        public bool ContactInfoOpenState;
        public bool CallsOpenState;
        public bool AddressOpenState;
        public bool DocumentOpenState;
        public bool RelationOpenState;
        public bool CommentInfoOpenState;
        public bool CuratorProductOpenState;
        public int SelectedCountryID { get; set; }
        List<CommentContract> OldCompanyComments = new List<CommentContract>();
        public List<CommentContract> CompanyComments { get; set; } = new List<CommentContract>();
        protected async Task SubmitClientAsync(ClientContract model)
        {
            ClientInfoContract contract = new ClientInfoContract();
            contract.ResponsiblePerson = (await AuthenticationStateTask).User.FindFirst(x=>x.Type == ClaimTypes.GivenName).Value;
            contract.Source = 3; //SourceType.CRM;
            model.ClientComments.ForEach(x => x.Creator = contract.ResponsiblePerson);
            //model.Relations.ForEach(x => x.DocumentType = 3); // Document.INN


            model.ContactsInfo.ForEach(x => x.ContactComments.ForEach(x =>
            {
                x.Creator = contract.ResponsiblePerson;
                x.FullName = User.Identity.Name;
            }));
            CompanyComments.ForEach(
                x => {
                    x.Creator = contract.ResponsiblePerson;
                    x.FullName = User.Identity.Name;
                });
            model.ClientComments = CompanyComments;

            contract.ResponsiblePerson = null;
            contract.Clients.Add(model);

            var result = await State.CreateClient(contract);
            SetState(result);
            await BaseCompo.Close();
            StateHasChanged();
        }


        public async Task CommentInfoExpand()
        {
            if (!CommentInfoOpenState && !CallsOpenState)
            {
                var data = await State.GetClientInfo<List<CommentContract>>($"api/comment({CompanyContractModel.INN})?$select=text,fullname,create_timestamp");
                Console.WriteLine(data);
                CompanyListItemModel.ClientComments = data;


            }
        }

        public void AddCommentItem()
        {
            CompanyContractModel.ClientComments.Add(new CommentContract()
            {
                FullName = User.Identity.Name,
                Creator = User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value
            });
        }

        public void AddClientCommentItem()
        {
            CompanyComments.Add(new CommentContract());
        }

        public void RemoveCommentItem(CommentContract comment)
        {
            CompanyComments.Remove(comment);
        }
        public void AddDocumentItem()
        {
            CompanyListItemModel.Documents.Add(new DocumentContract());
        }

        public void AddRelationItem()
        {
            ((List<RelationContract>)CompanyListItemModel.Relations).Add(new CRMRelationContract() { DocumentType = (int)DocumentTypeEnum.INN /*INN*/});
        }

        public void AddCompanyCommentItem()
        {
            CompanyListItemModel.ClientComments.Add(new CommentContract());
        }

        public void OnAddOption(Zircon.Core.ISourceItem item)
        {
            if (!CompanyListItemModel.Tags.Any(x => x.Id == Convert.ToInt32(item.Key.ToString())))
            {
                CompanyListItemModel.Tags.Add(new TagContract() { Id = Convert.ToInt32(item.Key.ToString()), Name = item.Value });
            }
        }
        public async void CuratorProductsExpand()
        {
            if (!CuratorProductOpenState)
            {
                if (!ClientContractModel.INN.HasValue) return;

                var result = await State.GetCuratorPolicies(new CuratorProductRequest { Inn = ClientContractModel.INN, LogonName = (await AuthenticationStateTask).User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value });

                if (result.Success)
                {
                    CuratorProducts = result.Model/*.OrderByDescending(x => x.CreatedDate).ToList()*/;

                    StateHasChanged();
                }
            }
        }

        public async void ClientInfoExpand()
        {
            if (!ClientInfoOpenState)
            {
                CompanyListItemModel = (CompanyListItemModel)CompanyContractModel;
                CompanyListItemModel = await State.GetClientInfo<CompanyListItemModel>($"api/company({CompanyContractModel.INN})?$select=inn,companyname&$expand=tags($select=name,id),clientcomments");
                CompanyListItemModel.ClientType = ClientType.Company;
                ClientInfoOpenState = true;
                StateHasChanged();
            }
        }
        public async void ContactInfoExpand()
        {
            if (!ContactInfoOpenState)
            {
                CompanyListItemModel.ContactsInfo = await State.GetClientInfo<List<ContactInfoContract>>($"api/contact({CompanyContractModel.INN})?$select=value,type&$expand=contactcomments($select=text,creator)");
                CompanyListItemModel.ContactsInfo = CompanyListItemModel.ContactsInfo.Where(x => x.Type != (int)ContactTypeEnum.MobileToken).ToList();

                ContactInfoOpenState = true;
                StateHasChanged();
            }
        }
        public async void CallsExpand()
        {
            if (!CallsOpenState)
            {
                CompanyListItemModel.ContactsInfo = await State.GetClientInfo<List<ContactInfoContract>>($"api/contact({CompanyContractModel.INN})?$select=value,type&$expand=calls($select=responsibleperson,direction,calltimestamp)");
                CallsOpenState = true;
                StateHasChanged();
            }
        }
        public async void AddressExpand()
        {
            if (!AddressOpenState)
            {
                CompanyListItemModel.Address = await State.GetClientInfo<AddressContract>($"api/address({CompanyContractModel.INN})?$select=countryId,regOrCityId,districtOrStreet,additionalInfo");
                AddressOpenState = true;
                StateHasChanged();
            }
        }
        public async void DocumentExpand()
        {
            if (!DocumentOpenState)
            {
                CompanyListItemModel.Documents = await State.GetClientInfo<List<DocumentContract>>($"api/document({CompanyContractModel.INN})?$select=documentnumber,documenttype,documentexpiredate");
                DocumentOpenState = true;
                StateHasChanged();
            }
        }
        public async void RelationExpand()
        {
            if (!RelationOpenState)
            {
                var releatedClients = await State.GetClientInfo<IEnumerable<ClientRelationModel>>($"api/clientrelation({CompanyContractModel.INN})?$select=RelationTypeId,firstname, lastname,fathername,companyname,relationalINN");

                CompanyContractModel.Relations = new List<RelationContract>();
                if (releatedClients != null)
                {
                    foreach (var client in releatedClients)
                    {
                        ((List<RelationContract>)CompanyContractModel.Relations).Add(new RelationContract() { ClientINN = client.RelationalINN, ClientName = client.CompanyName ?? client.FirstName, RelationType = client.RelationTypeId });
                    }
                }
                RelationOpenState = true;
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            User = (await AuthenticationStateTask).User;
            CompanyListItemModel = (CompanyListItemModel)CompanyContractModel;
            CompanyListItemModel = await State.GetClientInfo<CompanyListItemModel>($"api/company({CompanyContractModel.INN})?$select=inn,companyname&$expand=tags($select=name,id),clientcomments");
            CompanyListItemModel.ClientType = ClientType.Company;
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

    }
}
