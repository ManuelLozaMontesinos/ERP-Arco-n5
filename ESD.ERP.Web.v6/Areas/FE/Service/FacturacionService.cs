using ESD.Core;
using ESD.Core.Logging;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Models.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Linq;

namespace ESD.ERP.Web.Areas.FE.Service
{
   public class EstadoComprobante
   {
      public const string GENERADO = "1";
      public const string ENVIADO_ACEPTADO = "2";
      public const string ENVIADO_ACEPTADO_CON_OBS = "3";
      public const string ENVIADO_RECHAZADO = "4";
      public const string ARCHIVO_CON_ERROR = "5";
      public const string ENVIADO_VERIFICACION = "6";
      public const string CONTINGENCIA_ENVIADO_VERIFICACION = "7";
   }
   public class FacturacionService : IFacturacionService
   {
      private readonly AppHelper appHelper;
      private readonly IEstadoSunatProxy estadoSunatProxy;
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly INotaCreditoProxy notaCreditoProxy;
      private readonly IGuiaRemisionProxy guiaRemisionProxy;

      public FacturacionService(
         AppHelper appHelper,
         IEstadoSunatProxy estadoSunatProxy,
         IComprobanteProxy comprobanteProxy,
         INotaCreditoProxy notaCreditoProxy,
         IGuiaRemisionProxy guiaRemisionProxy
         )
      {
         this.appHelper = appHelper;
         this.estadoSunatProxy = estadoSunatProxy;
         this.comprobanteProxy = comprobanteProxy;
         this.notaCreditoProxy = notaCreditoProxy;
         this.guiaRemisionProxy = guiaRemisionProxy;
      }

      public void GenerarEnviarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId, string envioEnLinea)
      {

         try
         {

            DataSet dataSet = obtenerDataPorTipoDocumento(tipoDocumentoComercial, comprobanteId);

            RestClient _client = new(baseAddress);
            string apiResourceName = string.Empty;

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  apiResourceName = "api/documento";
                  break;

               case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
                  apiResourceName = "api/guia";
                  break;
            }

            var request = new RestRequest(apiResourceName, Method.POST) { RequestFormat = DataFormat.Json };
            var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  InvoiceDTO invoiceDTO = CreateInvoiceDTOFromData(dataSet);
                  invoiceDTO.EnvioEnLinea = envioEnLinea;
                  var invoiceJson = JsonConvert.SerializeObject(invoiceDTO, settings);
                  request.AddParameter("application/json", invoiceJson, ParameterType.RequestBody);
                  break;

               case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
                  DespatchAdviceDTO despatchAdviceDTO = CreateDespatchAdviceDTOFromData(dataSet);
                  var despatchJson = JsonConvert.SerializeObject(despatchAdviceDTO, settings);
                  request.AddParameter("application/json", despatchJson, ParameterType.RequestBody);
                  break;
            }

            ResponseDTO responseDTO = executeRequest(_client,request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            string codigoEstado = obtenerEstadoSunat(responseDTO.EstadoComprobante);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO() { 
               Codigo = codigoEstado
            });

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  ComprobanteDTO comprobanteDTO = new()
                  {
                     ComprobanteId = comprobanteId,
                     ComprobanteElectronicoId = responseDTO.ComprobanteElectronicoId,
                     CodigoHash = responseDTO.HashCode,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };

                  comprobanteProxy.ActualizarEmisionElectronica(comprobanteDTO);
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  NotaCreditoDTO notaCreditoDTO = new()
                  {
                     NotaCreditoId = comprobanteId,
                     ComprobanteElectronicoId = responseDTO.ComprobanteElectronicoId,
                     CodigoHash = responseDTO.HashCode,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };
                  notaCreditoProxy.ActualizarEmisionElectronica(notaCreditoDTO);
                  break;

