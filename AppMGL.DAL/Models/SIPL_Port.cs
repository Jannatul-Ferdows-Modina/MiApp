using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_Port
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int PortId
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

		public bool? IsAirLine
		{
			get;
			set;
		}

		public int? StateID
		{
			get;
			set;
		}

		public int? CityID
		{
			get;
			set;
		}
        public string port_code
        {
            get;
            set;
        }
    }
}
