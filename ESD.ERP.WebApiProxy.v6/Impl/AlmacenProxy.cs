using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AlmacenProxy : WebApiProxyClient, IAlmacenProxy
	{
		private const string API_CONTROLLER = "Almacen";
		public AlmacenDTO GetById(AlmacenDTO dto)
		{
			var result = RestClient.PostApi<AlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AlmacenDTO> Search(AlmacenDTO dto)
		{
			var result = RestClient.PostApi<List<AlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AlmacenDTO SingleSearch(AlmacenDTO dto)
		{
			var result = RestClient.PostApi<AlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AlmacenDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
