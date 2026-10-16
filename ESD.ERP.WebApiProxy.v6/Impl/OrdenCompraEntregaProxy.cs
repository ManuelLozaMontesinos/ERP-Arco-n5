using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenCompraEntregaProxy : WebApiProxyClient, IOrdenCompraEntregaProxy
	{
		private const string API_CONTROLLER = "OrdenCompraEntrega";
		public OrdenCompraEntregaDTO GetById(OrdenCompraEntregaDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenCompraEntregaDTO> Search(OrdenCompraEntregaDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenCompraEntregaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenCompraEntregaDTO SingleSearch(OrdenCompraEntregaDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenCompraEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenCompraEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenCompraEntregaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void AgregarDesdeOrdenCompraProducto(OrdenCompraEntregaDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarDesdeOrdenCompraProducto"), dto);
		}
   }
}
