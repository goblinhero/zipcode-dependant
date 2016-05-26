namespace Xena.Micro.ZipCodeService
{
    public class Zipcode
    {
        public string Zip { get; set; }
        public string City { get; set; }
        public string AdditionalInfo { get; set; }
        public Country Country { get; set; }
    }
}