using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class AlmacenVolumenProductoBusiness : IAlmacenVolumenProductoBusiness
	{
		private readonly IAlmacenVolumenProductoRepository almacenVolumenProductoRepository;

		public AlmacenVolumenProductoBusiness(
			IAlmacenVolumenProductoRepository almacenVolumenProductoRepository
			)
		{
			this.almacenVolumenProductoRepository = almacenVolumenProductoRepository;
		}
		public AlmacenVolumenProductoDTO GetById(int id)
		{
			try
			{
				AlmacenVolumenProducto almacenVolumenProducto = almacenVolumenProductoRepository.GetById(id);

				AlmacenVolumenProductoDTO almacenVolumenProductoDTO = Mapper.Map<AlmacenVolumenProductoDTO>(almacenVolumenProducto);

				return almacenVolumenProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenVolumenProductoDTO> Search(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				List<AlmacenVolumenProducto> almacenVolumenProductoList = almacenVolumenProductoRepository.Search(Mapper.Map<AlmacenVolumenProducto>(dto));

				List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = Mapper.Map<List<AlmacenVolumenProductoDTO>>(almacenVolumenProductoList);

				return almacenVolumenProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AlmacenVolumenProductoDTO SingleSearch(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				AlmacenVolumenProducto almacenVolumenProducto = almacenVolumenProductoRepository.SingleSearch(Mapper.Map<AlmacenVolumenProducto>(dto));

				AlmacenVolumenProductoDTO almacenVolumenProductoDTO = Mapper.Map<AlmacenVolumenProductoDTO>(almacenVolumenProducto);

				return almacenVolumenProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				AlmacenVolumenProducto almacenVolumenProducto = Mapper.Map<AlmacenVolumenProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = almacenVolumenProductoRepository.Insert(almacenVolumenProducto);

					dto.AlmacenVolumenProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				AlmacenVolumenProducto almacenVolumenProducto = Mapper.Map<AlmacenVolumenProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = almacenVolumenProductoRepository.Update(almacenVolumenProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				AlmacenVolumenProducto almacenVolumenProducto = Mapper.Map<AlmacenVolumenProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenVolumenProductoRepository.Delete(almacenVolumenProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<AlmacenVolumenProductoDTO> ListarPorSedeId(AlmacenVolumenProductoDTO dto)
      {
			try
			{
				List<AlmacenVolumenProducto> almacenVolumenProductoList = almacenVolumenProductoRepository.ListarPorSedeId(Mapper.Map<AlmacenVolumenProducto>(dto));

				List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = Mapper.Map<List<AlmacenVolumenProductoDTO>>(almacenVolumenProductoList);

				return almacenVolumenProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarLote(List<AlmacenVolumenProductoDTO> list)
		{
			try
			{
				List<AlmacenVolumenProducto> almacenVolumenProductoList = Mapper.Map<List<AlmacenVolumenProducto>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenVolumenProductoRepository.ActualizarLote(almacenVolumenProductoList);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<dynamic> ListarStockMinimo(AlmacenVolumenProductoDTO dto)
		{
			try
			{
				List<dynamic> dynamicList = almacenVolumenProductoRepository.ListarStockMinimo(Mapper.Map<AlmacenVolumenProducto>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
