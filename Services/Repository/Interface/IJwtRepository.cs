using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface IJwtRepository<T> where T : class
    {
        string GenerarToken(T clase);
    }
}
