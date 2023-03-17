using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.ServiceModels
{
    public class AddressSearchRequest
    {
        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? IpAddress { get; set; }

        public bool SkipIpGeolocation { get; set; } = false;

        public string GetAddressString()
        {
            string addressString = string.Empty;

            if (!string.IsNullOrEmpty(Address))
            {
                addressString = $"{Address}, ";
            }

            if (!string.IsNullOrEmpty(City))
            {
                addressString += $"{City}, ";
            }

            if (!string.IsNullOrEmpty(State))
            {
                addressString += $"{State} ";
            }

            if (!string.IsNullOrEmpty(PostalCode))
            {
                addressString += $"{PostalCode}";
            }

            return addressString.Trim();
        }
    }
}
