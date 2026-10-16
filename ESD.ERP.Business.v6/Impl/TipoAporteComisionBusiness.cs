using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoAporteComisionBusiness : ITipoAporteComisionBusiness
	{
		private readonly ITipoAporteComisionRepository tipoAporteComisionRepository;

		public TipoAporteComisionBusiness(
			ITipoAporteComisionRepository tipoAporteComisionRepository
			)
		{
			this.tipoAporteComisionRepository = tipoAporteComisionRepository;
		}
		public TipoAporteComisionDTO GetById(int id)
		{
			try
			{
				TipoAporteComision tipoAporteComision = tipoAporteComisionRepository.GetById(id);

				TipoAporteComisionDTO tipoAporteComisionDTO = Mapper.Map<TipoAporteComisionDTO>(tipoAporteComision);

				return tipoAporteComisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoAporteComisionDTO> Search(TipoAporteComisionDTO dto)
		{
			try
			{
				List<TipoAporteComision> tipoAporteComisionList = tipoAporteComisionRepository.Search(Mapper.Map<TipoAporteComision>(dto));

				List<TipoAporteComisionDTO> tipoAporteComisionDTOList = Mapper.Map<List<TipoAporteComisionDTO>>(tipoAporteComisionList);

				return tipoAporteComisionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAporteComisionDTO SingleSearch(TipoAporteComisionDTO dto)
		{
			try
			{
				TipoAporteComision tipoAporteComision = tipoAporteComisionRepository.SingleSearch(Mapper.Map<TipoAporteComision>(dto));

				TipoAporteComisionDTO tipoAporteComisionDTO = Mapper.Map<TipoAporteComisionDTO>(tipoAporteComision);

				return tipoAporteComisionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoAporteComisionDTO dto)
		{
			try
			{
				TipoAporteComision tipoAporteComision = Mapper.Map<TipoAporteComision>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoAporteComisionRepository.Insert(tipoAporteComision);

					dto.TipoAporteComisionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoAporteComisionDTO dto)
		{
			try
			{
				TipoAporteComision tipoAporteComision = Mapper.Map<TipoAporteComision>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoAporteComisionRepository.Update(tipoAporteComision);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoAporteComisionDTO dto)
		{
			try
			{
				TipoAporteComision tipoAporteComision = Mapper.Map<TipoAporteComision>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoAporteComisionRepository.Delete(tipoAporteComision);

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
