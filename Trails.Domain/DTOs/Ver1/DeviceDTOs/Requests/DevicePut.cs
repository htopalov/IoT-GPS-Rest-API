using System.ComponentModel.DataAnnotations;

namespace Trails.Domain.DTOs.Ver1.DeviceDTOs.Requests
{
    public class DevicePut
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string SimCardNumber { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
