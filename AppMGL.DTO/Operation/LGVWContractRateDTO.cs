using System;

namespace AppMGL.DTO.Operation
{
    public class LGVWContractRateDTO
    {
        public decimal ContractID { get; set; }
        public string ContractNo { get; set; }
        public string Carrier { get; set; }
        public string ContractDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MQC { get; set; }
        public string ContractType { get; set; }
        public string Remark { get; set; }
        public string Attachments { get; set; }
        //Added for Contrate Rate Insert
        public int IsValid { get; set; }
        public int CarrierID { get; set; }
        public int MQCTYPE { get; set; }
        public int? fkOriginID { get; set; }
        public int? fkDestinationID { get; set; }
        public double mqcvalue { get; set; }
        public string ContractTypeVal { get; set; }
        public string Status { get; set; }
        public string TradeLane { get; set; }
        public string SCTermNo { get; set; }

        public string AmedementNo { get; set; }

        public string AmedementDate { get; set; }

    }
}