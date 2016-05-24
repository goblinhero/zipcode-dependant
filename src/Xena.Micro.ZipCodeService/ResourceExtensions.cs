using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xena.Micro.ZipCodeService
{
    public static class ResourceExtensions
    {
        public static string GetLocalizedCountryName(string countryName)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var localizedKey = string.Format("Localized_Country_{0}", countryName);
            return Countries.ResourceManager.GetString(localizedKey, currentCulture);
        }
    }
}
