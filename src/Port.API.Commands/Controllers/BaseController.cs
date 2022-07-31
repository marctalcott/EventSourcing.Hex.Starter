using System;
using System.Linq;
using Domain.ES.EventStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Port.API.Commands.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        protected EventUserInfo GetUserInfoFromApiKey(IConfiguration configuration)
        {
            var apiKey = HttpContext.Request.Headers["ApiKey"];

            try
            {
                var keys = configuration.GetSection("ApiKeys").GetChildren();
                return new EventUserInfo(keys.Single(x => x.Key == apiKey).Value, true);
            }
            catch (Exception)
            {
                string errorMessage = "Invalid API key.";
                throw new ApplicationException(errorMessage);
            }
        }

        protected EventUserInfo GetUserInfo()
        {
            var name = GetNameFromToken();
            var appMetadata = GetAppMetadataFromToken();

            return new EventUserInfo(name, appMetadata);
        }

        private AppMetaData GetAppMetadataFromToken()
        {
            string appMetadataType = "app_metadata";
            var user = HttpContext.User;
            var appMetaData = user.Claims.SingleOrDefault(x =>
                x.Type == appMetadataType);
            return appMetaData == null
                ? new AppMetaData(Guid.Empty.ToString(), "")
                : JsonConvert.DeserializeObject<AppMetaData>(appMetaData?.Value);
        }

        private string GetNameFromToken()
        {
            string claimsId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var user = HttpContext.User;
            string nameIdentifier = user.Claims.SingleOrDefault(x =>
                x.Type == claimsId)?.Value;


            //TODO: This should throw an exception but doing so breaks the creation of accounts
            if (string.IsNullOrWhiteSpace(nameIdentifier))
            {
                // throw new ApplicationException("We expected a nameIdentifer in the token and it was not there.");
                nameIdentifier = "todo|anonymous";
            }

            return nameIdentifier;
        }
    }
    
    public class AuthProviderUserModel
    {
        /// <summary>
        /// Id at Auth ServiceProvider
        /// </summary>
        public string UserId { get; }
        public AppMetaData AppMetadata { get; }

        public AuthProviderUserModel(AppMetaData appMetadata, string userId = "Anonymous")
        {
            UserId = userId;
            AppMetadata = appMetadata;
        }
    }
   
}