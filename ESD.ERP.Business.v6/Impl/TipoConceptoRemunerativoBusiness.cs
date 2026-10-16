using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoConceptoRemunerativoBusiness : ITipoConceptoRemunerativoBusiness
	{
		private readonly ITipoConceptoRemunerativoRepository tipoConceptoRemunerativoRepository;

		public TipoConceptoRemunerativoBusiness(
			ITipoConceptoRemunerativoRepository tipoConceptoRemunerativoRepository
			)
		{
			this.tipoConceptoRemunerativoRepository = tipoConceptoRemunerativoRepository;
		}
		public TipoConceptoRemunerativoDTO GetById(int id)
		{
			try
			{
				TipoConceptoRemunerativo tipoConceptoRemunerativo = tipoConceptoRemunerativoRepository.GetById(id);

				TipoConceptoRemunerativoDTO tipoConceptoRemunerativoDTO = Mapper.Map<TipoConceptoRemunerativoDTO>(tipoConceptoRemunerativo);

				return tipoConceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoConceptoRemunerativoDTO> Search(TipoConceptoRemunerativoDTO dto)
		{
			try
			{
				List<TipoConceptoRemunerativo> tipoConceptoRemunerativoList = tipoConceptoRemunerativoRepository.Search(Mapper.Map<TipoConceptoRemunerativo>(dto));

				List<TipoConceptoRemunerativoDTO> tipoConceptoRemunerativoDTOList = Mapper.Map<List<TipoConceptoRemunerativoDTO>>(tipoConceptoRemunerativoList);

				return tipoConceptoRemunerativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoConceptoRemunerativoDTO SingleSearch(TipoConceptoRemunerativoDTO dto)
		{
			try
			{
				TipoConceptoRemunerativo tipoConceptoRemunerativo = tipoConceptoRemunerativoRepository.SingleSearch(Mapper.Map<TipoConceptoRemunerativo>(dto));

				TipoConceptoRemunerativoDTO tipoConceptoRemunerativoDTO = Mapper.Map<TipoConceptoRemunerativoDTO>(tipoConceptoRemunerativo);

				return tipoConceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoConceptoRemunerativoDTO dto)
		{
			try
			{
				TipoConceptoRemunerativo tipoConceptoRemunerativo = Mapper.Map<TipoConceptoRemunerativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoConceptoRemunerativoRepository.Insert(tipoConceptoRemunerativo);

					dto.TipoConceptoRemunerativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoConceptoRemunerativoDTO dto)
		{
			try
			{
				TipoConceptoRemunerativo tipoConceptoRemunerativo = Mapper.Map<TipoConceptoRemunerativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoConceptoRemunerativoRepository.Update(tipoConceptoRemunerativo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoConceptoRemunerativoDTO dto)
		{
			try
			{
				TipoConceptoRemunerativo tipoConceptoRemunerativo = Mapper.Map<TipoConceptoRemunerativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoConceptoRemunerativoRepository.Delete(tipoConceptoRemunerativo);

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
