using System.Text.Json.Serialization;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public class SearchResultEntryPoint
    {
        /// <summary>
        /// A location represented as a latitude and longitude.
        /// </summary>
        [JsonPropertyName("position")]
        public CoordinateAbbreviated Position { get; set; }


        /// <summary>
        /// The type of entry point. Value can be either main or minor.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}