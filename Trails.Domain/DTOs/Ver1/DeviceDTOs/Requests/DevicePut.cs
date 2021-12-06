using System.ComponentModel.DataAnnotations;
using Trails.Common;

namespace Trails.Domain.DTOs.Ver1.DeviceDTOs.Requests
{
    public class DevicePut
    {
        [Required]
        [MinLength(DeviceValidationConstants.DEVICE_NAME_MIN_LENGTH)]
        [MaxLength(DeviceValidationConstants.DEVICE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(DeviceValidationConstants.DEVICE_SIM_CARD_NUMBER_EXPRESSION)]
        public string SimCardNumber { get; set; }

        [Required]
        [MinLength(DeviceValidationConstants.DEVICE_DESCRIPTION_MIN_LENGTH)]
        [MaxLength(DeviceValidationConstants.DEVICE_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        //TODO: Get password hashing from identity
        [Required]
        public string PasswordHash { get; set; }
    }
}
