using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_Commodity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

		public int? CommodityTypeID
		{
			get;
			set;
		}
        public int? CommodityGroupId
        {
            get;
            set;
        }
        public bool? LOW_Value_High_Risk_Remark
        {
            get;
            set;
        }

        public virtual SIPL_CommodityType SIPL_CommodityType
		{
			get;
			set;
		}

        //public virtual SIPL_CommodityGroup SIPL_CommodityGroup
        //{
        //    get;
        //    set;
        //}
    }

	public class SIPL_CommodityDetail
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

		public int? CreatedBy
		{
			get;
			set;
		}

		//public DateTime? CreatedOn
		//{
		//	get;
		//	set;
		//}

		//public int? ModifiedBy
		//{
		//	get;
		//	set;
		//}

		//public DateTime? ModifiedOn
		//{
		//	get;
		//	set;
		//}

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

		public int? CommodityTypeID
		{
			get;
			set;
		}
        public int? CommodityGroupId
        {
            get;
            set;
        }
        public bool? LOW_Value_High_Risk_Remark
		{
			get;
			set;
		}

		
	}
}
