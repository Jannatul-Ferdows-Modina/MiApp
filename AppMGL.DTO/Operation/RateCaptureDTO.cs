using System;
using AppMGL.DTO.DataManagement;
using System.Collections.Generic;

namespace AppMGL.DTO.Operation
{
   public class RateCaptureDTO
    {
        public decimal ContractRateID { get; set; }
        public decimal ContractID { get; set; }
        public decimal CarrierID { get; set; }        
        public string CarrierName { get; set; }
        public string contractNo { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ApplicableToService { get; set; }
        public int? OriginCountryID { get; set; }
        public string OriginCountry { get; set; }
        public int? DestinationCountryID { get; set; }
        public string DischargeCountry { get; set; }
        public int? OriginCityID { get; set; }
        public string OriginCity { get; set; }
        public int? DestinationCityID { get; set; }
        public string DischargeCity { get; set; }
        public int? OrignStateID { get; set; }
        public string OriginState { get; set; }
        public int? DestinationStateID { get; set; }
        public string DischargeState { get; set; }
        public string Origin { get; set; }
        public string Discharge { get; set; }        
        public int? originID { get; set; }        
        public int? dischargeID { get; set; }
        public int? OriginModeID { get; set; }
        public string OriginMode { get; set; }
        public string DestinationMode { get; set; }
        public int? DestinationModeID { get; set; }
        public string Notes { get; set; } 
        public string AMDNO { get; set; }
        public string Commodity { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public long? ROWNUM { get; set; }
        public int? TotalCount { get; set; }
        public CommodityDTO[] CommodityDTOList { get; set; }
        public ContainerCharges[] ContainerChargesList { get; set; }
       public string RouteBy { get; set; }

        public string TranssitTime { get; set; }

        public string RouteBy2 { get; set; }
        public string TradeLane { get; set; }
        public string ScTermNo { get; set; }
        public string Container40Rate { get; set; }

        public string Container20Rate { get; set; }
        public string Container45Rate { get; set; }
        public string Container40RateBreakUp { get; set; }
        public string Container45RateBreakUp { get; set; }
        public string Container20RateBreakUp { get; set; }
        public string Status { get; set; }
        
    }

    public class ContainerCharges
    {
        public decimal? ContractRateID { get; set; }
        public decimal? CategoryID { get; set; }
        public int? containerTypeId { get; set; }
        public int? ChargeId { get; set; }
        public string CategoryName { get; set; }
        public string containerName { get; set; }
        public string ChargeName { get; set; }
        public DateTime? ChargeExpiryDate { get; set; }
        public decimal? BuyingAmt { get; set; }
        public decimal? ModeOfServiceID { get; set; }
        
    }
}
