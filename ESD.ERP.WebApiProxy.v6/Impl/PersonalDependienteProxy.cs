using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalDependienteProxy : WebApiProxyClient, IPersonalDependienteProxy
	{
		private const string API_CONTROLLER = "PersonalDependiente";
		public PersonalDependienteDTO GetById(PersonalDependienteDTO dto)
		{
			var result = RestClient.PostApi<PersonalDependienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalDependienteDTO> Search(PersonalDependienteDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalDependienteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalDependienteDTO SingleSearch(PersonalDependienteDTO dto)
		{
			var result = RestClient.PostApi<PersonalDependienteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalDependienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalDependienteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalDependienteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void ActualizarBaja(PersonalDependienteDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarBaja"), dto);
		}
   }
}
