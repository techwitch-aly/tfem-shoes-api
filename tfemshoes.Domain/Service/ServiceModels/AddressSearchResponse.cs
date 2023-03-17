using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Service.AzureMapsModels;

namespace tfemshoes.Domain.Service.ServiceModels
{
    public class AddressSearchResponse<T>
    {
        public bool Success { get; internal set; } = false;

        public T? ResponseModel { get; internal set; }
    }
}
