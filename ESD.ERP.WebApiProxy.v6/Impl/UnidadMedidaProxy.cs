using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class UnidadMedidaProxy : WebApiProxyClient, IUnidadMedidaProxy
	{
		private const string API_CONTROLLER = "UnidadMedida";
		public UnidadMedidaDTO GetById(UnidadMedidaDTO dto)
		{
			var result = RestClient.PostApi<UnidadMedidaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<UnidadMedidaDTO> Search(UnidadMedidaDTO dto)
		{
			var result = RestClient.PostApi<List<UnidadMedidaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public UnidadMedidaDTO SingleSearch(UnidadMedidaDTO dto)
		{
			var result = RestClient.PostApi<UnidadMedidaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(UnidadMedidaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(UnidadMedidaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(UnidadMedidaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public UnidadMedidaDTO ObtenerPorCodigo(UnidadMedidaDTO dto)
      {
			var result = RestClient.PostApi<UnidadMedidaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
   }
}
