using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EmpresaSedeProxy : WebApiProxyClient, IEmpresaSedeProxy
	{
		private const string API_CONTROLLER = "EmpresaSede";
		public EmpresaSedeDTO GetById(EmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<EmpresaSedeDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EmpresaSedeDTO> Search(EmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<List<EmpresaSedeDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EmpresaSedeDTO SingleSearch(EmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<EmpresaSedeDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EmpresaSedeDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
