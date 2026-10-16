using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PeriodicidadRemunerativaBusiness : IPeriodicidadRemunerativaBusiness
	{
		private readonly IPeriodicidadRemunerativaRepository periodicidadRemunerativaRepository;

		public PeriodicidadRemunerativaBusiness(
			IPeriodicidadRemunerativaRepository periodicidadRemunerativaRepository
			)
		{
			this.periodicidadRemunerativaRepository = periodicidadRemunerativaRepository;
		}
		public PeriodicidadRemunerativaDTO GetById(int id)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = periodicidadRemunerativaRepository.GetById(id);

				PeriodicidadRemunerativaDTO periodicidadRemunerativaDTO = Mapper.Map<PeriodicidadRemunerativaDTO>(periodicidadRemunerativa);

				return periodicidadRemunerativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PeriodicidadRemunerativaDTO> Search(PeriodicidadRemunerativaDTO dto)
		{
			try
			{
				List<PeriodicidadRemunerativa> periodicidadRemunerativaList = periodicidadRemunerativaRepository.Search(Mapper.Map<PeriodicidadRemunerativa>(dto));

				List<PeriodicidadRemunerativaDTO> periodicidadRemunerativaDTOList = Mapper.Map<List<PeriodicidadRemunerativaDTO>>(periodicidadRemunerativaList);

				return periodicidadRemunerativaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PeriodicidadRemunerativaDTO SingleSearch(PeriodicidadRemunerativaDTO dto)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = periodicidadRemunerativaRepository.SingleSearch(Mapper.Map<PeriodicidadRemunerativa>(dto));

				PeriodicidadRemunerativaDTO periodicidadRemunerativaDTO = Mapper.Map<PeriodicidadRemunerativaDTO>(periodicidadRemunerativa);

				return periodicidadRemunerativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PeriodicidadRemunerativaDTO dto)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = Mapper.Map<PeriodicidadRemunerativa>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = periodicidadRemunerativaRepository.Insert(periodicidadRemunerativa);

					dto.PeriodicidadRemunerativaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PeriodicidadRemunerativaDTO dto)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = Mapper.Map<PeriodicidadRemunerativa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = periodicidadRemunerativaRepository.Update(periodicidadRemunerativa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PeriodicidadRemunerativaDTO dto)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = Mapper.Map<PeriodicidadRemunerativa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					periodicidadRemunerativaRepository.Delete(periodicidadRemunerativa);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PeriodicidadRemunerativaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				PeriodicidadRemunerativa periodicidadRemunerativa = periodicidadRemunerativaRepository.ObtenerPorCodigo(codigo);

				PeriodicidadRemunerativaDTO periodicidadRemunerativaDTO = Mapper.Map<PeriodicidadRemunerativaDTO>(periodicidadRemunerativa);

				return periodicidadRemunerativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
