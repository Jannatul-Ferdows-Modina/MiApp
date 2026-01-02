using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int UserId
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string LoginID
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public int? fkRoleID
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public string ContactNo
		{
			get;
			set;
		}

		public string EmailID
		{
			get;
			set;
		}

		public bool? IsActive
		{
			get;
			set;
		}

		public int? CreatedBy
		{
			get;
			set;
		}

		public DateTime? CreatedOn
		{
			get;
			set;
		}

		public int? ModifiedBy
		{
			get;
			set;
		}

		public DateTime? ModifiedOn
		{
			get;
			set;
		}

		public bool? IsDeleted
		{
			get;
			set;
		}

		public bool? Flag
		{
			get;
			set;
		}

		public int? ClientID
		{
			get;
			set;
		}

		public int? DepartmentID
		{
			get;
			set;
		}
	}
}
