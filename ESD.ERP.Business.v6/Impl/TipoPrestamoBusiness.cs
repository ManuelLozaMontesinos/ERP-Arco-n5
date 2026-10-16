using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoPrestamoBusiness : ITipoPrestamoBusiness
	{
		private readonly ITipoPrestamoRepository tipoPrestamoRepository;

		public TipoPrestamoBusiness(
			ITipoPrestamoRepository tipoPrestamoRepository
			)
		{
			this.tipoPrestamoRepository = tipoPrestamoRepository;
		}
		public TipoPrestamoDTO GetById(int id)
		{
			try
			{
				TipoPrestamo tipoPrestamo = tipoPrestamoRepository.GetById(id);

				TipoPrestamoDTO tipoPrestamoDTO = Mapper.Map<TipoPrestamoDTO>(tipoPrestamo);

				return tipoPrestamoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoPrestamoDTO> Search(TipoPrestamoDTO dto)
		{
			try
			{
				List<TipoPrestamo> tipoPrestamoList = tipoPrestamoRepository.Search(Mapper.Map<TipoPrestamo>(dto));

				List<TipoPrestamoDTO> tipoPrestamoDTOList = Mapper.Map<List<TipoPrestamoDTO>>(tipoPrestamoList);

				return tipoPrestamoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoPrestamoDTO SingleSearch(TipoPrestamoDTO dto)
		{
			try
			{
				TipoPrestamo tipoPrestamo = tipoPrestamoRepository.SingleSearch(Mapper.Map<TipoPrestamo>(dto));

				TipoPrestamoDTO tipoPrestamoDTO = Mapper.Map<TipoPrestamoDTO>(tipoPrestamo);

				return tipoPrestamoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoPrestamoDTO dto)
		{
			try
			{
				TipoPrestamo tipoPrestamo = Mapper.Map<TipoPrestamo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoPrestamoRepository.Insert(tipoPrestamo);

					dto.TipoPrestamoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoPrestamoDTO dto)
		{
			try
			{
				TipoPrestamo tipoPrestamo = Mapper.Map<TipoPrestamo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoPrestamoRepository.Update(tipoPrestamo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoPrestamoDTO dto)
		{
			try
			{
				TipoPrestamo tipoPrestamo = Mapper.Map<TipoPrestamo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoPrestamoRepository.Delete(tipoPrestamo);

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
