using AutoMapper;

namespace tfemshoes.API
{
    /// <summary>
    /// Profile for Automapper
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Default constructor, add new type mappings here
        /// </summary>
        public MapperProfile()
        {
            CreateMap<Models.Store, Domain.Entities.Store>();
            CreateMap<Domain.Entities.Store, Models.Store>();
            CreateMap<Domain.Service.ServiceModels.SaveStoreRequest, Models.Store>();
            CreateMap<Models.Store, Domain.Service.ServiceModels.SaveStoreRequest>();
            CreateMap<Models.RadiusQuery, Domain.Service.ServiceModels.MapRadiusQueryRequest>();
            CreateMap<Domain.Service.ServiceModels.MapRadiusQueryRequest, Models.RadiusQuery>();
        }
    }
}
