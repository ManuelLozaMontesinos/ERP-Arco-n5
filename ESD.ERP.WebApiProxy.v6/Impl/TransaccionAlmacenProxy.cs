using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TransaccionAlmacenProxy : WebApiProxyClient, ITransaccionAlmacenProxy
	{
		private const string API_CONTROLLER = "TransaccionAlmacen";
		public TransaccionAlmacenDTO GetById(TransaccionAlmacenDTO dto)
		{
			var result = RestClient.PostApi<TransaccionAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TransaccionAlmacenDTO> Search(TransaccionAlmacenDTO dto)
		{
			var result = RestClient.PostApi<List<TransaccionAlmacenDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TransaccionAlmacenDTO SingleSearch(TransaccionAlmacenDTO dto)
		{
			var result = RestClient.PostApi<TransaccionAlmacenDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TransaccionAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TransaccionAlmacenDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TransaccionAlmacenDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
