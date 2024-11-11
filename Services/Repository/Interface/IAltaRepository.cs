using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface IAltaRepository<T> where T : class
    {
        Task<int> Alta(T obj);

        Task<(int rowsAffected, string mensaje)> InsertaODRPCON1(T obj);


    }
}
