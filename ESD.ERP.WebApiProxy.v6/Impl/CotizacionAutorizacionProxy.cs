using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CotizacionAutorizacionProxy : WebApiProxyClient, ICotizacionAutorizacionProxy
	{
		private const string API_CONTROLLER = "CotizacionAutorizacion";
		public CotizacionAutorizacionDTO GetById(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<CotizacionAutorizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CotizacionAutorizacionDTO> Search(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<List<CotizacionAutorizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CotizacionAutorizacionDTO SingleSearch(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<CotizacionAutorizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CotizacionAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public List<CotizacionAutorizacionDTO> ListarPorTipoAutorizacionIdList(CotizacionAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<List<CotizacionAutorizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorTipoAutorizacionIdList"), dto);
			return result;
		}
      public void Solicitar(CotizacionAutorizacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Solicitar"), dto);
		}
      public void Aprobar(CotizacionAutorizacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
      public void Desaprobar(CotizacionAutorizacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
		public void Cancelar(CotizacionAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cancelar"), dto);
		}
	}
}
