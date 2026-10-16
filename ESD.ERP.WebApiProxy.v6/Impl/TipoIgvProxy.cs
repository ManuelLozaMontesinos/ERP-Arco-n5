using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoIgvProxy : WebApiProxyClient, ITipoIgvProxy
	{
		private const string API_CONTROLLER = "TipoIgv";
		public TipoIgvDTO GetById(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<TipoIgvDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoIgvDTO> Search(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<List<TipoIgvDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoIgvDTO SingleSearch(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<TipoIgvDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoIgvDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoIgvDTO ObtenerPorCodigo(TipoIgvDTO dto)
		{
			var result = RestClient.PostApi<TipoIgvDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}