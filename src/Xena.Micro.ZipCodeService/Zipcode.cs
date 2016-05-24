namespace Xena.Micro.ZipCodeService
{
    public class Zipcode
    {
        public Zipcode(string zip, string city, string additionalInfo, Country country)
        {
            Zip = zip;
            City = city;
            AdditionalInfo = additionalInfo;
            Country = country;
        }

        public string Zip { get; set; }
        public string City { get; set; }
        public string AdditionalInfo { get; set; }
        public Country Country { get; set; }
    }
}