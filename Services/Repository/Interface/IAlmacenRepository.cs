using DTOs.DtosOuputs.DtosAlmacen;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface IAlmacenRepository<T> where T : class
    {
        Task<List<T>> ObtenerAlmacen();
        Task<List<T>> ObtenerAlmacenXCodigo(T Clase);
        List<ObtenerCorrelativoAlmacenDtoOuputs> ObtenerCorrelativoAlmacen(string tmvmag, string pcName, int codalg, int codcompania);
        Task<List<T>> ObtenerRegistro_FMOVALG2(T Clase);
        List<T> ObtenerRegistro_FMOVALG2_2(string etiqueta);
        Task<List<T>> UtilizaRegistroCorrelativoAlmacen(T Clase);
      
        List<UtilizaRegistroDtoOutputs> UtilizaRegistroCorrelativoAlmacen2(string tmvmag, int codalg, int codcompania);
        Task<List<T>> ValidarAlamcenXCcosto(T Clase);
        Task<List<T>> ObtenerCorrelativoAlmacen2(T Clase);
    }
}
