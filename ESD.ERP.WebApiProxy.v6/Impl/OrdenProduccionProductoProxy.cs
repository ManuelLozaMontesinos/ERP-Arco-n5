using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenProduccionProductoProxy : WebApiProxyClient, IOrdenProduccionProductoProxy
	{
		private const string API_CONTROLLER = "OrdenProduccionProducto";
		public OrdenProduccionProductoDTO GetById(OrdenProduccionProductoDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenProduccionProductoDTO> Search(OrdenProduccionProductoDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenProduccionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenProduccionProductoDTO SingleSearch(OrdenProduccionProductoDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenProduccionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenProduccionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenProduccionProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
        public void actualizarInsumos(OrdenProduccionProductoDTO dto)
        {
            RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "actualizarInsumos"), dto);
        }
    }
}
