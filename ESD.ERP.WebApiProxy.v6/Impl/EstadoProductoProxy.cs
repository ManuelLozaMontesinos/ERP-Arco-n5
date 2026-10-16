using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoProductoProxy : WebApiProxyClient, IEstadoProductoProxy
	{
		private const string API_CONTROLLER = "EstadoProducto";
		public EstadoProductoDTO GetById(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<EstadoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoProductoDTO> Search(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoProductoDTO SingleSearch(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<EstadoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoProductoDTO ObtenerPorCodigo(EstadoProductoDTO dto)
		{
			var result = RestClient.PostApi<EstadoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
