namespace GoChauffeurWebApi.Models
{
    public class Outstationinsurence
    {
        public int OutstationinsurenceId { get; set; }

        public int onewayPrice  { get; set; }
        public int Onewaypercentage { get; set; }
        public int RoundtripPrice { get; set; }
        public int Roundtrippercentage { get; set; }
    }
    
}
