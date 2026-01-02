using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_SITE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal SitId
		{
			get;
			set;
		}

		public decimal LcnId
		{
			get;
			set;
		}

		public decimal SitType
		{
			get;
			set;
		}

		public string SitName
		{
			get;
			set;
		}

		public string SitCode
		{
			get;
			set;
		}

		public bool? SitStatus
		{
			get;
			set;
		}

		public string SitReason
		{
			get;
			set;
		}

		public DateTime SitCreatedTs
		{
			get;
			set;
		}

		public DateTime? SitUpdatedTs
		{
			get;
			set;
		}

		public decimal? SitCreatedBy
		{
			get;
			set;
		}

		public decimal? SitUpdatedBy
		{
			get;
			set;
		}

		public virtual LG_LOCATION LG_LOCATION
		{
			get;
			set;
		}

		public virtual ICollection<LG_SITE_CONTACT_ROLE> LG_SITE_CONTACT_ROLE
		{
			get;
			set;
		}

		public LG_SITE()
		{
			LG_SITE_CONTACT_ROLE = new HashSet<LG_SITE_CONTACT_ROLE>();
		}
	}
}
