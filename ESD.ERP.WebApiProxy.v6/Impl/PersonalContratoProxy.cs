using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalContratoProxy : WebApiProxyClient, IPersonalContratoProxy
	{
		private const string API_CONTROLLER = "PersonalContrato";
		public PersonalContratoDTO GetById(PersonalContratoDTO dto)
		{
			var result = RestClient.PostApi<PersonalContratoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalContratoDTO> Search(PersonalContratoDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalContratoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalContratoDTO SingleSearch(PersonalContratoDTO dto)
		{
			var result = RestClient.PostApi<PersonalContratoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalContratoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalContratoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalContratoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
