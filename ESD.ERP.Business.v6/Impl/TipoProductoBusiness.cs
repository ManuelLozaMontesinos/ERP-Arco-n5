using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoProductoBusiness : ITipoProductoBusiness
	{
		private readonly ITipoProductoRepository tipoProductoRepository;

		public TipoProductoBusiness(
			ITipoProductoRepository tipoProductoRepository
			)
		{
			this.tipoProductoRepository = tipoProductoRepository;
		}
		public TipoProductoDTO GetById(int id)
		{
			try
			{
				TipoProducto tipoProducto = tipoProductoRepository.GetById(id);

				TipoProductoDTO tipoProductoDTO = Mapper.Map<TipoProductoDTO>(tipoProducto);

				return tipoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoProductoDTO> Search(TipoProductoDTO dto)
		{
			try
			{
				List<TipoProducto> tipoProductoList = tipoProductoRepository.Search(Mapper.Map<TipoProducto>(dto));

				List<TipoProductoDTO> tipoProductoDTOList = Mapper.Map<List<TipoProductoDTO>>(tipoProductoList);

				return tipoProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoProductoDTO SingleSearch(TipoProductoDTO dto)
		{
			try
			{
				TipoProducto tipoProducto = tipoProductoRepository.SingleSearch(Mapper.Map<TipoProducto>(dto));

				TipoProductoDTO tipoProductoDTO = Mapper.Map<TipoProductoDTO>(tipoProducto);

				return tipoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoProductoDTO dto)
		{
			try
			{
				TipoProducto tipoProducto = Mapper.Map<TipoProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoProductoRepository.Insert(tipoProducto);

					dto.TipoProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoProductoDTO dto)
		{
			try
			{
				TipoProducto tipoProducto = Mapper.Map<TipoProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoProductoRepository.Update(tipoProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoProductoDTO dto)
		{
			try
			{
				TipoProducto tipoProducto = Mapper.Map<TipoProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoProductoRepository.Delete(tipoProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoProductoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoProducto tipoProducto = tipoProductoRepository.ObtenerPorCodigo(codigo);

				TipoProductoDTO tipoProductoDTO = Mapper.Map<TipoProductoDTO>(tipoProducto);

				return tipoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
