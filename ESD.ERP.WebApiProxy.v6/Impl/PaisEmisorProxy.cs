using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PaisEmisorProxy : WebApiProxyClient, IPaisEmisorProxy
	{
		private const string API_CONTROLLER = "PaisEmisor";
		public PaisEmisorDTO GetById(PaisEmisorDTO dto)
		{
			var result = RestClient.PostApi<PaisEmisorDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PaisEmisorDTO> Search(PaisEmisorDTO dto)
		{
			var result = RestClient.PostApi<List<PaisEmisorDTO>>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PaisEmisorDTO SingleSearch(PaisEmisorDTO dto)
		{
			var result = RestClient.PostApi<PaisEmisorDTO>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PaisEmisorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PaisEmisorDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PaisEmisorDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER,MethodNameConstant.DELETE), dto);
		}
	}
}
