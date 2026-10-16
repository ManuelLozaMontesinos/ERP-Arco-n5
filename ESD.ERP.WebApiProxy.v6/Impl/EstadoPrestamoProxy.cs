using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoPrestamoProxy : WebApiProxyClient, IEstadoPrestamoProxy
	{
		private const string API_CONTROLLER = "EstadoPrestamo";
		public EstadoPrestamoDTO GetById(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPrestamoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoPrestamoDTO> Search(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoPrestamoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoPrestamoDTO SingleSearch(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPrestamoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoPrestamoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoPrestamoDTO ObtenerPorCodigo(EstadoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPrestamoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
