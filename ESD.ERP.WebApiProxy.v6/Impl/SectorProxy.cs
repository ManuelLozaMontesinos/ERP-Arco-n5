using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SectorProxy : WebApiProxyClient, ISectorProxy
	{
		private const string API_CONTROLLER = "Sector";
		public SectorDTO GetById(SectorDTO dto)
		{
			var result = RestClient.PostApi<SectorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SectorDTO> Search(SectorDTO dto)
		{
			var result = RestClient.PostApi<List<SectorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SectorDTO SingleSearch(SectorDTO dto)
		{
			var result = RestClient.PostApi<SectorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SectorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SectorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SectorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
