using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProveedorListaPrecioBusiness : IProveedorListaPrecioBusiness
	{
		private readonly IProveedorListaPrecioRepository proveedorListaPrecioRepository;

		public ProveedorListaPrecioBusiness(
			IProveedorListaPrecioRepository proveedorListaPrecioRepository
			)
		{
			this.proveedorListaPrecioRepository = proveedorListaPrecioRepository;
		}
		public ProveedorListaPrecioDTO GetById(int id)
		{
			try
			{
				ProveedorListaPrecio proveedorListaPrecio = proveedorListaPrecioRepository.GetById(id);

				ProveedorListaPrecioDTO proveedorListaPrecioDTO = Mapper.Map<ProveedorListaPrecioDTO>(proveedorListaPrecio);

				return proveedorListaPrecioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorListaPrecioDTO> Search(ProveedorListaPrecioDTO dto)
		{
			try
			{
				List<ProveedorListaPrecio> proveedorListaPrecioList = proveedorListaPrecioRepository.Search(Mapper.Map<ProveedorListaPrecio>(dto));

				List<ProveedorListaPrecioDTO> proveedorListaPrecioDTOList = Mapper.Map<List<ProveedorListaPrecioDTO>>(proveedorListaPrecioList);

				return proveedorListaPrecioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorListaPrecioDTO SingleSearch(ProveedorListaPrecioDTO dto)
		{
			try
			{
				ProveedorListaPrecio proveedorListaPrecio = proveedorListaPrecioRepository.SingleSearch(Mapper.Map<ProveedorListaPrecio>(dto));

				ProveedorListaPrecioDTO proveedorListaPrecioDTO = Mapper.Map<ProveedorListaPrecioDTO>(proveedorListaPrecio);

				return proveedorListaPrecioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProveedorListaPrecioDTO dto)
		{
			try
			{
				ProveedorListaPrecio proveedorListaPrecio = Mapper.Map<ProveedorListaPrecio>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = proveedorListaPrecioRepository.Insert(proveedorListaPrecio);

					dto.ProveedorListaPrecioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProveedorListaPrecioDTO dto)
		{
			try
			{
				ProveedorListaPrecio proveedorListaPrecio = Mapper.Map<ProveedorListaPrecio>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = proveedorListaPrecioRepository.Update(proveedorListaPrecio);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProveedorListaPrecioDTO dto)
		{
			try
			{
				ProveedorListaPrecio proveedorListaPrecio = Mapper.Map<ProveedorListaPrecio>(dto);

				using (TransactionScope tx = new())
				{
					proveedorListaPrecioRepository.Delete(proveedorListaPrecio);

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
