using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoCuotaBusiness : IEstadoCuotaBusiness
	{
		private readonly IEstadoCuotaRepository estadoCuotaRepository;

		public EstadoCuotaBusiness(
			IEstadoCuotaRepository estadoCuotaRepository
			)
		{
			this.estadoCuotaRepository = estadoCuotaRepository;
		}
		public EstadoCuotaDTO GetById(int id)
		{
			try
			{
				EstadoCuota estadoCuota = estadoCuotaRepository.GetById(id);

				EstadoCuotaDTO estadoCuotaDTO = Mapper.Map<EstadoCuotaDTO>(estadoCuota);

				return estadoCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoCuotaDTO> Search(EstadoCuotaDTO dto)
		{
			try
			{
				List<EstadoCuota> estadoCuotaList = estadoCuotaRepository.Search(Mapper.Map<EstadoCuota>(dto));

				List<EstadoCuotaDTO> estadoCuotaDTOList = Mapper.Map<List<EstadoCuotaDTO>>(estadoCuotaList);

				return estadoCuotaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoCuotaDTO SingleSearch(EstadoCuotaDTO dto)
		{
			try
			{
				EstadoCuota estadoCuota = estadoCuotaRepository.SingleSearch(Mapper.Map<EstadoCuota>(dto));

				EstadoCuotaDTO estadoCuotaDTO = Mapper.Map<EstadoCuotaDTO>(estadoCuota);

				return estadoCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoCuotaDTO dto)
		{
			try
			{
				EstadoCuota estadoCuota = Mapper.Map<EstadoCuota>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoCuotaRepository.Insert(estadoCuota);

					dto.EstadoCuotaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoCuotaDTO dto)
		{
			try
			{
				EstadoCuota estadoCuota = Mapper.Map<EstadoCuota>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoCuotaRepository.Update(estadoCuota);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoCuotaDTO dto)
		{
			try
			{
				EstadoCuota estadoCuota = Mapper.Map<EstadoCuota>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoCuotaRepository.Delete(estadoCuota);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoCuotaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoCuota estadoCuota = estadoCuotaRepository.ObtenerPorCodigo(codigo);

				EstadoCuotaDTO estadoCuotaDTO = Mapper.Map<EstadoCuotaDTO>(estadoCuota);

				return estadoCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
