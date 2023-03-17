using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Context;
using tfemshoes.Domain.Entities;
using tfemshoes.Domain.Service.AzureMapsModels;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class StoreEntityService : IStoreEntityService
    {
        private readonly TFemShoesContext _context;

        private readonly IAzureMapsQueryService _mapQueryService;

        private readonly GeometryFactory _geometryFactory;

        private readonly ILogger<StoreEntityService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public StoreEntityService(TFemShoesContext context, IAzureMapsQueryService mapQueryService, ILogger<StoreEntityService> logger)
        {
            _context = context;
            _mapQueryService = mapQueryService;
            _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Store> GetAllActiveStores()
        {
            return _context.Stores.Where(s => s.Active && s.Verified).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Store? GetStoreById(long id)
        {
            return _context.Stores.SingleOrDefault(s => s.StoreId == id);
        }

        public SaveStoreResponse SaveStore(SaveStoreRequest request)
        {
            var response = new SaveStoreResponse();

            #region Validate
            #endregion

            #region Lookup Coordinates from Address
            var searchRequest = new AddressSearchRequest()
            {
                Address = request.Address,
                City = request.City,
                State = request.State,
                PostalCode = request.ZipCode,
                Country = request.Country,
                SkipIpGeolocation = true
            };

            var mapLookup = _mapQueryService.GetAddressSearch(searchRequest);
            if (mapLookup == null || !mapLookup.Success || mapLookup.ResponseModel == null)
            {
                throw new ArgumentException("Address not found in map lookup service");
            }

            // Filter the results to the highest score result
            var bestMatch = mapLookup.ResponseModel.Results.Aggregate((i1, i2) => i1.Score > i2.Score ? i1 : i2);
            #endregion

            #region Map Request to Entity
            // The Geography SQL type expects Longitude/Latitude which is opposite of normal presentation
            var store = new Store()
            {
                StoreName = request.StoreName,
                Address = request.Address,
                Address2 = request.Address2,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Country = request.Country,
                Active = true,
                Verified = false,
                Latitude = Convert.ToDecimal(bestMatch.Position.Lat),
                Longitude = Convert.ToDecimal(bestMatch.Position.Lon),
                Location = _geometryFactory.CreatePoint(new Coordinate(bestMatch.Position.Lon, bestMatch.Position.Lat))
            };
            #endregion

            var entity = _context.Stores.Add(store);
            _context.SaveChanges();

            response.Success = true;
            response.NewStoreId = entity.Entity.StoreId;
            return response;
        }

        public IEnumerable<Store> GetStoresInRadius(MapRadiusQueryRequest request)
        {
            // If the request provides coordinates
            // We can just query the database directly
            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                _logger.LogInformation("Direct latitude/longitude provided, bypassing Azure Maps call");
                var query = GetStoresInDistance(Convert.ToDouble(request.Latitude), Convert.ToDouble(request.Longitude), request.DistanceInMiles);
                return query.ToList();
            }

            // Otherwise, we need to obtain coordinates from Azure Maps
            // Pass in whatever address search information is provided
            _logger.LogInformation("Only address information provided, finding coordinates from Azure Maps");

            AddressSearchResponse<SearchAddressResponse> mapLookup;
            if (string.IsNullOrEmpty(request.FreeformText))
            {
                var searchRequest = new AddressSearchRequest()
                {
                    Address = request.StreetAddress,
                    City = request.City,
                    State = request.State,
                    PostalCode = request.PostalCode,
                    IpAddress = request.IpAddress
                };

                // Use the partial lookup method since we might not have a full address supplied
                // This method attempts to format something close to a complete address
                mapLookup = _mapQueryService.GetPartialAddressSearch(searchRequest);
            }
            else
            {
                // The simple method directly passes the user supplied data to the API with no structure
                mapLookup = _mapQueryService.GetSimpleAddressSearch(request.FreeformText, request.IpAddress ?? "");
            }
            
            if (mapLookup.Success && mapLookup.ResponseModel != null)
            {
                // Find the highest score result
                var bestResult = mapLookup.ResponseModel.Results.Aggregate((i1, i2) => i1.Score > i2.Score ? i1 : i2);
                if (bestResult != null)
                {
                    if (request.DistanceInMiles == 0)
                    {
                        request.DistanceInMiles = 10;
                    }
                    var queryResult = GetStoresInDistance(bestResult.Position.Lat, bestResult.Position.Lon, request.DistanceInMiles);
                    return queryResult.ToList();
                }
            }

            return Enumerable.Empty<Store>();
        }

        /// <summary>
        /// Queries the database for Stores that are within a certain distance from a given geographic point
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="distance">Distance in miles</param>
        /// <returns></returns>
        private IQueryable<Store> GetStoresInDistance(double latitude, double longitude, int distance)
        {
            // Ensure that longitude is passed in first
            // For GIS point creation, we must pass in long/lat order
            var point = _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            var dbResult = _context.Stores.Where(s => s.Location != null && s.Location.Distance(point) <= MilesToMeters(distance));
            return dbResult;
        }

        /// <summary>
        /// Miles to Meters (Miles x 1609)
        /// </summary>
        /// <param name="miles"></param>
        /// <returns></returns>
        private int MilesToMeters(int miles)
        {
            return miles * 1609;
        }
    }
}
