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
            var host = Environment.GetEnvironmentVariable("zIPCODE_SERVICE_HOST");
            var port = Environment.GetEnvironmentVariable("zIPCODE_SERVICE_PORT");
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{host}:{port}/DK/Zip/9000");
                if (!result.IsSuccessStatusCode)
                {
                    return new NotAcceptableResponse();
                }
                var zip = await result.Content.ReadAsStringAsync();
                return zip;
            }
        }

    }
}