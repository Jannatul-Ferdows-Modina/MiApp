using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_BookingStatus
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int StatusID
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public string Type
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
