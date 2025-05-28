using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosAlta;
using DTOs.DtosInputs.DtosGeneralInputs;
using DTOs.DtosOuputs.DtosAlmacen;
using DTOs.DtosOuputs.DtosEtiqueta;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.Win32;
using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;
using ServiceStack;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_MOV_ALM.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AltaController : ControllerBase
    {
        public readonly IAltaRepository<Alta> _AltaRepository;
        public readonly IAlmacenRepository<Almacen> _AlmacenRepository;
        public readonly IGeneralRepository<General> _GeneralRepository;

        public AltaController(IAltaRepository<Alta> AltaRepository, IAlmacenRepository<Almacen> AlmacenRepository, IGeneralRepository<General> GeneralRepository)
        {

            _AltaRepository = AltaRepository;
            _AlmacenRepository = AlmacenRepository;
            _GeneralRepository = GeneralRepository;
        }

        [HttpPost]
        [Route("GrabarAlta")]
        public async Task<IActionResult> GrabarAlta(GrabarAltaDtoInputs obj)
        {
            object response = null;

            Alta obj_Alta = new Alta();
            obj_Alta.codigo = obj.codigo;
            obj_Alta.codcia = obj.codcia;
            obj_Alta.codalg = obj.codalg;
            obj_Alta.tmvmag = obj.tmvmag;
            obj_Alta.nmvmag = obj.nmvmag;
            obj_Alta.cscmag = obj.cscmag;
            obj_Alta.secma2 = obj.secma2;
            obj_Alta.codtmv = obj.codtmv;
            obj_Alta.cremag = obj.cremag;
            obj_Alta.cencos = obj.cencos;
            obj_Alta.ademag = obj.ademag;
            obj_Alta.ucrmag = obj.ucrmag;
            obj_Alta.caemag = obj.caemag;
            obj_Alta.refere = obj.refere;
            obj_Alta.ctdor1 = obj.ctdor1;
            obj_Alta.anoor1 = obj.anoor1;
            obj_Alta.nroor1 = obj.nroor1;
            obj_Alta.cscor2 = obj.cscor2;
            obj_Alta.fecmag = obj.fecmag;
            obj_Alta.pcname = obj.pcname;
            obj_Alta.ncrma2 = obj.ncrma2;

            try
            {
                int res = await _AltaRepository.Alta(obj_Alta);
                if (res > 0)
                {
                    response = new
                    {
                        success = true,
                        message = "Registro Guardado",
                        result = res

                    };
                }

                else
                {
                    response = new
                    {
                        success = true,
                        message = "Registro no Guardado",
                        result = res

                    };
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("InsertaODRPCON1")]
        public async Task<IActionResult> InsertaODRPCON1(InsertaODRPCON1Inputs obj)
        {
            object response = null;

            Alta obj_Alta = new Alta();
            obj_Alta.codcia = obj.codcia;
            obj_Alta.codtdc = "PQ";
            obj_Alta.anopc1 = obj.anopc1;
            obj_Alta.nropc1 = obj.nropc1;
            obj_Alta.codalg = obj.codalg;
            obj_Alta.fempc1 = obj.fempc1;
            obj_Alta.cimpc1 = "100";
            obj_Alta.obspc1 = "PRECONSUMO GENERADO EN PRETEJEDURIA";
            obj_Alta.codtmv = obj.codtmv;
            obj_Alta.cencos = obj.cencos;
            obj_Alta.uscpc1 = obj.uscpc1;
            obj_Alta.fhcpc1 = obj.fhcpc1;
            obj_Alta.stapc1 = 2;
            obj_Alta.nprpc1 = "FAB083";
            obj_Alta.fgcpc1 = 0;

            try
            {
                var res = await _AltaRepository.InsertaODRPCON1(obj_Alta);
                if (res.rowsAffected > 0)
                {
                    response = new
                    {
                        success = true,
                        message = res.mensaje,
                        result = res.rowsAffected
                    };
                }

                else
                {
                    response = new
                    {
                        success = true,
                        message = res.mensaje,
                        result = res.rowsAffected
                    };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            return new JsonResult(response);
        }
        
        [HttpPost]
        [Route("ProcesarGuardado")]
        public async Task<IActionResult> ProcesarGuardado(ProcesarGuardadoDtoInputs obj)
        {
            Tools.Log.Write(Convert.ToString(2), "Inicio del End Point: ProcesarGuardado");
            object response = null;
            int resGuardarAlta=0;
            
            Alta obj_alta = new Alta();
            Almacen obj_almacen = new Almacen();
            General obj_general = new General();

            List<UtilizaRegistroDtoOutputs> ListaUtilizaRegistro= new List<UtilizaRegistroDtoOutputs>();
            List<ObtenerCorrelativoAlmacenDtoOuputs> ListaObtenerCorrelativoAlmacen = new List<ObtenerCorrelativoAlmacenDtoOuputs>();
            List<Almacen> ListaAlmacen = new List<Almacen>();
            try
            {
                Tools.Log.Write(Convert.ToString(2), "Inicio funcion: ProcesarFecha(" + obj.fecmag + ")");
                bool resultadoProcesarFecha=  ProcesarFecha(obj.fecmag);
                Tools.Log.Write(Convert.ToString(2), "Resultado resultadoProcesarFecha = " + resultadoProcesarFecha.ToString());

                if (resultadoProcesarFecha)
                {
                    Tools.Log.Write(Convert.ToString(2), "Validacion StockEmpaque (" + JsonSerializer.Serialize(obj) + ")");
                    int resultadoValidarStockEmpaque = ValidarStockEmpaque(obj);
                    Tools.Log.Write(Convert.ToString(2), "Resultado: resultadoValidarStockEmpaque = " + resultadoValidarStockEmpaque.ToString());

                    if (resultadoValidarStockEmpaque > 0)
                    {
                        Tools.Log.Write(Convert.ToString(2), "Stock mayor a 0, respuesta: Hay " + resultadoValidarStockEmpaque + " etiqueta que no se puede registrar por falta de stock.");
                        response = new
                        {
                            success = false,
                            message = "Hay " + resultadoValidarStockEmpaque + " etiqueta que no se puede registrar por falta de stock.",
                            result = 0
                        };
                    }
                    else
                    {
                        Tools.Log.Write(Convert.ToString(2), "Funcion BloquearRegistroCorrelativoNull ");
                        //BLOQUEA EL CORRELATIVO PARA NO SER USADO POR OTRO EQUIPO
                        int respuestaBloquearRegistroCorrelativoNull = _GeneralRepository.BloquearRegistroCorrelativoNull(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);
                        Tools.Log.Write(Convert.ToString(2), "respuestaBloquearRegistroCorrelativoNull = " + respuestaBloquearRegistroCorrelativoNull.ToString());

                        Tools.Log.Write(Convert.ToString(2), "Funcion ObtenerCorrelativoAlmacen ");
                        ListaObtenerCorrelativoAlmacen = _AlmacenRepository.ObtenerCorrelativoAlmacen(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);
                        Tools.Log.Write(Convert.ToString(2), "respuestaBloquearRegistroCorrelativoNull = " + JsonSerializer.Serialize(respuestaBloquearRegistroCorrelativoNull));

                        int nota = ListaObtenerCorrelativoAlmacen[0].nota;
                        Tools.Log.Write(Convert.ToString(2), "Nota = " + ListaObtenerCorrelativoAlmacen[0].nota);

                        if (nota > 0)
                        {
                            Tools.Log.Write(Convert.ToString(2), "si la nota es mayor a 0");
                            Tools.Log.Write(Convert.ToString(2), "Funcion UtilizaRegistroCorrelativoAlmacen2");
                            ListaUtilizaRegistro = _AlmacenRepository.UtilizaRegistroCorrelativoAlmacen2(obj.tmvmag, obj.codalg, obj.codcompania);
                            Tools.Log.Write(Convert.ToString(2), "ListaUtilizaRegistro = " + JsonSerializer.Serialize(ListaUtilizaRegistro));

                            string PcNameUsando = ListaUtilizaRegistro[0].pcName;
                            Tools.Log.Write(Convert.ToString(2), "PcNameUsando = " + ListaUtilizaRegistro[0].pcName);

                            if (!PcNameUsando.Equals(obj.pcName))
                            {
                                Tools.Log.Write(Convert.ToString(2), "El equipo " + PcNameUsando + " esta utilizando el correlativo.");
                                response = new
                                {
                                    success = false,
                                    message = "El equipo " + PcNameUsando + " esta utilizando el registro.",
                                    result = 0
                                };
                            }
                            else
                            {
                                Tools.Log.Write(Convert.ToString(2), "Funcion BloquearRegistroCorrelativo2");
                                int respuestaBloquearRegistroCorrelativo = _GeneralRepository.BloquearRegistroCorrelativo2(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);
                                Tools.Log.Write(Convert.ToString(2), "respuestaBloquearRegistroCorrelativo = " + respuestaBloquearRegistroCorrelativo.ToString());

                                Tools.Log.Write(Convert.ToString(2), "Grabado de etiquetas");
                                foreach (var item in obj.TotalEtiquetas)
                                {
                                    Tools.Log.Write(Convert.ToString(2), "Etiqueta: " + item.CodigoEtiqueta);
                                    Tools.Log.Write(Convert.ToString(2), "JSON : " + JsonSerializer.Serialize(item));

                                    string codigoEtiqueta = item.CodigoEtiqueta;
                                    string codigoArticulo = item.codigoArticulo;
                                    string cscmag = item.secuencia; // Secuencia
                                    int secma2 = 0;
                                    string cremag = item.salida_peso.ToString();
                                    string ucrmag = obj.usuario;// Falta  usuario de sesion
                                    string caemag = item.salida_cantidad.ToString();   //cantidad empaque
                                    string codtex = item.codigoExistencia;
                                    string codexiv = item.codigoArticulo;
                                    obj_almacen.CodAlg = obj.codalg;
                                    obj_general.codtex = Convert.ToInt32(Convert.ToDouble(codtex));

                                    Tools.Log.Write(Convert.ToString(2), "Funcion ValidarAlamcenXCcosto \n JSON Almacen: " + JsonSerializer.Serialize(obj_almacen));
                                    ListaAlmacen = await _AlmacenRepository.ValidarAlamcenXCcosto(obj_almacen);

                                    int codAlmacen = ListaAlmacen[0].CodAlg;
                                    string Codcos = ListaAlmacen[0].CodCos;
                                    if (obj.codalg == codAlmacen && !obj.cencos.Equals(Codcos))
                                    {
                                        Tools.Log.Write(Convert.ToString(2), "El centro de costo no corresponde al almacen");
                                        response = new
                                        {
                                            success = false,
                                            message = "El centro de costo no corresponde al almacen",
                                            result = 0
                                        };
                                    }
                                    else
                                    {
                                        Tools.Log.Write(Convert.ToString(2), "Funcion CriterioTipoExistencia(\n" + JsonSerializer.Serialize(obj_general) + ")");
                                        int resCriterioTipoExistencia = await _GeneralRepository.CriterioTipoExistencia(obj_general);
                                        if (resCriterioTipoExistencia > 0)
                                        {
                                            Tools.Log.Write(Convert.ToString(2), "Si resCriterioTipoExistencia > 0");
                                            //  ValidaUsoCorrelativo(altaViewModel, codtex, codigoEtiqueta, codcia, codalg, tmvmag, nmvmag, cscmag, secma2, codtmv, cremag, cencos, ademag, ucrmag,
                                            //         caemag, refere, ctdor1, anoor1, nroor1, cscor2, fecmag, pcName, ncrma2);
                                        }
                                        else
                                        {
                                            Tools.Log.Write(Convert.ToString(2), "Si resCriterioTipoExistencia <= 0");
                                            Tools.Log.Write(Convert.ToString(2), "Asignacion de parametos para enviar a EndPoint");

                                            obj_alta.codigo = codigoEtiqueta;
                                            obj_alta.codcia = obj.codcompania;
                                            obj_alta.codalg = obj.codalg;
                                            obj_alta.tmvmag = obj.tmvmag;
                                            //obj_alta.nmvmag = int.TryParse(obj.nmvmag, out var result) ? result : 0;
                                            obj_alta.nmvmag = int.TryParse(nota.ToString(), out var result) ? result : 0;
                                            obj_alta.cscmag = string.IsNullOrEmpty(cscmag) ? 0 : Convert.ToInt32(cscmag);
                                            obj_alta.secma2 = secma2;
                                            obj_alta.codtmv = obj.codtmv;
                                            obj_alta.cremag = Convert.ToDecimal(cremag);
                                            obj_alta.cencos =string.IsNullOrEmpty(obj.cencos) ? 0 : Convert.ToInt32(obj.cencos);
                                            obj_alta.ademag = obj.ademag;
                                            obj_alta.ucrmag = ucrmag;
                                            obj_alta.caemag = Convert.ToInt32(Convert.ToDouble(caemag));
                                            obj_alta.refere = int.TryParse(obj.refere, out var result1) ? result1 : 0;
                                            obj_alta.ctdor1 = obj.ctdor1;
                                            obj_alta.anoor1 =string.IsNullOrEmpty(obj.anoor1) ? 0 : Convert.ToInt32(obj.anoor1);
                                            obj_alta.nroor1 =string.IsNullOrEmpty(obj.nroor1) ? 0 : Convert.ToInt32(obj.nroor1);
                                            obj_alta.cscor2 = string.IsNullOrEmpty(obj.cscor2) ? 0 : Convert.ToInt32(obj.cscor2);
                                            obj_alta.fecmag = obj.fecmag;
                                            obj_alta.pcname = obj.pcName;
                                            obj_alta.ncrma2 =string.IsNullOrEmpty(obj.ncrma2) ? 0 : Convert.ToInt32(obj.ncrma2);

                                            Tools.Log.Write(Convert.ToString(2), "obj_alta = \n " + JsonSerializer.Serialize(obj_alta));
                                            Tools.Log.Write(Convert.ToString(2), "Ejecucion de metodo de Alta");
                                            resGuardarAlta = await _AltaRepository.Alta(obj_alta);
                                            Tools.Log.Write(Convert.ToString(2), "ResultadoresGuardarAlta resGuardarAlta = " + resGuardarAlta.ToString());

                                            Tools.Log.Write(Convert.ToString(2), "Ejecucion de metodo de DesbloquearRegistro2");
                                            int resDesbloquear = _GeneralRepository.DesbloquearRegistro2(codigoEtiqueta);
                                            Tools.Log.Write(Convert.ToString(2), "Resultadores DesbloquearRegistro2 resDesbloquear = " + resDesbloquear.ToString());
                                        }
                                    }
                                }

                                Tools.Log.Write(Convert.ToString(2), "Ejecucion de metodo de DesbloquearFABCORRE2");
                                int resDesbloquearFABCORRE2=  _GeneralRepository.DesbloquearFABCORRE2(obj.codcompania, obj.codalg, obj.tmvmag);
                                Tools.Log.Write(Convert.ToString(2), "resDesbloquearFABCORRE2 = " + resDesbloquearFABCORRE2.ToString());

                                if (resGuardarAlta == -1)
                                {
                                    Tools.Log.Write(Convert.ToString(2), "Registro grabado satisfactoriamente, Movimiento Nº "+ nota);

                                    response = new
                                    {
                                        success = true,
                                        message = "Registro grabado satisfactoriamente, Movimiento Nº "+ nota,
                                        result = 1
                                    };
                                }
                                else
                                {
                                    Tools.Log.Write(Convert.ToString(2), "Registro no grabado");
                                    response = new
                                    {
                                        success = false,
                                        message = "Registro no grabado",
                                        result = 0
                                    };
                                }
                            }
                        }
                    }
                }
                else
                {
                    Tools.Log.Write(Convert.ToString(2), "Fecha inválida");
                    response = new
                    {
                        success = false,
                        message = "Fecha inválida",
                        result = 0
                    };
                }
            }
            catch (Exception ex)
            {
                Tools.Log.Write(Convert.ToString(2), "Error: " + ex.Message.ToString());
                return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }

            Tools.Log.Write(Convert.ToString(2), "Fin del End Point");
            Tools.Log.Write(Convert.ToString(1), "Fin del End Point");
            return new JsonResult(response);
        }

        private bool ProcesarFecha(string fecmag)
        {
            string fecsis, fecmen, fecact, fecmagX2 = fecmag;
            bool validacion;
            General obj_general = new General();
            ObtenerDiasPermitidosDtoOutputs obj_ObtenerDiasPermitidos = new ObtenerDiasPermitidosDtoOutputs();
            ObtenerHoraSistemaDtoOutputs obj_ObtenerHoraSistema=new ObtenerHoraSistemaDtoOutputs();
            ObtenerFechaSistemaAyerDtoOutputs obj_ObtenerFechaSistemaAyer = new ObtenerFechaSistemaAyerDtoOutputs();
            ObtenerFechaSistemaDtoOutputs obj_ObtenerFechaSistema = new ObtenerFechaSistemaDtoOutputs();
            ObtenerDiasPermitidosAyerDtoOutputs obj_ObtenerDiasPermitidosAyer = new ObtenerDiasPermitidosAyerDtoOutputs();

            obj_ObtenerHoraSistema =  _GeneralRepository.ObtenerHoraSistema();
            obj_ObtenerFechaSistemaAyer =  _GeneralRepository.ObtenerFechaSistemaAyer();
            obj_ObtenerDiasPermitidos =  _GeneralRepository.ObtenerDiasPermitidos();
            obj_ObtenerDiasPermitidosAyer = _GeneralRepository.ObtenerDiasPermitidosAyer();
            obj_ObtenerFechaSistema =  _GeneralRepository.ObtenerFechaSistema();

            int hora = obj_ObtenerHoraSistema.horaSistema;
            int minuto = obj_ObtenerHoraSistema.minutoSistema;
            int segundo = obj_ObtenerHoraSistema.segundoSistema;
            int calculo = (hora * 3600) + (minuto * 60) + segundo;

            if (calculo >= 0 && calculo <= 25199)
            {
                fecsis = obj_ObtenerFechaSistemaAyer.fechaSistemaAyer;
                int fecsisX = int.Parse(fecsis.Substring(0, 4) + fecsis.Substring(5, 2) + fecsis.Substring(8, 2));
                fecmen = obj_ObtenerDiasPermitidosAyer.fecMen;
                fecact = obj_ObtenerDiasPermitidosAyer.fecAct;
                fecmagX2 = fecmagX2.Substring(0, 4) + fecmagX2.Substring(5, 2) + fecmagX2.Substring(8, 2);

                if (int.Parse(fecmagX2) < int.Parse(fecmen) || int.Parse(fecmagX2) > int.Parse(fecact))
                {
                    validacion = false;
                }
                else if (fecsisX != int.Parse(fecmagX2))
                {
                    validacion = false;
                }
                else
                {
                    validacion = true;
                }
            }
            else
            {
                fecsis = obj_ObtenerFechaSistema.fechaSistema;
                int fecsisX = int.Parse(fecsis.Substring(0, 4) + fecsis.Substring(5, 2) + fecsis.Substring(8, 2));
                fecmen = obj_ObtenerDiasPermitidos.fecMen;
                fecact = obj_ObtenerDiasPermitidos.fecAct;
                fecmagX2 = fecmagX2.Substring(0, 4) + fecmag.Substring(5, 2) + fecmag.Substring(8, 2);

                if (int.Parse(fecmagX2) < int.Parse(fecmen) || int.Parse(fecmagX2) > int.Parse(fecact))
                {
                    validacion = false;
                }
                else if (fecsisX != int.Parse(fecmagX2))
                {
                    validacion = false;
                }
                else
                {
                    validacion = true;
                }
            }
            return validacion;
        }
        private int ValidarStockEmpaque(ProcesarGuardadoDtoInputs obj)
            //(List<ObtenerDatosEtiquetaDtoInputs> obj, List<DtoObtenerDatosStockEmpaqueInput> etiquetasStock, string ObtenerTodasEtiquetas)
        {
            int contador=0;
            List<ObtenerDatosStockEmpaqueDtoOutput> ListaObtenerDatosStockEmpaque = new List<ObtenerDatosStockEmpaqueDtoOutput>();

            ListaObtenerDatosStockEmpaque= _GeneralRepository.ObtenerDatosStockEmpaque(obj.obtenerTodasEtiquetas);
            foreach (var etiquetaStock in obj.etiquetasStock)
            {
                string codigoEtiquetaStock = etiquetaStock.codigoEtiqueta;
                double stockRealStock = etiquetaStock.stockreal;

                foreach (var datosStock in ListaObtenerDatosStockEmpaque)
                {
                    string codigoEtiquetaDatos = datosStock.codigoEtiqueta;
                    double stockRealDatos = datosStock.stockreal;

                    if (codigoEtiquetaStock.Equals(codigoEtiquetaDatos))
                    {
                        if (stockRealDatos < stockRealStock)
                        {
                            contador++;
                        }
                    }
                }
            }
            return contador;
        }

        [HttpPost]
        [Route("ProcesoLecturaEtiqueta")]
        public async Task<IActionResult> ProcesoLecturaEtiqueta(ProcesoLecturaEtiquetaDtoInputs obj)
        {
            //log
            Tools.Log.Write(Convert.ToString(1), "Inicio api ProcesoLecturaEtiqueta");
            Tools.Log.Write(Convert.ToString(2), "Inicio api ProcesoLecturaEtiqueta");
            Tools.Log.Write(Convert.ToString(1), "Etiqueta: " + obj.etiqueta);
            Tools.Log.Write(Convert.ToString(2), "Etiqueta: " + obj.etiqueta);

            object response = null;
            List<EtiquetaDtoOutput> obj_EtiquetaDtoOutputs = new List<EtiquetaDtoOutput>();

            try
            {
                //BLOQUEA FMOVALG1
                Tools.Log.Write(Convert.ToString(1), "Inicio de funcion BloquearRegistro");
                Tools.Log.Write(Convert.ToString(2), "Inicio de la funcion BloquearRegistro");

                int resultadoBloquearRegistro = BloquearRegistro(obj.etiqueta, obj.pcName);

                Tools.Log.Write(Convert.ToString(2), "resultadoBloquearRegistro = " + resultadoBloquearRegistro);
                Tools.Log.Write(Convert.ToString(2), "Fin de la funcion BloquearRegistro");
                Tools.Log.Write(Convert.ToString(2), "switch");

                switch (resultadoBloquearRegistro)
                {
                    case 1:
                    case 8888:
                        Tools.Log.Write(Convert.ToString(1), "Case 1 o 8888");
                        Tools.Log.Write(Convert.ToString(2), "case 8888: ");

                        Tools.Log.Write(Convert.ToString(1), "Validacion de movimientos");
                        Tools.Log.Write(Convert.ToString(2), "Inicio funcion validarRegistroFMOVALG2");

                        int resultvalidarRegistroFMOVALG2= validarRegistroFMOVALG2(obj.etiqueta);

                        Tools.Log.Write(Convert.ToString(2), "resultvalidarRegistroFMOVALG2 = " + resultvalidarRegistroFMOVALG2.ToString());

                        if (resultvalidarRegistroFMOVALG2 == 0)
                        {
                            Tools.Log.Write(Convert.ToString(1), "Etiqueta no tiene movimientos");
                            Tools.Log.Write(Convert.ToString(2), "si resultvalidarRegistroFMOVALG2 es 0");
                            response = new
                            {
                                success = false,
                                message = "Etiqueta no tiene ingreso en detalle de movimiento",
                                result = obj_EtiquetaDtoOutputs
                            };
                        }
                        else
                        {
                            Tools.Log.Write(Convert.ToString(2), "si resultvalidarRegistroFMOVALG2 es distinto a 0");
                            List<Etiqueta> obj_ListaEtiqueta=new List<Etiqueta>();

                            Tools.Log.Write(Convert.ToString(1), "Inicio de funcion ObtenerDatosEtiqueta2");
                            Tools.Log.Write(Convert.ToString(2), "Funcion ObtenerDatosEtiqueta2");

                            obj_ListaEtiqueta = _GeneralRepository.ObtenerDatosEtiqueta2(obj.etiqueta);
                            Tools.Log.Write(Convert.ToString(2), "resultado de la ObtenerDatosEtiqueta2, obj_ListaEtiqueta = " + JsonSerializer.Serialize(obj_ListaEtiqueta));

                            if (obj_ListaEtiqueta.Count == 0)
                            {
                                Tools.Log.Write(Convert.ToString(1), "Etiqueta no existe");
                                Tools.Log.Write(Convert.ToString(2), "Etiqueta no existe");
                                response = new
                                {
                                    success = false,
                                    message = "La etiqueta no existe" ,
                                    result = obj_EtiquetaDtoOutputs

                                };
                            }
                            else
                            {
                                Tools.Log.Write(Convert.ToString(1), "Etiqueta si existe");
                                Tools.Log.Write(Convert.ToString(2), "Etiqueta si existe");

                                Tools.Log.Write(Convert.ToString(2), "Validacion: \n" +
                                    obj_ListaEtiqueta[0].codcia.ToString() + " = " + obj.codcompania.ToString() + "\n" +
                                    obj_ListaEtiqueta[0].tmvma1.ToString() + " = I \n" +
                                    obj_ListaEtiqueta[0].tmvmag.ToString() + " = I \n" +
                                    obj_ListaEtiqueta[0].tmvmag.ToString() + " = S \n" + 
                                    obj_ListaEtiqueta[0].codalg.ToString() + " = " + obj.almacen.ToString());

                                if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                && obj_ListaEtiqueta[0].tmvma1 == "I"
                                && (obj_ListaEtiqueta[0].tmvmag == "I" 
                                    || obj_ListaEtiqueta[0].tmvmag == "S")
                                && (int)obj_ListaEtiqueta[0].codalg == Convert.ToInt32(obj.almacen)
                                )
                                {
                                    Tools.Log.Write(Convert.ToString(2), "Validacion de existencia");
                                    if (obj.listCodExis.Contains(obj_ListaEtiqueta[0].codexi.ToString()) || Convert.ToInt32(obj.almacen) == 81) 
                                    {
                                        Tools.Log.Write(Convert.ToString(2), "Paso validacion de existencia");
                                        Tools.Log.Write(Convert.ToString(2), "funcion UtilizaRegistro2");
                                        string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);
                                        if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                        {
                                            Tools.Log.Write(Convert.ToString(2), "Validacion correcta de UtilizaRegistro2");
                                            Tools.Log.Write(Convert.ToString(2), "Etiquetas: ");

                                            foreach (Etiqueta obj_L in obj_ListaEtiqueta)
                                            {
                                                Tools.Log.Write(Convert.ToString(2),  obj_L.codigo);
                                                EtiquetaDtoOutput obj_EtiquetaDtoOutput = new EtiquetaDtoOutput();
                                                obj_EtiquetaDtoOutput.codexi = obj_L.codexi;
                                                obj_EtiquetaDtoOutput.codchi = obj_L.codchi;
                                                obj_EtiquetaDtoOutput.nlhmag = obj_L.nlhmag;
                                                obj_EtiquetaDtoOutput.cremang = obj_L.cremang;
                                                obj_EtiquetaDtoOutput.unimed = obj_L.unimed;
                                                obj_EtiquetaDtoOutput.caemag = obj_L.caemag;
                                                obj_EtiquetaDtoOutput.umemag = obj_L.umemag;
                                                obj_EtiquetaDtoOutput.codigo = obj_L.codigo;
                                                obj_EtiquetaDtoOutput.codalg = obj_L.codalg;
                                                obj_EtiquetaDtoOutput.codcia = obj_L.codcia;
                                                obj_EtiquetaDtoOutput.tmvma1 = obj_L.tmvma1;
                                                obj_EtiquetaDtoOutput.tmvmag = obj_L.tmvmag;
                                                obj_EtiquetaDtoOutput.ademag = obj_L.ademag;
                                                obj_EtiquetaDtoOutput.codtex = obj_L.codtex;
                                                obj_EtiquetaDtoOutput.codprv = obj_L.codprv;
                                                obj_EtiquetaDtoOutput.trhmag = obj_L.trhmag;
                                                obj_EtiquetaDtoOutput.urhmag = obj_L.urhmag;
                                                obj_EtiquetaDtoOutput.fecmag = obj_L.fecmag.ToString("dd-MM-yyyy");
                                                obj_EtiquetaDtoOutput.ltomag = obj_L.ltomag;
                                                obj_EtiquetaDtoOutput.peso_unitario = obj_EtiquetaDtoOutput.cremang / obj_EtiquetaDtoOutput.caemag;
                                                obj_EtiquetaDtoOutput.desexi = obj_L.desexi;
                                                obj_EtiquetaDtoOutput.destipexi = obj_L.destipexi;
                                                obj_EtiquetaDtoOutput.desmaq = obj_L.desmaq;

                                                obj_EtiquetaDtoOutputs.Add(obj_EtiquetaDtoOutput);
                                            }
                                            response = new
                                            {
                                                success = true,
                                                message = "Datos de Etiqueta Obtenidos",
                                                result = obj_EtiquetaDtoOutputs
                                            };
                                        }
                                        else
                                        {
                                            response = new
                                            {
                                                success = false,
                                                message = "El registro está utilizado por: " + PcNameObtenido,
                                                result = obj_EtiquetaDtoOutputs

                                            };
                                        }
                                    }
                                else
                                {
                                    _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                    response = new
                                    {
                                        success = false,
                                        message = "Etiqueta pertenece a una existencia distinta a la orden ingresada.",
                                        result = obj_EtiquetaDtoOutputs

                                    };
                                }
                            }
                            else if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                 && obj_ListaEtiqueta[0].tmvma1 == "I"
                                && obj_ListaEtiqueta[0].tmvmag == "T"
                                && (int)obj_ListaEtiqueta[0].ademag == Convert.ToInt32(obj.almacen)
                                )
                            {
                                Tools.Log.Write(Convert.ToString(2), "Paso segunda validacion");
                                if (obj.listCodExis.Contains(obj_ListaEtiqueta[0].codexi.ToString()) || Convert.ToInt32(obj.almacen) == 81)
                                {
                                    Tools.Log.Write(Convert.ToString(2), "funcion UtilizaRegistro2");
                                    string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);
                                    if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                    {
                                        Tools.Log.Write(Convert.ToString(2), "Validacion correcta de UtilizaRegistro2");
                                        Tools.Log.Write(Convert.ToString(2), "Etiquetas: ");
                                        foreach (Etiqueta obj_L in obj_ListaEtiqueta)
                                        {
                                            EtiquetaDtoOutput obj_EtiquetaDtoOutput = new EtiquetaDtoOutput();
                                            obj_EtiquetaDtoOutput.codexi = obj_L.codexi;
                                            obj_EtiquetaDtoOutput.codchi = obj_L.codchi;
                                            obj_EtiquetaDtoOutput.nlhmag = obj_L.nlhmag;
                                            obj_EtiquetaDtoOutput.cremang = obj_L.cremang;
                                            obj_EtiquetaDtoOutput.unimed = obj_L.unimed;
                                            obj_EtiquetaDtoOutput.caemag = obj_L.caemag;
                                            obj_EtiquetaDtoOutput.umemag = obj_L.umemag;
                                            obj_EtiquetaDtoOutput.codigo = obj_L.codigo;
                                            obj_EtiquetaDtoOutput.codalg = obj_L.codalg;
                                            obj_EtiquetaDtoOutput.codcia = obj_L.codcia;
                                            obj_EtiquetaDtoOutput.tmvma1 = obj_L.tmvma1;
                                            obj_EtiquetaDtoOutput.tmvmag = obj_L.tmvmag;
                                            obj_EtiquetaDtoOutput.ademag = obj_L.ademag;
                                            obj_EtiquetaDtoOutput.codtex = obj_L.codtex;
                                            obj_EtiquetaDtoOutput.codprv = obj_L.codprv;
                                            obj_EtiquetaDtoOutput.trhmag = obj_L.trhmag;
                                            obj_EtiquetaDtoOutput.urhmag = obj_L.urhmag;
                                            obj_EtiquetaDtoOutput.fecmag = obj_L.fecmag.ToString("dd-MM-yyyy");
                                            obj_EtiquetaDtoOutput.ltomag = obj_L.ltomag;
                                            obj_EtiquetaDtoOutput.peso_unitario = obj_EtiquetaDtoOutput.cremang / obj_EtiquetaDtoOutput.caemag;
                                            obj_EtiquetaDtoOutput.desexi = obj_L.desexi;
                                            obj_EtiquetaDtoOutput.destipexi = obj_L.destipexi;
                                                obj_EtiquetaDtoOutput.desmaq = obj_L.desmaq;

                                                obj_EtiquetaDtoOutputs.Add(obj_EtiquetaDtoOutput);
                                        }
                                        response = new
                                        {
                                            success = true,
                                            message = "Datos de Etiqueta Obtenidos",
                                            result = obj_EtiquetaDtoOutputs
                                        };
                                    }
                                    else
                                    {
                                        Tools.Log.Write(Convert.ToString(2), "El registro está utilizado por: " + PcNameObtenido);
                                        response = new
                                        {
                                            success = false,
                                            message = "El registro está utilizado por: " + PcNameObtenido,
                                            result = obj_EtiquetaDtoOutputs

                                        };
                                    }
                                }
                                else
                                {
                                    _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                    response = new
                                    {
                                        success = false,
                                        message = "Etiqueta pertenece a una existencia distinta a la orden ingresada.",
                                        result = obj_EtiquetaDtoOutputs

                                    };
                                }
                            }
                            else
                            {
                                Tools.Log.Write(Convert.ToString(2), "Etiqueta no pertenece");
                                Tools.Log.Write(Convert.ToString(2), "ejecucion de funcion DesbloquearRegistro2");

                                _GeneralRepository.DesbloquearRegistro2(obj.etiqueta);
                                response = new
                                {
                                    success = false,
                                    message = "Etiqueta no pertenece.",
                                    result = obj_EtiquetaDtoOutputs

                                };
                            }
                        }
                    }
                    break;
                case 9999:
                    Tools.Log.Write(Convert.ToString(1), "Case 9999 No existe el registro");
                    Tools.Log.Write(Convert.ToString(2), "Case 9999 No existe el registro");
                    response = new
                    {
                        success = false,
                        message = "Etiqueta: " + obj.etiqueta +  " no existe.",
                        result = obj_EtiquetaDtoOutputs

                    };
                    break;
                case 0:
                    Tools.Log.Write(Convert.ToString(1), "Case 0 Etiqueta no pertenece");
                    Tools.Log.Write(Convert.ToString(2), "Case 0 Etiqueta no pertenece");
                    response = new
                    {
                        success = false,
                        message = "El registro se encuentra bloqueado por otro equipo.",
                        result = obj_EtiquetaDtoOutputs

                    };
                    break;
            }
        }
        catch (Exception ex)
        {
            Tools.Log.Write(Convert.ToString(1), "Error no se pudo obtener los datos de la etiqueta");
            Tools.Log.Write(Convert.ToString(2), "Error ex = " + ex.Message);
            return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
        }

        return new JsonResult(response);
    }

    private int validarRegistroFMOVALG2(string etiqueta)
    {
        List<Almacen> Lista_Almacen = new List<Almacen>();
        Lista_Almacen = _AlmacenRepository.ObtenerRegistro_FMOVALG2_2(etiqueta);

        return Lista_Almacen.Count();
    }

    private bool ValidarFechaProceso(string fechaIngresada, string fechaObtenida)
    {
        string fechaInputString = fechaIngresada;
        var dateFormat = "dd/MM/yyyy";
        bool respuesta = false;

        try
        {
            // Convierte las fechas ingresadas y la fecha de proceso
            DateTime fechaInput = DateTime.ParseExact(fechaInputString, dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime fechaProcesoDt = DateTime.ParseExact(fechaObtenida, dateFormat, System.Globalization.CultureInfo.InvariantCulture);

            // Verifica que ambas fechas no sean nulas
            if (fechaInput != null && fechaProcesoDt != null)
            {
                // Compara si la fecha de proceso es anterior o igual a la fecha de entrada
                if (fechaProcesoDt <= fechaInput)
                {
                    // Deshabilita el control
                  //  dateInput.Enabled = false;
                    respuesta = true;
                }
                else
                {
                    // Muestra un mensaje de alerta y enfoca el control
                /*    MessageBox.Show("No es posible utilizar el item seleccionado porque en la fecha del movimiento, el item no figura con stock en el almacén",
                                    "Movimiento Almacén",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    dateInput.Focus();*/
                                respuesta = false;
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Devuelve false en caso de error en el parsing o valores nulos
            return respuesta;
        }

        private string FormatearFecha(string fecha)
        {
            try
            {
                // Define los formatos de fecha original y nuevo
                var originalFormat = "dd-MM-yyyy";
                var newFormat = "dd/MM/yyyy";

                // Intenta convertir la fecha
                DateTime date = DateTime.ParseExact(fecha, originalFormat, System.Globalization.CultureInfo.InvariantCulture);

                // Devuelve la fecha en el nuevo formato
                return date.ToString(newFormat);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private int BloquearRegistro(string etiqueta, string pcName)
        {
            return _GeneralRepository.BloquearRegistro2(etiqueta, pcName);
        }
    }
}
