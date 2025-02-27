using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using CRM.Operation.Models;
using CRM.Operation.Models.Login;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;
using Zircon.Core.Extensions;
using CRM.Client.States;
using CRM.Client.Shared.Components;
using CRM.Data.Enums;
using Zircon.Core;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class ContactCardView
    {
        [CascadingParameter]
        public CRM.Client.Shared.ComponentInstance BaseCompo { get; set; }

        [CascadingParameter(Name = "ClientContractModel")]
        public ClientContract ClientContractModel { get; set; }

        [CascadingParameter(Name = "RoleModel")]
        private RoleModel RoleModel { get; set; }

        public List<CommentContract> ClientComments { get; set; } = new List<CommentContract>();


        public List<CuratorProductModel> CuratorProducts { get; set; } = new List<CuratorProductModel>();
        public ContactListItemModel ContactListItemModel { get; set; } = new ContactListItemModel();

        public bool IsEdit = false;
        public bool ClientInfoOpenState;
        public bool CuratorProductOpenState;
        public bool ContactInfoOpenState;
        public bool CommentInfoOpenState;
        public bool CallsOpenState;
        public bool AddressOpenState;
        public bool DocumentOpenState;
        public bool RelationOpenState;
        private ClaimsPrincipal User;
        public int SelectedCountryID { get; set; }

        protected async Task SubmitClientAsync(ClientContract model)
        {
            ClientInfoContract contract = new ClientInfoContract();
            contract.ResponsiblePerson = User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value;
            contract.Source = 3; //SourceType.CRM; 
            model.ContactsInfo.ForEach(x => x.ContactComments.ForEach(x =>
            {
                x.Creator = contract.ResponsiblePerson;
                x.FullName = User.Identity.Name;
            }));
            ClientComments.ForEach(
                x =>
                {
                    x.Creator = contract.ResponsiblePerson;
                    x.FullName = User.Identity.Name;
                });
            model.ClientComments = ClientComments;
            //model.Relations.ForEach(x => x.DocumentType = 3); // Document.INN
            contract.ResponsiblePerson = null;
            contract.Clients.Add(model);

            var result = await State.CreateOrUpdateClient(contract);
            SetState(result);
            await BaseCompo.Close();
            StateHasChanged();
        }


        public void AddContactItem()
        {
            ContactListItemModel.ContactsInfo.Add(new ContactInfoContract());
        }

        public void AddDocumentItem()
        {
            ContactListItemModel.Documents.Add(new DocumentContract());
        }

        public void AddRelationItem()
        {
            ((List<RelationContract>)ContactListItemModel.Relations).Add(new CRMRelationContract() { DocumentType = (int)DocumentTypeEnum.INN /*INN*/});
        }
        public void RemoveRelationItem(RelationContract contract)
        {
            ((List<RelationContract>)ContactListItemModel.Relations).Remove(contract);
        }

        public void AddClientCommentItem()
        {
            ClientComments.Add(new CommentContract());
        }

        public void RemoveCommentItem(CommentContract comment)
        {
            ClientComments.Remove(comment);
        }

        public void OnAddOption(Zircon.Core.ISourceItem item)
        {
            if (!ContactListItemModel.Tags.Any(x => x.Id == Convert.ToInt32(item.Key.ToString())))
            {
                ContactListItemModel.Tags.Add(new TagContract() { Id = Convert.ToInt32(item.Key.ToString()), Name = item.Value });
            }
        }

        public async void ClientInfoExpand()
        {
            if (!ClientInfoOpenState)
            {
                ContactListItemModel = (ContactListItemModel)ClientContractModel;
                ContactListItemModel = await State.GetClientInfo<ContactListItemModel>($"api/physicalPerson({ClientContractModel.INN})?$select= inn,firstname,lastname,fathername,fullname, pinnumber,birthdate,monthlyincome,position,positioncustom&$expand=tags($select=name,id),clientcomments");
                ContactListItemModel.ClientType = ClientType.Pyhsical;
                ClientInfoOpenState = true;
                StateHasChanged();
            }
        }
        private List<ProductBaseModel> InforceProductList = new List<ProductBaseModel>();
        private List<ProductBaseModel> InOtherStatusProductList = new List<ProductBaseModel>();
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

        public async void ContactInfoExpand()
        {
            if (!ContactInfoOpenState && !CallsOpenState)
            {
                ContactListItemModel.ContactsInfo =
                    await State.GetClientInfo<List<ContactInfoContract>>($"api/contact({ClientContractModel.INN})?$select=value,type");
                ContactListItemModel.ContactsInfo = ContactListItemModel.ContactsInfo.Where(x => x.Type != (int)ContactTypeEnum.MobileToken).ToList();
                ContactInfoOpenState = true;
                StateHasChanged();
            }
        }

        public async Task CommentInfoExpand()
        {
            if (!CommentInfoOpenState && !CallsOpenState)
            {
                var data = await State.GetClientInfo<List<CommentContract>>($"api/comment({ClientContractModel.INN})?$select=text,fullname,create_timestamp");
                Console.WriteLine(data);
                ContactListItemModel.ClientComments = data;


            }
        }

        public async void CallsExpand()
        {
            if (!CallsOpenState)
            {
                ContactListItemModel.ContactsInfo = await State.GetClientInfo<List<ContactInfoContract>>($"api/contact({ClientContractModel.INN})?$select=value,type&$expand=calls($select=responsibleperson,direction,calltimestamp)");
                CallsOpenState = true;
                StateHasChanged();
            }
        }

        public async void AddressExpand()
        {
            if (!AddressOpenState)
            {
                ContactListItemModel.Address = await State.GetClientInfo<AddressContract>($"api/address({ClientContractModel.INN})?$select=countryId,regOrCityId,districtOrStreet,additionalInfo");
                AddressOpenState = true;
                StateHasChanged();
            }
        }
        public async void DocumentExpand()
        {
            if (!DocumentOpenState)
            {
                ContactListItemModel.Documents = await State.GetClientInfo<List<DocumentContract>>($"api/document({ClientContractModel.INN})?$select=documentnumber,documenttype,documentexpiredate");
                DocumentOpenState = true;
                StateHasChanged();
            }
        }
        public async void RelationExpand()
        {
            if (!RelationOpenState)
            {
                var releatedClients = await State.GetClientInfo<IEnumerable<ClientRelationModel>>($"api/clientrelation({ClientContractModel.INN})?$select=relationTypeId,firstname, lastname,fathername,companyname, relationalINN");

                ContactListItemModel.Relations = new List<RelationContract>();
                if (releatedClients != null)
                {
                    foreach (var client in releatedClients)
                    {
                        ((List<RelationContract>)ContactListItemModel.Relations).Add(new RelationContract() { ClientINN = client.RelationalINN, ClientName = client.CompanyName ?? $"{client.FirstName} {client.LastName} {client.FatherName}", RelationType = client.RelationTypeId });
                    }
                }
                RelationOpenState = true;
                StateHasChanged();
            }
        }



        public string DuplicateRelationError;

        public void CheckForDuplicate(RelationContract currentRelation)
        {

            DuplicateRelationError = null;
            var duplicate = ContactListItemModel.Relations
                .Where(r => r != currentRelation)
                .Any(r => r.RelationType == currentRelation.RelationType && r.ClientINN == currentRelation.ClientINN);


            if (duplicate)
            {
                DuplicateRelationError = "Seçilən əlaqə tipində bu məlumatı artıq qeyd etmisiniz!";
            }
        }

        protected override async Task OnInitializedAsync()
        {

            User = (await AuthenticationStateTask).User;
            bool isHide = await HideSensetiveData();

            ContactListItemModel = (ContactListItemModel)ClientContractModel;
            ContactListItemModel = await State.GetClientInfo<ContactListItemModel>($"api/physicalPerson({ClientContractModel.INN})?$select= inn,firstname,lastname,fathername, fullname, pinnumber,birthdate,monthlyincome,position,positioncustom&$expand=tags($select=name,id),clientcomments");
            ContactListItemModel.ClientType = ClientType.Pyhsical;
            
            ContactListItemModel.MonthlyIncome = isHide ? null : ContactListItemModel.MonthlyIncome;

            //foreach(CommentContract comment in ContactListItemModel.ClientComments)
            // {
            //     OldClientComments.Add(new CommentContract() { Creator = comment.Creator, Id = comment.Id, Text = comment.Text });
            // }
        }

        public async Task<bool> HideSensetiveData()
        {
            try
            {
                var user = await State.HasUserLoggedIn();
                var parameter = user.Permissions.FirstOrDefault(x => x != null && x.Name == "CRMRole").Parameters.FirstOrDefault(x => x.Key == "HideSensitiveData");

                return bool.TryParse(parameter.Value, out bool result) && result;
            }
            catch
            {
                return false;
            }
        }
    }
}
