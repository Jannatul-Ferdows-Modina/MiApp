using System;

namespace AppMGL.DTO.DataManagement
{
    public class ContainerTypeDTO
    {

        public int? ContainerTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool Flag { get; set; }
        public double? InsideLengthInch { get; set; }
        public double? InsideLengthMeter { get; set; }
        public double? InsideWidthInch { get; set; }
        public double? InsideWidthMeter { get; set; }
        public double? InsideHeightInch { get; set; }
        public double? InsideHeightMeter { get; set; }
        public double? DoorHeightInch { get; set; }
        public double? DoorHeightM { get; set; }
        public double? ContainerCapacitycft { get; set; }
        public double? ContainerCapacityCM { get; set; }
        public double? TareWeightLbs { get; set; }
        public double? TareWeightKGS { get; set; }
        public double? MaxiCargoLbs { get; set; }
        public double? MaxiCargoKGS { get; set; }
        public double? Doorwidthinch { get; set; }
        public double? DoorwidthM { get; set; }
        public decimal? CCID { get; set; }
        public decimal? ContainerCategoryID { get; set; }
    }
}