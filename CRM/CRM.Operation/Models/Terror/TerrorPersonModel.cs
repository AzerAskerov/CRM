using System;
using System.Collections.Generic;

namespace CRM.Operation.Models.Terror
{
    public class TerrorPersonModel
    {
        public TerrorPersonModel()
        {
            PhoneNumber = new List<string>();
            Email = new List<string>();
        }
        
        private string _fullName;

        public int? Inn { get; set; }
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string FullName
        {
            get => _fullName ?? $"{FirstName} {LastName}";
            set => _fullName = value;
        }
        public string PassportNr{ get; set; }
        public string Pin { get; set; }
        public DateTime? Birthdate{ get; set; }
        public List<string> Addresses{ get; set; }
        public List<string> Email{ get; set; }
        public List<string> PhoneNumber{ get; set; }
        
        
    }
}