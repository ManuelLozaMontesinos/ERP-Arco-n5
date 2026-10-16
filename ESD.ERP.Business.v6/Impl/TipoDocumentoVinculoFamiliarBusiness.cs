using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoDocumentoVinculoFamiliarBusiness : ITipoDocumentoVinculoFamiliarBusiness
	{
		private readonly ITipoDocumentoVinculoFamiliarRepository tipoDocumentoVinculoFamiliarRepository;

		public TipoDocumentoVinculoFamiliarBusiness(
			ITipoDocumentoVinculoFamiliarRepository tipoDocumentoVinculoFamiliarRepository
			)
		{
			this.tipoDocumentoVinculoFamiliarRepository = tipoDocumentoVinculoFamiliarRepository;
		}
		public TipoDocumentoVinculoFamiliarDTO GetById(int id)
		{
			try
			{
				TipoDocumentoVinculoFamiliar tipoDocumentoVinculoFamiliar = tipoDocumentoVinculoFamiliarRepository.GetById(id);

				TipoDocumentoVinculoFamiliarDTO tipoDocumentoVinculoFamiliarDTO = Mapper.Map<TipoDocumentoVinculoFamiliarDTO>(tipoDocumentoVinculoFamiliar);

				return tipoDocumentoVinculoFamiliarDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDocumentoVinculoFamiliarDTO> Search(TipoDocumentoVinculoFamiliarDTO dto)
		{
			try
			{
				List<TipoDocumentoVinculoFamiliar> tipoDocumentoVinculoFamiliarList = tipoDocumentoVinculoFamiliarRepository.Search(Mapper.Map<TipoDocumentoVinculoFamiliar>(dto));

				List<TipoDocumentoVinculoFamiliarDTO> tipoDocumentoVinculoFamiliarDTOList = Mapper.Map<List<TipoDocumentoVinculoFamiliarDTO>>(tipoDocumentoVinculoFamiliarList);

				return tipoDocumentoVinculoFamiliarDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDocumentoVinculoFamiliarDTO SingleSearch(TipoDocumentoVinculoFamiliarDTO dto)
		{
			try
			{
				TipoDocumentoVinculoFamiliar tipoDocumentoVinculoFamiliar = tipoDocumentoVinculoFamiliarRepository.SingleSearch(Mapper.Map<TipoDocumentoVinculoFamiliar>(dto));

				TipoDocumentoVinculoFamiliarDTO tipoDocumentoVinculoFamiliarDTO = Mapper.Map<TipoDocumentoVinculoFamiliarDTO>(tipoDocumentoVinculoFamiliar);

				return tipoDocumentoVinculoFamiliarDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoDocumentoVinculoFamiliarDTO dto)
		{
			try
			{
				TipoDocumentoVinculoFamiliar tipoDocumentoVinculoFamiliar = Mapper.Map<TipoDocumentoVinculoFamiliar>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoDocumentoVinculoFamiliarRepository.Insert(tipoDocumentoVinculoFamiliar);

					dto.TipoDocumentoVinculoFamiliarId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoDocumentoVinculoFamiliarDTO dto)
		{
			try
			{
				TipoDocumentoVinculoFamiliar tipoDocumentoVinculoFamiliar = Mapper.Map<TipoDocumentoVinculoFamiliar>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoDocumentoVinculoFamiliarRepository.Update(tipoDocumentoVinculoFamiliar);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoDocumentoVinculoFamiliarDTO dto)
		{
			try
			{
				TipoDocumentoVinculoFamiliar tipoDocumentoVinculoFamiliar = Mapper.Map<TipoDocumentoVinculoFamiliar>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoDocumentoVinculoFamiliarRepository.Delete(tipoDocumentoVinculoFamiliar);

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
