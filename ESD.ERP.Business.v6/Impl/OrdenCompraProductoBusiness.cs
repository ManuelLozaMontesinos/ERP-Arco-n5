using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenCompraProductoBusiness : IOrdenCompraProductoBusiness
	{
		private readonly IOrdenCompraProductoRepository ordenCompraProductoRepository;
		private readonly IOrdenCompraRepository ordenCompraRepository;

		public OrdenCompraProductoBusiness(
			IOrdenCompraProductoRepository ordenCompraProductoRepository,
			IOrdenCompraRepository ordenCompraRepository
			)
		{
			this.ordenCompraProductoRepository = ordenCompraProductoRepository;
			this.ordenCompraRepository = ordenCompraRepository;
		}
		public OrdenCompraProductoDTO GetById(int id)
		{
			try
			{
				OrdenCompraProducto ordenCompraProducto = ordenCompraProductoRepository.GetById(id);

				OrdenCompraProductoDTO ordenCompraProductoDTO = Mapper.Map<OrdenCompraProductoDTO>(ordenCompraProducto);

				return ordenCompraProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenCompraProductoDTO> Search(OrdenCompraProductoDTO dto)
		{
			try
			{
				List<OrdenCompraProducto> ordenCompraProductoList = ordenCompraProductoRepository.Search(Mapper.Map<OrdenCompraProducto>(dto));

				List<OrdenCompraProductoDTO> ordenCompraProductoDTOList = Mapper.Map<List<OrdenCompraProductoDTO>>(ordenCompraProductoList);

				return ordenCompraProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenCompraProductoDTO SingleSearch(OrdenCompraProductoDTO dto)
		{
			try
			{
				OrdenCompraProducto ordenCompraProducto = ordenCompraProductoRepository.SingleSearch(Mapper.Map<OrdenCompraProducto>(dto));

				OrdenCompraProductoDTO ordenCompraProductoDTO = Mapper.Map<OrdenCompraProductoDTO>(ordenCompraProducto);

				return ordenCompraProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenCompraProductoDTO dto)
		{
			try
			{
				OrdenCompraProducto ordenCompraProducto = Mapper.Map<OrdenCompraProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = ordenCompraProductoRepository.Insert(ordenCompraProducto);

					ordenCompraRepository.ActualizarMonto(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					dto.OrdenCompraProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenCompraProductoDTO dto)
		{
			try
			{
				OrdenCompraProducto ordenCompraProducto = Mapper.Map<OrdenCompraProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = ordenCompraProductoRepository.Update(ordenCompraProducto);

					ordenCompraRepository.ActualizarMonto(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenCompraProductoDTO dto)
		{
			try
			{
				OrdenCompraProducto ordenCompraProducto = Mapper.Map<OrdenCompraProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					ordenCompraProductoRepository.Delete(ordenCompraProducto);

					ordenCompraRepository.ActualizarMonto(new OrdenCompra()
					{
						OrdenCompraId = dto.OrdenCompraId,
						UsuarioCreacionId = dto.UsuarioCreacionId
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
