using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrigenRegistroProxy : WebApiProxyClient, IOrigenRegistroProxy
	{
		private const string API_CONTROLLER = "OrigenRegistro";
		public OrigenRegistroDTO GetById(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<OrigenRegistroDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrigenRegistroDTO> Search(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<List<OrigenRegistroDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrigenRegistroDTO SingleSearch(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<OrigenRegistroDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrigenRegistroDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public OrigenRegistroDTO ObtenerPorCodigo(OrigenRegistroDTO dto)
		{
			var result = RestClient.PostApi<OrigenRegistroDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
