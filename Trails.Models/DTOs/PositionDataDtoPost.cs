using System.ComponentModel.DataAnnotations.Schema;

namespace Trails.Models.DTOs
{
    public class PositionDataDtoPost
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Speed { get; set; }

        [ForeignKey(nameof(Device))]
        public string DeviceId { get; set; }
    }
}
