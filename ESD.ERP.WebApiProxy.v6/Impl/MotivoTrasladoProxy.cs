using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MotivoTrasladoProxy : WebApiProxyClient, IMotivoTrasladoProxy
	{
		private const string API_CONTROLLER = "MotivoTraslado";
		public MotivoTrasladoDTO GetById(MotivoTrasladoDTO dto)
		{
			var result = RestClient.PostApi<MotivoTrasladoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MotivoTrasladoDTO> Search(MotivoTrasladoDTO dto)
		{
			var result = RestClient.PostApi<List<MotivoTrasladoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MotivoTrasladoDTO SingleSearch(MotivoTrasladoDTO dto)
		{
			var result = RestClient.PostApi<MotivoTrasladoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MotivoTrasladoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MotivoTrasladoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MotivoTrasladoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
