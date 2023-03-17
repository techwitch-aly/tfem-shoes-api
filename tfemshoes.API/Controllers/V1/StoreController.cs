using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tfemshoes.API.Authorization;
using tfemshoes.Domain.Service;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.API.V1.Controllers
{
    /// <summary>
    /// API Controller for returning and handling all Store related data
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class StoreController : ControllerBase
    {
        #region Dependencies
        private readonly IStoreEntityService _entityService;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the controller, takes in all required dependencies
        /// </summary>
        /// <param name="entityService">Store entity service from the Domain layer</param>
        /// <param name="mapper">AutoMapper helper</param>
        /// <param name="logger"></param>
        public StoreController(IStoreEntityService entityService, IMapper mapper, ILogger<StoreController> logger)
        {
            _entityService = entityService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// Returns a list of all stores that are considered active by the TFem Shoes program 
        /// </summary>
        /// <returns>A list of Store models</returns>
        /// <response code="200">Will return an empty list if no data is found</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public IEnumerable<Models.Store> Get()
        {
            var stores = _entityService.GetAllActiveStores();
            var storeModels = _mapper.Map<IEnumerable<Domain.Entities.Store>, IEnumerable<Models.Store>>(stores);
            return storeModels;
        }

        /// <summary>
        /// Returns a specific store by the ID
        /// </summary>
        /// <param name="id">A store identifier to query for</param>
        /// <returns>A store model for that ID, or 404 if the ID does not exist</returns>
        /// <response code="200">If the specified store is found</response>
        /// <response code="404">If the specified ID cannot be found</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public ActionResult<Models.Store> Get(long id)
        {
            var store = _entityService.GetStoreById(id);
            if (store == null)
            {
                return NotFound();
            }
            var storeModel = _mapper.Map<Models.Store>(store);
            return Ok(storeModel);
        }

        /// <summary>
        /// Saves a new store and submits it for review by the TFem Shoes Team
        /// </summary>
        /// <param name="store">Model representing the basic details required to submit a store for review</param>
        /// <returns>201 with the new Get route or a 400 for validation errors</returns>
        /// <response code="201">If the store is saved and pending review</response>
        /// <response code="400">If the store data cannot be saved</response>
        /// <response code="401">If no authentication is provided</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public IActionResult Post(Models.Store store)
        {
            var saveRequest = _mapper.Map<Models.Store, SaveStoreRequest>(store);
            var response = _entityService.SaveStore(saveRequest);

            if (response.Success)
            {
                return CreatedAtAction("Get", response.NewStoreId);
            }
            else
            {
                _logger.LogWarning($"Creating a new store failed: {response.Message}");
                return BadRequest($"Validation failed: {response.Message}");
            }
        }
    }
}
