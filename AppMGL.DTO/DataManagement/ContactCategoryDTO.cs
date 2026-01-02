namespace AppMGL.DTO.DataManagement
{
    public class ContactCategoryDTO
    {
        public int? ContactCategoryId { get; set; }
        public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsVendor { get; set; }
        public int? Commodity { get; set; }
        public int? Origin { get; set; }
        public int? Destination { get; set; }
    }

    public class WarehouseLocationDTO
    {
        public int? LocationId { get; set; }
        public string Storagelocation { get; set; }
        public string BlockNumber { get; set; }        
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int? BlockId { get; set; }

    }

    public class WarehouseSaveResultDTO
    {
        public int? LocationId { get; set; }
        
        public int? BlockId { get; set; }

    }
    public class WarehouseMappingDTO
    {
        public int? ID { get; set; }
        public int?  QuatationId { get; set; }       
        public string QuatationNo { get; set; }
        public int? WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public string BlockNumber { get; set; }

        public string BlockId { get; set; }

        public string Description { get; set; }

        public System.DateTime CreatedOn { get; set; }
        public System.DateTime? ModifiedOn { get; set; } // Nullable to handle null values
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int? SIT_ID { get; set; }
        public int? TotalCount { get; set; }
        public bool IsActive { get; set; }

        public string ChallanNo { get; set; }
        public System.DateTime? ChallanDate { get; set; }

        public string Warehouseinwardno { get; set; }

        public string ShipperName { get; set; }      
        public string ConsigneeName { get; set; }
        public string BLNo { get; set; }
        public string Marks { get; set; }
        public string PackagesDescription { get; set; }
        public string GrossWt { get; set; }
        public string CBM { get; set; }
        public decimal FOBAmount { get; set; }
        public string FreightStatus { get; set; }
        public CargoMaineFest[] CargoMaineFest { get; set; }
    }
    public class CargoMaineFest
    {
        public int? WareHouseId { get; set; }
        public int? CargoMaineId { get; set; }
        public string ConsigneeName { get; set; }
        public string ShipperName { get; set; }
        public string BLNo { get; set; }
        public string Marks { get; set; }
        public string PackagesDescription { get; set; }
        public string GrossWt { get; set; }
        public string CBM { get; set; }
        public decimal? FOBAmount { get; set; }
        public string FreightStatus { get; set; }
        public int? FirstQuantity { get; set; }
        public string FirstUOM { get; set; }
        public int? SecondQuantity { get; set; }
        public string SecondUOM { get; set; }
        public string QuatationNo { get; set; }
        public string ChallanNo { get; set; }
        public int? BalanceQty { get; set; }
        public int? AllocatedQty { get; set; }
        public int? StuffedQty { get; set; }
        public int? OldStuffedQty { get; set; }
        public int? OldAllocatedQty { get; set; }
        public string WareHouseName { get; set; }
        public string Warehouseinwardno { get; set; }
        public string ContainerNo { get; set; }
        
    }
}