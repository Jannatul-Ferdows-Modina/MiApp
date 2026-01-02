using AppMGL.DTO.DataManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
    public class sipl_warehouselocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LocationID { get; set; }

        public int BlockId { get; set; }
        public string StorageLocation { get; set; }
        public string BlockNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; } // Nullable to handle null values
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        public int? TotalCount { get; set; }
    }

    public class sipl_warehousemappinglist
    {

        public int ID { get; set; }
        public string QuatationNo { get; set; }
        public int? WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public string BlockNumber { get; set; }

        public string BlockId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; } // Nullable to handle null values
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int? SIT_ID { get; set; }
        public int? TotalCount { get; set; }

        public bool IsActive { get; set; }

        public string ChallanNo { get; set; }
        public System.DateTime? ChallanDate { get; set; }
        public List<blocknumberlist> blocknumberlist { get; set; }
        public string Warehouseinwardno { get; set; }

        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string BLNo { get; set; }
        public string Marks { get; set; }
        public string PackagesDescription { get; set; }
        public string GrossWt { get; set; }
        public string CBM { get; set; }
        public decimal? FOBAmount { get; set; }
        public string FreightStatus { get; set; }
        public CargoMaineFest[] CargoMaineFest { get; set; }
    }

    public class quotationsearch
    {
        public string QuotationID { get; set; }
        public string QuotationNo { get; set; }
    }


    public class warehoselocationsearch {
        public int LocationId { get; set; }
        public string StorageLocation { get; set; }
    }

    public class blocknumberlist
    {
        public int LocationId { get; set; }
        public int BlockId { get; set; }
        public string Blocknumber { get; set; }
    }
}
