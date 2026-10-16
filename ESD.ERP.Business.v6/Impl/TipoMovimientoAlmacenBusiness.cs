using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoMovimientoAlmacenBusiness : ITipoMovimientoAlmacenBusiness
	{
		private readonly ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository;

		public TipoMovimientoAlmacenBusiness(
			ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository
			)
		{
			this.tipoMovimientoAlmacenRepository = tipoMovimientoAlmacenRepository;
		}
		public TipoMovimientoAlmacenDTO GetById(int id)
		{
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.GetById(id);

				TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = Mapper.Map<TipoMovimientoAlmacenDTO>(tipoMovimientoAlmacen);

				return tipoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoMovimientoAlmacenDTO> Search(TipoMovimientoAlmacenDTO dto)
		{
			try
			{
				List<TipoMovimientoAlmacen> tipoMovimientoAlmacenList = tipoMovimientoAlmacenRepository.Search(Mapper.Map<TipoMovimientoAlmacen>(dto));

				List<TipoMovimientoAlmacenDTO> tipoMovimientoAlmacenDTOList = Mapper.Map<List<TipoMovimientoAlmacenDTO>>(tipoMovimientoAlmacenList);

				return tipoMovimientoAlmacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoMovimientoAlmacenDTO SingleSearch(TipoMovimientoAlmacenDTO dto)
		{
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.SingleSearch(Mapper.Map<TipoMovimientoAlmacen>(dto));

				TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = Mapper.Map<TipoMovimientoAlmacenDTO>(tipoMovimientoAlmacen);

				return tipoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoMovimientoAlmacenDTO dto)
		{
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = Mapper.Map<TipoMovimientoAlmacen>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoMovimientoAlmacenRepository.Insert(tipoMovimientoAlmacen);

					dto.TipoMovimientoAlmacenId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoMovimientoAlmacenDTO dto)
		{
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = Mapper.Map<TipoMovimientoAlmacen>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoMovimientoAlmacenRepository.Update(tipoMovimientoAlmacen);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoMovimientoAlmacenDTO dto)
		{
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = Mapper.Map<TipoMovimientoAlmacen>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoMovimientoAlmacenRepository.Delete(tipoMovimientoAlmacen);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public TipoMovimientoAlmacenDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				TipoMovimientoAlmacen tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.ObtenerPorCodigo(codigo);

				TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = Mapper.Map<TipoMovimientoAlmacenDTO>(tipoMovimientoAlmacen);

				return tipoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
