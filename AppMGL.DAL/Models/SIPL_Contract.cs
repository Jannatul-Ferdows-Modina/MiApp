using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
    public class SIPL_Contract
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal ContractID
        {
            get;
            set;
        }

        public string ContractNo
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public int IsValid
        {
            get;
            set;
        }

        public int CarrierID
        {
            get;
            set;
        }

        public DateTime ContractDate
        {
            get;
            set;
        }

        public int MQCTYPE
        {
            get;
            set;
        }

        public double MQC
        {
            get;
            set;
        }

        public int? ContractType
        {
            get;
            set;
        }

        public int? fkOriginID
        {
            get;
            set;
        }

        public int? fkDestinationID
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public string Attachments
        {
            get;
            set;
        }

        public string TradeLane
        {
            get;
            set;
        }

        public string SCTermNo
        {
            get;
            set;
        }
    }
}
