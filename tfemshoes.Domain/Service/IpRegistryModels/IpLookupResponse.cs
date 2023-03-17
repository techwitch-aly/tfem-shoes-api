using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.IpRegistryModels
{
    public  class IpLookupResponse
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("location")]
        public IpLocation Location { get; set; }
    }
}
