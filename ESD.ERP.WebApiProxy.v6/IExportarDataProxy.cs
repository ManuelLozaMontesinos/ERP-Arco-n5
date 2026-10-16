using System.Collections.Generic;

namespace ESD.ERP.WebApiProxy
{
   public interface IExportarDataProxy
	{
		byte[] Exportar(Dictionary<string, string> data);
		byte[] ExportarXml(Dictionary<string, string> data);
	}
}
