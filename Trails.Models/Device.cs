using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trails.Models
{
    public class Device
    {
        public Device()
        {
            this.PositionData = new HashSet<PositionData>();
        }

        [Key]
        [MaxLength(15)]
        public string DeviceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string SimCardNumber { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public string AccessKey { get; set; }

        public string Salt { get; set; }

        public ICollection<PositionData> PositionData { get; set; }
    }
}
