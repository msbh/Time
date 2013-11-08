using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
namespace TimeAtten.Services.Utilities
{
    public class RegEx
    {
        public const string HexColorPattern = @"#[0-9a-zA-Z]{3,6}";

        public static bool IsValidEmail(string email)
        {
            if (email == null)
            {
                email = "";
            }
            if (email != "")
            {
                try
                {
                    email = new MailAddress(email).Address;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
            else { return false; }

            return true;
        }
    }
}