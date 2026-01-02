using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_CommodityType
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int CommodityTypeID
		{
			get;
			set;
		}

		public string CommodityType
		{
			get;
			set;
		}

		public virtual ICollection<SIPL_Commodity> SIPL_Commodity
		{
			get;
			set;
		}

		public SIPL_CommodityType()
		{
			SIPL_Commodity = new HashSet<SIPL_Commodity>();
		}
	}
}
