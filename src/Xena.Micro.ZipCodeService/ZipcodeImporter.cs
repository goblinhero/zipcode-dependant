using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace Xena.Micro.ZipCodeService
{
    public class ZipcodeImporter
    {
        public bool TryImport(out IEnumerable<string> errors)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var repo = new ZipcodeRepository();

            using (Stream stream = assembly.GetManifestResourceStream("Xena.Micro.ZipCodeService.no-postnr.csv"))
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
            {
                var csvReader = new CsvReader(reader, false, ';');
                var country = new Country("NO");
                repo.AddCountry(country);
                while (csvReader.ReadNextRecord())
                {
                    string zip = csvReader[0];
                    string city = csvReader[1];
                    string additionalInfo = string.Format("{0} {1}", csvReader[2], csvReader[3]);
                    repo.TryAdd(new Zipcode(zip, city, additionalInfo, country));
                }
            }
            using (Stream stream = assembly.GetManifestResourceStream("Xena.Micro.ZipCodeService.dk-postnr.csv"))
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
            {
                var csvReader = new CsvReader(reader, false, ';');
                var country = new Country("DK");
                repo.AddCountry(country);
                while (csvReader.ReadNextRecord())
                {
                    string zip = csvReader[0];
                    string city = csvReader[1];
                    string additionalInfo = csvReader[2];
                    repo.TryAdd(new Zipcode(zip, city, additionalInfo, country));
                }
            }
            errors = new string[0];
            return true;
        }
    }

    public class ZipcodeRepository
    {
        private static Dictionary<Country, IDictionary<string, Zipcode>> _countrySpecificZipcodes = new Dictionary<Country, IDictionary<string, Zipcode>>();
        private static IList<Zipcode> _allZipcodes = new List<Zipcode>();


        public void AddCountry(Country country)
        {
            _countrySpecificZipcodes.Add(country, new Dictionary<string, Zipcode>());
        }
        public bool TryAdd(Zipcode zipcode)
        {
            _allZipcodes.Add(zipcode);
            try
            {
                _countrySpecificZipcodes[zipcode.Country].Add(zipcode.Zip, zipcode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGetZip(string zipcode, out Zipcode zip, Country country = null)
        {
            if (country != null)
            {
                IDictionary<string, Zipcode> zipcodes;
                if (!_countrySpecificZipcodes.TryGetValue(country, out zipcodes))
                {
                    zip = null;
                    return false;
                }
                return zipcodes.TryGetValue(zipcode, out zip);
            }
            foreach (var countryZips in _countrySpecificZipcodes.Values)
            {
                if (countryZips.TryGetValue(zipcode, out zip))
                    return true;
            }
            zip = null;
            return false;
        }

        public bool GetZipByCountry(Country country,out IList<Zipcode> zipCodes, out IEnumerable<string> errors, int page = 0, int pageSize = 10)
        {
            if (country == null)
            {
                errors = new[] {"Country must be supplied"};
                zipCodes = new List<Zipcode>();
                return false;
            }
            IDictionary<string, Zipcode> dict;
            if (!_countrySpecificZipcodes.TryGetValue(country, out dict))
            {
                errors = new[] { "Country does not have any zip codes (yet)" };
                zipCodes = new List<Zipcode>();
                return false;
            }
            zipCodes = dict.Values.Skip(page*pageSize).Take(pageSize).ToList();
            errors = new string[0];
            return true;
        }
    }
}