using System;
using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
    public class tbl_scheduler_TempContractRateDetail
    {
        [Key]
        public int CCID
        {
            get;
            set;
        }

        public string Commodity
        {
            get;
            set;
        }

        public string Route
        {
            get;
            set;
        }

        public string OriginType
        {
            get;
            set;
        }

        public string DestinationType
        {
            get;
            set;
        }

        public string Origin
        {
            get;
            set;
        }

        public string Destination
        {
            get;
            set;
        }

        public string ViaType1
        {
            get;
            set;
        }

        public string ViaType2
        {
            get;
            set;
        }

        public string Via1Name
        {
            get;
            set;
        }

        public string Via2Name
        {
            get;
            set;
        }

        public double? Rate
        {
            get;
            set;
        }

        public string ContractNo
        {
            get;
            set;
        }

        public DateTime? StartDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

        public decimal? ContractID
        {
            get;
            set;
        }

        public string Carrier
        {
            get;
            set;
        }

        public int? RouteId
        {
            get;
            set;
        }

        public double? Standard20
        {
            get;
            set;
        }

        public double? Standard40
        {
            get;
            set;
        }

        public double? HighCube40
        {
            get;
            set;
        }

        public double? C45F
        {
            get;
            set;
        }

        public double? SurST20
        {
            get;
            set;
        }

        public double? SurST40
        {
            get;
            set;
        }

        public double? SurHC40
        {
            get;
            set;
        }

        public double? Sur45F
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public string ContractRemark
        {
            get;
            set;
        }

        public DateTime? LastUpdatedOn
        {
            get;
            set;
        }

        public double? C45F1
        {
            get;
            set;
        }
    }
}
