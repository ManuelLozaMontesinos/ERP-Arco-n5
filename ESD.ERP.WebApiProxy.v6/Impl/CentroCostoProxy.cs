using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CentroCostoProxy : WebApiProxyClient, ICentroCostoProxy
	{
		private const string API_CONTROLLER = "CentroCosto";
		public CentroCostoDTO GetById(CentroCostoDTO dto)
		{
			var result = RestClient.PostApi<CentroCostoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CentroCostoDTO> Search(CentroCostoDTO dto)
		{
			var result = RestClient.PostApi<List<CentroCostoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CentroCostoDTO SingleSearch(CentroCostoDTO dto)
		{
			var result = RestClient.PostApi<CentroCostoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CentroCostoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CentroCostoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CentroCostoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
