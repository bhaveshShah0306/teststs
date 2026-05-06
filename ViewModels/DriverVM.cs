namespace GoChauffeurWebApi.ViewModels
{
    public class DriverVM
    {
        public int DriverId { get; set; }
        public string? DriverRecID { get; set; }
        public string? DriverName { get; set; }
        public DateTime? DOB { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string? Address { get; set; }

        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? PermenentAddress { get; set; }
        public string? PermenentState { get; set; }
        public string? PermenentCity { get; set; }
        public string? PermenentCountry { get; set; }
        public string? PermenentPostalCode { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int? StatusId { get; set; }
        public string? AadharNumber { get; set; }
        public string? AadharNumberFrontImage { get; set; }
        public string? AadharNumberBackImage { get; set; }
        public string? PANNumber { get; set; }
        public string? PANNumberFrontImage { get; set; }
        public string? PANNumberBackImage { get; set; }
        public string? Licence { get; set; }
        public string? LicenceFrontImage { get; set; }
        public string? LicenceBackImage { get; set; }
        public string? Image { get; set; }
        public string? Qualification { get; set; }
        public string? Status { get; set; }
        public bool IsVerified { get; set; }
        public string? VehicleTypeIds { get; set; }
        public List<VehicletypeVM>? VechileTypeList { get; set; }
        public string? TransmissionTypeId { get; set; }
        public List<TransmissionTypeVM>? TransmissionType { get; set; }
        public string? TransmissionTypeName { get; set; }
        public int? Experiance { get; set; }
        public int? RatingId { get; set; }
        public string? CurrentLocation { get; set; }
        public string? VerificationStatusIds { get; set; }
        public List<VerificationStatusVMS>? verificationStatuses{ get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? NoOfTripsCount { get; set; }
        public int? ReferedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public string? ReferCode { get; set; }
        public string? Licencevalidddate { get; set; }

        public string? BankName { get; set; }
        public string? AccountNo { get; set; }
        public string? IFSCCODE { get; set; }
        public string? AccountHolderName { get; set; }
        public string? Branch { get; set; }
        public string? BloodGroup { get; set; }
        /// toob
        public Boolean? IsDriverActive { get; set; }
        public Boolean? IsBlock { get; set; }
        public Boolean? IsPaymentDone { get; set; }
        public Decimal? TodaysEarning { get; set; }

        public Decimal? TodaysLogInHrs { get; set; }

        public string? reasonforoffline { get; set; }
    }
    public class VehicletypeVM
    {
        public string? VehicleTypeName { get; set; }
    }

    public class VerificationStatusVMS
    {
        public int VerificationStatusId { get; set; }
        public string? VerificationStatusName { get; set; }
    }


}
