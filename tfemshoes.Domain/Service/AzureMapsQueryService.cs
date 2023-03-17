using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using tfemshoes.Domain.Service.AzureMapsModels;
using tfemshoes.Domain.Service.IpRegistryModels;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.Domain.Service
{
    public class AzureMapsQueryService : IAzureMapsQueryService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureMapsQueryService> _logger;

        public AzureMapsQueryService(IConfiguration configuration, ILogger<AzureMapsQueryService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public AddressSearchResponse<SearchAddressStructuredResponse> GetAddressSearch(AddressSearchRequest request)
        {
            var response = new AddressSearchResponse<SearchAddressStructuredResponse>();
            string key = _configuration["AzureMapsKey"];

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://atlas.microsoft.com/search/address/structured/json");

            string baseQuery = $"?subscription-key={key}&api-version=1.0&language=en-US";

            if (!string.IsNullOrEmpty(request.Address))
            {
                string streetNumber = request.Address.Split(' ')[0];
                string streetName = request.Address.Replace($"{streetNumber} ", "");
                baseQuery += $"&streetNumber={streetNumber}";
                baseQuery += $"&streetName={streetName}";
            }

            baseQuery += $"&municipality={request.City}";
            baseQuery += $"&countrySubdivision={request.State}";
            baseQuery += $"&postalCode={request.PostalCode}";
            baseQuery += $"&countryCode={request.Country}";

            var res = client.GetAsync(baseQuery).Result;

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = res.Content.ReadAsStringAsync().Result;
                var val = JsonSerializer.Deserialize<SearchAddressStructuredResponse>(json);
                response.Success = true;
                response.ResponseModel = val;
            }

            return response;
        }

        public AddressSearchResponse<SearchAddressResponse> GetPartialAddressSearch(AddressSearchRequest request)
        {
            return GetSimpleAddressSearch(request.GetAddressString(), request.IpAddress ?? "", request.SkipIpGeolocation);
        }

        public AddressSearchResponse<SearchAddressResponse> GetSimpleAddressSearch(string addressQuery, string ipAddress, bool skipIpGeolocation)
        {
            var addressSearchResponse = new AddressSearchResponse<SearchAddressResponse>();
            string queryPrefix = "";

            // See if we can get some data from the caller's IP address
            if (!skipIpGeolocation)
            {
                _logger.LogInformation("Attempting to get more address info from IP {ipAddress}", ipAddress);
                var ipLocationData = GeolocateIpAddress(ipAddress);

                if (ipLocationData != null)
                {
                    if (ipLocationData.Location.Region != null)
                    {
                        // Try to strip the country code from the region code if we can
                        string stateCode = ipLocationData.Location.Region.Code.Replace("US-", "");
                        if (!addressQuery.Contains(stateCode))
                        {
                            addressQuery += $" {stateCode}";
                        }
                    }

                    // Set the Country limit if we can based on IP
                    if (ipLocationData?.Location?.Country?.Code != null)
                    {
                        queryPrefix += $"&countrySet={ipLocationData.Location.Country.Code}";
                    }
                }

                _logger.LogInformation("Enhanced address query with IP geolocation to {addressQuery}", addressQuery);
            }

            var apiResult = AddressSearch($"{queryPrefix}&query={HttpUtility.UrlEncode(addressQuery)}");

           if (apiResult != null)
            {
                addressSearchResponse.Success = true;
                addressSearchResponse.ResponseModel = apiResult;
            }

            return addressSearchResponse;
        }

        private SearchAddressResponse? AddressSearch(string query)
        {
            string key = _configuration["AzureMapsKey"];

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://atlas.microsoft.com/search/address/json");

            string baseQuery = $"?subscription-key={key}&api-version=1.0&language=en-US&typeahead=true";

            _logger.LogInformation("Azure Maps Lookup for base query: {addressQuery}", query);

            string fullQuery = baseQuery + query;

            var res = client.GetAsync(fullQuery).Result;

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = res.Content.ReadAsStringAsync().Result;
                var val = JsonSerializer.Deserialize<SearchAddressResponse>(json);
                return val;
            }

            return null;
        }

        private IpLookupResponse GeolocateIpAddress(string ipAddress)
        {
            string apiKey = _configuration["IpRegistryApiKey"];

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.ipregistry.co");

            if (ipAddress == "::1" || string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = "154.6.21.171";
            }

            _logger.LogInformation("Calling to ipregistry API with IP {ipAddress}", ipAddress);
            string query = $"{ipAddress}?key={apiKey}";

            var res = client.GetAsync(query).Result;

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = res.Content.ReadAsStringAsync().Result;
                var val = JsonSerializer.Deserialize<IpLookupResponse>(json);

                return val ?? new IpLookupResponse();
            }

            return new IpLookupResponse();
        }
    }
}
