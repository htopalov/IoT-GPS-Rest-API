using System.ComponentModel.DataAnnotations.Schema;
using Trails.Domain.Models;

namespace Trails.Domain.DTOs.Ver1.PositionDataDTOs.Requests
{
    public class PositionDataPost
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Speed { get; set; }

        [ForeignKey(nameof(Device))]
        public string DeviceId { get; set; }
    }
}
