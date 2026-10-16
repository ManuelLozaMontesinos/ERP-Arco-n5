using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICargoPersonalProxy
	{
		CargoPersonalDTO GetById(CargoPersonalDTO dto);
		List<CargoPersonalDTO> Search(CargoPersonalDTO dto);
		CargoPersonalDTO SingleSearch(CargoPersonalDTO dto);
		int Insert(CargoPersonalDTO dto);
		int Update(CargoPersonalDTO dto);
		void Delete(CargoPersonalDTO dto);
		CargoPersonalDTO ObtenerPorCodigo(CargoPersonalDTO dto);
	}
}
