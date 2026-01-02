using System;


namespace AppMGL.DTO.Operation
{
    public class RouteDTO
    {
        public int? RouteId { get; set; }
        public string RouteName { get; set; }
        public string OriginType { get; set; }
        public string DestinationType { get; set; }
        public int? fkOriginID { get; set; }
        public int? fkDestinationID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int? ViaID { get; set; }
        public int? Via1 { get; set; }
        public int? Via2 { get; set; }
        public string Via1Name { get; set; }
        public string Via2Name { get; set; }
        public string ViaType1 { get; set; }
        public string ViaType2 { get; set; }
        public int? TotalCount { get; set; }

    }
}

