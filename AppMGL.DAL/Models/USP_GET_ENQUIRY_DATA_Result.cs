using System;

namespace AppMGL.DAL.Models
{
	public class USP_GET_ENQUIRY_DATA_Result
	{
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

		public DateTime? ExpectedDate
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
	}
}
