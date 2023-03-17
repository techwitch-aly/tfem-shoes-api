﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.AzureMapsModels
{
    public class SearchAddressResult
    {
        /// <summary>
        /// The address of the result
        /// </summary>
        [JsonPropertyName("address")]
        public SearchResultAddress Address { get; set; }

        /// <summary>
        /// Optional section. Reference ids for use with the Get Search Polygon API.
        /// </summary>
        [JsonPropertyName("dataSources")]
        public DataSources DataSources { get; set; }

        /// <summary>
        /// Entry Points array
        /// </summary>
        [JsonPropertyName("entryPoints")]
        public SearchResultEntryPoint[] EntryPoints { get; set; }

        /// <summary>
        /// Id property
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// A location represented as a latitude and longitude.
        /// </summary>
        [JsonPropertyName("position")]
        public CoordinateAbbreviated Position { get; set; }

        /// <summary>
        /// The value within a result set to indicate the relative matching score between results. You can use this to determine that result x is twice as likely to be as relevant as result y if the value of x is 2x the value of y. The values vary between queries and is only meant as a relative value for one result set.
        /// </summary>
        [JsonPropertyName("score")]
        public double Score { get; set; }

        /// <summary>
        /// One of:
        /// POI, 
        /// Street, 
        /// Geography, 
        /// Point Address, 
        /// Address Range, 
        /// Cross Street
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The viewport that covers the result represented by the top-left and bottom-right coordinates of the viewport.
        /// </summary>
        [JsonPropertyName("viewport")]
        public SearchResultViewport Viewport { get; set; }

        [JsonPropertyName("dist")]
        public decimal Dist { get; set; }
    }
}
