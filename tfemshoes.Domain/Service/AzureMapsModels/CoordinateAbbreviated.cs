using System.Text.Json.Serialization;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public class CoordinateAbbreviated
    {
        /// <summary>
        /// Latitude property
        /// </summary>
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// Longitude property
        /// </summary>
        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }
}