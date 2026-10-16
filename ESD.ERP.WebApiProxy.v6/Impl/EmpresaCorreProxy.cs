using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EmpresaCorreProxy : WebApiProxyClient, IEmpresaCorreProxy
	{
		private const string API_CONTROLLER = "EmpresaCorre";
		public EmpresaCorrelativoDTO GetById(EmpresaCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<EmpresaCorrelativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EmpresaCorrelativoDTO> Search(EmpresaCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<List<EmpresaCorrelativoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EmpresaCorrelativoDTO SingleSearch(EmpresaCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<EmpresaCorrelativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EmpresaCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EmpresaCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EmpresaCorrelativoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
