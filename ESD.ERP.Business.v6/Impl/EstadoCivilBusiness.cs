using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoCivilBusiness : IEstadoCivilBusiness
	{
		private readonly IEstadoCivilRepository estadoCivilRepository;

		public EstadoCivilBusiness(
			IEstadoCivilRepository estadoCivilRepository
			)
		{
			this.estadoCivilRepository = estadoCivilRepository;
		}
		public EstadoCivilDTO GetById(int id)
		{
			try
			{
				EstadoCivil estadoCivil = estadoCivilRepository.GetById(id);

				EstadoCivilDTO estadoCivilDTO = Mapper.Map<EstadoCivilDTO>(estadoCivil);

				return estadoCivilDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoCivilDTO> Search(EstadoCivilDTO dto)
		{
			try
			{
				List<EstadoCivil> estadoCivilList = estadoCivilRepository.Search(Mapper.Map<EstadoCivil>(dto));

				List<EstadoCivilDTO> estadoCivilDTOList = Mapper.Map<List<EstadoCivilDTO>>(estadoCivilList);

				return estadoCivilDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoCivilDTO SingleSearch(EstadoCivilDTO dto)
		{
			try
			{
				EstadoCivil estadoCivil = estadoCivilRepository.SingleSearch(Mapper.Map<EstadoCivil>(dto));

				EstadoCivilDTO estadoCivilDTO = Mapper.Map<EstadoCivilDTO>(estadoCivil);

				return estadoCivilDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoCivilDTO dto)
		{
			try
			{
				EstadoCivil estadoCivil = Mapper.Map<EstadoCivil>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoCivilRepository.Insert(estadoCivil);

					dto.EstadoCivilId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoCivilDTO dto)
		{
			try
			{
				EstadoCivil estadoCivil = Mapper.Map<EstadoCivil>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoCivilRepository.Update(estadoCivil);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoCivilDTO dto)
		{
			try
			{
				EstadoCivil estadoCivil = Mapper.Map<EstadoCivil>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoCivilRepository.Delete(estadoCivil);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoCivilDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoCivil estadoCivil = estadoCivilRepository.ObtenerPorCodigo(codigo);

				EstadoCivilDTO estadoCivilDTO = Mapper.Map<EstadoCivilDTO>(estadoCivil);

				return estadoCivilDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
