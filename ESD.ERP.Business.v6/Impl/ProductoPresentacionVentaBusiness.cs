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
	public class ProductoPresentacionVentaBusiness : IProductoPresentacionVentaBusiness
	{
		private readonly IProductoPresentacionVentaRepository productoPresentacionVentaRepository;
		private readonly IProductoRepository productoRepository;

		public ProductoPresentacionVentaBusiness(
			IProductoPresentacionVentaRepository productoPresentacionVentaRepository,
			IProductoRepository productoRepository
			)
		{
			this.productoPresentacionVentaRepository = productoPresentacionVentaRepository;
			this.productoRepository = productoRepository;
		}
		public ProductoPresentacionVentaDTO GetById(int id)
		{
			try
			{
				ProductoPresentacionVenta productoPresentacionVenta = productoPresentacionVentaRepository.GetById(id);

				ProductoPresentacionVentaDTO productoPresentacionVentaDTO = Mapper.Map<ProductoPresentacionVentaDTO>(productoPresentacionVenta);

				return productoPresentacionVentaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProductoPresentacionVentaDTO> Search(ProductoPresentacionVentaDTO dto)
		{
			try
			{
				List<ProductoPresentacionVenta> productoPresentacionVentaList = productoPresentacionVentaRepository.Search(Mapper.Map<ProductoPresentacionVenta>(dto));

				List<ProductoPresentacionVentaDTO> productoPresentacionVentaDTOList = Mapper.Map<List<ProductoPresentacionVentaDTO>>(productoPresentacionVentaList);

				return productoPresentacionVentaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProductoPresentacionVentaDTO SingleSearch(ProductoPresentacionVentaDTO dto)
		{
			try
			{
				ProductoPresentacionVenta productoPresentacionVenta = productoPresentacionVentaRepository.SingleSearch(Mapper.Map<ProductoPresentacionVenta>(dto));

				ProductoPresentacionVentaDTO productoPresentacionVentaDTO = Mapper.Map<ProductoPresentacionVentaDTO>(productoPresentacionVenta);

				return productoPresentacionVentaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProductoPresentacionVentaDTO dto)
		{
			try
			{
				ProductoPresentacionVenta productoPresentacionVenta = Mapper.Map<ProductoPresentacionVenta>(dto);

				productoPresentacionVenta = productoPresentacionVentaRepository.SingleSearch(productoPresentacionVenta);
				if (productoPresentacionVenta != null)
					throw new Exception(SR.GetString("EX_unidad_medida_ya_registrada"));

				productoPresentacionVenta = Mapper.Map<ProductoPresentacionVenta>(dto);
				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = productoPresentacionVentaRepository.Insert(productoPresentacionVenta);

					dto.ProductoPresentacionVentaId = newId;

					productoRepository.ActualizarTienePresentacionVenta(new Producto() {
						ProductoId = productoPresentacionVenta.ProductoId,
						TienePresentacionVenta = SiNoConstante.S,
						UsuarioCreacionId = productoPresentacionVenta.UsuarioCreacionId
					});

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProductoPresentacionVentaDTO dto)
		{
			try
			{
				ProductoPresentacionVenta productoPresentacionVenta = Mapper.Map<ProductoPresentacionVenta>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = productoPresentacionVentaRepository.Update(productoPresentacionVenta);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProductoPresentacionVentaDTO dto)
		{
			try
			{
				ProductoPresentacionVenta productoPresentacionVenta = Mapper.Map<ProductoPresentacionVenta>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					productoPresentacionVentaRepository.Delete(productoPresentacionVenta);

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
