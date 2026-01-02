using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_Country
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int CountryId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
        public string CountryCode
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

		public int? ContinentID
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

		public virtual SIPL_Continent SIPL_Continent
		{
			get;
			set;
		}
	}
}
