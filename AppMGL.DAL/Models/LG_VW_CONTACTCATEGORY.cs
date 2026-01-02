using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_VW_CONTACTCATEGORY
	{
		[Key]
		public int ContactCategoryId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public bool? IsActive
		{
			get;
			set;
		}

		public int? Commodity
		{
			get;
			set;
		}

		public int? Origin
		{
			get;
			set;
		}

		public int? Destination
		{
			get;
			set;
		}

		public bool? IsVendor
		{
			get;
			set;
		}
       
    }
}
