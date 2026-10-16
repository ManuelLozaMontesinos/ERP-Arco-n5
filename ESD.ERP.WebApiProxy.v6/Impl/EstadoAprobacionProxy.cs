using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoAprobacionProxy : WebApiProxyClient, IEstadoAprobacionProxy
	{
		private const string API_CONTROLLER = "EstadoAprobacion";
		public EstadoAprobacionDTO GetById(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoAprobacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoAprobacionDTO> Search(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoAprobacionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoAprobacionDTO SingleSearch(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoAprobacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoAprobacionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoAprobacionDTO ObtenerPorCodigo(EstadoAprobacionDTO dto)
		{
			var result = RestClient.PostApi<EstadoAprobacionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
