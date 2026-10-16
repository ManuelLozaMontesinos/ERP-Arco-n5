using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RegimenPensionarioComisionProxy : WebApiProxyClient, IRegimenPensionarioComisionProxy
	{
		private const string API_CONTROLLER = "RegimenPensionarioComision";
		public RegimenPensionarioComisionDTO GetById(RegimenPensionarioComisionDTO dto)
		{
			var result = RestClient.PostApi<RegimenPensionarioComisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RegimenPensionarioComisionDTO> Search(RegimenPensionarioComisionDTO dto)
		{
			var result = RestClient.PostApi<List<RegimenPensionarioComisionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RegimenPensionarioComisionDTO SingleSearch(RegimenPensionarioComisionDTO dto)
		{
			var result = RestClient.PostApi<RegimenPensionarioComisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RegimenPensionarioComisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RegimenPensionarioComisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RegimenPensionarioComisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
