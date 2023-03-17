using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Service.AzureMapsModels;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.Domain.Service
{
    public interface IAzureMapsQueryService
    {
        AddressSearchResponse<SearchAddressStructuredResponse> GetAddressSearch(AddressSearchRequest request);

        AddressSearchResponse<SearchAddressResponse> GetPartialAddressSearch(AddressSearchRequest request);

        AddressSearchResponse<SearchAddressResponse> GetSimpleAddressSearch(string addressQuery, string ipAddress, bool skipIpGeolocation = false);
    }
}
