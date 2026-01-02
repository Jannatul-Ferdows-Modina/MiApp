using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_Department
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int DepartmentID
		{
			get;
			set;
		}

		public string Department
		{
			get;
			set;
		}

		public int? fkTransportModeID
		{
			get;
			set;
		}

		public int? fkOperationTypeID
		{
			get;
			set;
		}

		public string Prefix
		{
			get;
			set;
		}

		public decimal LastEnquiryNo
		{
			get;
			set;
		}

		public decimal EnquiryControlNo
		{
			get;
			set;
		}

		public int? DisplayOrder
		{
			get;
			set;
		}

		public string DeptCode
		{
			get;
			set;
		}
	}
}
