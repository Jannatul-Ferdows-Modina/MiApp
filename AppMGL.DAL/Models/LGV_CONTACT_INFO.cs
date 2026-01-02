using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
    public class LGV_CONTACT_INFO
    {
        public string CompanyName
        {
            get;
            set;
        }

        public string TelNo
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string ContactPerson
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string WebSite
        {
            get;
            set;
        }

        public string ZipCode
        {
            get;
            set;
        }

        [Key]
        public int ContactID
        {
            get;
            set;
        }

        public string MobNo
        {
            get;
            set;
        }

        public string Fax
        {
            get;
            set;
        }

        public string TaxID
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public int? fkCountryId
        {
            get;
            set;
        }

        public int? StateID
        {
            get;
            set;
        }

        public string Continent
        {
            get;
            set;
        }

        public string CountryName
        {
            get;
            set;
        }

        public bool? IsDeleted
        {
            get;
            set;
        }

        public string AccountDetail
        {
            get;
            set;
        }

        public string AccountNo
        {
            get;
            set;
        }

        public string CustomerCode
        {
            get;
            set;
        }

        public string ContactCategoryID
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string RepresentativeID
        {
            get;
            set;
        }

        public string GalRepresentative
        {
            get;
            set;
        }

        public string BranchName
        {
            get;
            set;
        }
    }
}
