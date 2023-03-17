using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public  class SearchAddressResponse
    {
        /// <summary>
        /// Results array
        /// </summary>
        [JsonPropertyName("results")]
        public SearchAddressResult[] Results { get; set; }
    }
}
