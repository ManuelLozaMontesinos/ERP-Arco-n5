using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalMarcacionProxy : WebApiProxyClient, IPersonalMarcacionProxy
	{
		private const string API_CONTROLLER = "PersonalMarcacion";
		public PersonalMarcacionDTO GetById(PersonalMarcacionDTO dto)
		{
			var result = RestClient.PostApi<PersonalMarcacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalMarcacionDTO> Search(PersonalMarcacionDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalMarcacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalMarcacionDTO SingleSearch(PersonalMarcacionDTO dto)
		{
			var result = RestClient.PostApi<PersonalMarcacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalMarcacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalMarcacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalMarcacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public string ListarMarcacionHoraGrid(PersonalMarcacionDTO dto)
      {
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarMarcacionHoraGrid"), dto);
			return result;
		}
      public void SincronizarMarcacion(PersonalMarcacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SincronizarMarcacion"), dto);
		}
      public List<PersonalDTO> ListarPersonalConHoras(PersonalMarcacionDTO dto)
      {
			var result = RestClient.PostApi<List<PersonalDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPersonalConHoras"), dto);
			return result;
		}
      public void ActualizarHoraLote(PersonalMarcacionDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarHoraLote"), dto);
		}
   }
}
