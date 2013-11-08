using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TimeAtten.Framework.Validations;
using TimeAtten.Models;

namespace TimeAtten.Framework.Admin
{
    public class OwnerModal
    {
        [Required]
        public string Username { get; set; }
        public string id { get; set; }
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
    }

    public class OwnerCreation
    {
        public OwnerModal OwnerModal { get; set; }
        public Company Company { get; set; }
        public EMP_REC UserProfile { get; set; }
        public User User { get; set; }

        public OwnerCreation()
        {
            OwnerModal = new OwnerModal();
            Company = new Company();
            UserProfile = new EMP_REC();
            User = new User();
        }
    }
    public class OwnerMultipleList
    {
        public List<OwnerModal> OwnerModal { get; set; }
        public List<Company> Company { get; set; }

        public OwnerMultipleList()
        {
            OwnerModal = new List<OwnerModal>();
            Company = new  List<Company>();
        }
    }
}
