using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MesProxy : WebApiProxyClient, IMesProxy
	{
		private const string API_CONTROLLER = "Mes";
		public MesDTO GetById(MesDTO dto)
		{
			var result = RestClient.PostApi<MesDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MesDTO> Search(MesDTO dto)
		{
			var result = RestClient.PostApi<List<MesDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MesDTO SingleSearch(MesDTO dto)
		{
			var result = RestClient.PostApi<MesDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MesDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MesDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MesDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public MesDTO ObtenerPorCodigo(MesDTO dto)
		{
			var result = RestClient.PostApi<MesDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
