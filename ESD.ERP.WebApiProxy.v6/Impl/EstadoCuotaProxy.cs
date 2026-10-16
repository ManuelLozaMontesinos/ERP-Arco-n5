using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoCuotaProxy : WebApiProxyClient, IEstadoCuotaProxy
	{
		private const string API_CONTROLLER = "EstadoCuota";
		public EstadoCuotaDTO GetById(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<EstadoCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoCuotaDTO> Search(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoCuotaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoCuotaDTO SingleSearch(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<EstadoCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoCuotaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoCuotaDTO ObtenerPorCodigo(EstadoCuotaDTO dto)
		{
			var result = RestClient.PostApi<EstadoCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
