using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_LoadType
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int LoadTypeId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public bool? IsActive
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

		public bool? IsDeleted
		{
			get;
			set;
		}

		public bool? Flag
		{
			get;
			set;
		}
	}
}
