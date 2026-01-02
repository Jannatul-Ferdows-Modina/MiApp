using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_City
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int CityId
		{
			get;
			set;
		}

		public int? fkStateId
		{
			get;
			set;
		}

		public string Name
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

		public virtual ICollection<SIPL_Contact> SIPL_Contact
		{
			get;
			set;
		}

		public SIPL_City()
		{
			SIPL_Contact = new HashSet<SIPL_Contact>();
		}
	}
}
