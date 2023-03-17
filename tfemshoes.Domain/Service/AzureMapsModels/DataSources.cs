using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public  class DataSources
    {
        /// <summary>
        /// Information about the geometric shape of the result. Only present if type == Geography.
        /// </summary>
        [JsonPropertyName("geometry")]
        public DataSourcesGeometry Geometry { get; set; }
    }
}
