using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tfemshoes.API.Authorization;
using tfemshoes.Domain.Service;

namespace tfemshoes.API.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AddressController : ControllerBase
    {
        #region Dependencies
        private readonly IAzureMapsQueryService _mapService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the controller, takes in all required dependencies
        /// </summary>
        /// <param name="mapService">Azure Maps query service</param>
        /// <param name="mapper">AutoMapper helper</param>
        /// <param name="logger"></param>
        public AddressController(IAzureMapsQueryService mapService, IMapper mapper, ILogger<AddressController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _mapService = mapService;
        }
        #endregion

        /// <summary>
        /// Return a list of potential full addresses based on user supplied input
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Will return an empty list if no addresses match the query</response>
        /// <response code="400">Will return when no search paramter is supplied</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Models.Address>> PredicitiveLookup([FromQuery] string userInput)
        {
            if (string.IsNullOrEmpty(userInput) || userInput.Length < 2)
            {
                return BadRequest("Must supply a query and at least 2 characters");
            }

            var searchResult = _mapService.GetSimpleAddressSearch(userInput, Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            if (searchResult != null && searchResult.Success && searchResult.ResponseModel != null)
            {
                var addressOptions = searchResult.ResponseModel.Results.Select(s => new Models.Address()
                {
                    StreetAddress = $"{s.Address.StreetNumber} {s.Address.StreetName}",
                    City = s.Address.Municipality,
                    State = s.Address.CountrySubdivision,
                    PostalCode = s.Address.ExtendedPostalCode,
                    Country = s.Address.Country
                });

                return Ok(addressOptions);
            }

            return Ok(Enumerable.Empty<Models.Address>());
        }
    }
}
