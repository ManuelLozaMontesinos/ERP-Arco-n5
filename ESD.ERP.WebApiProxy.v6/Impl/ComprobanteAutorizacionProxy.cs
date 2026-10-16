using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteAutorizacionProxy : WebApiProxyClient, IComprobanteAutorizacionProxy
	{
		private const string API_CONTROLLER = "ComprobanteAutorizacion";
		public ComprobanteAutorizacionDTO GetById(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteAutorizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteAutorizacionDTO> Search(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteAutorizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteAutorizacionDTO SingleSearch(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteAutorizacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public List<ComprobanteAutorizacionDTO> ListarPorTipoAutorizacionIdList(ComprobanteAutorizacionDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteAutorizacionDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorTipoAutorizacionIdList"), dto);
			return result;
		}
		public void Solicitar(ComprobanteAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Solicitar"), dto);
		}
		public void Aprobar(ComprobanteAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
		public void Desaprobar(ComprobanteAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
		public void Cancelar(ComprobanteAutorizacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Cancelar"), dto);
		}
	}
}
