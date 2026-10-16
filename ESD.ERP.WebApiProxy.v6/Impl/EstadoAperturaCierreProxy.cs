using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoAperturaCierreProxy : WebApiProxyClient, IEstadoAperturaCierreProxy
	{
		private const string API_CONTROLLER = "EstadoAperturaCierre";
		public EstadoAperturaCierreDTO GetById(EstadoAperturaCierreDTO dto)
		{
			var result = RestClient.PostApi<EstadoAperturaCierreDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoAperturaCierreDTO> Search(EstadoAperturaCierreDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoAperturaCierreDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoAperturaCierreDTO SingleSearch(EstadoAperturaCierreDTO dto)
		{
			var result = RestClient.PostApi<EstadoAperturaCierreDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoAperturaCierreDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoAperturaCierreDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoAperturaCierreDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
