using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RegimenSaludBusiness : IRegimenSaludBusiness
	{
		private readonly IRegimenSaludRepository regimenSaludRepository;

		public RegimenSaludBusiness(
			IRegimenSaludRepository regimenSaludRepository
			)
		{
			this.regimenSaludRepository = regimenSaludRepository;
		}
		public RegimenSaludDTO GetById(int id)
		{
			try
			{
				RegimenSalud regimenSalud = regimenSaludRepository.GetById(id);

				RegimenSaludDTO regimenSaludDTO = Mapper.Map<RegimenSaludDTO>(regimenSalud);

				return regimenSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RegimenSaludDTO> Search(RegimenSaludDTO dto)
		{
			try
			{
				List<RegimenSalud> regimenSaludList = regimenSaludRepository.Search(Mapper.Map<RegimenSalud>(dto));

				List<RegimenSaludDTO> regimenSaludDTOList = Mapper.Map<List<RegimenSaludDTO>>(regimenSaludList);

				return regimenSaludDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenSaludDTO SingleSearch(RegimenSaludDTO dto)
		{
			try
			{
				RegimenSalud regimenSalud = regimenSaludRepository.SingleSearch(Mapper.Map<RegimenSalud>(dto));

				RegimenSaludDTO regimenSaludDTO = Mapper.Map<RegimenSaludDTO>(regimenSalud);

				return regimenSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RegimenSaludDTO dto)
		{
			try
			{
				RegimenSalud regimenSalud = Mapper.Map<RegimenSalud>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = regimenSaludRepository.Insert(regimenSalud);

					dto.RegimenSaludId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RegimenSaludDTO dto)
		{
			try
			{
				RegimenSalud regimenSalud = Mapper.Map<RegimenSalud>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = regimenSaludRepository.Update(regimenSalud);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RegimenSaludDTO dto)
		{
			try
			{
				RegimenSalud regimenSalud = Mapper.Map<RegimenSalud>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					regimenSaludRepository.Delete(regimenSalud);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RegimenSaludDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				RegimenSalud regimenSalud = regimenSaludRepository.ObtenerPorCodigo(codigo);

				RegimenSaludDTO regimenSaludDTO = Mapper.Map<RegimenSaludDTO>(regimenSalud);

				return regimenSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
