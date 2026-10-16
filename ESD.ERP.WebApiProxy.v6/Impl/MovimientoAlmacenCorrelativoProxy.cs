using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenCorrelativoProxy : WebApiProxyClient, IMovimientoAlmacenCorrelativoProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacenCorrelativo";
		public MovimientoAlmacenCorrelativoDTO GetById(MovimientoAlmacenCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenCorrelativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenCorrelativoDTO> Search(MovimientoAlmacenCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenCorrelativoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenCorrelativoDTO SingleSearch(MovimientoAlmacenCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenCorrelativoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenCorrelativoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenCorrelativoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
