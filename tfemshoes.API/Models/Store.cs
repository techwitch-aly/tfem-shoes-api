using System.ComponentModel.DataAnnotations;

namespace tfemshoes.API.Models
{
    /// <summary>
    /// Model for the front-end to represent stores
    /// </summary>
    public class Store
    {
        /// <summary>
        /// The name of the Store to display
        /// </summary>
        [Required]
        public string StoreName { get; set; }

        /// <summary>
        /// The street address of the store
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// The suite number or other secondary address information
        /// </summary>
        public string? Address2 { get; set; }

        /// <summary>
        /// The city where the store is located
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// The ZIP/Postal Code of the store
        /// </summary>
        [Required]
        public string ZipCode { get; set; }

        /// <summary>
        /// The state of the store
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// The country of the store
        /// </summary>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Latitude value of the store pin on Google Maps
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Longitude value of the store pin on Google Maps
        /// </summary>
        public decimal? Longitude { get; set; }
    }
}
