using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class LG_CONTACT_WORK_TYPE
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public decimal CwtId
		{
			get;
			set;
		}

		public string CwtName
		{
			get;
			set;
		}

		public bool CwtStatus
		{
			get;
			set;
		}

		public DateTime CwtCreatedTs
		{
			get;
			set;
		}

		public decimal CwtCreatedBy
		{
			get;
			set;
		}

		public DateTime? CwtUpdatedTs
		{
			get;
			set;
		}

		public decimal? CwtUpdatedBy
		{
			get;
			set;
		}

		public virtual ICollection<LG_CONTACT> LG_CONTACT
		{
			get;
			set;
		}

		public LG_CONTACT_WORK_TYPE()
		{
			LG_CONTACT = new HashSet<LG_CONTACT>();
		}
	}
}
