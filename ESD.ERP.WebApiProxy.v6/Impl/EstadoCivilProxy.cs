using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoCivilProxy : WebApiProxyClient, IEstadoCivilProxy
	{
		private const string API_CONTROLLER = "EstadoCivil";
		public EstadoCivilDTO GetById(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<EstadoCivilDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoCivilDTO> Search(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoCivilDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoCivilDTO SingleSearch(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<EstadoCivilDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoCivilDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoCivilDTO ObtenerPorCodigo(EstadoCivilDTO dto)
		{
			var result = RestClient.PostApi<EstadoCivilDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
