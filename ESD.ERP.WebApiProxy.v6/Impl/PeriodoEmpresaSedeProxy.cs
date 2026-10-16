using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PeriodoEmpresaSedeProxy : WebApiProxyClient, IPeriodoEmpresaSedeProxy
	{
		private const string API_CONTROLLER = "PeriodoEmpresaSede";
		public PeriodoEmpresaSedeDTO GetById(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<PeriodoEmpresaSedeDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PeriodoEmpresaSedeDTO> Search(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<List<PeriodoEmpresaSedeDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PeriodoEmpresaSedeDTO SingleSearch(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<PeriodoEmpresaSedeDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PeriodoEmpresaSedeDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public List<EmpresaDTO> ObtenerEmpresaList(PeriodoEmpresaSedeDTO dto)
		{
			var result = RestClient.PostApi<List<EmpresaDTO>>(WebApiMethod.Create(API_CONTROLLER, "ObtenerEmpresaList"), dto);
			return result;
		}
	}
}
