using System;
using CRM.Operation.Attributes;




namespace CRM.Operation.Models
{
    public class MedicineModel
    {
        [DisplayNameLocalized("MedicineModel.CardNumber")]
        public string CardNumber { get; set; }
        [DisplayNameLocalized("MedicineModel.ClinicName")]
        public string ClinicName { get; set; }
        //[DisplayNameLocalized("MedicineModel.ComplaintNote")]
        //public string ComplaintNote { get; set; }
        [DisplayNameLocalized("MedicineModel.PolicyNumber")]
        public string PolicyNumber { get; set; }
        [DisplayNameLocalized("MedicineModel.ServiceDepartment")]
        public string ServiceDepartment { get; set; }
        [DisplayNameLocalized("MedicineModel.ServiceDate")]
        public DateTime? ServiceDate { get; set; }
        [DisplayNameLocalized("MedicineModel.CompanyName")]
        public string CompanyName { get; set; }
        
    }
}
