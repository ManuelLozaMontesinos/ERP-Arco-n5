using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoProveedorProxy : WebApiProxyClient, ITipoProveedorProxy
	{
		private const string API_CONTROLLER = "TipoProveedor";
		public TipoProveedorDTO GetById(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<TipoProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoProveedorDTO> Search(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<List<TipoProveedorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoProveedorDTO SingleSearch(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<TipoProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoProveedorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoProveedorDTO ObtenerPorCodigo(TipoProveedorDTO dto)
		{
			var result = RestClient.PostApi<TipoProveedorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
