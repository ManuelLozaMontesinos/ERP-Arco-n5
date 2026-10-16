using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMovimientoAlmacenCierreBusiness
	{
		MovimientoAlmacenCierreDTO GetById(int id);
		List<MovimientoAlmacenCierreDTO> Search(MovimientoAlmacenCierreDTO dto);
		MovimientoAlmacenCierreDTO SingleSearch(MovimientoAlmacenCierreDTO dto);
		int Insert(MovimientoAlmacenCierreDTO dto);
		int Update(MovimientoAlmacenCierreDTO dto);
		void Delete(MovimientoAlmacenCierreDTO dto);
      void Aperturar(MovimientoAlmacenCierreDTO dto);
		void Cerrar(MovimientoAlmacenCierreDTO dto);
		void Reprocesar(MovimientoAlmacenCierreDTO dto);
		void ReprocesarSaldoAnual(MovimientoAlmacenCierreDTO dto);
   }
}
