using tfemshoes.Domain.Entities;
using tfemshoes.Domain.Service.ServiceModels;

namespace tfemshoes.Domain.Service
{
    /// <summary>
    /// Interface representing the Entity Serivce for stores, allowing access to the database
    /// </summary>
    public interface IStoreEntityService
    {
        /// <summary>
        /// Returns a list of all stores that are considered active by the TFem Shoes program
        /// </summary>
        /// <returns></returns>
        IEnumerable<Store> GetAllActiveStores();

        /// <summary>
        /// Returns a specific store from the database for the given primary key parameter
        /// </summary>
        /// <param name="id">A primary key value for the Store</param>
        /// <returns>The store represented by the parameter, or null if no store is found</returns>
        Store? GetStoreById(long id);

        SaveStoreResponse SaveStore(SaveStoreRequest request);

        IEnumerable<Store> GetStoresInRadius(MapRadiusQueryRequest request);
    }
}
