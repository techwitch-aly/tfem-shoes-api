using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfemshoes.Domain.Service.ServiceModels
{
    public class SaveStoreResponse
    {
        public bool Success { get; internal set; } = false;

        public long NewStoreId { get; internal set; }

        public string Message { get; internal set; }
    }
}
