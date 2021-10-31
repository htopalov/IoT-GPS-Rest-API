using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trails.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }

        [Required]
        [MaxLength(15)]
        public string Imei { get; set; }

        [Required]
        [MaxLength(15)]
        public string SimCardNumber { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public string AccessKey { get; set; }

        public ICollection<PositionData> PositionData { get; set; }
    }
}
