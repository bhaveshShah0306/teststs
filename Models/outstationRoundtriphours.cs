using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GoChauffeurWebApi.Models
{
    public class OutstationRoundtriphours
    {
        [Key]
        public int HoursId { get; set; }
        public int? HoursName { get; set; }
        public int TripTypeId { get; set; }
        public int? Charges { get; set; }
        public Boolean? IsinHours { get; set; }
    }
}
