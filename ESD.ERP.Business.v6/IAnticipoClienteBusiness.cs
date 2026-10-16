using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAnticipoClienteBusiness
	{
		AnticipoClienteDTO GetById(int id);
		List<AnticipoClienteDTO> Search(AnticipoClienteDTO dto);
		AnticipoClienteDTO SingleSearch(AnticipoClienteDTO dto);
		int Insert(AnticipoClienteDTO dto);
		int Update(AnticipoClienteDTO dto);
		void Delete(AnticipoClienteDTO dto);
		void Extornar(AnticipoClienteDTO dto);
		void Cerrar(AnticipoClienteDTO dto);
		void Emitir(AnticipoClienteDTO dto);
      void Aplicar(AnticipoClienteDTO dto);
		void AnularAplicacion(AnticipoClienteDTO dto);
		void AnularAsignacionPedido(AnticipoClienteDTO dto);
		void DividirMonto(AnticipoClienteDTO dto);
   }
}
