using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProductoSubProcesoProduccionBusiness : IProductoSubProcesoProduccionBusiness
	{
		private readonly IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository;

		public ProductoSubProcesoProduccionBusiness(
			IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository
			)
		{
			this.productoSubProcesoProduccionRepository = productoSubProcesoProduccionRepository;
		}
		public ProductoSubProcesoProduccionDTO GetById(int id)
		{
			try
			{
				ProductoSubProcesoProduccion productoSubProcesoProduccion = productoSubProcesoProduccionRepository.GetById(id);

				ProductoSubProcesoProduccionDTO productoSubProcesoProduccionDTO = Mapper.Map<ProductoSubProcesoProduccionDTO>(productoSubProcesoProduccion);

				return productoSubProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProductoSubProcesoProduccionDTO> Search(ProductoSubProcesoProduccionDTO dto)
		{
			try
			{
				List<ProductoSubProcesoProduccion> productoSubProcesoProduccionList = productoSubProcesoProduccionRepository.Search(Mapper.Map<ProductoSubProcesoProduccion>(dto));

				List<ProductoSubProcesoProduccionDTO> productoSubProcesoProduccionDTOList = Mapper.Map<List<ProductoSubProcesoProduccionDTO>>(productoSubProcesoProduccionList);

				return productoSubProcesoProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProductoSubProcesoProduccionDTO SingleSearch(ProductoSubProcesoProduccionDTO dto)
		{
			try
			{
				ProductoSubProcesoProduccion productoSubProcesoProduccion = productoSubProcesoProduccionRepository.SingleSearch(Mapper.Map<ProductoSubProcesoProduccion>(dto));

				ProductoSubProcesoProduccionDTO productoSubProcesoProduccionDTO = Mapper.Map<ProductoSubProcesoProduccionDTO>(productoSubProcesoProduccion);

				return productoSubProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProductoSubProcesoProduccionDTO dto)
		{
			try
			{
				ProductoSubProcesoProduccion productoSubProcesoProduccion = Mapper.Map<ProductoSubProcesoProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = productoSubProcesoProduccionRepository.Insert(productoSubProcesoProduccion);

					dto.ProductoSubProcesoProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProductoSubProcesoProduccionDTO dto)
		{
			try
			{
				ProductoSubProcesoProduccion productoSubProcesoProduccion = Mapper.Map<ProductoSubProcesoProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = productoSubProcesoProduccionRepository.Update(productoSubProcesoProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProductoSubProcesoProduccionDTO dto)
		{
			try
			{
				ProductoSubProcesoProduccion productoSubProcesoProduccion = Mapper.Map<ProductoSubProcesoProduccion>(dto);

				using (TransactionScope tx = new())
				{
					productoSubProcesoProduccionRepository.Delete(productoSubProcesoProduccion);

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
