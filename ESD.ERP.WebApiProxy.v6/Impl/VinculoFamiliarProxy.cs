using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class VinculoFamiliarProxy : WebApiProxyClient, IVinculoFamiliarProxy
	{
		private const string API_CONTROLLER = "VinculoFamiliar";
		public VinculoFamiliarDTO GetById(VinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<VinculoFamiliarDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<VinculoFamiliarDTO> Search(VinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<List<VinculoFamiliarDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public VinculoFamiliarDTO SingleSearch(VinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<VinculoFamiliarDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(VinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(VinculoFamiliarDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(VinculoFamiliarDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
