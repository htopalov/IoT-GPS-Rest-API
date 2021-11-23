using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trails.Common;

namespace Trails.Domain.Models
{
    public class Device
    {
        public Device()
        {
            this.PositionData = new HashSet<PositionData>();
        }

        [Key]
        [MaxLength(DeviceValidationConstants.DEVICE_ID_MAX_LENGTH)]
        public string DeviceId { get; set; }

        [Required]
        [MaxLength(DeviceValidationConstants.DEVICE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DeviceValidationConstants.DEVICE_SIM_CARD_NUMBER_MAX_LENGTH)]
        public string SimCardNumber { get; set; }

        [MaxLength(DeviceValidationConstants.DEVICE_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<PositionData> PositionData { get; set; }
    }
}
