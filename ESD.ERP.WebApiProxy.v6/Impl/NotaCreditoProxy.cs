using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class NotaCreditoProxy : WebApiProxyClient, INotaCreditoProxy
	{
		private const string API_CONTROLLER = "NotaCredito";
		public NotaCreditoDTO GetById(NotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<NotaCreditoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<NotaCreditoDTO> Search(NotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<List<NotaCreditoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public NotaCreditoDTO SingleSearch(NotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<NotaCreditoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(NotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(NotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(NotaCreditoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void Emitir(NotaCreditoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Emitir"), dto);
		}
      public void ActualizarEmisionElectronica(NotaCreditoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEmisionElectronica"), dto);
		}
		public void ActualizarEstadoSunat(NotaCreditoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEstadoSunat"), dto);
		}
      public void ActualizarPorComprobanteElectronicoIdList(NotaCreditoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarPorComprobanteElectronicoIdList"), dto);
		}
      public void Cerrar(NotaCreditoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cerrar"), dto);
	  }
		public void EmitirInterno(NotaCreditoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "EmitirInterno"), dto);
		}
   }
}
