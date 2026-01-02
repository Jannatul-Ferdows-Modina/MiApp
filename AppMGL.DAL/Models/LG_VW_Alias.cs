using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_VW_Alias
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int AliasID
		{
			get;
			set;
		}

		public string Alias
		{
			get;
			set;
		}

		public int? AliasReferenceID
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public int? CreatedBy
		{
			get;
			set;
		}

		public DateTime? CreatedOn
		{
			get;
			set;
		}

		public int? ModifiedBy
		{
			get;
			set;
		}

		public DateTime? ModifiedOn
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
	}
}
