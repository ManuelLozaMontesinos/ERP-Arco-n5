using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class FamiliaProductoProxy : WebApiProxyClient, IFamiliaProductoProxy
	{
		private const string API_CONTROLLER = "FamiliaProducto";
		public FamiliaProductoDTO GetById(FamiliaProductoDTO dto)
		{
			var result = RestClient.PostApi<FamiliaProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<FamiliaProductoDTO> Search(FamiliaProductoDTO dto)
		{
			var result = RestClient.PostApi<List<FamiliaProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public FamiliaProductoDTO SingleSearch(FamiliaProductoDTO dto)
		{
			var result = RestClient.PostApi<FamiliaProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(FamiliaProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(FamiliaProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(FamiliaProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
