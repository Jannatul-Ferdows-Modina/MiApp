using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_USSTATE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal UstId
		{
			get;
			set;
		}

		public string UstName
		{
			get;
			set;
		}

		public string UstAbbrev
		{
			get;
			set;
		}

		public bool? UstStatus
		{
			get;
			set;
		}

		public DateTime UstCreatedTs
		{
			get;
			set;
		}

		public DateTime? UstUpdatedTs
		{
			get;
			set;
		}

		public decimal? UstCreatedBy
		{
			get;
			set;
		}

		public decimal? UstUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_LOCATION> LG_LOCATION
		{
			get;
			set;
		}

		public LG_USSTATE()
		{
			LG_LOCATION = new HashSet<LG_LOCATION>();
		}
	}
}
