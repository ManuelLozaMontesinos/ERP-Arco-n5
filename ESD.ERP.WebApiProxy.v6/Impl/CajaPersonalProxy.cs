using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CajaPersonalProxy : WebApiProxyClient, ICajaPersonalProxy
	{
		private const string API_CONTROLLER = "CajaPersonal";
		public CajaPersonalDTO GetById(CajaPersonalDTO dto)
		{
			var result = RestClient.PostApi<CajaPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CajaPersonalDTO> Search(CajaPersonalDTO dto)
		{
			var result = RestClient.PostApi<List<CajaPersonalDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CajaPersonalDTO SingleSearch(CajaPersonalDTO dto)
		{
			var result = RestClient.PostApi<CajaPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CajaPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CajaPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CajaPersonalDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
