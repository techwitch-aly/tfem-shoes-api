using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfemshoes.Domain.Entities;

namespace tfemshoes.Domain.Service
{
    public interface IUserService
    {
        Task<User?> Authenticate(string username, string password);

        void Register(string username, string password);
    }
}
