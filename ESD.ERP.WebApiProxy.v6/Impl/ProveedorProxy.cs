using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorProxy : WebApiProxyClient, IProveedorProxy
	{
		private const string API_CONTROLLER = "Proveedor";
		public ProveedorDTO GetById(ProveedorDTO dto)
		{
			var result = RestClient.PostApi<ProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorDTO> Search(ProveedorDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorDTO SingleSearch(ProveedorDTO dto)
		{
			var result = RestClient.PostApi<ProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void ActualizarTipoProveedor(ProveedorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarTipoProveedor"), dto);
		}
   }
}