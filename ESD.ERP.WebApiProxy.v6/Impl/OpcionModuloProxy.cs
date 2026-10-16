using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OpcionModuloProxy : WebApiProxyClient, IOpcionModuloProxy
	{
		private const string API_CONTROLLER = "OpcionModulo";
		public OpcionModuloDTO GetById(OpcionModuloDTO dto)
		{
			var result = RestClient.PostApi<OpcionModuloDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OpcionModuloDTO> Search(OpcionModuloDTO dto)
		{
			var result = RestClient.PostApi<List<OpcionModuloDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OpcionModuloDTO SingleSearch(OpcionModuloDTO dto)
		{
			var result = RestClient.PostApi<OpcionModuloDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OpcionModuloDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OpcionModuloDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OpcionModuloDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
