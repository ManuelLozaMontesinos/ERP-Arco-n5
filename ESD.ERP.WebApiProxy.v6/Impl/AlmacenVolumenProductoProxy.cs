using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class AlmacenVolumenProductoProxy : WebApiProxyClient, IAlmacenVolumenProductoProxy
	{
		private const string API_CONTROLLER = "AlmacenVolumenProducto";
		public AlmacenVolumenProductoDTO GetById(AlmacenVolumenProductoDTO dto)
		{
			var result = RestClient.PostApi<AlmacenVolumenProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<AlmacenVolumenProductoDTO> Search(AlmacenVolumenProductoDTO dto)
		{
			var result = RestClient.PostApi<List<AlmacenVolumenProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public AlmacenVolumenProductoDTO SingleSearch(AlmacenVolumenProductoDTO dto)
		{
			var result = RestClient.PostApi<AlmacenVolumenProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(AlmacenVolumenProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(AlmacenVolumenProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(AlmacenVolumenProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public List<AlmacenVolumenProductoDTO> ListarPorSedeId(AlmacenVolumenProductoDTO dto)
      {
			var result = RestClient.PostApi<List<AlmacenVolumenProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, "ListarPorSedeId"), dto);
			return result;
		}
      public void ActualizarLote(List<AlmacenVolumenProductoDTO> list)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarLote"), list);
		}
      public string ListarStockMinimoGrid(AlmacenVolumenProductoDTO dto)
      {
			var result = RestClient.PostApi<string>(WebApiMethod.Create(API_CONTROLLER, "ListarStockMinimoGrid"), dto);
			return result;
		}
   }
}
