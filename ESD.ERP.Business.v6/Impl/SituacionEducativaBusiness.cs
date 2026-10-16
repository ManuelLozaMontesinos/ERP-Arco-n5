using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SituacionEducativaBusiness : ISituacionEducativaBusiness
	{
		private readonly ISituacionEducativaRepository situacionEducativaRepository;

		public SituacionEducativaBusiness(
			ISituacionEducativaRepository situacionEducativaRepository
			)
		{
			this.situacionEducativaRepository = situacionEducativaRepository;
		}
		public SituacionEducativaDTO GetById(int id)
		{
			try
			{
				SituacionEducativa situacionEducativa = situacionEducativaRepository.GetById(id);

				SituacionEducativaDTO situacionEducativaDTO = Mapper.Map<SituacionEducativaDTO>(situacionEducativa);

				return situacionEducativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SituacionEducativaDTO> Search(SituacionEducativaDTO dto)
		{
			try
			{
				List<SituacionEducativa> situacionEducativaList = situacionEducativaRepository.Search(Mapper.Map<SituacionEducativa>(dto));

				List<SituacionEducativaDTO> situacionEducativaDTOList = Mapper.Map<List<SituacionEducativaDTO>>(situacionEducativaList);

				return situacionEducativaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionEducativaDTO SingleSearch(SituacionEducativaDTO dto)
		{
			try
			{
				SituacionEducativa situacionEducativa = situacionEducativaRepository.SingleSearch(Mapper.Map<SituacionEducativa>(dto));

				SituacionEducativaDTO situacionEducativaDTO = Mapper.Map<SituacionEducativaDTO>(situacionEducativa);

				return situacionEducativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SituacionEducativaDTO dto)
		{
			try
			{
				SituacionEducativa situacionEducativa = Mapper.Map<SituacionEducativa>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = situacionEducativaRepository.Insert(situacionEducativa);

					dto.SituacionEducativaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SituacionEducativaDTO dto)
		{
			try
			{
				SituacionEducativa situacionEducativa = Mapper.Map<SituacionEducativa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = situacionEducativaRepository.Update(situacionEducativa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SituacionEducativaDTO dto)
		{
			try
			{
				SituacionEducativa situacionEducativa = Mapper.Map<SituacionEducativa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					situacionEducativaRepository.Delete(situacionEducativa);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SituacionEducativaDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				SituacionEducativa situacionEducativa = situacionEducativaRepository.ObtenerPorCodigo(codigo);

				SituacionEducativaDTO situacionEducativaDTO = Mapper.Map<SituacionEducativaDTO>(situacionEducativa);

				return situacionEducativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
