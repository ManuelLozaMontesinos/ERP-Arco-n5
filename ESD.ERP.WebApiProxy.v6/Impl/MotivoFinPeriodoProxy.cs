using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class MotivoFinPeriodoProxy : WebApiProxyClient, IMotivoFinPeriodoProxy
	{
		private const string API_CONTROLLER = "MotivoFinPeriodo";
		public MotivoFinPeriodoDTO GetById(MotivoFinPeriodoDTO dto)
		{
			var result = RestClient.PostApi<MotivoFinPeriodoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<MotivoFinPeriodoDTO> Search(MotivoFinPeriodoDTO dto)
		{
			var result = RestClient.PostApi<List<MotivoFinPeriodoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public MotivoFinPeriodoDTO SingleSearch(MotivoFinPeriodoDTO dto)
		{
			var result = RestClient.PostApi<MotivoFinPeriodoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(MotivoFinPeriodoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(MotivoFinPeriodoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(MotivoFinPeriodoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
