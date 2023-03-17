using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public class SearchAddressStructuredResponse
    {
        /// <summary>
        /// Results array
        /// </summary>
        [JsonPropertyName("results")]
        public SearchAddressStructuredResult[] Results { get; set; }
    }
}
