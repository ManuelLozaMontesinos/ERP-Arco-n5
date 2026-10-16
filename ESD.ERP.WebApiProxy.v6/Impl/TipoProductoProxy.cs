using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoProductoProxy : WebApiProxyClient, ITipoProductoProxy
	{
		private const string API_CONTROLLER = "TipoProducto";
		public TipoProductoDTO GetById(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<TipoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoProductoDTO> Search(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoProductoDTO SingleSearch(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<TipoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoProductoDTO ObtenerPorCodigo(TipoProductoDTO dto)
		{
			var result = RestClient.PostApi<TipoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
