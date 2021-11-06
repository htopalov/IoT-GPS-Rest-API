using System.ComponentModel.DataAnnotations;

namespace Trails.Domain.DTOs.Device
{
    public class DeviceDtoPost
    {
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

        public string Password { get; set; }
    }
}
