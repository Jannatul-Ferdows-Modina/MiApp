using System;

namespace AppMGL.DAL.Models
{
	public class RPT_GET_QUOTATION_Result
	{
		public long? SerialNo
		{
			get;
			set;
		}

		public int? SystemRef
		{
			get;
			set;
		}

		public string MiamiRef
		{
			get;
			set;
		}

		public string BookingNo
		{
			get;
			set;
		}

		public string EnquiryNo
		{
			get;
			set;
		}

		public int? QuotationNo
		{
			get;
			set;
		}

		public DateTime? EnquiryDate
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public DateTime? ExpectedDate
		{
			get;
			set;
		}

		public string SourceCountry
		{
			get;
			set;
		}

		public string DestinationCountry
		{
			get;
			set;
		}

		public string PortOfOrigin
		{
			get;
			set;
		}

		public string PortOfDestination
		{
			get;
			set;
		}

		public string PickupRemark
		{
			get;
			set;
		}

		public decimal QuotationID
		{
			get;
			set;
		}

		public DateTime? QuotationSentDate
		{
			get;
			set;
		}
	}
}
