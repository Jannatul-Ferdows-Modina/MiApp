using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_CONTACT
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal CntId
		{
			get;
			set;
		}

		public decimal DptId
		{
			get;
			set;
		}

		public decimal TtlId
		{
			get;
			set;
		}

		public decimal CntType
		{
			get;
			set;
		}

		public string CntFirstName
		{
			get;
			set;
		}

		public string CntMiddleName
		{
			get;
			set;
		}

		public string CntLastName
		{
			get;
			set;
		}

		public string CntEmail
		{
			get;
			set;
		}

		public bool? CntStatus
		{
			get;
			set;
		}

		public string CntReason
		{
			get;
			set;
		}

		public DateTime CntCreatedTs
		{
			get;
			set;
		}

		public DateTime? CntUpdatedTs
		{
			get;
			set;
		}

		public decimal? CwtId
		{
			get;
			set;
		}

		public decimal? CntCreatedBy
		{
			get;
			set;
		}

		public decimal? CntUpdatedBy
		{
			get;
			set;
		}

		public string CntImageName
		{
			get;
			set;
		}

		public virtual LG_DEPARTMENT LG_DEPARTMENT
		{
			get;
			set;
		}

		public virtual LG_TITLE LG_TITLE
		{
			get;
			set;
		}

		public virtual ICollection<LG_LOCATION> LG_LOCATION
		{
			get;
			set;
		}

		public virtual ICollection<LG_SITE_CONTACT_ROLE> LG_SITE_CONTACT_ROLE
		{
			get;
			set;
		}

		public virtual ICollection<LG_USER> LG_USER
		{
			get;
			set;
		}

		public virtual LG_CONTACT_WORK_TYPE LG_CONTACT_WORK_TYPE
		{
			get;
			set;
		}
		public string Address { get; set; }
		public string CellNo { get; set; }
		public string ContactPerson { get; set; }
		public string Fax { get; set; }
		public string TelNo { get; set; }
		public string ZipCode { get; set; }
		public int? CountryId { get; set; }
		public int? StateId { get; set; }
		public int? CityId { get; set; }
		public int? ContinentId { get; set; }
		public string userroleid { get; set; }
		
		public LG_CONTACT()
		{
			LG_LOCATION = new HashSet<LG_LOCATION>();
			LG_SITE_CONTACT_ROLE = new HashSet<LG_SITE_CONTACT_ROLE>();
			LG_USER = new HashSet<LG_USER>();
		}
	}
}
