using System;

namespace AppMGL.DTO.Operation
{

    public class CityRDTO
    {
        public int CityId { get; set; }
        public int? fkStateId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }

    }

    
    public class UserRDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public int? fkRoleID { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }
        public int? ClientID { get; set; }
        public int? DepartmentID { get; set; }


    }


    public class StateRDTO
    {

        public int StateId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? fkCountryId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }
        public string Abb { get; set; }

        public CountryRDTO Country { get; set; }

    }

    public class CountryRDTO
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ContinentID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }

        public ContinentRDTO Continent { get; set; }
    }

    public class ContinentRDTO
    {
        public int ContinentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Flag { get; set; }
    }                   

    
}