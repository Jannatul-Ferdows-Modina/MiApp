using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_ACTION
	{
		[Key]
		public decimal ActId
		{
			get;
			set;
		}

		public string ActName
		{
			get;
			set;
		}

		public string ActCaption
		{
			get;
			set;
		}

		public DateTime ActCreatedTs
		{
			get;
			set;
		}

		public DateTime? ActUpdatedTs
		{
			get;
			set;
		}

		public decimal? ActCreatedBy
		{
			get;
			set;
		}

		public decimal? ActUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_MAM_ROL_MAP> LG_MAM_ROL_MAP
		{
			get;
			set;
		}

		public LG_ACTION()
		{
			LG_MAM_ROL_MAP = new HashSet<LG_MAM_ROL_MAP>();
		}
	}
}
