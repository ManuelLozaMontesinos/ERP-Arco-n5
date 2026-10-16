using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AlmacenTransaccionProxy : WebApiProxyClient, IAlmacenTransaccionProxy
	{
		private const string API_CONTROLLER = "AlmacenTransaccion";
		public AlmacenTransaccionDTO GetById(AlmacenTransaccionDTO dto)
		{
			var result = RestClient.PostApi<AlmacenTransaccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AlmacenTransaccionDTO> Search(AlmacenTransaccionDTO dto)
		{
			var result = RestClient.PostApi<List<AlmacenTransaccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AlmacenTransaccionDTO SingleSearch(AlmacenTransaccionDTO dto)
		{
			var result = RestClient.PostApi<AlmacenTransaccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AlmacenTransaccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AlmacenTransaccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AlmacenTransaccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
