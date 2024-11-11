using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface IMovimientoRepository<T> where T : class
    {
        List<Movimientos> ConsultaMovimientos(T obj);
    }
}
