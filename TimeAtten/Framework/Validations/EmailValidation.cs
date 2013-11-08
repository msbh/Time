using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using TimeAtten.Services.Utilities;

namespace TimeAtten.Framework.Validations
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return RegEx.IsValidEmail((string)value);
        }
    }
}