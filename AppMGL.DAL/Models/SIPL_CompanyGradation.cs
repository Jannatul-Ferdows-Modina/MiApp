using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_CompanyGradation
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int GradationID
		{
			get;
			set;
		}

		public string CompanyGradation
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public int CreatedBy
		{
			get;
			set;
		}

		public int ModifiedBy
		{
			get;
			set;
		}

		public DateTime? CreatedOn
		{
			get;
			set;
		}

		public DateTime? ModifiedOn
		{
			get;
			set;
		}

		public bool? IsActive
		{
			get;
			set;
		}
	}
}
