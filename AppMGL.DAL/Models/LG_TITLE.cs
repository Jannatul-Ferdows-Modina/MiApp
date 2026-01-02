using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_TITLE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal TtlId
		{
			get;
			set;
		}

		public string TtlName
		{
			get;
			set;
		}

		public bool? TtlStatus
		{
			get;
			set;
		}

		public DateTime TtlCreatedTs
		{
			get;
			set;
		}

		public DateTime? TtlUpdatedTs
		{
			get;
			set;
		}

		public decimal? TtlCreatedBy
		{
			get;
			set;
		}

		public decimal? TtlUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_CONTACT> LG_CONTACT
		{
			get;
			set;
		}

		public LG_TITLE()
		{
			LG_CONTACT = new HashSet<LG_CONTACT>();
		}
	}
}
