using CRM.Operation.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zircon.Core.Attributes;

namespace CRM.Operation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NormalizeAttribute : ValidationAttribute
    {
        
       
        public NormalizeAttribute() 
        {
           
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
