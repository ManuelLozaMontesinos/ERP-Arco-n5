using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoImpuestoBusiness : ITipoImpuestoBusiness
	{
		private readonly ITipoImpuestoRepository tipoImpuestoRepository;

		public TipoImpuestoBusiness(
			ITipoImpuestoRepository tipoImpuestoRepository
			)
		{
			this.tipoImpuestoRepository = tipoImpuestoRepository;
		}
		public TipoImpuestoDTO GetById(int id)
		{
			try
			{
				TipoImpuesto tipoImpuesto = tipoImpuestoRepository.GetById(id);

				TipoImpuestoDTO tipoImpuestoDTO = Mapper.Map<TipoImpuestoDTO>(tipoImpuesto);

				return tipoImpuestoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoImpuestoDTO> Search(TipoImpuestoDTO dto)
		{
			try
			{
				List<TipoImpuesto> tipoImpuestoList = tipoImpuestoRepository.Search(Mapper.Map<TipoImpuesto>(dto));

				List<TipoImpuestoDTO> tipoImpuestoDTOList = Mapper.Map<List<TipoImpuestoDTO>>(tipoImpuestoList);

				return tipoImpuestoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoImpuestoDTO SingleSearch(TipoImpuestoDTO dto)
		{
			try
			{
				TipoImpuesto tipoImpuesto = tipoImpuestoRepository.SingleSearch(Mapper.Map<TipoImpuesto>(dto));

				TipoImpuestoDTO tipoImpuestoDTO = Mapper.Map<TipoImpuestoDTO>(tipoImpuesto);

				return tipoImpuestoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoImpuestoDTO dto)
		{
			try
			{
				TipoImpuesto tipoImpuesto = Mapper.Map<TipoImpuesto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoImpuestoRepository.Insert(tipoImpuesto);

					dto.TipoImpuestoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoImpuestoDTO dto)
		{
			try
			{
				TipoImpuesto tipoImpuesto = Mapper.Map<TipoImpuesto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoImpuestoRepository.Update(tipoImpuesto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoImpuestoDTO dto)
		{
			try
			{
				TipoImpuesto tipoImpuesto = Mapper.Map<TipoImpuesto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoImpuestoRepository.Delete(tipoImpuesto);

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
