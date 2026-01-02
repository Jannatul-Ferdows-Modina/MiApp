using System;

namespace AppMGL.DTO.Operation
{
    public class ContractRateModalDTO
    {
        public decimal ContractID { get; set; }
        public string ContractNo { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int IsValid { get; set; }
        public System.DateTime ContractDate { get; set; }
        public int MQCTYPE { get; set; }
        public double MQC { get; set; }
        public Nullable<int> ContractType { get; set; }
        public string Remark { get; set; }
        public string Attachments { get; set; }

    }
}