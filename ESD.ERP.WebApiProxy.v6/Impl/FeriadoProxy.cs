using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class FeriadoProxy : WebApiProxyClient, IFeriadoProxy
	{
		private const string API_CONTROLLER = "Feriado";
		public FeriadoDTO GetById(FeriadoDTO dto)
		{
			var result = RestClient.PostApi<FeriadoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<FeriadoDTO> Search(FeriadoDTO dto)
		{
			var result = RestClient.PostApi<List<FeriadoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public FeriadoDTO SingleSearch(FeriadoDTO dto)
		{
			var result = RestClient.PostApi<FeriadoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(FeriadoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(FeriadoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(FeriadoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
