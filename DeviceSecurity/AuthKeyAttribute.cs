using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trails.Models.Context;
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

            var positionDataFromRequestBody = context.ActionArguments["positionDataDtoPost"] as PositionDataDtoPost;
            var deviceIdFromRequestBody = positionDataFromRequestBody.DeviceId;

            var dbContext = context.HttpContext
                .RequestServices
                .GetService(typeof(TrailsContext)) as TrailsContext;

            var deviceFromDb = await dbContext.Devices.FindAsync(deviceIdFromRequestBody);
            var extractedAccessKey = deviceFromDb.AccessKey;
            var extractedSalt = deviceFromDb.Salt;

            var decryptedKey = CryptoGenerator.Decrypt(extractedAuthKey);
            var restoredAccessKey = HashGenerator.ComputeHash(Encoding.UTF8.GetBytes(decryptedKey),
                Encoding.UTF8.GetBytes(extractedSalt));

            if (extractedAccessKey != restoredAccessKey)
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
    }
}