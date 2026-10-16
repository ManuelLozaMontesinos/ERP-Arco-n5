using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoSunatProxy : WebApiProxyClient, IEstadoSunatProxy
	{
		private const string API_CONTROLLER = "EstadoSunat";
		public EstadoSunatDTO GetById(EstadoSunatDTO dto)
		{
			var result = RestClient.PostApi<EstadoSunatDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoSunatDTO> Search(EstadoSunatDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoSunatDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoSunatDTO SingleSearch(EstadoSunatDTO dto)
		{
			var result = RestClient.PostApi<EstadoSunatDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoSunatDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoSunatDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoSunatDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public EstadoSunatDTO ObtenerPorCodigo(EstadoSunatDTO dto)
      {
			var result = RestClient.PostApi<EstadoSunatDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
   }
}
