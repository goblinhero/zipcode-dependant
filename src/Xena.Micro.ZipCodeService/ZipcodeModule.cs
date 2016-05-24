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
            using (var client = new HttpClient())
            {
                var req = await client.GetAsync("http://zipcode-1/DK/Zip/9000");
                if (req.IsSuccessStatusCode)
                {
                    var str = await req.Content.ReadAsStringAsync();
                    return str;
                }
            }
            return Environment.GetEnvironmentVariables();
        }

        private object GetZipcodes(string countryCode)
        {
            var country = new Country(countryCode);
            IList<Zipcode> zipCodes;
            IEnumerable<string> errors;
            if (_repository.GetZipByCountry(country, out zipCodes, out errors))
                return zipCodes;
            return new NotAcceptableResponse { ReasonPhrase = string.Join(Environment.NewLine, errors) };
        }

        private object GetZipcode(string countryCode, string zip)
        {
            var country = new Country(countryCode);
            Zipcode zipcode;
            if (_repository.TryGetZip(zip, out zipcode, country))
                return zipcode;
            return new NotFoundResponse();
        }
    }
}