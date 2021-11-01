using System.ComponentModel.DataAnnotations;

namespace Trails.Models.DTOs
{
    public class DeviceDtoPut
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string SimCardNumber { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
