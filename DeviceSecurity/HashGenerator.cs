using System;
using System.Security.Cryptography;

namespace DeviceSecurity
{
    public static class HashGenerator
    {
        public static string ComputeHash(byte[] accessKey, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(accessKey, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }
    }
}
