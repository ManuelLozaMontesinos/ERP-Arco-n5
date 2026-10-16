using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MotivoBajaDependienteBusiness : IMotivoBajaDependienteBusiness
	{
		private readonly IMotivoBajaDependienteRepository motivoBajaDependienteRepository;

		public MotivoBajaDependienteBusiness(
			IMotivoBajaDependienteRepository motivoBajaDependienteRepository
			)
		{
			this.motivoBajaDependienteRepository = motivoBajaDependienteRepository;
		}
		public MotivoBajaDependienteDTO GetById(int id)
		{
			try
			{
				MotivoBajaDependiente motivoBajaDependiente = motivoBajaDependienteRepository.GetById(id);

				MotivoBajaDependienteDTO motivoBajaDependienteDTO = Mapper.Map<MotivoBajaDependienteDTO>(motivoBajaDependiente);

				return motivoBajaDependienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MotivoBajaDependienteDTO> Search(MotivoBajaDependienteDTO dto)
		{
			try
			{
				List<MotivoBajaDependiente> motivoBajaDependienteList = motivoBajaDependienteRepository.Search(Mapper.Map<MotivoBajaDependiente>(dto));

				List<MotivoBajaDependienteDTO> motivoBajaDependienteDTOList = Mapper.Map<List<MotivoBajaDependienteDTO>>(motivoBajaDependienteList);

				return motivoBajaDependienteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MotivoBajaDependienteDTO SingleSearch(MotivoBajaDependienteDTO dto)
		{
			try
			{
				MotivoBajaDependiente motivoBajaDependiente = motivoBajaDependienteRepository.SingleSearch(Mapper.Map<MotivoBajaDependiente>(dto));

				MotivoBajaDependienteDTO motivoBajaDependienteDTO = Mapper.Map<MotivoBajaDependienteDTO>(motivoBajaDependiente);

				return motivoBajaDependienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MotivoBajaDependienteDTO dto)
		{
			try
			{
				MotivoBajaDependiente motivoBajaDependiente = Mapper.Map<MotivoBajaDependiente>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = motivoBajaDependienteRepository.Insert(motivoBajaDependiente);

					dto.MotivoBajaDependienteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MotivoBajaDependienteDTO dto)
		{
			try
			{
				MotivoBajaDependiente motivoBajaDependiente = Mapper.Map<MotivoBajaDependiente>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = motivoBajaDependienteRepository.Update(motivoBajaDependiente);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MotivoBajaDependienteDTO dto)
		{
			try
			{
				MotivoBajaDependiente motivoBajaDependiente = Mapper.Map<MotivoBajaDependiente>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					motivoBajaDependienteRepository.Delete(motivoBajaDependiente);

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
