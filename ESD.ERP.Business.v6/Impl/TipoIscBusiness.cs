using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoIscBusiness : ITipoIscBusiness
	{
		private readonly ITipoIscRepository tipoIscRepository;

		public TipoIscBusiness(
			ITipoIscRepository tipoIscRepository
			)
		{
			this.tipoIscRepository = tipoIscRepository;
		}
		public TipoIscDTO GetById(int id)
		{
			try
			{
				TipoIsc tipoIsc = tipoIscRepository.GetById(id);

				TipoIscDTO tipoIscDTO = Mapper.Map<TipoIscDTO>(tipoIsc);

				return tipoIscDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoIscDTO> Search(TipoIscDTO dto)
		{
			try
			{
				List<TipoIsc> tipoIscList = tipoIscRepository.Search(Mapper.Map<TipoIsc>(dto));

				List<TipoIscDTO> tipoIscDTOList = Mapper.Map<List<TipoIscDTO>>(tipoIscList);

				return tipoIscDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoIscDTO SingleSearch(TipoIscDTO dto)
		{
			try
			{
				TipoIsc tipoIsc = tipoIscRepository.SingleSearch(Mapper.Map<TipoIsc>(dto));

				TipoIscDTO tipoIscDTO = Mapper.Map<TipoIscDTO>(tipoIsc);

				return tipoIscDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoIscDTO dto)
		{
			try
			{
				TipoIsc tipoIsc = Mapper.Map<TipoIsc>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoIscRepository.Insert(tipoIsc);

					dto.TipoIscId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoIscDTO dto)
		{
			try
			{
				TipoIsc tipoIsc = Mapper.Map<TipoIsc>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoIscRepository.Update(tipoIsc);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoIscDTO dto)
		{
			try
			{
				TipoIsc tipoIsc = Mapper.Map<TipoIsc>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoIscRepository.Delete(tipoIsc);

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
