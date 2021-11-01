using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Trails.Models.DTOs;
using Utilities;

namespace DeviceSecurity
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class AuthKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string AUTHKEYNAME = "AuthKey";
        private const int encryptedKeyLength = 64;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AUTHKEYNAME, out var extractedAuthKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = Messages.KeyNotProvided
                };
                return;
            }

            var positionDataFromRequestBody = context.ActionArguments["positionDataDtoPost"] as PositionDataDtoPost;
            var deviceIdFromRequestBody = positionDataFromRequestBody.DeviceId;
            var extractedCredentials = await GetCredentialsFromDb(deviceIdFromRequestBody);


            //check if length of encrypted keys is the same (it has to be)
            //if not doesn't even try to decrypt it
            if (extractedAuthKey.ToString().Length != encryptedKeyLength)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = Messages.KeyNotValid
                };
                return;
            }

            var decryptedKey = CryptoGenerator.Decrypt(extractedAuthKey);
            var extractedSalt = extractedCredentials["Salt"];
            var restoredAccessKey = HashGenerator.ComputeHash(Encoding.UTF8.GetBytes(decryptedKey), Encoding.UTF8.GetBytes(extractedSalt));


            if (extractedCredentials["AccessKey"] != restoredAccessKey)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = Messages.KeyNotValid
                };
                return;
            }

            await next();
        }



        //using ado. net to retrieve hashed key and salt for specific device and return it
        private async Task<Dictionary<string,string>> GetCredentialsFromDb(string deviceIdFromRequestBody)
        {
            var credentials = new Dictionary<string, string>();

            await using (var connection = new SqlConnection(Config.CONNECTION_STRING))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(Config.sqlCommandStr, connection);
                command.Parameters.AddWithValue("@deviceId", deviceIdFromRequestBody);
                var sqlReader = await command.ExecuteReaderAsync();
                await using (sqlReader)
                {
                    while (await sqlReader.ReadAsync())
                    {
                        credentials.Add("AccessKey", sqlReader.GetString(0));
                        credentials.Add("Salt", sqlReader.GetString(1));
                    }
                }
            }

            return credentials;
        }
    }
}
