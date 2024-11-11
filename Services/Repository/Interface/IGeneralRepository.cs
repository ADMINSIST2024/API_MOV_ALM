using DTOs.DtosInputs.DtosAlta;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interface
{
    public interface IGeneralRepository<T> where T : class
    {
        ObtenerFechaSistemaDtoOutputs ObtenerFechaSistema();
        Task<List<Etiqueta>> ObtenerDatosEtiqueta(Etiqueta obj);
        List<Etiqueta> ObtenerDatosEtiqueta2(string etiqueta);

        Task<int> BloquearRegistroNull(T obj);

        Task<General> UtilizaRegistro(General obj);
        string UtilizaRegistro2(string etiqueta);


        Task<int> BloquearRegistro(T obj);
        int BloquearRegistro2(string etiqueta, string pcName);
        Task<int> ValidarTipoListado(T obj);

        Task<int> DesbloquearRegistro(T obj);
        int DesbloquearRegistro2(string codigo);

        ObtenerHoraSistemaDtoOutputs ObtenerHoraSistema();

        ObtenerFechaSistemaAyerDtoOutputs ObtenerFechaSistemaAyer();

        ObtenerDiasPermitidosDtoOutputs ObtenerDiasPermitidos();

        ObtenerDiasPermitidosAyerDtoOutputs ObtenerDiasPermitidosAyer();

        List<ObtenerDatosStockEmpaqueDtoOutput> ObtenerDatosStockEmpaque(string ObtenerTodasEtiquetas);

        int BloquearRegistroCorrelativoNull(string tmvmag, string pcName, int codalg, int codcompania);
        Task<int> BloquearRegistroCorrelativo(T obj);
        int BloquearRegistroCorrelativo2(string tmvmag, string pcName, int codalg, int codcompania);

        Task<int> CriterioTipoExistencia(T obj);
        Task<List<General>> ValidaUsoCorrelativo();

        Task<List<General>> ObtenerFMOVALG2(T obj);

        Task<int> DesbloquearTABCORRE(T obj);

        Task<int> DesbloquearFABCORRE(T obj);
        int DesbloquearFABCORRE2(int codcompania, int codalg, string tmvmag);

       General ValidarLogin(T obj);

        List<General> ObtenerDatosOrden(T obj);

        List<General> ObtenerEstadoCarga(T obj);

        List<General> ExisteConsecutivo(T obj);



    }
    
}
