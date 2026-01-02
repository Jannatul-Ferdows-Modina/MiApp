using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_DEPARTMENT
	{
		[Key]
		public decimal DptId
		{
			get;
			set;
		}

		public string DptName
		{
			get;
			set;
		}

		public bool? DptStatus
		{
			get;
			set;
		}

		public DateTime DptCreatedTs
		{
			get;
			set;
		}

		public DateTime? DptUpdatedTs
		{
			get;
			set;
		}

		public decimal? DptCreatedBy
		{
			get;
			set;
		}

		public decimal? DptUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_CONTACT> LG_CONTACT
		{
			get;
			set;
		}

		public LG_DEPARTMENT()
		{
			LG_CONTACT = new HashSet<LG_CONTACT>();
		}
	}
}
