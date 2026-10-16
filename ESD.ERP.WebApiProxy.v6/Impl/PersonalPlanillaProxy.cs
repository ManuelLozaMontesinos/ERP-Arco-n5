using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalPlanillaProxy : WebApiProxyClient, IPersonalPlanillaProxy
	{
		private const string API_CONTROLLER = "PersonalPlanilla";
		public PersonalPlanillaDTO GetById(PersonalPlanillaDTO dto)
		{
			var result = RestClient.PostApi<PersonalPlanillaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalPlanillaDTO> Search(PersonalPlanillaDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalPlanillaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalPlanillaDTO SingleSearch(PersonalPlanillaDTO dto)
		{
			var result = RestClient.PostApi<PersonalPlanillaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalPlanillaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalPlanillaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalPlanillaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
