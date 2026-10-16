using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoBaseImpoBusiness : ITipoBaseImponibleBusiness
	{
		private readonly ITipoBaseImponibleRepository tipoBaseImponibleRepository;

		public TipoBaseImpoBusiness(
			ITipoBaseImponibleRepository tipoBaseImponibleRepository
			)
		{
			this.tipoBaseImponibleRepository = tipoBaseImponibleRepository;
		}
		public TipoBaseImponibleDTO GetById(int id)
		{
			try
			{
				TipoBaseImponible tipoBaseImponible = tipoBaseImponibleRepository.GetById(id);

				TipoBaseImponibleDTO tipoBaseImponibleDTO = Mapper.Map<TipoBaseImponibleDTO>(tipoBaseImponible);

				return tipoBaseImponibleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoBaseImponibleDTO> Search(TipoBaseImponibleDTO dto)
		{
			try
			{
				List<TipoBaseImponible> tipoBaseImponibleList = tipoBaseImponibleRepository.Search(Mapper.Map<TipoBaseImponible>(dto));

				List<TipoBaseImponibleDTO> tipoBaseImponibleDTOList = Mapper.Map<List<TipoBaseImponibleDTO>>(tipoBaseImponibleList);

				return tipoBaseImponibleDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoBaseImponibleDTO SingleSearch(TipoBaseImponibleDTO dto)
		{
			try
			{
				TipoBaseImponible tipoBaseImponible = tipoBaseImponibleRepository.SingleSearch(Mapper.Map<TipoBaseImponible>(dto));

				TipoBaseImponibleDTO tipoBaseImponibleDTO = Mapper.Map<TipoBaseImponibleDTO>(tipoBaseImponible);

				return tipoBaseImponibleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoBaseImponibleDTO dto)
		{
			try
			{
				TipoBaseImponible tipoBaseImponible = Mapper.Map<TipoBaseImponible>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoBaseImponibleRepository.Insert(tipoBaseImponible);

					dto.TipoBaseImponibleId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoBaseImponibleDTO dto)
		{
			try
			{
				TipoBaseImponible tipoBaseImponible = Mapper.Map<TipoBaseImponible>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoBaseImponibleRepository.Update(tipoBaseImponible);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoBaseImponibleDTO dto)
		{
			try
			{
				TipoBaseImponible tipoBaseImponible = Mapper.Map<TipoBaseImponible>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoBaseImponibleRepository.Delete(tipoBaseImponible);

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
