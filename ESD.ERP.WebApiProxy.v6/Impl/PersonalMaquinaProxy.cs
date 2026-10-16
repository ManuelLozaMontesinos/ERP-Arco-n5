using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalMaquinaProxy : WebApiProxyClient, IPersonalMaquinaProxy
	{
		private const string API_CONTROLLER = "PersonalMaquina";
		public PersonalMaquinaDTO GetById(PersonalMaquinaDTO dto)
		{
			var result = RestClient.PostApi<PersonalMaquinaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalMaquinaDTO> Search(PersonalMaquinaDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalMaquinaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalMaquinaDTO SingleSearch(PersonalMaquinaDTO dto)
		{
			var result = RestClient.PostApi<PersonalMaquinaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalMaquinaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalMaquinaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalMaquinaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
    }
}
