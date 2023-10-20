using IceCreamCompanySync.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace IceCreamCompanySync.HttpHandler
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache cache;
        private readonly string _apiCompanyId;
        private readonly string _apiUserId;
        private readonly string _apiUserSecret;
        private readonly string _authenticateUrl;
        public TokenService(IMemoryCache cache, IConfiguration config)
        {
            this.cache = cache;
            _apiCompanyId = config["Authenriacion:apiCompanyId"];
            _apiUserId = config["Authenriacion:apiUserId"];
            _apiUserSecret = config["Authenriacion:apiUserSecret"];
            _authenticateUrl = $"{config["UniLoaderAPIUrl"]}/authenticate";
        }

        public string FetchToken()
        {
            string token = string.Empty;

            //check if tocken is currently cached
            if (!cache.TryGetValue("TOKEN", out token))
            {
                var tokenmodel = this.GetTokenFromApi();
                cache.Set("TOKEN", tokenmodel);

                token = tokenmodel;
            }

            //check if token expired and refresh it 
            if (!IsValid(token))
            {
                var tokenmodel = this.GetTokenFromApi();
                cache.Set("TOKEN", tokenmodel);

                token = tokenmodel;
            }

            return token;
        }

        public string GetNewToken()
        {
            string token = this.GetTokenFromApi();
            cache.Set("TOKEN", token);

            return token;
        }

        private string GetTokenFromApi()
        {
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var payload = new Dictionary<string, string>
                {
                    { "apiCompanyId" ,_apiCompanyId},
                    { "apiUserId", _apiUserId},
                    { "apiUserSecret", _apiUserSecret}
               };
                string strPayload = JsonConvert.SerializeObject(payload);
                var webRequest = new HttpRequestMessage(HttpMethod.Post, _authenticateUrl)
                {
                    Content = new StringContent(strPayload, Encoding.UTF8, "application/json")
                };

                var response = client.Send(webRequest);
                using var reader = new StreamReader(response.Content.ReadAsStream());
                return reader.ReadToEnd().Replace("\"", "");
            }
        }

        private bool IsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }
    }
}
