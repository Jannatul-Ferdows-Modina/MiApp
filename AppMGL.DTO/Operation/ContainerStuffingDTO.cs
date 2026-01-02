using System;
using AppMGL.DTO.DataManagement;
using System.Collections.Generic;

namespace AppMGL.DTO.Operation
{
    public class ContainerStuffingDTO
    {
        public int? StuffingId { get; set; }
        public string StuffingNo { get; set; }
        public string StuffingDescription { get; set; }
        public int? ContainerId { get; set; }
        public string ContainerNo { get; set; }
        public string ContainerDescription { get; set; }
        public int? SiteId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UserId { get; set; }
        public int? TotalCount { get; set; }
        public string CreatedBy { get; set; }
        public string QuotationId { get; set; }
        public QuotationListDTO[] QuotationDetail { get; set; }
    }
    public class QuotationDetail
    {
        public string QuotationId { get; set; }
        public string QuotationNo { get; set; }
    }
    public class ContainerBookedDTO
    {
        public int? BookedId { get; set; }
        public string BookedNo { get; set; }
        public string BookingDescription { get; set; }
        public string ContainerId { get; set; }
        public int? SiteId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UserId { get; set; }
        public int? TotalCount { get; set; }
        public string CreatedBy { get; set; }
        public ContainerDetail[] ContainerDetail { get; set; }
    }
    public class ContainerDetail
    {
        public string ContainerId { get; set; }
        public string ContainerNo { get; set; }
    }
}
