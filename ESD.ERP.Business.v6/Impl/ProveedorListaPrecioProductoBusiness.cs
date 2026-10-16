using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProveedorListaPrecioProductoBusiness : IProveedorListaPrecioProductoBusiness
	{
		private readonly IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository;

		public ProveedorListaPrecioProductoBusiness(
			IProveedorListaPrecioProductoRepository proveedorListaPrecioProductoRepository
			)
		{
			this.proveedorListaPrecioProductoRepository = proveedorListaPrecioProductoRepository;
		}
		public ProveedorListaPrecioProductoDTO GetById(int id)
		{
			try
			{
				ProveedorListaPrecioProducto proveedorListaPrecioProducto = proveedorListaPrecioProductoRepository.GetById(id);

				ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO = Mapper.Map<ProveedorListaPrecioProductoDTO>(proveedorListaPrecioProducto);

				return proveedorListaPrecioProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorListaPrecioProductoDTO> Search(ProveedorListaPrecioProductoDTO dto)
		{
			try
			{
				List<ProveedorListaPrecioProducto> proveedorListaPrecioProductoList = proveedorListaPrecioProductoRepository.Search(Mapper.Map<ProveedorListaPrecioProducto>(dto));

				List<ProveedorListaPrecioProductoDTO> proveedorListaPrecioProductoDTOList = Mapper.Map<List<ProveedorListaPrecioProductoDTO>>(proveedorListaPrecioProductoList);

				return proveedorListaPrecioProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorListaPrecioProductoDTO SingleSearch(ProveedorListaPrecioProductoDTO dto)
		{
			try
			{
				ProveedorListaPrecioProducto proveedorListaPrecioProducto = proveedorListaPrecioProductoRepository.SingleSearch(Mapper.Map<ProveedorListaPrecioProducto>(dto));

				ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO = Mapper.Map<ProveedorListaPrecioProductoDTO>(proveedorListaPrecioProducto);

				return proveedorListaPrecioProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProveedorListaPrecioProductoDTO dto)
		{
			try
			{
				ProveedorListaPrecioProducto proveedorListaPrecioProducto = Mapper.Map<ProveedorListaPrecioProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = proveedorListaPrecioProductoRepository.Insert(proveedorListaPrecioProducto);

					dto.ProveedorListaPrecioProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProveedorListaPrecioProductoDTO dto)
		{
			try
			{
				ProveedorListaPrecioProducto proveedorListaPrecioProducto = proveedorListaPrecioProductoRepository.GetById(dto.Id);

				if(proveedorListaPrecioProducto.FechaVigente != dto.FechaVigente)
            {
					proveedorListaPrecioProducto.ValorUnitarioAnterior = proveedorListaPrecioProducto.ValorUnitario;
					proveedorListaPrecioProducto.PrecioUnitarioAnterior = proveedorListaPrecioProducto.PrecioUnitario;
					proveedorListaPrecioProducto.FechaAnterior = proveedorListaPrecioProducto.FechaVigente;
				}

				proveedorListaPrecioProducto.ValorUnitario = dto.ValorUnitario;
				proveedorListaPrecioProducto.PrecioUnitario = dto.PrecioUnitario;
				proveedorListaPrecioProducto.FechaVigente = dto.FechaVigente;
				proveedorListaPrecioProducto.UsuarioCreacionId = dto.UsuarioCreacionId;

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = proveedorListaPrecioProductoRepository.Update(proveedorListaPrecioProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProveedorListaPrecioProductoDTO dto)
		{
			try
			{
				ProveedorListaPrecioProducto proveedorListaPrecioProducto = Mapper.Map<ProveedorListaPrecioProducto>(dto);

				using (TransactionScope tx = new())
				{
					proveedorListaPrecioProductoRepository.Delete(proveedorListaPrecioProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarLote(List<ProveedorListaPrecioProductoDTO> list)
		{
			try
			{
				List<ProveedorListaPrecioProducto> proveedorListaPrecioProductoList = Mapper.Map<List<ProveedorListaPrecioProducto>>(list);

				using (TransactionScope tx = new())
				{
					proveedorListaPrecioProductoRepository.ActualizarLote(proveedorListaPrecioProductoList);

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
