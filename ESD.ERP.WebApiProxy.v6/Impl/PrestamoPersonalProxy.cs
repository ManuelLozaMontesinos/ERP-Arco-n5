using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PrestamoPersonalProxy : WebApiProxyClient, IPrestamoPersonalProxy
	{
		private const string API_CONTROLLER = "PrestamoPersonal";
		public PrestamoPersonalDTO GetById(PrestamoPersonalDTO dto)
		{
			var result = RestClient.PostApi<PrestamoPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PrestamoPersonalDTO> Search(PrestamoPersonalDTO dto)
		{
			var result = RestClient.PostApi<List<PrestamoPersonalDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PrestamoPersonalDTO SingleSearch(PrestamoPersonalDTO dto)
		{
			var result = RestClient.PostApi<PrestamoPersonalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PrestamoPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PrestamoPersonalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PrestamoPersonalDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
