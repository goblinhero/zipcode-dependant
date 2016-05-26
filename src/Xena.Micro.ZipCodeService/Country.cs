using System;
using System.Collections.Generic;

namespace Xena.Micro.ZipCodeService
{
    [Serializable]
    public class Country
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsInEU { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Country other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Country)) return false;
            return Equals((Country)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}