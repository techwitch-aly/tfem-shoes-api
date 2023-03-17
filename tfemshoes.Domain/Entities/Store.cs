using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tfemshoes.Domain.Entities
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StoreId { get; set; }

        [MaxLength(150)]
        public string StoreName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string? Address2 { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(20)]
        public string ZipCode { get; set; }

        [MaxLength(5)]
        public string State { get; set; }

        [MaxLength(30)]
        public string Country { get; set; }

        public bool Active { get; set; }

        public bool Verified { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [Column(TypeName = "geography")]
        public Point? Location { get; set; }
    }
}
