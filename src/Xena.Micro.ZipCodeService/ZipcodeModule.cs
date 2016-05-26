using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;

namespace Xena.Micro.ZipCodeService
{
    public class ZipcodeModule : NancyModule
    {
        private ZipcodeRepository _repository = new ZipcodeRepository();
        public ZipcodeModule()
        {
            Get["/"] = p => GetService();
        }

        private async Task<object> GetService()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            var host = environmentVariables["ZIPCODE_SERVICE_HOST"];
            var port = environmentVariables["ZIPCODE_SERVICE_PORT"];
            try
            {
                using (var client = new HttpClient())
                {
                    var urlCalled = $"http://{host}:{port}/DK/Zip/9000";
                    //return new
                    //{
                    //    host,
                    //    port,
                    //    urlCalled, 
                    //    variables = environmentVariables
                    //};
                    var result = await client.GetAsync(urlCalled);
                    if (!result.IsSuccessStatusCode)
                    {
                        return new
                        {
                            error= $"something went odd calling {urlCalled}:{result.ReasonPhrase}",
                            variables=environmentVariables
                        };
                    }
                    var zip = await result.Content.ReadAsStringAsync();
                    return zip;
                }
            }
            catch (Exception ex)
            {
                return $"Something broke: {ex.Message} Stack:{ex.StackTrace}";              
            }
        }

    }
}