               case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
                  GuiaRemisionDTO guiaRemisionDTO = new()
                  {
                     GuiaRemisionId = comprobanteId,
                     ComprobanteElectronicoId = responseDTO.ComprobanteElectronicoId,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };
                  guiaRemisionProxy.ActualizarEmisionElectronica(guiaRemisionDTO);
                  break;
            }

         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
      }
      private DataSet obtenerDataPorTipoDocumento(string tipoDocumentoComercial, int comprobanteId) {

         Dictionary<string, string> parameters = new();

         string reportName = string.Empty;
         string tableName = string.Empty;

         switch (tipoDocumentoComercial)
         {
            case TipoDocumentoComercialConstante.FACTURA:
            case TipoDocumentoComercialConstante.BOLETA_VENTA:
               reportName = "sp_fact_e_facturaBoleta";
               parameters.Add("comprobanteId", comprobanteId.ToString());
               tableName = "factura";
               break;

            case TipoDocumentoComercialConstante.NOTA_CREDITO:
               reportName = "sp_fact_e_notaCredito";
               parameters.Add("notaCreditoId", comprobanteId.ToString());
               tableName = "factura";
               break;

            case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
            case TipoDocumentoComercialConstante.GUIA_REMISION_TRANSPORTE:
               reportName = "sp_fact_e_guiaRemision";
               parameters.Add("guiaRemisionId", comprobanteId.ToString());
               tableName = "guiaRemision";
               break;
         }

         DataSet dataSet = new();

         byte[] fileBytes = appHelper.ExecuteExportXml(reportName, parameters);

         DataTable dataTable = new();

         dataTable.ReadXml(new MemoryStream(fileBytes));
         dataTable.TableName = tableName;
         dataSet.Tables.Add(dataTable);

         if(tipoDocumentoComercial == TipoDocumentoComercialConstante.FACTURA)
         {
            parameters = new();
            reportName = "sp_fact_e_facturaCuota";
            parameters.Add("comprobanteId", comprobanteId.ToString());
            fileBytes = appHelper.ExecuteExportXml(reportName, parameters);

            dataTable = new DataTable();
            dataTable.ReadXml(new MemoryStream(fileBytes));
            dataTable.TableName = "facturaCuota";
            dataSet.Tables.Add(dataTable);
         }

         return dataSet;
      }
      public InvoiceDTO CreateInvoiceDTOFromData(DataSet data)
      {
         InvoiceDTO invoiceDTO = null;
         DataTable dataTable = data.Tables["factura"];

         foreach (DataRow row in dataTable.Rows)
         {
            if (invoiceDTO == null)
            {
               invoiceDTO = new InvoiceDTO
               {
                  TipoComprobante = Convert.ToInt32(row["TipoComprobante"]),
                  Serie = row["Serie"].ToString(),
                  Numero = row["Numero"].ToString(),
                  Fecha = Convert.ToDateTime(row["Fecha"]),
                  TipoOperacion = row["TipoOperacion"].ToString(),
                  TipoMoneda = row["TipoMoneda"].ToString(),
                  ProveedorNombre = row["ProveedorNombre"].ToString(),
                  ProveedorNombreComercial = row["ProveedorNombreComercial"].ToString(),
                  ProveedorDireccion = row["ProveedorDireccion"].ToString(),
                  ProveedorTipoDocumento = row["ProveedorTipoDocumento"].ToString(),
                  ProveedorRUC = row["ProveedorRUC"].ToString(),
                  ProveedorUbigeo = row["ProveedorUbigeo"].ToString(),
                  ProveedorUrbanizacionZona = row["ProveedorUrbanizacionZona"].ToString(),
                  ProveedorDepartamento = row["ProveedorDepartamento"].ToString(),
                  ProveedorProvincia = row["ProveedorProvincia"].ToString(),
                  ProveedorDistrito = row["ProveedorDistrito"].ToString(),
                  ProveedorPais = row["ProveedorPais"].ToString(),
                  CodigoEstablecimiento = row["CodigoEstablecimiento"].ToString(),
                  ClienteNombre = row["ClienteNombre"].ToString(),
                  ClienteDireccion = row["ClienteDireccion"].ToString(),
                  ClienteNumeroDocumento = row["ClienteNumeroDocumento"].ToString(),
                  ClienteTipoDocumento = row["ClienteTipoDocumento"].ToString()
               };
               if (invoiceDTO.TipoComprobante == Convert.ToInt32(TipoDocumentoComercialConstante.NOTA_CREDITO))
               {
                  invoiceDTO.TipoNota = row["TipoNota"].ToString();
                  invoiceDTO.Sustento = row["Sustento"].ToString();
                  invoiceDTO.TipoDocumentoReferencia = row["TipoDocumentoReferencia"].ToString();
                  invoiceDTO.DocumentoReferenciaSerie = row["DocumentoReferenciaSerie"].ToString();
                  invoiceDTO.DocumentoReferenciaNumero = row["DocumentoReferenciaNumero"].ToString();
               }
               else
               {
                  if(Convert.ToDecimal(row["MontoTotalAnticipo"]) > decimal.Zero)
                  {
                     invoiceDTO.TipoDocumentoReferencia = row["TipoDocAnticipo"].ToString();
                     invoiceDTO.DocumentoReferenciaSerie = row["SerieAnticipo"].ToString();
                     invoiceDTO.DocumentoReferenciaNumero = row["NumeroAnticipo"].ToString();
                     invoiceDTO.DocumentoReferenciaMontoNeto = Convert.ToDecimal(row["MontoNetoAnticipo"]);
                     invoiceDTO.DocumentoReferenciaMontoIGV = Convert.ToDecimal(row["MontoIGVAnticipo"]);
                     invoiceDTO.DocumentoReferenciaMontoTotal = Convert.ToDecimal(row["MontoTotalAnticipo"]);
                  }
               }
               invoiceDTO.PorcentajeIGV = Convert.ToDecimal(row["PorcentajeIGV"]);
               invoiceDTO.TotalGravadoDocumento = Convert.ToDecimal(row["TotalGravadoDocumento"]);
               invoiceDTO.TotalImpuestoDocumento = Convert.ToDecimal(row["TotalImpuestoDocumento"]);
               invoiceDTO.TotalDocumento = Convert.ToDecimal(row["TotalDocumento"]);
               invoiceDTO.TotalGravado = Convert.ToDecimal(row["TotalGravado"])- invoiceDTO.DocumentoReferenciaMontoNeto;
               invoiceDTO.TotalIGV = Convert.ToDecimal(row["TotalIGV"]);
               invoiceDTO.TotalInafecto = Convert.ToDecimal(row["TotalInafecto"]);
               invoiceDTO.TotalGratuito = Convert.ToDecimal(row["TotalGratuito"]);
               invoiceDTO.PorcentajeDescuentoGlobal = Convert.ToDecimal(row["porcentajeDescuentoGlobal"]);
               invoiceDTO.TotalDescuentoGlobal = Convert.ToDecimal(row["TotalDescuentoGlobal"]);
               invoiceDTO.TotalDescuentoItem = Convert.ToDecimal(row["TotalDescuentoItem"]);
               invoiceDTO.Total = Convert.ToDecimal(row["Total"]);

               if (invoiceDTO.TipoComprobante != Convert.ToInt32(TipoDocumentoComercialConstante.NOTA_CREDITO))
               {
                  invoiceDTO.MontoLetras = row["MontoLetras"].ToString();
               }

               invoiceDTO.FormaPago = row["FormaPago"].ToString();
               invoiceDTO.MontoFormaPago = Convert.ToDecimal(row["Total"]);

               invoiceDTO.TipoDetraccion = row["TipoDetraccion"].ToString();
               invoiceDTO.PorcentajeDetraccion = Convert.ToDecimal(row["PorcentajeDetraccion"]);
               invoiceDTO.MontoDetraccion = Convert.ToDecimal(row["MontoDetraccion"]);
               invoiceDTO.CuentaDetraccion = row["CuentaDetraccion"].ToString();
            }

            InvoiceItemDTO invoiceItemDTO = new()
            {
               Codigo = row["Codigo"].ToString(),
               Descripcion = row["Descripcion"].ToString(),
               UnidadMedida = row["UnidadMedida"].ToString(),
               Cantidad = Convert.ToDecimal(row["Cantidad"]),
               ValorUnitario = Convert.ToDecimal(row["ValorUnitario"]),
               PrecioUnitario = Convert.ToDecimal(row["PrecioUnitario"]),
               TipoAfectacion = row["TipoAfectacion"].ToString(),
               MontoBruto = Convert.ToDecimal(row["MontoBruto"]),
               PorcentajeDescuento = Convert.ToDecimal(row["PorcentajeDescuento"]),
               MontoDescuento = Convert.ToDecimal(row["MontoDescuento"]),
               MontoNeto = Convert.ToDecimal(row["MontoNeto"]),
               MontoIGV = Convert.ToDecimal(row["MontoIGV"]),
               MontoTotal = Convert.ToDecimal(row["MontoTotal"])
            };
            invoiceDTO.Items.Add(invoiceItemDTO);
         }

         if (data.Tables.Contains("facturaCuota"))
         {
            dataTable = data.Tables["facturaCuota"];
            foreach (DataRow row in dataTable.Rows)
            {
               FormaPagoCuotaDTO formaPagoCuotaDTO = new()
               {
                  Cuota = row["Cuota"].ToString(),
                  Fecha = Convert.ToDateTime(row["Fecha"]),
                  Monto = Convert.ToDecimal(row["Monto"])
               };
               invoiceDTO.FormaPagoCuotas.Add(formaPagoCuotaDTO);
            }
         }

         return invoiceDTO;
      }
      public DespatchAdviceDTO CreateDespatchAdviceDTOFromData(DataSet data)
      {
         DespatchAdviceDTO despatchAdviceDTO = null;
         DataTable dataTable = data.Tables["guiaRemision"];

         foreach (DataRow row in dataTable.Rows)
         {
            if (despatchAdviceDTO == null)
            {
               despatchAdviceDTO = new DespatchAdviceDTO
               {
                  TipoComprobante = Convert.ToInt32(row["TipoComprobante"]),
                  Serie = row["Serie"].ToString(),
                  Numero = row["Numero"].ToString(),
                  Fecha = Convert.ToDateTime(row["Fecha"]),
                  Hora = DateTime.Now,
                  EmpresaRUC = row["EmpresaRUC"].ToString(),
                  EmpresaNombre = row["EmpresaNombre"].ToString(),
                  EmpresaNombreComercial = row["EmpresaNombreComercial"].ToString(),
                  ProveedorNombre = row["ProveedorNombre"].ToString(),
                  ProveedorTipoDocumento = row["ProveedorTipoDocumento"].ToString(),
                  ProveedorNumeroDocumento = row["ProveedorNumeroDocumento"].ToString(),
                  ClienteNombre = row["ClienteNombre"].ToString(),
                  ClienteTipoDocumento = row["ClienteTipoDocumento"].ToString(),
                  ClienteNumeroDocumento = row["ClienteNumeroDocumento"].ToString(),
                  MotivoTrasladoCodigo = row["MotivoTrasladoCodigo"].ToString(),
                  EsTrasladoDesdeProveedor = row["EsTrasladoDesdeProveedor"].ToString(),
                  PesoBrutoTotal = Convert.ToDecimal(row["PesoBrutoTotal"]),
                  FechaTraslado = Convert.ToDateTime(row["FechaTraslado"]),
                  VehiculoPlaca = row["VehiculoPlaca"].ToString(),
                  VehiculoPlaca2 = row["VehiculoPlaca2"].ToString(),
                  ConductorTipoDocumento = row["ConductorTipoDocumento"].ToString(),
                  ConductorNombre = row["ConductorNombre"].ToString(),
                  ConductorApellidoPaternoMaterno = row["ConductorApellidoPaternoMaterno"].ToString(),
                  ConductorNumeroDocumento = row["ConductorNumeroDocumento"].ToString(),
                  ConductorNumeroLicencia = row["ConductorNumeroLicencia"].ToString(),
                  PuntoPartidaUbigeo = row["PuntoPartidaUbigeo"].ToString(),
                  PuntoPartidaDireccion = row["PuntoPartidaDireccion"].ToString(),
                  PuntoPartidaCodigoSunat = row["PuntoPartidaCodigoSunat"].ToString(),
                  PuntoLlegadaUbigeo = row["PuntoLlegadaUbigeo"].ToString(),
                  PuntoLlegadaDireccion = row["PuntoLlegadaDireccion"].ToString(),
                  PuntoLlegadaCodigoSunat = row["PuntoLlegadaCodigoSunat"].ToString(),
                  TipoDocumentoComercialReferencia = row["TipoDocumentoComercialReferencia"].ToString(),
                  SerieDocumentoReferencia = row["SerieDocumentoReferencia"].ToString(),
                  NumeroDocumentoReferencia = row["NumeroDocumentoReferencia"].ToString()
               };

            }

            DespatchLineDTO despatchLineDTO = new()
            {
               Codigo = row["Codigo"].ToString(),
               Descripcion = row["Descripcion"].ToString(),
               UnidadMedida = row["UnidadMedida"].ToString(),
               Cantidad = Convert.ToDecimal(row["Cantidad"]),
            };
            despatchAdviceDTO.Items.Add(despatchLineDTO);
         }

         return despatchAdviceDTO;
      }
      private ResponseDTO executeRequest(RestClient restClient, RestRequest restRequest) {
         var responseData = restClient.Execute<ResponseDTO>(restRequest);

         if (responseData.StatusCode != HttpStatusCode.OK)
         {
            Logger.Info("executeRequest:: [StatusCode=" + responseData.StatusCode.ToString() + "]");
            Logger.Info("executeRequest:: [ErrorMessage=" + responseData.ErrorMessage + "]");
            Logger.Info("executeRequest:: [Content=" + responseData.Content + "]");

            if (responseData.ErrorException is WebException)
            {

               WebException webException = (WebException)responseData.ErrorException;

               Logger.Info("executeRequest:: [ErrorException=" + responseData.ErrorException.ToString() + "]");

               switch (webException.Status)
               {
                  case WebExceptionStatus.ConnectFailure:
                     throw new Exception(SR.GetString("FE_servicio_no_disponible"));
                  default:
                     throw new Exception(webException.Message);
               }
            }

            throw new Exception(responseData.ErrorMessage);
         }

         return responseData.Data;
      }
      private string obtenerEstadoSunat(string estadoComprobante)
      {

         string codigoEstado = string.Empty;

         switch (estadoComprobante)
         {
            case EstadoComprobante.GENERADO:
               codigoEstado = EstadoSunatConstante.PENDIENTE_ENVIO;
               break;
            case EstadoComprobante.ENVIADO_VERIFICACION:
               codigoEstado = EstadoSunatConstante.VERIFICACION;
               break;
            case EstadoComprobante.ARCHIVO_CON_ERROR:
               codigoEstado = EstadoSunatConstante.ARCHIVO_CON_ERROR;
               break;
            case EstadoComprobante.ENVIADO_ACEPTADO:
               codigoEstado = EstadoSunatConstante.APROBADO;
               break;
            case EstadoComprobante.ENVIADO_ACEPTADO_CON_OBS:
               codigoEstado = EstadoSunatConstante.APROBADO_CON_OBS;
               break;
            case EstadoComprobante.ENVIADO_RECHAZADO:
               codigoEstado = EstadoSunatConstante.RECHAZADO;
               break;
         }

         return codigoEstado;
      }
      public string GenerarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId)
      {

         try
         {
            DataSet dataSet = obtenerDataPorTipoDocumento(tipoDocumentoComercial, comprobanteId);

            InvoiceDTO invoiceDTO = CreateInvoiceDTOFromData(dataSet);
            invoiceDTO.EnvioEnLinea = SiNoConstante.N;

            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/documento/GenerarDocumento", Method.POST) { RequestFormat = DataFormat.Json };
            var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

            var varJson = JsonConvert.SerializeObject(invoiceDTO, settings);
            request.AddParameter("application/json", varJson, ParameterType.RequestBody);

            Logger.Info("GenerarDocumento:: [baseAddress="+ baseAddress + "," +
               "Resource=]" + request.Resource);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO()
            {
               Codigo = EstadoSunatConstante.PENDIENTE_ENVIO
            });

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  ComprobanteDTO comprobanteDTO = new()
                  {
                     ComprobanteId = comprobanteId,
                     ComprobanteElectronicoId = responseDTO.ComprobanteElectronicoId,
                     CodigoHash = responseDTO.HashCode,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };

                  comprobanteProxy.ActualizarEmisionElectronica(comprobanteDTO);
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  NotaCreditoDTO notaCreditoDTO = new()
                  {
                     NotaCreditoId = comprobanteId,
                     ComprobanteElectronicoId = responseDTO.ComprobanteElectronicoId,
                     CodigoHash = responseDTO.HashCode,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };
                  notaCreditoProxy.ActualizarEmisionElectronica(notaCreditoDTO);
                  break;
            }

            return SR.GetString("FE_documento_generado");

         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
      }
      public string EnviarDocumento(string baseAddress, string tipoDocumentoComercial, int comprobanteId)
      {
         int comprobanteElectronicoId = 0;

         try
         {

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = comprobanteId });
                  comprobanteElectronicoId = comprobanteDTO.ComprobanteElectronicoId.Value;
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = comprobanteId });
                  comprobanteElectronicoId = notaCreditoDTO.ComprobanteElectronicoId.Value;
                  break;
            }

            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/documento/EnviarDocumento", Method.POST) { RequestFormat = DataFormat.Json };
            var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

            var varJson = JsonConvert.SerializeObject(comprobanteElectronicoId, settings);
            request.AddParameter("application/json", varJson, ParameterType.RequestBody);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            string codigoEstado = obtenerEstadoSunat(responseDTO.EstadoComprobante);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO()
            {
               Codigo = codigoEstado
            });

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  comprobanteProxy.ActualizarEstadoSunat(new ComprobanteDTO()
                  {
                     ComprobanteId = comprobanteId,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  });
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  notaCreditoProxy.ActualizarEstadoSunat(new NotaCreditoDTO()
                  {
                     NotaCreditoId = comprobanteId,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  });
                  break;
            }

            return responseDTO.SunatDescripcion;

         }
         catch (Exception ex)
         {
            Logger.Error(ex);
            throw;
         }
      }
      public string ObtenerResumenTicket(string baseAddress, string ruc, DateTime fecha)
      {

         try
         {
            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/gestion", Method.GET);

            request.AddParameter("ruc", ruc);
            request.AddParameter("fecha", fecha.ToString("o"));

            var responseData = _client.Execute(request);

            if (responseData.StatusCode != HttpStatusCode.OK)
            {
               if (responseData.ErrorException is WebException)
               {
                  WebException webException = (WebException)responseData.ErrorException;
                  switch (webException.Status)
                  {
                     case WebExceptionStatus.ConnectFailure:
                        throw new Exception(SR.GetString("EX_fact_e_servicio_no_disponible"));
                     default:
                        throw new Exception(webException.Message);
                  }
               }

               throw new Exception(responseData.ErrorMessage);
            }

            string output = JsonConvert.DeserializeObject<string>(responseData.Content);

            return output;
         }
         catch (Exception ex)
         {
            Logger.Error(ex.ToString());
            throw;
         }
      }
      public string EnviarResumen(string baseAddress, int empresaId, string ruc, DateTime fecha)
      {
         Dictionary<string, string> parameters = new();

         try
         {
            parameters.Add("empresaId", empresaId.ToString());
            parameters.Add("fechaDesde", string.Format(StringFormatConstant.DATE_ISO,fecha));

            byte[] fileBytes = appHelper.ExecuteExportXml("sp_fact_e_resumenBoleta", parameters);

            DataTable dataTable = new();

            dataTable.ReadXml(new MemoryStream(fileBytes));

            ResumenDTO resumenDTO = dataToResumenDTO(fecha, dataTable);

            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/resumen/", Method.POST) { RequestFormat = DataFormat.Json };
            var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

            var varJson = JsonConvert.SerializeObject(resumenDTO, settings);
            request.AddParameter("application/json", varJson, ParameterType.RequestBody);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO()
            {
               Codigo = EstadoSunatConstante.VERIFICACION
            });

            List<int> documentoElectronicoIdList = resumenDTO.Items.Where(x => x.TipoDocumento == TipoDocumentoComercialConstante.BOLETA_VENTA).
               Select(x => x.DocumentoElectronicoId).ToList();

            ComprobanteDTO comprobanteDTO = new()
            {
               ComprobanteElectronicoIdList = documentoElectronicoIdList,
               EstadoSunatId = estadoSunatDTO.EstadoSunatId
            };
            comprobanteProxy.ActualizarPorComprobanteElectronicoIdList(comprobanteDTO);

            documentoElectronicoIdList = resumenDTO.Items.Where(x => x.TipoDocumento == TipoDocumentoComercialConstante.NOTA_CREDITO).
               Select(x => x.DocumentoElectronicoId).ToList();

            if (documentoElectronicoIdList != null)
            {
               NotaCreditoDTO notaCreditoDTO = new()
               {
                  ComprobanteElectronicoIdList = documentoElectronicoIdList,
                  EstadoSunatId = estadoSunatDTO.EstadoSunatId
               };
               notaCreditoProxy.ActualizarPorComprobanteElectronicoIdList(notaCreditoDTO);
            }

            return SR.GetString("FE_resumen_enviado");
         }
         catch (Exception ex)
         {
            Logger.Error(ex.ToString());
            throw;
         }
      }
      private ResumenDTO dataToResumenDTO(DateTime fecha, DataTable data)
      {
         ResumenDTO resumenDTO = null;

         foreach (DataRow row in data.Rows)
         {
            if (resumenDTO == null)
            {
               resumenDTO = new ResumenDTO
               {
                  CorrelativoEnvio = 1,
                  FechaComprobante = fecha,
                  FechaResumen = fecha,
                  ProveedorNombre = row["ProveedorNombre"].ToString()
               };
               resumenDTO.ProveedorNombre = row["ProveedorNombre"].ToString();
               resumenDTO.ProveedorTipoDocumento = row["ProveedorTipoDocumento"].ToString();
               resumenDTO.ProveedorRUC = row["ProveedorRUC"].ToString();
            }

            ResumenItemDTO resumenItemDTO = new()
            {
               DocumentoElectronicoId = Convert.ToInt32(row["DocumentoElectronicoId"]),
               TipoDocumento = row["TipoDocumento"].ToString(),
               Serie = row["Serie"].ToString(),
               Correlativo = row["Correlativo"].ToString(),
               TipoMoneda = row["TipoMoneda"].ToString(),
               ClienteTipoDocumentoIdentidad = row["ClienteTipoDocumentoIdentidad"].ToString(),
               ClienteNumeroDocumentoIdentidad = row["ClienteNumeroDocumentoIdentidad"].ToString(),
               TotalGravado = Convert.ToDecimal(row["TotalGravado"]),
               TotalInafecto = Convert.ToDecimal(row["TotalInafecto"]),
               TotalExonerado = Convert.ToDecimal(row["TotalExonerado"]),
               TotalOtrosCargos = Convert.ToDecimal(row["TotalOtrosCargos"]),
               TotalIGV = Convert.ToDecimal(row["TotalIGV"]),
               TotalISC = Convert.ToDecimal(row["TotalISC"]),
               TotalOtrosTributos = Convert.ToDecimal(row["TotalOtrosTributos"]),
               Total = Convert.ToDecimal(row["Total"]),
               TotalVentaGratuita = Convert.ToDecimal(row["TotalVentaGratuita"]),
               TipoDocumentoModifica = row["TipoDocumentoModifica"].ToString(),
               SerieModifica = row["SerieModifica"].ToString(),
               CorrelativoModifica = row["CorrelativoModifica"].ToString(),
               RegimenPercepcion = row["RegimenPercepcion"].ToString(),
               BaseImponiblePercepcion = Convert.ToDecimal(row["BaseImponiblePercepcion"]),
               MontoPercepcion = Convert.ToDecimal(row["MontoPercepcion"]),
               MontoTotalPercepcion = Convert.ToDecimal(row["MontoTotalPercepcion"]),
               Estado = row["Estado"].ToString()
            };

            resumenDTO.Items.Add(resumenItemDTO);
         }

         return resumenDTO;
      }
      public string ActualizarResumenCDR(string baseAddress, string ruc, string numeroTicket, List<int> comprobanteElectronicoIdList,
         List<int> notaElectronicaIdList)
      {
         try
         {

            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/resumen/ActualizarCDRPorTicket", Method.POST) { RequestFormat = DataFormat.Json };
            var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

            ResumenDTO resumenDTO = new()
            {
               ProveedorRUC = ruc,
               NumeroTicket = numeroTicket
            };

            var varJson = JsonConvert.SerializeObject(resumenDTO, settings);
            request.AddParameter("application/json", varJson, ParameterType.RequestBody);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            string codigoEstado = obtenerEstadoSunat(responseDTO.EstadoComprobante);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO()
            {
               Codigo = codigoEstado
            });

            ComprobanteDTO comprobanteDTO = new()
            {
               ComprobanteElectronicoIdList = comprobanteElectronicoIdList,
               EstadoSunatId = estadoSunatDTO.EstadoSunatId
            };
            comprobanteProxy.ActualizarPorComprobanteElectronicoIdList(comprobanteDTO);

            if (notaElectronicaIdList != null)
            {
               //NotaCreditoDTO notaCreditoDTO = new NotaCreditoDTO
               //{
               //   ComprobanteElectronicoIdList = comprobanteElectronicoIdList,
               //   EstadoSunatId = estadoSunatDTO.EstadoSunatId
               //};
               //notaCreditoProxy.ActualizarPorComprobanteElectronicoIdList(comprobanteDTO);
            }

            return responseDTO.SunatDescripcion;
         }
         catch (Exception ex)
         {
            Logger.Error(ex.ToString());
            throw;
         }
      }
      public string ActualizarCDR(string baseAddress, string tipoDocumentoComercial, int comprobanteId)
      {

         try
         {

            int comprobanteElectronicoId = 0;
            string codigoHash = string.Empty;
            string resource = "api/documento/ActualizarCDR";

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = comprobanteId });
                  comprobanteElectronicoId = comprobanteDTO.ComprobanteElectronicoId.Value;
                  codigoHash = comprobanteDTO.CodigoHash;
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = comprobanteId });
                  comprobanteElectronicoId = notaCreditoDTO.ComprobanteElectronicoId.Value;
                  codigoHash = notaCreditoDTO.CodigoHash;
                  break;

               case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
                  GuiaRemisionDTO guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = comprobanteId });
                  comprobanteElectronicoId = guiaRemisionDTO.ComprobanteElectronicoId.Value;
                  resource = "api/guia/ActualizarCDR";
                  break;

            }

            RestClient _client = new(baseAddress);
            var request = new RestRequest(resource, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("documentoId", comprobanteElectronicoId, ParameterType.GetOrPost);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            string codigoEstado = obtenerEstadoSunat(responseDTO.EstadoComprobante);

            EstadoSunatDTO estadoSunatDTO = estadoSunatProxy.ObtenerPorCodigo(new EstadoSunatDTO()
            {
               Codigo = codigoEstado
            });

            switch (tipoDocumentoComercial)
            {
               case TipoDocumentoComercialConstante.FACTURA:
               case TipoDocumentoComercialConstante.BOLETA_VENTA:
                  ComprobanteDTO comprobanteDTO = new()
                  {
                     ComprobanteId = comprobanteId,
                     ComprobanteElectronicoId = comprobanteElectronicoId,
                     CodigoHash = codigoHash,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };

                  comprobanteProxy.ActualizarEmisionElectronica(comprobanteDTO);
                  break;

               case TipoDocumentoComercialConstante.NOTA_CREDITO:
                  NotaCreditoDTO notaCreditoDTO = new()
                  {
                     NotaCreditoId = comprobanteId,
                     ComprobanteElectronicoId = comprobanteElectronicoId,
                     CodigoHash = codigoHash,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };
                  notaCreditoProxy.ActualizarEmisionElectronica(notaCreditoDTO);
                  break;

               case TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE:
                  GuiaRemisionDTO guiaRemisionDTO = new()
                  {
                     GuiaRemisionId = comprobanteId,
                     ComprobanteElectronicoId = comprobanteElectronicoId,
                     EstadoSunatId = estadoSunatDTO.EstadoSunatId
                  };
                  guiaRemisionProxy.ActualizarEmisionElectronica(guiaRemisionDTO);
                  break;
            }

            return responseDTO.SunatDescripcion;

         }
         catch (Exception ex)
         {
            Logger.Error(ex.ToString());
            throw;
         }
      }
      public ResponseDTO ConsultarDocumentoEnviado(string baseAddress, int comprobanteElectronicoId)
      {
         try
         {

            RestClient _client = new(baseAddress);
            var request = new RestRequest("api/documento/ConsultarDocumentoEnviado", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("documentoId", comprobanteElectronicoId, ParameterType.GetOrPost);

            ResponseDTO responseDTO = executeRequest(_client, request);

            if (!responseDTO.Success)
               throw new Exception(responseDTO.Error);

            return responseDTO;

         }
         catch (Exception ex)
         {
            Logger.Error(ex.ToString());
            throw;
         }
      }
   }
}
