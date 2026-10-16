using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ImpuestoTasaBusiness : IImpuestoTasaBusiness
	{
		private readonly IImpuestoTasaRepository impuestoTasaRepository;

		public ImpuestoTasaBusiness(
			IImpuestoTasaRepository impuestoTasaRepository
			)
		{
			this.impuestoTasaRepository = impuestoTasaRepository;
		}
		public ImpuestoTasaDTO GetById(int id)
		{
			try
			{
				ImpuestoTasa impuestoTasa = impuestoTasaRepository.GetById(id);

				ImpuestoTasaDTO impuestoTasaDTO = Mapper.Map<ImpuestoTasaDTO>(impuestoTasa);

				return impuestoTasaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ImpuestoTasaDTO> Search(ImpuestoTasaDTO dto)
		{
			try
			{
				List<ImpuestoTasa> impuestoTasaList = impuestoTasaRepository.Search(Mapper.Map<ImpuestoTasa>(dto));

				List<ImpuestoTasaDTO> impuestoTasaDTOList = Mapper.Map<List<ImpuestoTasaDTO>>(impuestoTasaList);

				return impuestoTasaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ImpuestoTasaDTO SingleSearch(ImpuestoTasaDTO dto)
		{
			try
			{
				ImpuestoTasa impuestoTasa = impuestoTasaRepository.SingleSearch(Mapper.Map<ImpuestoTasa>(dto));

				ImpuestoTasaDTO impuestoTasaDTO = Mapper.Map<ImpuestoTasaDTO>(impuestoTasa);

				return impuestoTasaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ImpuestoTasaDTO dto)
		{
			try
			{
				ImpuestoTasa impuestoTasa = Mapper.Map<ImpuestoTasa>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = impuestoTasaRepository.Insert(impuestoTasa);

					dto.ImpuestoTasaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ImpuestoTasaDTO dto)
		{
			try
			{
				ImpuestoTasa impuestoTasa = Mapper.Map<ImpuestoTasa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = impuestoTasaRepository.Update(impuestoTasa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ImpuestoTasaDTO dto)
		{
			try
			{
				ImpuestoTasa impuestoTasa = Mapper.Map<ImpuestoTasa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					impuestoTasaRepository.Delete(impuestoTasa);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
