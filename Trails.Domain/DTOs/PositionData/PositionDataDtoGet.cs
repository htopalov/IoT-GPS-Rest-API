using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trails.Domain.DTOs.PositionData
{
    public class PositionDataDtoGet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Speed { get; set; }

        [ForeignKey(nameof(Device))]
        public string DeviceId { get; set; }
    }
}
