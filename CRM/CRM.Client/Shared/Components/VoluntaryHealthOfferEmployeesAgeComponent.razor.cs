using System.Collections.Generic;
using CRM.Operation.Models.DealOfferModels;
using Microsoft.AspNetCore.Components;

namespace CRM.Client.Shared.Components
{
    public partial class VoluntaryHealthOfferEmployeesAgeComponent
    {
        [Parameter]
        public bool ReadOnly { get; set; }

        [Parameter]
        public VoluntaryHealthInsuranceOfferModel VoluntaryHealthInsuranceOfferModel { get; set; }
        
        protected override void OnInitialized()
        {
            //Initialize employee group list if it's null.
            VoluntaryHealthInsuranceOfferModel!.VoluntaryHealthInsuranceEmployeeGroups ??= new List<VoluntaryHealthInsuranceEmployeeGroupViewModel>();
            base.OnInitialized();
        }
        
        private void AddEmployeeGroupItem()
        {
            var employeeGroupViewModel = new VoluntaryHealthInsuranceEmployeeGroupViewModel();
            
            VoluntaryHealthInsuranceOfferModel.VoluntaryHealthInsuranceEmployeeGroups.Add(employeeGroupViewModel);
        }
        
        private void RemoveOfferItem(VoluntaryHealthInsuranceEmployeeGroupViewModel groupViewModel)
        {
            if (!VoluntaryHealthInsuranceOfferModel.VoluntaryHealthInsuranceEmployeeGroups.Contains(groupViewModel))
                return;
            
            VoluntaryHealthInsuranceOfferModel.VoluntaryHealthInsuranceEmployeeGroups.Remove(groupViewModel);
        }
    }
}