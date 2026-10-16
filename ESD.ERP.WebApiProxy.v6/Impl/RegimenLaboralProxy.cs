using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RegimenLaboralProxy : WebApiProxyClient, IRegimenLaboralProxy
	{
		private const string API_CONTROLLER = "RegimenLaboral";
		public RegimenLaboralDTO GetById(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<RegimenLaboralDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RegimenLaboralDTO> Search(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<List<RegimenLaboralDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RegimenLaboralDTO SingleSearch(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<RegimenLaboralDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RegimenLaboralDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public RegimenLaboralDTO ObtenerPorCodigo(RegimenLaboralDTO dto)
		{
			var result = RestClient.PostApi<RegimenLaboralDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
