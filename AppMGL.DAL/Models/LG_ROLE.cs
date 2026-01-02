using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_ROLE
	{
		[Key]
		public decimal RleId
		{
			get;
			set;
		}

		public string RleName
		{
			get;
			set;
		}

		public bool? RleStatus
		{
			get;
			set;
		}

		public DateTime RleCreatedTs
		{
			get;
			set;
		}

		public DateTime? RleUpdatedTs
		{
			get;
			set;
		}

		public bool RleDisplay
		{
			get;
			set;
		}

		public string rleType
		{
			get;
			set;
		}

		public decimal? RleCreatedBy
		{
			get;
			set;
		}

		public decimal? RleUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_MAM_ROL_MAP> LG_MAM_ROL_MAP
		{
			get;
			set;
		}

		public virtual ICollection<LG_SITE_CONTACT_ROLE> LG_SITE_CONTACT_ROLE
		{
			get;
			set;
		}

		public LG_ROLE()
		{
			LG_MAM_ROL_MAP = new HashSet<LG_MAM_ROL_MAP>();
			LG_SITE_CONTACT_ROLE = new HashSet<LG_SITE_CONTACT_ROLE>();
		}
	}
}
