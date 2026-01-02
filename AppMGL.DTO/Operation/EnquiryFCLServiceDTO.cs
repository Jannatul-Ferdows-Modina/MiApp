using System;


namespace AppMGL.DTO.Operation
{
   public  class EnquiryFCLServiceDTO
    {
        public decimal ServiceRefAllID { get; set; }
        public decimal ServiceRefID { get; set; }
        public int RefID { get; set; }
        public string RefType { get; set; }
        public int? ServiceRequiredID { get; set; }
        public int? NoofContainer { get; set; }
        public int? ContainerTypeID { get; set; }
        public int? Quantity { get; set; }

    }
}
