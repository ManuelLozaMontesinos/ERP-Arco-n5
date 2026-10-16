using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteTrazaProxy : WebApiProxyClient, IComprobanteTrazaProxy
	{
		private const string API_CONTROLLER = "ComprobanteTraza";
		public ComprobanteTrazaDTO GetById(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteTrazaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteTrazaDTO> Search(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteTrazaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteTrazaDTO SingleSearch(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteTrazaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteTrazaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public ComprobanteTrazaDTO ObtenerPorCodigo(ComprobanteTrazaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteTrazaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
