using System;

namespace AppMGL.DTO.DataManagement
{
    public class CommodityDTO
    {

        public int? CommodityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool Flag { get; set; }
        public int? CommodityTypeID { get; set; }

        public int? CommodityGroupId { get; set; }

        public string CommodityTypeName { get; set; }
        public bool LOW_Value_High_Risk_Remark { get;  set; }

        public int? TotalCount { get; set; }
        public string CommodityType { get; set; }

        public string CommodityGroupName { get; set; }

    }
}