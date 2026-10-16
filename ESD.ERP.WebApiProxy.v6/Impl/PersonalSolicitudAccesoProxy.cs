using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PersonalSolicitudAccesoProxy : WebApiProxyClient, IPersonalSolicitudAccesoProxy
	{
		private const string API_CONTROLLER = "PersonalSolicitudAcceso";
		public PersonalSolicitudAccesoDTO GetById(PersonalSolicitudAccesoDTO dto)
		{
			var result = RestClient.PostApi<PersonalSolicitudAccesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PersonalSolicitudAccesoDTO> Search(PersonalSolicitudAccesoDTO dto)
		{
			var result = RestClient.PostApi<List<PersonalSolicitudAccesoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PersonalSolicitudAccesoDTO SingleSearch(PersonalSolicitudAccesoDTO dto)
		{
			var result = RestClient.PostApi<PersonalSolicitudAccesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PersonalSolicitudAccesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PersonalSolicitudAccesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PersonalSolicitudAccesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void Aprobar(PersonalSolicitudAccesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
		public void Desaprobar(PersonalSolicitudAccesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
	}
}
