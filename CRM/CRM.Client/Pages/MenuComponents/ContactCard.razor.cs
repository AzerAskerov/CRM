using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

using CRM.Data.Database;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Enums;

namespace CRM.Client.Pages.MenuComponents
{
    public partial class ContactCard
    {
        [CascadingParameter]
        public CRM.Client.Shared.ComponentInstance BaseCompo { get; set; }
        [Parameter]
        public ClientContract ClientContractModel { get; set; }

        [Parameter]
        public List<Position> Positions { get; set; }
        public int SelectedCountryID { get; set; }
        public ClaimsPrincipal User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ClientContractModel.ClientType = ClientType.Pyhsical;
            
            //Positions = _db.Position.ToList();
            ClientContractModel.ContactsInfo = new List<ContactInfoContract>() { new ContactInfoContract() };
            ClientContractModel.Documents = new List<DocumentContract>() { new DocumentContract()};
            ClientContractModel.Relations = new List<CRMRelationContract>() { new CRMRelationContract() { DocumentType = (int)DocumentTypeEnum.INN /*INN*/} };
            ClientContractModel.ClientComments = new List<CommentContract>();
            ClientContractModel.Tags = new List<TagContract>();
            ClientContractModel.Address = new AddressContract();

            User = (await AuthenticationStateTask).User;
            await base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected async Task SubmitClientAsync(ClientContract model)
        {//card
            var user = (await AuthenticationStateTask).User;
           
            ClientInfoContract contract = new ClientInfoContract();
            contract.ResponsiblePerson = User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value;
            Console.WriteLine("ResponsiblePerson: " + contract.ResponsiblePerson);
            contract.Source = 3; //SourceType.CRM;
            
            model.ClientComments.ForEach(x => {
                x.Creator = contract.ResponsiblePerson;
                x.FullName = User.Identity.Name;
            }) ;
            model.ContactsInfo.ForEach(x => x.ContactComments.ForEach(x => x.Creator = contract.ResponsiblePerson));
            //model.Relations.ForEach(x => x.DocumentType = 3); // Document.INN
            contract.ResponsiblePerson = null;
           contract.Clients.Add(model);

            var result = await State.CreateClient(contract);
            SetState(result);
            await BaseCompo.Close();
            StateHasChanged();


        }

        public void AddContactItem()
        {
            ClientContractModel.ContactsInfo.Add(new ContactInfoContract());
        }

        protected void RemoveContactItem(ContactInfoContract item)
        {
            ClientContractModel.ContactsInfo.Remove(item);
        }

        public void AddCommentItem()
        {
            ClientContractModel.ClientComments.Add(new CommentContract() { FullName = User.Identity.Name,
                Creator = User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value
            });
        }

        public void RemoveCommentItem(CommentContract comment)
        {
            ClientContractModel.ClientComments.Remove(comment);
        }


        public void AddDocumentItem()
        {
            ClientContractModel.Documents.Add(new DocumentContract());
        }

        protected void RemoveDocumentItem(DocumentContract item)
        {
            ClientContractModel.Documents.Remove(item);
        }

        protected void AddRelationItem()
        {
            ((List<CRMRelationContract>)ClientContractModel.Relations).Add(new CRMRelationContract() { DocumentType = (int)DocumentTypeEnum.INN /*INN*/});
        }

        protected void RemoveRelationItem(RelationContract item)
        {
            ((List<CRMRelationContract>)ClientContractModel.Relations).Remove((CRMRelationContract)item);
        }

        public void OnAddOption(Zircon.Core.ISourceItem item)
        {
            if (!ClientContractModel.Tags.Any(x => x.Id == Convert.ToInt32(item.Key.ToString())))
            {
                ClientContractModel.Tags.Add(new TagContract() { Id = Convert.ToInt32(item.Key.ToString()), Name = item.Value });
            }
        }
    }
}
