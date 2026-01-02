using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_SITE_CONTACT_ROLE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal ScrId
		{
			get;
			set;
		}

		public decimal? SitId
		{
			get;
			set;
		}

		public decimal CntId
		{
			get;
			set;
		}

		public decimal RleId
		{
			get;
			set;
		}

		public DateTime ScrCreatedTs
		{
			get;
			set;
		}

		public DateTime? ScrUpdatedTs
		{
			get;
			set;
		}

		public decimal? ScrCreatedBy
		{
			get;
			set;
		}

		public decimal? ScrUpdatedBy
		{
			get;
			set;
		}

		public virtual LG_CONTACT LG_CONTACT
		{
			get;
			set;
		}

		public virtual LG_ROLE LG_ROLE
		{
			get;
			set;
		}

		public virtual LG_SITE LG_SITE
		{
			get;
			set;
		}
	}
}
