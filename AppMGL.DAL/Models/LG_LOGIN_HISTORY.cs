using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_LOGIN_HISTORY
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal LhyId
		{
			get;
			set;
		}

		public string LhyLoginName
		{
			get;
			set;
		}

		public bool? lhyLoginSuccess
		{
			get;
			set;
		}

		public string lhyLoginComment
		{
			get;
			set;
		}

		public DateTime? lhyCreatedTs
		{
			get;
			set;
		}
	}
}
