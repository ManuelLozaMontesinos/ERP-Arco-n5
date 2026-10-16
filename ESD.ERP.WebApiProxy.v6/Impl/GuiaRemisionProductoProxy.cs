using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class GuiaRemisionProductoProxy : WebApiProxyClient, IGuiaRemisionProductoProxy
	{
		private const string API_CONTROLLER = "GuiaRemisionProducto";
		public GuiaRemisionProductoDTO GetById(GuiaRemisionProductoDTO dto)
		{
			var result = RestClient.PostApi<GuiaRemisionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<GuiaRemisionProductoDTO> Search(GuiaRemisionProductoDTO dto)
		{
			var result = RestClient.PostApi<List<GuiaRemisionProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public GuiaRemisionProductoDTO SingleSearch(GuiaRemisionProductoDTO dto)
		{
			var result = RestClient.PostApi<GuiaRemisionProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(GuiaRemisionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(GuiaRemisionProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(GuiaRemisionProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
