using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalPeriodoLaboralProxy : WebApiProxyClient, IPersonalPeriodoLaboralProxy
	{
		private const string API_CONTROLLER = "PersonalPeriodoLaboral";
		public PersonalPeriodoLaboralDTO GetById(PersonalPeriodoLaboralDTO dto)
		{
			var result = RestClient.PostApi<PersonalPeriodoLaboralDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalPeriodoLaboralDTO> Search(PersonalPeriodoLaboralDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalPeriodoLaboralDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalPeriodoLaboralDTO SingleSearch(PersonalPeriodoLaboralDTO dto)
		{
			var result = RestClient.PostApi<PersonalPeriodoLaboralDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalPeriodoLaboralDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalPeriodoLaboralDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalPeriodoLaboralDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void ActualizarCese(PersonalPeriodoLaboralDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarCese"), dto);
		}
   }
}
