using System;


namespace AppMGL.DTO.Operation
{
    public class LGVWDisplayRateDTO
    {

        public int CCID { get; set; }
        public string Commodity { get; set; }
        public string Route { get; set; }
        public string OriginType { get; set; }
        public string DestinationType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ViaType1 { get; set; }
        public string ViaType2 { get; set; }
        public string Via1Name { get; set; }
        public string Via2Name { get; set; }
        public Nullable<double> Rate { get; set; }
        public string ContractNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<decimal> ContractID { get; set; }
        public string Carrier { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<double> Standard20 { get; set; }
        public Nullable<double> Standard40 { get; set; }
        public Nullable<double> HighCube40 { get; set; }
        public Nullable<double> C45F { get; set; }
        public Nullable<double> SurST20 { get; set; }
        public Nullable<double> SurST40 { get; set; }
        public Nullable<double> SurHC40 { get; set; }
        public Nullable<double> Sur45F { get; set; }
        public string Remarks { get; set; }
        public string ContractRemark { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public string Status { get; set; }
        public int?  TotalCount { get; set; }
        public ContractCommodities[] ContractCommoditiesList { get; set; }

       

    }

    public class ContractCommodities
    {
        public int? CommodityId { get; set; }
        public string Commodity { get; set; }
        public int? CommodityTypeID { get; set; }
        public string CommodityTypeName { get; set; }
        public string Route { get; set; }
        public int? RouteId { get; set; }
        public string ContractNo { get; set; }
        public int ContactID { get; set; }
        public double? TotalRate { get; set; }
        public ContractCharges[] ContractChargesList { get; set; }
    }

    public class ContractCharges
    {
        public int? RefID { get; set; }
        public int? RouteId { get; set; }
        public int? CommodityId { get; set; }
        public string ContainerName { get; set; }
        public double? Rate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ContactID { get; set; }        
        public ContractSurcharge[] ContractSurchargeList { get; set; }
    }

    public class ContractSurcharge
    {
        public int? CommodityId { get; set; }
        public string Commodity { get; set; }
        public int? CommodityTypeID { get; set; }
        public string CommodityTypeName { get; set; }
        public string Route { get; set; }
        public int? RouteId { get; set; }
        public string ContractNo { get; set; }
        public double? TotalRate { get; set; }
    }



}
