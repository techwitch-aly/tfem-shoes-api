using System.Text.Json.Serialization;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public class SearchResultViewport
    {
        /// <summary>
        /// A location represented as a latitude and longitude.
        /// </summary>
        [JsonPropertyName("btmRightPoint")]
        public CoordinateAbbreviated BtmRightPoint { get; set; }

        /// <summary>
        /// A location represented as a latitude and longitude.
        /// </summary>
        [JsonPropertyName("topLeftPoint")]
        public CoordinateAbbreviated TopLeftPoint { get; set; }
    }
}