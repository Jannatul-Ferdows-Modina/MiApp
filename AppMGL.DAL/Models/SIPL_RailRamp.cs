using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_RailRamp
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int railId
		{
			get;
			set;
		}

		public string railRamp
		{
			get;
			set;
		}

		public DateTime? railDate
		{
			get;
			set;
		}
	}
}
