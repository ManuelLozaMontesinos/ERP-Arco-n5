using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GuiaRemisionProxy : WebApiProxyClient, IGuiaRemisionProxy
	{
		private const string API_CONTROLLER = "GuiaRemision";
		public GuiaRemisionDTO GetById(GuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<GuiaRemisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GuiaRemisionDTO> Search(GuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<List<GuiaRemisionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GuiaRemisionDTO SingleSearch(GuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<GuiaRemisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void GenerarDesdeComprobante(GuiaRemisionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "GenerarDesdeComprobante"), dto);
		}
		public void Emitir(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Emitir"), dto);
		}
		public void EmitirSunat(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "EmitirSunat"), dto);
		}
		public void SolicitarModificacion(GuiaRemisionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarModificacion"), dto);
		}
      public void Aprobar(GuiaRemisionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
      public void Desaprobar(GuiaRemisionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
		public void ActualizarEmisionElectronica(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEmisionElectronica"), dto);
		}
		public void ActualizarEstado(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarEstado"), dto);
		}
		public void ActualizarPeso(GuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarPeso"), dto);
		}

    }
}
