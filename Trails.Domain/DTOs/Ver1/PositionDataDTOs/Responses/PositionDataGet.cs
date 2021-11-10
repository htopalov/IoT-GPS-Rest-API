using System;

namespace Trails.Domain.DTOs.Ver1.PositionDataDTOs.Responses
{
    public class PositionDataGet
    {
        public Guid DataId { get; set; }

        public string Timestamp { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Speed { get; set; }

        public string DeviceId { get; set; }
    }
}
