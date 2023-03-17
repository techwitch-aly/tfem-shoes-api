using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tfemshoes.API.Authorization;
using tfemshoes.Domain.Service;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.API.Controllers.V1
{
    /// <summary>
    /// API Controller for returning and handling all Map/Geography related data
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class MapController : ControllerBase
    {
        #region Dependencies
        private readonly IStoreEntityService _entityService;
        private readonly IMapper _mapper;
        private readonly ILogger<MapController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the controller, takes in all required dependencies
        /// </summary>
        /// <param name="entityService">Store entity service from the Domain layer</param>
        /// <param name="mapper">AutoMapper helper</param>
        /// <param name="logger"></param>
        public MapController(IStoreEntityService entityService, IMapper mapper, ILogger<MapController> logger)
        {
            _entityService = entityService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// Method to return Stores within a certain radius of supplied data
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Currently this method supports looking up Stores near a lat/lon pair, or via limited address attributes.
        /// Attributes include:
        /// City
        /// State
        /// ZIP/Postal Code
        /// </remarks>
        /// <param name="query">A model representing the possible data that can be used as input</param>
        /// <response code="200">Will return an empty list if no stores match the query</response>
        /// <response code="400">Will return when no search paramter options are supplied</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Models.Store>> RadiusOf([FromQuery] Models.RadiusQuery query)
        {
            _logger.LogInformation("Begin radius query");

            // If we get no query object, we cannot process
            if (query == null)
            {
                return BadRequest("Mising query parameters");
            }

            // If we get no lookup data, we cannot process
            if (string.IsNullOrEmpty(query.City)
                && string.IsNullOrEmpty(query.StreetAddress)
                && string.IsNullOrEmpty(query.PostalCode)
                && string.IsNullOrEmpty(query.State)
                && string.IsNullOrEmpty(query.FreeformText)
                && !query.Latitude.HasValue
                && !query.Longitude.HasValue)
            {
                return BadRequest("Mising query parameters");
            }

            var radiusRequest = _mapper.Map<Models.RadiusQuery, MapRadiusQueryRequest>(query);
            radiusRequest.IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var stores = _entityService.GetStoresInRadius(radiusRequest);
            var storeModels = _mapper.Map<IEnumerable<Domain.Entities.Store>, IEnumerable<Models.Store>>(stores);
            return Ok(storeModels);
        }
    }
}
