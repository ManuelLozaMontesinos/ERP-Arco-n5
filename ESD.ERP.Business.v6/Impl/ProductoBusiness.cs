using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;

namespace ESD.ERP.Business.Impl
{
	public class ProductoBusiness : IProductoBusiness
	{
		private readonly IProductoRepository productoRepository;
		private readonly ITablaRepository tablaRepository;
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly IListaPrecioRepository listaPrecioRepository;
		private readonly IListaPrecioProductoRepository listaPrecioProductoRepository;

		public ProductoBusiness(
			IProductoRepository productoRepository,
			ITablaRepository tablaRepository,
			IFamiliaProductoRepository familiaProductoRepository,
			IListaPrecioRepository listaPrecioRepository,
			IListaPrecioProductoRepository listaPrecioProductoRepository
			)
		{
			this.productoRepository = productoRepository;
			this.tablaRepository = tablaRepository;
			this.familiaProductoRepository = familiaProductoRepository;
			this.listaPrecioRepository = listaPrecioRepository;
			this.listaPrecioProductoRepository = listaPrecioProductoRepository;
		}
		public ProductoDTO GetById(int id)
		{
			try
			{
				Producto producto = productoRepository.GetById(id);

				ProductoDTO productoDTO = Mapper.Map<ProductoDTO>(producto);

				return productoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProductoDTO> Search(ProductoDTO dto)
		{
			try
			{
				List<Producto> productoList = productoRepository.Search(Mapper.Map<Producto>(dto));

				List<ProductoDTO> productoDTOList = Mapper.Map<List<ProductoDTO>>(productoList);

				return productoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProductoDTO SingleSearch(ProductoDTO dto)
		{
			try
			{
				Producto producto = productoRepository.SingleSearch(Mapper.Map<Producto>(dto));

				ProductoDTO productoDTO = Mapper.Map<ProductoDTO>(producto);

				return productoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProductoDTO dto)
		{
			try
			{
				Producto producto = Mapper.Map<Producto>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					Tabla tabla = tablaRepository.ObtenerPorCodigo(TablaConstante.PRODUCTO);
					FamiliaProducto familiaProducto = familiaProductoRepository.GetById(producto.FamiliaProductoId);
					familiaProducto.Correlativo += 1;
					familiaProductoRepository.ActualizarCorrelativo(familiaProducto);

					string codigo = string.Concat(familiaProducto.ClaseProducto.GrupoProducto.Codigo, familiaProducto.ClaseProducto.Codigo,
						familiaProducto.Codigo, familiaProducto.Correlativo.ToString(tabla.FormatoCorrelativo));

					producto.Codigo = codigo;
					newId = productoRepository.Insert(producto);

					producto = productoRepository.GetById(newId);

					List<ListaPrecio> listaPrecioList = listaPrecioRepository.Search(new ListaPrecio() { 
						EmpresaSede = new EmpresaSede() { EmpresaId = producto.FamiliaProducto.ClaseProducto.GrupoProducto.TipoProducto.EmpresaId },
						Estado = EstadoConstante.ACTIVO
					});

					foreach(ListaPrecio listaPrecio in listaPrecioList)
               {
						listaPrecioProductoRepository.Insert(new ListaPrecioProducto() { 
							ListaPrecioId = listaPrecio.EntityId,
							ProductoId = newId,
							PermiteDescuento = SiNoConstante.N,
							Estado = EstadoConstante.INACTIVO,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});
					}

					dto.ProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProductoDTO dto)
		{
			try
			{
				Producto producto = Mapper.Map<Producto>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = productoRepository.Update(producto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProductoDTO dto)
		{
			try
			{
				Producto producto = Mapper.Map<Producto>(dto);

				using (TransactionScope tx = new())
				{

					productoRepository.Delete(producto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
        public void ActualizarEstado(ProductoDTO dto)
        {
            try
            {
                Producto producto = Mapper.Map<Producto>(dto);

                using (TransactionScope tx = new())
                {

                    productoRepository.ActualizarEstado(new Producto()
                    {
                        ProductoId = dto.ProductoId,
                        UsuarioCreacionId = dto.UsuarioCreacionId,
                        Estado = dto.Estado
                    });

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ActualizarPesoMasterPack(ProductoDTO dto)
      {
			try
			{
				Producto producto = Mapper.Map<Producto>(dto);

				using (TransactionScope tx = new())
				{
					productoRepository.ActualizarPesoMasterPack(producto);

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
