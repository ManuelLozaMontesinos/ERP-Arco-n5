using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoPlanillaProcesoProxy : WebApiProxyClient, IEstadoPlanillaProcesoProxy
	{
		private const string API_CONTROLLER = "EstadoPlanillaProceso";
		public EstadoPlanillaProcesoDTO GetById(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPlanillaProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoPlanillaProcesoDTO> Search(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoPlanillaProcesoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoPlanillaProcesoDTO SingleSearch(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPlanillaProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoPlanillaProcesoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoPlanillaProcesoDTO ObtenerPorCodigo(EstadoPlanillaProcesoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPlanillaProcesoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
