using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class LeadSourceProxy : WebApiProxyClient, ILeadSourceProxy
	{
		private const string API_CONTROLLER = "LeadSource";
		public LeadSourceDTO GetById(LeadSourceDTO dto)
		{
			var result = RestClient.PostApi<LeadSourceDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<LeadSourceDTO> Search(LeadSourceDTO dto)
		{
			var result = RestClient.PostApi<List<LeadSourceDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public LeadSourceDTO SingleSearch(LeadSourceDTO dto)
		{
			var result = RestClient.PostApi<LeadSourceDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(LeadSourceDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(LeadSourceDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(LeadSourceDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
