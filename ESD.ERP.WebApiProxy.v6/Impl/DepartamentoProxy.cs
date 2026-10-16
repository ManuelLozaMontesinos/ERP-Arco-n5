using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class DepartamentoProxy : WebApiProxyClient, IDepartamentoProxy
	{
		private const string API_CONTROLLER = "Departamento";
		public DepartamentoDTO GetById(DepartamentoDTO dto)
		{
			var result = RestClient.PostApi<DepartamentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<DepartamentoDTO> Search(DepartamentoDTO dto)
		{
			var result = RestClient.PostApi<List<DepartamentoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public DepartamentoDTO SingleSearch(DepartamentoDTO dto)
		{
			var result = RestClient.PostApi<DepartamentoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(DepartamentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(DepartamentoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(DepartamentoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
