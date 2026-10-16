using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICargoPersonalBusiness
	{
		CargoPersonalDTO GetById(int id);
		List<CargoPersonalDTO> Search(CargoPersonalDTO dto);
		CargoPersonalDTO SingleSearch(CargoPersonalDTO dto);
		int Insert(CargoPersonalDTO dto);
		int Update(CargoPersonalDTO dto);
		void Delete(CargoPersonalDTO dto);
		CargoPersonalDTO ObtenerPorCodigo(string codigo);
	}
}
