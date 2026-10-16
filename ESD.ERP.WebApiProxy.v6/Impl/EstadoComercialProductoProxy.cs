using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoComercialProductoProxy : WebApiProxyClient, IEstadoComercialProductoProxy
	{
		private const string API_CONTROLLER = "EstadoComercialProducto";
		public EstadoComercialProductoDTO GetById(EstadoComercialProductoDTO dto)
		{
			var result = RestClient.PostApi<EstadoComercialProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoComercialProductoDTO> Search(EstadoComercialProductoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoComercialProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoComercialProductoDTO SingleSearch(EstadoComercialProductoDTO dto)
		{
			var result = RestClient.PostApi<EstadoComercialProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoComercialProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoComercialProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoComercialProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public EstadoComercialProductoDTO ObtenerPorCodigo(EstadoComercialProductoDTO dto)
      {
			var result = RestClient.PostApi<EstadoComercialProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
   }
}
