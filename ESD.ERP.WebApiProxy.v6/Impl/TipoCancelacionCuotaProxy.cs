using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoCancelacionCuotaProxy : WebApiProxyClient, ITipoCancelacionCuotaProxy
	{
		private const string API_CONTROLLER = "TipoCancelacionCuota";
		public TipoCancelacionCuotaDTO GetById(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<TipoCancelacionCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoCancelacionCuotaDTO> Search(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<List<TipoCancelacionCuotaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoCancelacionCuotaDTO SingleSearch(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<TipoCancelacionCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoCancelacionCuotaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public TipoCancelacionCuotaDTO ObtenerPorCodigo(TipoCancelacionCuotaDTO dto)
		{
			var result = RestClient.PostApi<TipoCancelacionCuotaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
