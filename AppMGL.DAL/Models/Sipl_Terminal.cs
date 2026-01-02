using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class Sipl_Terminal
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int terminalId
		{
			get;
			set;
		}

		public string terminalName
		{
			get;
			set;
		}

		public DateTime? terminalDate
		{
			get;
			set;
		}
	}
}
