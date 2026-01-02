using System;


namespace AppMGL.DTO.Operation
{
    public class EnquiryAIRServiceDTO
    {
        public decimal? ServiceRefAllID { get; set; }
        public decimal? ServiceRefID { get; set; }
        public int? RefID { get; set; }
        public string RefType { get; set; }
        public int? ServiceRequiredID { get; set; }
        public int? PieceCount { get; set; }
        public decimal? Weight { get; set; }
        public decimal? TotalWt { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }
    }
}
