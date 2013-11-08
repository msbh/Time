using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAtten.Services.Services;

namespace TimeAtten.Framework.Admin
{
    public class Country
    {
        public string CountryName { get; set; }
        public int Value { get; set; }
    }

    public class CountriesModel
    {
        UserServices UserServc = new UserServices();
        public IEnumerable<Country> Countries { get; set; }
        public SelectList CountrySelectList { get; set; }
        public Country Country { get; set; }

        public CountriesModel()
        {
            this.Countries = UserServc.GetCountriesSelect();
            this.CountrySelectList = new SelectList(Countries, "Value", "CountryName");
        }
    }
}
