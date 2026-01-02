using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_MODULE
	{
		[Key]
		public decimal ModId
		{
			get;
			set;
		}

		public decimal MteId
		{
			get;
			set;
		}

		public string ModName
		{
			get;
			set;
		}

		public string ModCaption
		{
			get;
			set;
		}

		public DateTime ModCreatedTs
		{
			get;
			set;
		}

		public DateTime? ModUpdatedTs
		{
			get;
			set;
		}

		public decimal? ModCreatedBy
		{
			get;
			set;
		}

		public decimal? ModUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_MAM_ROL_MAP> LG_MAM_ROL_MAP
		{
			get;
			set;
		}

		public virtual LG_MODULE_TYPE LG_MODULE_TYPE
		{
			get;
			set;
		}

		public LG_MODULE()
		{
			LG_MAM_ROL_MAP = new HashSet<LG_MAM_ROL_MAP>();
		}
	}
}
