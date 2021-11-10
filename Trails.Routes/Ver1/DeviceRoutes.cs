namespace Trails.Routes.Ver1
{
    public static class DeviceRoutes
    {
        public const string GetAll = ApiRoutes.Base + "/devices";

        public const string Create = ApiRoutes.Base + "/devices";

        public const string Get = ApiRoutes.Base + "/devices/{deviceId}";

        public const string Update = ApiRoutes.Base + "/devices/{deviceId}";

        public const string Delete = ApiRoutes.Base + "/devices/{deviceId}";

        public const string GetAllDeviceData = ApiRoutes.Base + "/device_positions/{deviceId}";

    }
}
