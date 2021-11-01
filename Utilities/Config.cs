namespace Utilities
{
    public static class Config
    {
        //TODO: Change this values to secrets!
        public const string CONNECTION_STRING = "Server=.;Integrated Security = true; Database=TrailsDB";
        public const string sqlCommandStr = "SELECT AccessKey,Salt FROM Devices WHERE DeviceId = @deviceId";
        public const string EncryptionKey = "thisisyourcode!!";
 
    }
}
