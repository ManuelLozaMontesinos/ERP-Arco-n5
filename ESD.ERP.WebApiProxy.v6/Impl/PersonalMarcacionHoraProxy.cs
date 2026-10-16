using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalMarcacionHoraProxy : WebApiProxyClient, IPersonalMarcacionHoraProxy
	{
		private const string API_CONTROLLER = "PersonalMarcacionHora";
		public PersonalMarcacionHoraDTO GetById(PersonalMarcacionHoraDTO dto)
		{
			var result = RestClient.PostApi<PersonalMarcacionHoraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalMarcacionHoraDTO> Search(PersonalMarcacionHoraDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalMarcacionHoraDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalMarcacionHoraDTO SingleSearch(PersonalMarcacionHoraDTO dto)
		{
			var result = RestClient.PostApi<PersonalMarcacionHoraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalMarcacionHoraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalMarcacionHoraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalMarcacionHoraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarLote(List<PersonalMarcacionHoraDTO> list)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
	}
}
