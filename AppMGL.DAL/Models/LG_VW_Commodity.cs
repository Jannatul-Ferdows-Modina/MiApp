using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_VW_Commodity
	{
		[Key]
		public int CommodityId
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

		public int? CommodityTypeID
		{
			get;
			set;
		}

		public string CommodityType
		{
			get;
			set;
		}
	}
}
