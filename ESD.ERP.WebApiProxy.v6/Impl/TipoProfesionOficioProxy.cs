using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoProfesionOficioProxy : WebApiProxyClient, ITipoProfesionOficioProxy
	{
		private const string API_CONTROLLER = "TipoProfesionOficio";
		public TipoProfesionOficioDTO GetById(TipoProfesionOficioDTO dto)
		{
			var result = RestClient.PostApi<TipoProfesionOficioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoProfesionOficioDTO> Search(TipoProfesionOficioDTO dto)
		{
			var result = RestClient.PostApi<List<TipoProfesionOficioDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoProfesionOficioDTO SingleSearch(TipoProfesionOficioDTO dto)
		{
			var result = RestClient.PostApi<TipoProfesionOficioDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoProfesionOficioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoProfesionOficioDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoProfesionOficioDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
