using System.ComponentModel.DataAnnotations;

namespace AppMGL.DAL.Models
{
	public class LG_VW_Contract
	{
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

		public string Carrier
		{
			get;
			set;
		}

		public string StartDate
		{
			get;
			set;
		}

		public string EndDate
		{
			get;
			set;
		}

		public int IsValid
		{
			get;
			set;
		}

		public string ContractDate
		{
			get;
			set;
		}

		public string MQC
		{
			get;
			set;
		}

		public string ContractType
		{
			get;
			set;
		}

		public string curdate
		{
			get;
			set;
		}

		public int? diff
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public int CarrierID
		{
			get;
			set;
		}

		public int MQCTYPE
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

		public double mqcvalue
		{
			get;
			set;
		}

		public string ContractTypeVal
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}
        public int? TotalCount
        {
            get;
            set;
        }
        public string TradeLane { get; set; }
        public string SCTermNo { get; set; }

        public string AmedementNo { get; set; }

        public string AmedementDate { get; set; }

    }
}
