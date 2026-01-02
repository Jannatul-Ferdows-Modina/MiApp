using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_TIMEZONE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal TmzId
		{
			get;
			set;
		}

		public string TmzName
		{
			get;
			set;
		}

		public string TmzOffset
		{
			get;
			set;
		}

		public string TmzAbbrev
		{
			get;
			set;
		}

		public string TmzLocation
		{
			get;
			set;
		}

		public bool? TmzStatus
		{
			get;
			set;
		}

		public DateTime TmzCreatedTs
		{
			get;
			set;
		}

		public DateTime? TmzUpdatedTs
		{
			get;
			set;
		}

		public decimal? TmzCreatedBy
		{
			get;
			set;
		}

		public decimal? TmzUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_LOCATION> LG_LOCATION
		{
			get;
			set;
		}

		public LG_TIMEZONE()
		{
			LG_LOCATION = new HashSet<LG_LOCATION>();
		}
	}
}
