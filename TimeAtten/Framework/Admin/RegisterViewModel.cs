using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TimeAtten.Framework.Validations;

namespace TimeAtten.Framework.Admin
{
   // [PropertiesMustMatch("Password", "ConfirmPassword")]
    public class RegisterViewModel 
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailValidation(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Sex { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string BirthdayDate { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Phone { get; set; }


        public string Address { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }


        [Required]
        [Display(Name = "Terms and Conditions")]
        [RegularExpression("[T|t]rue", ErrorMessage = "You must accept the Terms and Conditions.")]
        public bool Terms { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //   var userServices = ServiceLocator.Get<UserServices>();
        //    int usernameMin =4; //SiteConfig.UsernameMin.ToInt();
        //    int usernameMax = 20;//SiteConfig.UsernameMax.ToInt();
        //    int passwordMinimum = 3;//SiteConfig.PasswordMin.ToInt();
        //    if (Username == null)
        //    { Username = ""; }
        //    if (Username != "")
        //    {
        //        if (Username.Length > usernameMax || Username.Length < usernameMin)
        //        {
        //            string message = string.Format("Username length must be between {0} and {1} characters long", usernameMin, usernameMax);
        //            yield return new ValidationResult("Username length must be between " + usernameMin + " and " + usernameMax + " characters long", new[] { "Username" });
        //        }
        //        else if (userServices.GetUser(Username) != null)
        //            yield return new ValidationResult("Username already taken", new[] { "Username" });
        //    }
        //    else
        //    {
        //        string message = string.Format("Username length must be between {0} and {1} characters long", usernameMin, usernameMax);
        //        yield return new ValidationResult("Username length must be between " + usernameMin + " and " + usernameMax + " characters long", new[] { "Username" });
        //    }

        //    if (userServices.EmailInUse(Email))
        //        yield return new ValidationResult("Email is already in use by another user", new[] { "Email" });
        //    if (Password == null)
        //    {
        //        Password = "";
        //    }
        //    if (Password != "")
        //    {
        //        if (Password.Length < passwordMinimum)
        //        {
        //            string message = string.Format("Password must be at least {0}", passwordMinimum);
        //            yield return new ValidationResult(message, new[] { "Password" });
        //        }
        //    }
        //    else
        //    {
        //        string message = string.Format("Password must be at least {0}", passwordMinimum);
        //        yield return new ValidationResult(message, new[] { "Password" });
        //    }
        //}
    }
    //public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
    //        : base(_defaultErrorMessage)
    //    {
    //        OriginalProperty = originalProperty;
    //        ConfirmProperty = confirmProperty;
    //    }

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Username or email")]
        public string UsernameOrEmail { get; set; }
    }

    public class BothLoginHeader
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }

        public BothLoginHeader()
        {
            RegisterViewModel = new RegisterViewModel();
            ForgotPasswordViewModel = new ForgotPasswordViewModel();
            LoginViewModel = new LoginViewModel();
          
        }
    }
}