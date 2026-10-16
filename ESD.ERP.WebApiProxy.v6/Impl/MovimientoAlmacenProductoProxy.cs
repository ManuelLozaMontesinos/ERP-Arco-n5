using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MovimientoAlmacenProductoProxy : WebApiProxyClient, IMovimientoAlmacenProductoProxy
	{
		private const string API_CONTROLLER = "MovimientoAlmacenProducto";
		public MovimientoAlmacenProductoDTO GetById(MovimientoAlmacenProductoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MovimientoAlmacenProductoDTO> Search(MovimientoAlmacenProductoDTO dto)
		{
			var result = RestClient.PostApi<List<MovimientoAlmacenProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MovimientoAlmacenProductoDTO SingleSearch(MovimientoAlmacenProductoDTO dto)
		{
			var result = RestClient.PostApi<MovimientoAlmacenProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MovimientoAlmacenProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MovimientoAlmacenProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MovimientoAlmacenProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void AgregarDesdeReserva(MovimientoAlmacenProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarDesdeReserva"), dto);
		}
      public void ActualizarProductoReclasificacion(MovimientoAlmacenProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarProductoReclasificacion"), dto);
		}
      public void AgregarOrdenCompraProducto(MovimientoAlmacenProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarOrdenCompraProducto"), dto);
      }
   }
}
