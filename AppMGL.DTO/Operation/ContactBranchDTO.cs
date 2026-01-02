using System;

namespace AppMGL.DTO.Operation
{
    public class ContactBranchDTO
    {
        public int ContactBranchID { get; set; }
        public int? ContactID { get; set; }
        public string BranchName { get; set; }
        public string TelNo { get; set; }
        public string Address { get; set; }
        public string CellNo { get; set; }
        public string Fax { get; set; }
        public string TaxID { get; set; }
        public int StateID { get; set; }
        public int? City { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string ZipCode { get; set; }
    }
}