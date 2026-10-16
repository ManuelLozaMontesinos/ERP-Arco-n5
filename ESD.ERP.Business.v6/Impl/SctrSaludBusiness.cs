using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SctrSaludBusiness : ISctrSaludBusiness
	{
		private readonly ISctrSaludRepository sctrSaludRepository;

		public SctrSaludBusiness(
			ISctrSaludRepository sctrSaludRepository
			)
		{
			this.sctrSaludRepository = sctrSaludRepository;
		}
		public SctrSaludDTO GetById(int id)
		{
			try
			{
				SctrSalud sctrSalud = sctrSaludRepository.GetById(id);

				SctrSaludDTO sctrSaludDTO = Mapper.Map<SctrSaludDTO>(sctrSalud);

				return sctrSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SctrSaludDTO> Search(SctrSaludDTO dto)
		{
			try
			{
				List<SctrSalud> sctrSaludList = sctrSaludRepository.Search(Mapper.Map<SctrSalud>(dto));

				List<SctrSaludDTO> sctrSaludDTOList = Mapper.Map<List<SctrSaludDTO>>(sctrSaludList);

				return sctrSaludDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SctrSaludDTO SingleSearch(SctrSaludDTO dto)
		{
			try
			{
				SctrSalud sctrSalud = sctrSaludRepository.SingleSearch(Mapper.Map<SctrSalud>(dto));

				SctrSaludDTO sctrSaludDTO = Mapper.Map<SctrSaludDTO>(sctrSalud);

				return sctrSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SctrSaludDTO dto)
		{
			try
			{
				SctrSalud sctrSalud = Mapper.Map<SctrSalud>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = sctrSaludRepository.Insert(sctrSalud);

					dto.SctrSaludId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SctrSaludDTO dto)
		{
			try
			{
				SctrSalud sctrSalud = Mapper.Map<SctrSalud>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = sctrSaludRepository.Update(sctrSalud);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SctrSaludDTO dto)
		{
			try
			{
				SctrSalud sctrSalud = Mapper.Map<SctrSalud>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					sctrSaludRepository.Delete(sctrSalud);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SctrSaludDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				SctrSalud sctrSalud = sctrSaludRepository.ObtenerPorCodigo(codigo);

				SctrSaludDTO sctrSaludDTO = Mapper.Map<SctrSaludDTO>(sctrSalud);

				return sctrSaludDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
