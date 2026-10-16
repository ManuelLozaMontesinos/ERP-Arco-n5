using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class SituacionTrabajadorProxy : WebApiProxyClient, ISituacionTrabajadorProxy
	{
		private const string API_CONTROLLER = "SituacionTrabajador";
		public SituacionTrabajadorDTO GetById(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<SituacionTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<SituacionTrabajadorDTO> Search(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<List<SituacionTrabajadorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public SituacionTrabajadorDTO SingleSearch(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<SituacionTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(SituacionTrabajadorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public SituacionTrabajadorDTO ObtenerPorCodigo(SituacionTrabajadorDTO dto)
		{
			var result = RestClient.PostApi<SituacionTrabajadorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
