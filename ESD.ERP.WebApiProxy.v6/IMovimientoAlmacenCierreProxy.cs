using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoAlmacenCierreProxy
	{
		MovimientoAlmacenCierreDTO GetById(MovimientoAlmacenCierreDTO dto);
		List<MovimientoAlmacenCierreDTO> Search(MovimientoAlmacenCierreDTO dto);
		MovimientoAlmacenCierreDTO SingleSearch(MovimientoAlmacenCierreDTO dto);
		int Insert(MovimientoAlmacenCierreDTO dto);
		int Update(MovimientoAlmacenCierreDTO dto);
		void Delete(MovimientoAlmacenCierreDTO dto);
      void Cerrar(MovimientoAlmacenCierreDTO dto);
      void Aperturar(MovimientoAlmacenCierreDTO dto);
		void Reprocesar(MovimientoAlmacenCierreDTO dto);
		void ReprocesoAnual(MovimientoAlmacenCierreDTO dto);
   }
}
