using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenBusiness
	{
		MovimientoAlmacenDTO GetById(int id);
		List<MovimientoAlmacenDTO> Search(MovimientoAlmacenDTO dto);
		MovimientoAlmacenDTO SingleSearch(MovimientoAlmacenDTO dto);
		int Insert(MovimientoAlmacenDTO dto);
		int Update(MovimientoAlmacenDTO dto);
		void Delete(MovimientoAlmacenDTO dto);
		int RecepcionarTransferencia(MovimientoAlmacenDTO dto);
		List<MovimientoAlmacenDTO> BuscarPorEmpresa(MovimientoAlmacenDTO dto);
		void Solicitar(MovimientoAlmacenDTO dto);
		void Aprobar(MovimientoAlmacenDTO dto);
		void Desaprobar(MovimientoAlmacenDTO dto);
		void ActualizarMesContable(MovimientoAlmacenDTO dto);
		void SolicitarModificacionEdicion(MovimientoAlmacenDTO dto);

    }
}
