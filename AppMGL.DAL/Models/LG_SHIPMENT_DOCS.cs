using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_SHIPMENT_DOCS
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal SdId
		{
			get;
			set;
		}

		public int DocumentCommonID
		{
			get;
			set;
		}

		public string SdDocName
		{
			get;
			set;
		}

		public string sdDocType
		{
			get;
			set;
		}

		public DateTime SdCreatedTs
		{
			get;
			set;
		}

		public decimal SdCreatedBy
		{
			get;
			set;
		}

		public DateTime? SdUpdatedTs
		{
			get;
			set;
		}

		public decimal? SdUpdatedBy
		{
			get;
			set;
		}

		public bool IsSysGenerated
		{
			get;
			set;
		}
	}
}
