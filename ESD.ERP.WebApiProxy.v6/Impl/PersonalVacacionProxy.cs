using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalVacacionProxy : WebApiProxyClient, IPersonalVacacionProxy
	{
		private const string API_CONTROLLER = "PersonalVacacion";
		public PersonalVacacionDTO GetById(PersonalVacacionDTO dto)
		{
			var result = RestClient.PostApi<PersonalVacacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalVacacionDTO> Search(PersonalVacacionDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalVacacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalVacacionDTO SingleSearch(PersonalVacacionDTO dto)
		{
			var result = RestClient.PostApi<PersonalVacacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalVacacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalVacacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalVacacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
