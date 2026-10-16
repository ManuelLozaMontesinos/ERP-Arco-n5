using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CargoPersonalBusiness : ICargoPersonalBusiness
	{
		private readonly ICargoPersonalRepository cargoPersonalRepository;
		private readonly ITablaBusiness tablaBusiness;

		public CargoPersonalBusiness(
			ICargoPersonalRepository cargoPersonalRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.cargoPersonalRepository = cargoPersonalRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public CargoPersonalDTO GetById(int id)
		{
			try
			{
				CargoPersonal cargoPersonal = cargoPersonalRepository.GetById(id);

				CargoPersonalDTO cargoPersonalDTO = Mapper.Map<CargoPersonalDTO>(cargoPersonal);

				return cargoPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CargoPersonalDTO> Search(CargoPersonalDTO dto)
		{
			try
			{
				List<CargoPersonal> cargoPersoList = cargoPersonalRepository.Search(Mapper.Map<CargoPersonal>(dto));

				List<CargoPersonalDTO> cargoPersonalDTOList = Mapper.Map<List<CargoPersonalDTO>>(cargoPersoList);

				return cargoPersonalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CargoPersonalDTO SingleSearch(CargoPersonalDTO dto)
		{
			try
			{
				CargoPersonal cargoPersonal = cargoPersonalRepository.SingleSearch(Mapper.Map<CargoPersonal>(dto));

				CargoPersonalDTO cargoPersonalDTO = Mapper.Map<CargoPersonalDTO>(cargoPersonal);

				return cargoPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CargoPersonalDTO dto)
		{
			try
			{
				CargoPersonal cargoPersonal = Mapper.Map<CargoPersonal>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO() { Tabla = TablaConstante.CARGO_PERSONAL };
					cargoPersonal.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = cargoPersonalRepository.Insert(cargoPersonal);

					dto.CargoPersonalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CargoPersonalDTO dto)
		{
			try
			{
				CargoPersonal cargoPersonal = Mapper.Map<CargoPersonal>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = cargoPersonalRepository.Update(cargoPersonal);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CargoPersonalDTO dto)
		{
			try
			{
				CargoPersonal cargoPersonal = Mapper.Map<CargoPersonal>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cargoPersonalRepository.Delete(cargoPersonal);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CargoPersonalDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				CargoPersonal cargoPersonal = cargoPersonalRepository.ObtenerPorCodigo(codigo);

				CargoPersonalDTO cargoPersonalDTO = Mapper.Map<CargoPersonalDTO>(cargoPersonal);

				return cargoPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
