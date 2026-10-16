using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoDescuentoBusiness : ITipoDescuentoBusiness
	{
		private readonly ITipoDescuentoRepository tipoDescuentoRepository;

		public TipoDescuentoBusiness(
			ITipoDescuentoRepository tipoDescuentoRepository
			)
		{
			this.tipoDescuentoRepository = tipoDescuentoRepository;
		}
		public TipoDescuentoDTO GetById(int id)
		{
			try
			{
				TipoDescuento tipoDescuento = tipoDescuentoRepository.GetById(id);

				TipoDescuentoDTO tipoDescuentoDTO = Mapper.Map<TipoDescuentoDTO>(tipoDescuento);

				return tipoDescuentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoDescuentoDTO> Search(TipoDescuentoDTO dto)
		{
			try
			{
				List<TipoDescuento> tipoDescuentoList = tipoDescuentoRepository.Search(Mapper.Map<TipoDescuento>(dto));

				List<TipoDescuentoDTO> tipoDescuentoDTOList = Mapper.Map<List<TipoDescuentoDTO>>(tipoDescuentoList);

				return tipoDescuentoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDescuentoDTO SingleSearch(TipoDescuentoDTO dto)
		{
			try
			{
				TipoDescuento tipoDescuento = tipoDescuentoRepository.SingleSearch(Mapper.Map<TipoDescuento>(dto));

				TipoDescuentoDTO tipoDescuentoDTO = Mapper.Map<TipoDescuentoDTO>(tipoDescuento);

				return tipoDescuentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoDescuentoDTO dto)
		{
			try
			{
				TipoDescuento tipoDescuento = Mapper.Map<TipoDescuento>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoDescuentoRepository.Insert(tipoDescuento);

					dto.TipoDescuentoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoDescuentoDTO dto)
		{
			try
			{
				TipoDescuento tipoDescuento = Mapper.Map<TipoDescuento>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoDescuentoRepository.Update(tipoDescuento);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoDescuentoDTO dto)
		{
			try
			{
				TipoDescuento tipoDescuento = Mapper.Map<TipoDescuento>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoDescuentoRepository.Delete(tipoDescuento);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoDescuentoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoDescuento tipoDescuento = tipoDescuentoRepository.ObtenerPorCodigo(codigo);

				TipoDescuentoDTO tipoDescuentoDTO = Mapper.Map<TipoDescuentoDTO>(tipoDescuento);

				return tipoDescuentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
