using System;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
   public class LG_View_WarehouseLocation
    {
             [Key]
            public int? LocationID { get; set; }
            public string StorageLocation { get; set; }
            public string BlockNumber { get; set; }
            public bool IsActive { get; set; }
            public DateTime? CreatedOn { get; set; }
            public DateTime? ModifiedOn { get; set; } // Nullable to handle null values
            public string ModifiedBy { get; set; }
            public string CreatedBy { get; set; }
        
    }
}
