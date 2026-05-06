public class VehicleTypeListVM
{
    public int? VehicleTypeId { get; set; }
    public string? VehicleTypeName { get; set; }
    public string? VehicleIcon { get; set; }
    public List<TransmissionTypelistVM> Transmissions { get; set; }
}

public class TransmissionTypelistVM
{
    public int TransmissionTypeId { get; set; }
    public string? TransmissionName { get; set; }
    public string? TransmissionIcon { get; set; }
}
