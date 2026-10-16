using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenCompraProxy : WebApiProxyClient, IOrdenCompraProxy
	{
		private const string API_CONTROLLER = "OrdenCompra";
		public OrdenCompraDTO GetById(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenCompraDTO> Search(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenCompraDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenCompraDTO SingleSearch(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public OrdenCompraDTO ObtenerPorCodigo(OrdenCompraDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
		public void SolicitarAutorizacion(OrdenCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarAutorizacion"), dto);
		}
		public void SolicitarModificacion(OrdenCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarModificacion"), dto);
		}
		public void Aprobar(OrdenCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
		public void Desaprobar(OrdenCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
      public void CambiarEstado(OrdenCompraDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "CambiarEstado"), dto);
		}
	}
}
