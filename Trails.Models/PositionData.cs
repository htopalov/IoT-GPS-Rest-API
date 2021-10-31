using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trails.Models
{
    public class PositionData
    {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Speed { get; set; }

        [ForeignKey(nameof(Device))]
        public int DeviceId { get; set; }

        public Device Device { get; set; }
    }
}
