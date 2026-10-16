using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalRemuneracionProxy : WebApiProxyClient, IPersonalRemuneracionProxy
	{
		private const string API_CONTROLLER = "PersonalRemuneracion";
		public PersonalRemuneracionDTO GetById(PersonalRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<PersonalRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalRemuneracionDTO> Search(PersonalRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalRemuneracionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalRemuneracionDTO SingleSearch(PersonalRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<PersonalRemuneracionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalRemuneracionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalRemuneracionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
