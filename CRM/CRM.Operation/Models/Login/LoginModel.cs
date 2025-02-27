


using CRM.Operation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CRM.Operation.Models.Login
{
    public class LoginModel
    {
        [RequiredLocalized]
        //[StringLengthLocalized(7, 15)]
        [DisplayNameLocalized("LoginModel.Login")]
        public string Login { get; set; }

        [RequiredLocalized]
        [DataType(DataType.Password)]
        [DisplayNameLocalized("Login.Password")]
        public string Password { get; set; }

        public string Language { get; set; }
        public string UserRequestIp { get; set; }
    }
}
