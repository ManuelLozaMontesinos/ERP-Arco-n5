using ESD.Core.Service;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISeguridadProxy
	{
		ServiceResponse Autenticar(UsuarioDTO dto);
	}
}
