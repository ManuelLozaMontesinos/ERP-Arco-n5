using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoGuiaRemisionProxy : WebApiProxyClient, IEstadoGuiaRemisionProxy
	{
		private const string API_CONTROLLER = "EstadoGuiaRemision";
		public EstadoGuiaRemisionDTO GetById(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<EstadoGuiaRemisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoGuiaRemisionDTO> Search(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoGuiaRemisionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoGuiaRemisionDTO SingleSearch(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<EstadoGuiaRemisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoGuiaRemisionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoGuiaRemisionDTO ObtenerPorCodigo(EstadoGuiaRemisionDTO dto)
		{
			var result = RestClient.PostApi<EstadoGuiaRemisionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
