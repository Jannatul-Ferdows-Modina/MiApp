using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMGL.DAL.Models
{
	public class SIPL_ContainerType
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int ContainerTypeId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
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

		public double? InsideLengthInch
		{
			get;
			set;
		}

		public double? InsideLengthMeter
		{
			get;
			set;
		}

		public double? InsideWidthInch
		{
			get;
			set;
		}

		public double? InsideWidthMeter
		{
			get;
			set;
		}

		public double? InsideHeightInch
		{
			get;
			set;
		}

		public double? InsideHeightMeter
		{
			get;
			set;
		}

		public double? DoorHeightinch
		{
			get;
			set;
		}

		public double? DoorHeightM
		{
			get;
			set;
		}

		public double? ContainerCapacitycft
		{
			get;
			set;
		}

		public double? ContainerCapacityCM
		{
			get;
			set;
		}

		public double? TareWeightLbs
		{
			get;
			set;
		}

		public double? TareWeightKGS
		{
			get;
			set;
		}

		public double? MaxiCargoLbs
		{
			get;
			set;
		}

		public double? MaxiCargoKGS
		{
			get;
			set;
		}

		public double? Doorwidthinch
		{
			get;
			set;
		}

		public double? DoorwidthM
		{
			get;
			set;
		}

		public decimal? CCID
		{
			get;
			set;
		}
	}
}
