using System;


namespace AppMGL.DTO.Home
{
   public class DashboardDTO
    {
         public int? EYTD { get; set; }
        public int? EMTD { get; set; }
        public int? EUPQU { get; set; }
        public int? EUPQF { get; set; }

        public int? QYTD { get; set; }
        public int? QMTD { get; set; }
        public int? QPAU { get; set; }
        public int? QPAF { get; set; }
        public int? QSAU { get; set; }
        public int? QSAF { get; set; }
        public int? QSBU { get; set; }
        public int? QSBF { get; set; }

        public int? BYTD { get; set; }
        public int? BMTD { get; set; }
        public int? BTDU { get; set; }
        public int? BTDF { get; set; }
        public int? BWLCU { get; set; }
        public int? BWLCF { get; set; }
        public int? BASCU { get; set; }
        public int? BASCF { get; set; }

        public int? TPFTU { get; set; }
        public int? TPDRU { get; set; }
        public int? TPMDU { get; set; }
        public int? TCCSU { get; set; }
        public int? TPFTF { get; set; }
        public int? TPDRF { get; set; }
        public int? TPMDF { get; set; }
        public int? TCCSF { get; set; }

        public int? ActDue { get; set; }
        public int? TotAct { get; set; }
        public DateTime? LastUpdated { get; set; }    	
        public long DashboardId { get; set; }
        public long SiteId { get; set; }
        public int? BCRTSC { get; set; }
        public int? BCRTSF { get; set; }

        public int? readyforinvoice { get; set; }
        public int? pendinginvoice { get; set; }
        public int? invoicecompleted { get; set; }
        public int? AESP { get; set; }

        public int? LEYTD { get; set; }
        public int? LEMTD { get; set; }
        public int? LEUPQU { get; set; }
        public int? LEUPQF { get; set; }

    }

    public class DashboardTargetDTO
    {
        public string SitName { get; set; }
        public string TargetMonth { get; set; }
        public string TargetValue { get; set; }
        //public string ActualValue { get; set; }
        public string ActualQty { get; set; }
        public string CBM { get; set; }
        public string Inch40 { get; set; }
        public string Inch45 { get; set; }
        public string Inch20 { get; set; }
        public string DifferenceValue { get; set; }
        public string FranchiseId { get; set; }
        
    }
    public class TargetFilter
    {
        public string FrenchieId { get; set; }
        public string TargetYear { get; set; }
        
    }

}
