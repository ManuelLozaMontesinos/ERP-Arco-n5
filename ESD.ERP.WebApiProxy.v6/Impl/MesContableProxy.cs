using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MesContableProxy : WebApiProxyClient, IMesContableProxy
	{
		private const string API_CONTROLLER = "MesContable";
		public MesContableDTO GetById(MesContableDTO dto)
		{
			var result = RestClient.PostApi<MesContableDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MesContableDTO> Search(MesContableDTO dto)
		{
			var result = RestClient.PostApi<List<MesContableDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MesContableDTO SingleSearch(MesContableDTO dto)
		{
			var result = RestClient.PostApi<MesContableDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MesContableDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MesContableDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MesContableDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public MesContableDTO ObtenerPorCodigo(MesContableDTO dto)
		{
			var result = RestClient.PostApi<MesContableDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
