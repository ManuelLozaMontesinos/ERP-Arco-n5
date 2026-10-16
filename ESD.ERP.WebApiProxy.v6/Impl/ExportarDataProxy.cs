using System.Collections.Generic;
using ESD.Core.Service.WebApi;

namespace ESD.ERP.WebApiProxy.Impl
{
   public class ExportarDataProxy : WebApiProxyClient, IExportarDataProxy 
	{
		private const string API_CONTROLLER = "ExportData";
		private const string API_KEY_NAME = "dataexport";

		public byte[] Exportar(Dictionary<string, string> data)
		{
			return RestClient.PostApi<byte[]>(API_KEY_NAME, WebApiMethod.Create(API_CONTROLLER, "Export"), data);
		}
      public byte[] ExportarXml(Dictionary<string, string> data)
      {
			return RestClient.PostApi<byte[]>(API_KEY_NAME, WebApiMethod.Create(API_CONTROLLER, "ExportXml"), data);
		}
   }
}