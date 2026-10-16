using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteProxy : WebApiProxyClient, IComprobanteProxy
	{
		private const string API_CONTROLLER = "Comprobante";
		public ComprobanteDTO GetById(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteDTO> Search(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteDTO SingleSearch(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void AplicarTasaPorMedioPago(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AplicarTasaPorMedioPago"), dto);
		}
		public void Emitir(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Emitir"), dto);
		}
		public void Cerrar(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cerrar"), dto);
		}
      public void ActualizarEmisionElectronica(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEmisionElectronica"), dto);
		}
      public List<ComprobanteDTO> ListarDocumentoEmitido(ComprobanteDTO dto)
      {
			var result = RestClient.PostApi<List<ComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarDocumentoEmitido"), dto);
			return result;
		}
      public void ActualizarEstadoSunat(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEstadoSunat"), dto);
		}
		public List<ComprobanteDTO> ListarResumenEstadoSunat(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarResumenEstadoSunat"), dto);
			return result;
		}
      public void ActualizarPorComprobanteElectronicoIdList(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarPorComprobanteElectronicoIdList"), dto);
		}
		public void EmitirCanje(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "EmitirCanje"), dto);
		}
		public void EliminarCanje(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "EliminarCanje"), dto);
		}
      public void ActualizarDetraccion(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarDetraccion"), dto);
		}
		public void ActualizarTipoComprobante(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarTipoComprobante"), dto);
		}
		public int AgregarComprobanteAnticipo(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "AgregarComprobanteAnticipo"), dto);
			return result;
		}
		public int ActualizarComprobanteAnticipo(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "ActualizarComprobanteAnticipo"), dto);
			return result;
		}
		public void AnularComprobanteAnticipo(ComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AnularComprobanteAnticipo"), dto);
		}
		public string ListarCobranzaGrid(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarCobranzaGrid"), dto);
			return result;
		}
      public List<ComprobanteDTO> BuscarDespacho(ComprobanteDTO dto)
      {
			var result = RestClient.PostApi<List<ComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, "BuscarDespacho"), dto);
			return result;
		}
      public void CerrarDespacho(ComprobanteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CerrarDespacho"), dto);
		}
		public List<ComprobanteDTO> BuscarPorEmpresaSede(ComprobanteDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, "BuscarPorEmpresaSede"), dto);
			return result;
		}
	}
}
