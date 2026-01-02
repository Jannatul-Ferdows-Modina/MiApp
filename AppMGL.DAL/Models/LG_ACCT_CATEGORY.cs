using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_ACCT_CATEGORY
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal AcyID
		{
			get;
			set;
		}

		public string AcyNAME
		{
			get;
			set;
		}

		public DateTime AcyCreatedTs
		{
			get;
			set;
		}

		public DateTime? Acy_UpdatedTs
		{
			get;
			set;
		}

		public decimal? Acy_CreatedBy
		{
			get;
			set;
		}

		public decimal? Acy_UpdatedBy
		{
			get;
			set;
		}
	}
}
