using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_Contact
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int ContactID
		{
			get;
			set;
		}

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

		public string WebSite
		{
			get;
			set;
		}

		public int? CityID
		{
			get;
			set;
		}

		public string ContactPerson
		{
			get;
			set;
		}

		public string TaxID
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public string Remarks
		{
			get;
			set;
		}

		public int? StateID
		{
			get;
			set;
		}

		public DateTime? CreatedOn
		{
			get;
			set;
		}

		public int? CreatedBy
		{
			get;
			set;
		}

		public int? ModifiedBy
		{
			get;
			set;
		}

		public DateTime? ModifiedOn
		{
			get;
			set;
		}

		public bool? IsActive
		{
			get;
			set;
		}

		public bool? IsDeleted
		{
			get;
			set;
		}

		public bool? Flag
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

		public string Attachment
		{
			get;
			set;
		}

		public string CompanyGradation
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

		public string RepresentativeID
		{
			get;
			set;
		}

		public int? ContinentId
		{
			get;
			set;
		}

		public int? CountryId
		{
			get;
			set;
		}

		public int? BillTo
		{
			get;
			set;
		}

		public string BillToCompanyName
		{
			get;
			set;
		}

		public string BillToAddress
		{
			get;
			set;
		}

		public virtual SIPL_City SIPL_City
		{
			get;
			set;
		}

		public virtual ICollection<SIPL_ContactBranch> SIPL_ContactBranch
		{
			get;
			set;
		}

		public virtual SIPL_State SIPL_State
		{
			get;
			set;
		}

		public SIPL_Contact()
		{
			SIPL_ContactBranch = new HashSet<SIPL_ContactBranch>();
		}
	}
}
