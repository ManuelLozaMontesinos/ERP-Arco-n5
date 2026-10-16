using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteProductoProxy : WebApiProxyClient, IComprobanteProductoProxy
	{
		private const string API_CONTROLLER = "ComprobanteProducto";
		public ComprobanteProductoDTO GetById(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteProductoDTO> Search(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteProductoDTO SingleSearch(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void AgregarCotizacionProducto(ComprobanteProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarCotizacionProducto"), dto);
		}
      public List<ComprobanteProductoDTO> ListarPendienteCanje(ComprobanteProductoDTO dto)
      {
			var result = RestClient.PostApi<List<ComprobanteProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPendienteCanje"), dto);
			return result;
		}
      public List<ComprobanteProductoDTO> ListarPorIdList(ComprobanteProductoDTO dto)
      {
			var result = RestClient.PostApi<List<ComprobanteProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorIdList"), dto);
			return result;
		}
		public void AgregarComprobanteProductoCanje(ComprobanteProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarComprobanteProductoCanje"), dto);
		}
		public void AgregarComprobanteProductoCanjeSelectivo(ComprobanteProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarComprobanteProductoCanjeSelectivo"), dto);
		}
		public int ActualizarCanje(ComprobanteProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "ActualizarCanje"), dto);
			return result;
		}
		public void EliminarCanje(ComprobanteProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "EliminarCanje"), dto);
		}
      public void AgregarComprobanteProductoEntrega(ComprobanteProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarComprobanteProductoEntrega"), dto);
		}
   }
}
