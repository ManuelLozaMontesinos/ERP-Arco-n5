using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RegimenLaboralBusiness : IRegimenLaboralBusiness
	{
		private readonly IRegimenLaboralRepository regimenLaboralRepository;

		public RegimenLaboralBusiness(
			IRegimenLaboralRepository regimenLaboralRepository
			)
		{
			this.regimenLaboralRepository = regimenLaboralRepository;
		}
		public RegimenLaboralDTO GetById(int id)
		{
			try
			{
				RegimenLaboral regimenLaboral = regimenLaboralRepository.GetById(id);

				RegimenLaboralDTO regimenLaboralDTO = Mapper.Map<RegimenLaboralDTO>(regimenLaboral);

				return regimenLaboralDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RegimenLaboralDTO> Search(RegimenLaboralDTO dto)
		{
			try
			{
				List<RegimenLaboral> regimenLaboralList = regimenLaboralRepository.Search(Mapper.Map<RegimenLaboral>(dto));

				List<RegimenLaboralDTO> regimenLaboralDTOList = Mapper.Map<List<RegimenLaboralDTO>>(regimenLaboralList);

				return regimenLaboralDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenLaboralDTO SingleSearch(RegimenLaboralDTO dto)
		{
			try
			{
				RegimenLaboral regimenLaboral = regimenLaboralRepository.SingleSearch(Mapper.Map<RegimenLaboral>(dto));

				RegimenLaboralDTO regimenLaboralDTO = Mapper.Map<RegimenLaboralDTO>(regimenLaboral);

				return regimenLaboralDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RegimenLaboralDTO dto)
		{
			try
			{
				RegimenLaboral regimenLaboral = Mapper.Map<RegimenLaboral>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = regimenLaboralRepository.Insert(regimenLaboral);

					dto.RegimenLaboralId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RegimenLaboralDTO dto)
		{
			try
			{
				RegimenLaboral regimenLaboral = Mapper.Map<RegimenLaboral>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = regimenLaboralRepository.Update(regimenLaboral);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RegimenLaboralDTO dto)
		{
			try
			{
				RegimenLaboral regimenLaboral = Mapper.Map<RegimenLaboral>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					regimenLaboralRepository.Delete(regimenLaboral);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenLaboralDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				RegimenLaboral regimenLaboral = regimenLaboralRepository.ObtenerPorCodigo(codigo);

				RegimenLaboralDTO regimenLaboralDTO = Mapper.Map<RegimenLaboralDTO>(regimenLaboral);

				return regimenLaboralDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
