using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProveedorProductoBusiness : IProveedorProductoBusiness
	{
		private readonly IProveedorProductoRepository proveedorProductoRepository;
		private readonly IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository;

		public ProveedorProductoBusiness(
			IProveedorProductoRepository proveedorProductoRepository,
			IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository
			)
		{
			this.proveedorProductoRepository = proveedorProductoRepository;
			this.proveedorListaPrecioProductoRepository = proveedorListaPrecioProductoRepository;
		}
		public ProveedorProductoDTO GetById(int id)
		{
			try
			{
				ProveedorProducto proveedorProducto = proveedorProductoRepository.GetById(id);

				ProveedorProductoDTO proveedorProductoDTO = Mapper.Map<ProveedorProductoDTO>(proveedorProducto);

				return proveedorProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorProductoDTO> Search(ProveedorProductoDTO dto)
		{
			try
			{
				List<ProveedorProducto> proveedorProductoList = proveedorProductoRepository.Search(Mapper.Map<ProveedorProducto>(dto));

				List<ProveedorProductoDTO> proveedorProductoDTOList = Mapper.Map<List<ProveedorProductoDTO>>(proveedorProductoList);

				return proveedorProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorProductoDTO SingleSearch(ProveedorProductoDTO dto)
		{
			try
			{
				ProveedorProducto proveedorProducto = proveedorProductoRepository.SingleSearch(Mapper.Map<ProveedorProducto>(dto));

				ProveedorProductoDTO proveedorProductoDTO = Mapper.Map<ProveedorProductoDTO>(proveedorProducto);

				return proveedorProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProveedorProductoDTO dto)
		{
			try
			{
				ProveedorProducto proveedorProducto = Mapper.Map<ProveedorProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = proveedorProductoRepository.Insert(proveedorProducto);

					dto.ProveedorProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProveedorProductoDTO dto)
		{
			try
			{
				ProveedorProducto proveedorProducto = Mapper.Map<ProveedorProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = proveedorProductoRepository.Update(proveedorProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProveedorProductoDTO dto)
		{
			try
			{
				ProveedorProducto proveedorProducto = Mapper.Map<ProveedorProducto>(dto);

				using (TransactionScope tx = new())
				{
					proveedorProductoRepository.Delete(proveedorProducto);

					proveedorListaPrecioProductoRepository.EliminarPorProveedorIdProductoId(new ProveedorListaPrecioProducto()
               {
						ProveedorListaPrecio = new ProveedorListaPrecio() { ProveedorId = dto.ProveedorId},
						ProductoId = dto.ProductoId
               });

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
