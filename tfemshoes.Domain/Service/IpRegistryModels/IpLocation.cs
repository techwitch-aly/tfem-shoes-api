using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.IpRegistryModels
{
    public  class IpLocation
    {
        [JsonPropertyName("country")]
        public IpCountry Country { get; set; }

        [JsonPropertyName("region")]
        public IpRegion Region { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("postal")]
        public string PostalCode { get; set; }
    }
}
