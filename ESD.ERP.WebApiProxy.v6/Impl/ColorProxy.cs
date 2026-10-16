using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ColorProxy : WebApiProxyClient, IColorProxy
	{
		private const string API_CONTROLLER = "Color";
		public ColorDTO GetById(ColorDTO dto)
		{
			var result = RestClient.PostApi<ColorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ColorDTO> Search(ColorDTO dto)
		{
			var result = RestClient.PostApi<List<ColorDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ColorDTO SingleSearch(ColorDTO dto)
		{
			var result = RestClient.PostApi<ColorDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ColorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ColorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ColorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
