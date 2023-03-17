using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.ServiceModels
{
    public class MapRadiusQueryRequest
    {
        public string? IpAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? FreeformText { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        public int DistanceInMiles { get; set; }
    }
}
