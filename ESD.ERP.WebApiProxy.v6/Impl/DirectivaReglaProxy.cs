using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class DirectivaReglaProxy : WebApiProxyClient, IDirectivaReglaProxy
	{
		private const string API_CONTROLLER = "DirectivaRegla";
		public DirectivaReglaDTO GetById(DirectivaReglaDTO dto)
		{
			var result = RestClient.PostApi<DirectivaReglaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<DirectivaReglaDTO> Search(DirectivaReglaDTO dto)
		{
			var result = RestClient.PostApi<List<DirectivaReglaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public DirectivaReglaDTO SingleSearch(DirectivaReglaDTO dto)
		{
			var result = RestClient.PostApi<DirectivaReglaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(DirectivaReglaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(DirectivaReglaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(DirectivaReglaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
