using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_COUNTRY
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal CryId
		{
			get;
			set;
		}

		public string CryName
		{
			get;
			set;
		}

		public string CryAbbrev
		{
			get;
			set;
		}

		public bool? CryStatus
		{
			get;
			set;
		}

		public DateTime CryCreatedTs
		{
			get;
			set;
		}

		public DateTime? CryUpdatedTs
		{
			get;
			set;
		}

		public decimal? CryPhoneCode
		{
			get;
			set;
		}

		public decimal? CryCreatedBy
		{
			get;
			set;
		}

		public decimal? CryUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_LOCATION> LG_LOCATION
		{
			get;
			set;
		}

		public LG_COUNTRY()
		{
			LG_LOCATION = new HashSet<LG_LOCATION>();
		}
	}
}
