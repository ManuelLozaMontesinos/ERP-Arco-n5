using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoVacacionProxy : WebApiProxyClient, IEstadoVacacionProxy
	{
		private const string API_CONTROLLER = "EstadoVacacion";
		public EstadoVacacionDTO GetById(EstadoVacacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoVacacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoVacacionDTO> Search(EstadoVacacionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoVacacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoVacacionDTO SingleSearch(EstadoVacacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoVacacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoVacacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoVacacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoVacacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
