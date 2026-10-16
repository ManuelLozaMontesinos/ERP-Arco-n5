using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class VariableCalculoProxy : WebApiProxyClient, IVariableCalculoProxy
	{
		private const string API_CONTROLLER = "VariableCalculo";
		public VariableCalculoDTO GetById(VariableCalculoDTO dto)
		{
			var result = RestClient.PostApi<VariableCalculoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<VariableCalculoDTO> Search(VariableCalculoDTO dto)
		{
			var result = RestClient.PostApi<List<VariableCalculoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public VariableCalculoDTO SingleSearch(VariableCalculoDTO dto)
		{
			var result = RestClient.PostApi<VariableCalculoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(VariableCalculoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(VariableCalculoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(VariableCalculoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
