using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class HoraRemunerativaProxy : WebApiProxyClient, IHoraRemunerativaProxy
	{
		private const string API_CONTROLLER = "HoraRemunerativa";
		public HoraRemunerativaDTO GetById(HoraRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<HoraRemunerativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<HoraRemunerativaDTO> Search(HoraRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<List<HoraRemunerativaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public HoraRemunerativaDTO SingleSearch(HoraRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<HoraRemunerativaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(HoraRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(HoraRemunerativaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(HoraRemunerativaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
