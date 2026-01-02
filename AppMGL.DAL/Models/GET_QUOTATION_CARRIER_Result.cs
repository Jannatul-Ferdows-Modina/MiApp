using System;

namespace AppMGL.DAL.Models
{
	public class GET_QUOTATION_CARRIER_Result
	{
		public int? Days
		{
			get;
			set;
		}

		public string CompanyName
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

		public bool? Selected
		{
			get;
			set;
		}

		public decimal? fkQuotationID
		{
			get;
			set;
		}

		public int? fkCarrierID
		{
			get;
			set;
		}

		public decimal? QuotationCarrierID
		{
			get;
			set;
		}
	}
}
