using System;

namespace AppMGL.DAL.Models
{
	public class USP_GET_ENQUIRY_LIST_Result
	{
		public long? ROWNUM
		{
			get;
			set;
		}

		public int? TotalCount
		{
			get;
			set;
		}

		public decimal EnquiryID
		{
			get;
			set;
		}

		public string EnquiryNo
		{
			get;
			set;
		}

		public DateTime? EnquiryDate
		{
			get;
			set;
		}

		public DateTime? ShipmentMoveDate
		{
			get;
			set;
		}

		public int? NoOfContainer
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string ContactPerson
		{
			get;
			set;
		}

		public string Commodity
		{
			get;
			set;
		}

		public string Remarks
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}
	}
}
