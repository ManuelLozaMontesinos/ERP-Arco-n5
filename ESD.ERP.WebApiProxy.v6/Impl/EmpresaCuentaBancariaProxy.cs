using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EmpresaCuentaBancariaProxy : WebApiProxyClient, IEmpresaCuentaBancariaProxy
	{
		private const string API_CONTROLLER = "EmpresaCuentaBancaria";
		public EmpresaCuentaBancariaDTO GetById(EmpresaCuentaBancariaDTO dto)
		{
			var result = RestClient.PostApi<EmpresaCuentaBancariaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EmpresaCuentaBancariaDTO> Search(EmpresaCuentaBancariaDTO dto)
		{
			var result = RestClient.PostApi<List<EmpresaCuentaBancariaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EmpresaCuentaBancariaDTO SingleSearch(EmpresaCuentaBancariaDTO dto)
		{
			var result = RestClient.PostApi<EmpresaCuentaBancariaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EmpresaCuentaBancariaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EmpresaCuentaBancariaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EmpresaCuentaBancariaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
