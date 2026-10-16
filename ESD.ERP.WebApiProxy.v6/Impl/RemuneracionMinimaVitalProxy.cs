using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class RemuneracionMinimaVitalProxy : WebApiProxyClient, IRemuneracionMinimaVitalProxy
	{
		private const string API_CONTROLLER = "RemuneracionMinimaVital";
		public RemuneracionMinimaVitalDTO GetById(RemuneracionMinimaVitalDTO dto)
		{
			var result = RestClient.PostApi<RemuneracionMinimaVitalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<RemuneracionMinimaVitalDTO> Search(RemuneracionMinimaVitalDTO dto)
		{
			var result = RestClient.PostApi<List<RemuneracionMinimaVitalDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public RemuneracionMinimaVitalDTO SingleSearch(RemuneracionMinimaVitalDTO dto)
		{
			var result = RestClient.PostApi<RemuneracionMinimaVitalDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(RemuneracionMinimaVitalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(RemuneracionMinimaVitalDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(RemuneracionMinimaVitalDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
