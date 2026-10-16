using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProductoDetalleBusiness : IProductoDetalleBusiness
	{
		private readonly IProductoDetalleRepository productoDetalleRepository;
		private readonly IProductoRepository productoRepository;

		public ProductoDetalleBusiness(
			IProductoDetalleRepository productoDetalleRepository,
			IProductoRepository productoRepository
			)
		{
			this.productoDetalleRepository = productoDetalleRepository;
			this.productoRepository = productoRepository;
		}
		public ProductoDetalleDTO GetById(int id)
		{
			try
			{
				ProductoDetalle productoDetalle = productoDetalleRepository.GetById(id);

				ProductoDetalleDTO productoDetalleDTO = Mapper.Map<ProductoDetalleDTO>(productoDetalle);

				return productoDetalleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProductoDetalleDTO> Search(ProductoDetalleDTO dto)
		{
			try
			{
				List<ProductoDetalle> productoDetalleList = productoDetalleRepository.Search(Mapper.Map<ProductoDetalle>(dto));

				List<ProductoDetalleDTO> productoDetalleDTOList = Mapper.Map<List<ProductoDetalleDTO>>(productoDetalleList);

				return productoDetalleDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProductoDetalleDTO SingleSearch(ProductoDetalleDTO dto)
		{
			try
			{
				ProductoDetalle productoDetalle = productoDetalleRepository.SingleSearch(Mapper.Map<ProductoDetalle>(dto));

				ProductoDetalleDTO productoDetalleDTO = Mapper.Map<ProductoDetalleDTO>(productoDetalle);

				return productoDetalleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProductoDetalleDTO dto)
		{
			try
			{
				ProductoDetalle productoDetalle = Mapper.Map<ProductoDetalle>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = productoDetalleRepository.Insert(productoDetalle);

					dto.ProductoDetalleId = newId;

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProductoDetalleDTO dto)
		{
			try
			{
				ProductoDetalle productoDetalle = Mapper.Map<ProductoDetalle>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = productoDetalleRepository.Update(productoDetalle);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProductoDetalleDTO dto)
		{
			try
			{
				ProductoDetalle productoDetalle = Mapper.Map<ProductoDetalle>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					productoDetalleRepository.Delete(productoDetalle);

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
