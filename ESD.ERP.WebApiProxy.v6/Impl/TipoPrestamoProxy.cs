using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoPrestamoProxy : WebApiProxyClient, ITipoPrestamoProxy
	{
		private const string API_CONTROLLER = "TipoPrestamo";
		public TipoPrestamoDTO GetById(TipoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<TipoPrestamoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoPrestamoDTO> Search(TipoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoPrestamoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoPrestamoDTO SingleSearch(TipoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<TipoPrestamoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoPrestamoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoPrestamoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
