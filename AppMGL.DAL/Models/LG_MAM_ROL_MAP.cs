using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_MAM_ROL_MAP
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal MrmId
		{
			get;
			set;
		}

		public decimal RleId
		{
			get;
			set;
		}

		public decimal ModId
		{
			get;
			set;
		}

		public decimal ActId
		{
			get;
			set;
		}

		public bool? MamIsEnable
		{
			get;
			set;
		}

		public DateTime MrmCreatedTs
		{
			get;
			set;
		}

		public DateTime? MrmUpdatedTs
		{
			get;
			set;
		}

		public decimal? MrmCreatedBy
		{
			get;
			set;
		}

		public decimal? MrmUpdatedBy
		{
			get;
			set;
		}

		public virtual LG_ACTION LG_ACTION
		{
			get;
			set;
		}

		public virtual LG_MODULE LG_MODULE
		{
			get;
			set;
		}

		public virtual LG_ROLE LG_ROLE
		{
			get;
			set;
		}
	}
}
