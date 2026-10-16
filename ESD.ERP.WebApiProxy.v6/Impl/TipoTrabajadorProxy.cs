using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoTrabajadorProxy : WebApiProxyClient, ITipoTrabajadorProxy
	{
		private const string API_CONTROLLER = "TipoTrabajador";
		public TipoTrabajadorDTO GetById(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<TipoTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoTrabajadorDTO> Search(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<List<TipoTrabajadorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoTrabajadorDTO SingleSearch(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<TipoTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoTrabajadorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoTrabajadorDTO ObtenerPorCodigo(TipoTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<TipoTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
