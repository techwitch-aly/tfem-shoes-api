namespace tfemshoes.API.Models
{
    /// <summary>
    /// Represents the data required to send a geographic radius query to the map service
    /// </summary>
    public class RadiusQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public string? FreeformText { get; set; }

        public string? StreetAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? DistanceInMiles { get; set; }
    }
}
