namespace Trails.Common
{
    public static class DeviceValidationConstants
    {
        public const int DEVICE_ID_LENGTH = 15;

        public const int DEVICE_NAME_MIN_LENGTH = 30;

        public const int DEVICE_NAME_MAX_LENGTH = 50;

        public const string DEVICE_SIM_CARD_NUMBER_EXPRESSION = @"^\+359\d{9}$";

        public const int DEVICE_DESCRIPTION_MIN_LENGTH = 30;

        public const int DEVICE_DESCRIPTION_MAX_LENGTH = 200;
    }
}
