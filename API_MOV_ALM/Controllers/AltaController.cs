using API_MOV_ALM.Models;
using DTOs.DtosInputs.DtosAlta;
using DTOs.DtosInputs.DtosGeneralInputs;
using DTOs.DtosOuputs.DtosAlmacen;
using DTOs.DtosOuputs.DtosEtiqueta;
using DTOs.DtosOuputs.DtosGeneralOutputs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;
using ServiceStack;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_MOV_ALM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

              bool resultadoProcesarFecha=  ProcesarFecha(obj.fecmag);

                if (resultadoProcesarFecha)
                {

                    int resultadoValidarStockEmpaque = ValidarStockEmpaque(obj);

                    if (resultadoValidarStockEmpaque > 0)
                    {
                       

                        response = new
                        {
                            success = false,
                            message = "Hay " + resultadoValidarStockEmpaque + " etiqueta que no se puede registrar por falta de stock.",
                            result = 0

                        };
                    }
                    else
                    {


                        int respuestaBloquearRegistroCorrelativoNull = _GeneralRepository.BloquearRegistroCorrelativoNull(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);

                        ListaObtenerCorrelativoAlmacen= _AlmacenRepository.ObtenerCorrelativoAlmacen(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);
                       
                        long nota = ListaObtenerCorrelativoAlmacen[0].nota;

                        if (nota > 0)
                        {


                            ListaUtilizaRegistro = _AlmacenRepository.UtilizaRegistroCorrelativoAlmacen2(obj.tmvmag, obj.codalg, obj.codcompania);


                            string PcNameUsando = ListaUtilizaRegistro[0].pcName;

                            if (!PcNameUsando.Equals(obj.pcName))
                            {


                                response = new
                                {
                                    success = false,
                                    message = "El equipo " + PcNameUsando + " esta utilizando el registro.",
                                    result = 0

                                };
                            }
                            else
                            {
                                int respuestaBloquearRegistroCorrelativo = _GeneralRepository.BloquearRegistroCorrelativo2(obj.tmvmag, obj.pcName, obj.codalg, obj.codcompania);


                                foreach (var item in obj.TotalEtiquetas)
                                {


                                    string codigoEtiqueta = item.CodigoEtiqueta;
                                    string codigoArticulo = item.codigoArticulo;
                                    string cscmag = item.secuencia; // Secuencia
                                    int secma2 = 0;
                                    string cremag = item.stock;
                                    string ucrmag = obj.usuario;// Falta  usuario de sesion
                                    string caemag = item.cantidad;   //cantidad empaque
                                    string codtex = item.codigoExistencia;
                                    string codexiv = item.codigoArticulo;
                                    obj_almacen.CodAlg = obj.codalg;
                                    obj_general.codtex = Convert.ToInt32(Convert.ToDouble(codtex));


                                    ListaAlmacen = await _AlmacenRepository.ValidarAlamcenXCcosto(obj_almacen);

                                    int codAlmacen = ListaAlmacen[0].CodAlg;
                                    string Codcos = ListaAlmacen[0].CodCos;
                                    if (obj.codalg == codAlmacen && !obj.cencos.Equals(Codcos))
                                    {
                                        response = new
                                        {
                                            success = false,
                                            message = "El centro de costo no corresponde al almacen",
                                            result = 0

                                        };

                                    }
                                    else
                                    {

                                        int resCriterioTipoExistencia = await _GeneralRepository.CriterioTipoExistencia(obj_general);
                                        if (resCriterioTipoExistencia > 0)
                                        {
                                            //  ValidaUsoCorrelativo(altaViewModel, codtex, codigoEtiqueta, codcia, codalg, tmvmag, nmvmag, cscmag, secma2, codtmv, cremag, cencos, ademag, ucrmag,
                                             //         caemag, refere, ctdor1, anoor1, nroor1, cscor2, fecmag, pcName, ncrma2);
                                            

                                        }
                                        else
                                        {

                                            obj_alta.codigo = codigoEtiqueta;
                                            obj_alta.codcia = obj.codcompania;
                                            obj_alta.codalg = obj.codalg;
                                            obj_alta.tmvmag = obj.tmvmag;
                                            obj_alta.nmvmag = int.TryParse(obj.nmvmag, out var result) ? result : 0;
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

                                            resGuardarAlta = await _AltaRepository.Alta(obj_alta);
                                           int resDesbloquear= _GeneralRepository.DesbloquearRegistro2(codigoEtiqueta);


                                        }

                                    }

                                }

                              int resDesbloquearFABCORRE2=  _GeneralRepository.DesbloquearFABCORRE2(obj.codcompania, obj.codalg, obj.tmvmag);
                               
                                if (resGuardarAlta == -1)
                                {
                                       
                                        response = new
                                    {
                                        success = true,
                                        message = "Registro grabado satisfactoriamente, Movimiento Nº "+ nota,
                                        result = 1

                                    };
                                }
                                else
                                {
                                 

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
                    // Acción si la validación falló

          
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
                   
                    return new JsonResult(new { success = false, message = "Error Catch: " + ex.Message, StackTrace = ex.StackTrace, result = "" });
            }
            
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

            object response = null;
            List<EtiquetaDtoOutput> obj_EtiquetaDtoOutputs = new List<EtiquetaDtoOutput>();


            try
            {
                //BLOQUEA FMOVALG1
                int resultadoBloquearRegistro = BloquearRegistro(obj.etiqueta, obj.pcName);
                switch (resultadoBloquearRegistro)
                {
                    case 1:
                    case 8888:
                        Console.WriteLine("API manejarResultadoBloqueo: Registro se encontró libre y fue bloqueado satisfactoriamente.");
                       int resultvalidarRegistroFMOVALG2= validarRegistroFMOVALG2(obj.etiqueta);

                        if (resultvalidarRegistroFMOVALG2 == 0)
                        {
                            response = new
                            {
                                success = false,
                                message = "Etiqueta no tiene ingreso en detalle de movimiento",
                                result = obj_EtiquetaDtoOutputs

                            };
                        }
                        else
                        { 

                            List<Etiqueta> obj_ListaEtiqueta=new List<Etiqueta>();
                            obj_ListaEtiqueta = _GeneralRepository.ObtenerDatosEtiqueta2(obj.etiqueta);

                            if (obj_ListaEtiqueta.Count == 0)
                            {
                                response = new
                                {
                                    success = false,
                                    message = "La etiqueta no existe" ,
                                    result = obj_EtiquetaDtoOutputs

                                };
                            }
                            else
                            {

                                if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                && obj_ListaEtiqueta[0].tmvma1 == "I"
                                && (obj_ListaEtiqueta[0].tmvmag == "I" || obj_ListaEtiqueta[0].tmvmag == "S")
                                && (int)obj_ListaEtiqueta[0].codalg == Convert.ToInt32(obj.almacen)
                                )
                                {
                                    string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);


                                    if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                    {


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
                                else if (Convert.ToInt32(obj_ListaEtiqueta[0].codcia) == Convert.ToInt32(obj.codcompania)
                                     && obj_ListaEtiqueta[0].tmvma1 == "I"
                                    && obj_ListaEtiqueta[0].tmvmag == "T"
                                    && (int)obj_ListaEtiqueta[0].ademag == Convert.ToInt32(obj.almacen)
                                    )
                                {
                                    string PcNameObtenido = _GeneralRepository.UtilizaRegistro2(obj.etiqueta);


                                    if (PcNameObtenido == null || PcNameObtenido.IsEmpty() || PcNameObtenido == obj.pcName)
                                    {


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
                                        message = "Etiqueta no pertenece.",
                                        result = obj_EtiquetaDtoOutputs

                                    };

                                }
                            }









                        }


                        break;
                    case 9999:

                        response = new
                        {
                            success = false,
                            message = "No existe el registro.",
                            result = obj_EtiquetaDtoOutputs

                        };
                        break;
                    case 0:
                       
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
