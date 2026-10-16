using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenProxy
	{
		MovimientoAlmacenDTO GetById(MovimientoAlmacenDTO dto);
		List<MovimientoAlmacenDTO> Search(MovimientoAlmacenDTO dto);
		MovimientoAlmacenDTO SingleSearch(MovimientoAlmacenDTO dto);
		int Insert(MovimientoAlmacenDTO dto);
		int Update(MovimientoAlmacenDTO dto);
		void Delete(MovimientoAlmacenDTO dto);
		int RecepcionarTransferencia(MovimientoAlmacenDTO dto);
		List<MovimientoAlmacenDTO> BuscarPorEmpresa(MovimientoAlmacenDTO dto);
		void SolicitarAutorizacion(MovimientoAlmacenDTO dto);
		void SolicitarModificacionEdicion(MovimientoAlmacenDTO dto);
        void Aprobar(MovimientoAlmacenDTO dto);
		void Desaprobar(MovimientoAlmacenDTO dto);
	}
}
