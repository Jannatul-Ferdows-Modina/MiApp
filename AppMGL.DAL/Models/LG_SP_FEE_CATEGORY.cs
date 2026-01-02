using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_SP_FEE_CATEGORY
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal SfcID
		{
			get;
			set;
		}

		public string SfcName
		{
			get;
			set;
		}

		public string SfcCode
		{
			get;
			set;
		}

		public DateTime SfcCreatedTs
		{
			get;
			set;
		}

		public DateTime? Sfc_UpdatedTs
		{
			get;
			set;
		}

		public decimal? Sfc_CreatedBy
		{
			get;
			set;
		}

		public decimal? Sfc_UpdatedBy
		{
			get;
			set;
		}

		public decimal? SfcPercentage
		{
			get;
			set;
		}
	}
}
