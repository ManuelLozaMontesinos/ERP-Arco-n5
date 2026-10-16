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
	public class FamiliaProductoBusiness : IFamiliaProductoBusiness
	{
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly ITablaRepository tablaRepository;
		private readonly IClaseProductoRepository claseProductoRepository;
		private readonly IProductoRepository productoRepository;

		public FamiliaProductoBusiness(
			IFamiliaProductoRepository familiaProductoRepository,
			IClaseProductoRepository claseProductoRepository,
			ITablaRepository tablaRepository,
			IProductoRepository productoRepository
			)
		{
			this.familiaProductoRepository = familiaProductoRepository;
			this.claseProductoRepository = claseProductoRepository;
			this.tablaRepository = tablaRepository;
			this.productoRepository = productoRepository;
		}
		public FamiliaProductoDTO GetById(int id)
		{
			try
			{
				FamiliaProducto familiaProducto = familiaProductoRepository.GetById(id);

				FamiliaProductoDTO familiaProductoDTO = Mapper.Map<FamiliaProductoDTO>(familiaProducto);

				return familiaProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<FamiliaProductoDTO> Search(FamiliaProductoDTO dto)
		{
			try
			{
				List<FamiliaProducto> familiaProductoList = familiaProductoRepository.Search(Mapper.Map<FamiliaProducto>(dto));

				List<FamiliaProductoDTO> familiaProductoDTOList = Mapper.Map<List<FamiliaProductoDTO>>(familiaProductoList);

				return familiaProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public FamiliaProductoDTO SingleSearch(FamiliaProductoDTO dto)
		{
			try
			{
				FamiliaProducto familiaProducto = familiaProductoRepository.SingleSearch(Mapper.Map<FamiliaProducto>(dto));

				FamiliaProductoDTO familiaProductoDTO = Mapper.Map<FamiliaProductoDTO>(familiaProducto);

				return familiaProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(FamiliaProductoDTO dto)
		{
			try
			{
				FamiliaProducto familiaProducto = Mapper.Map<FamiliaProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					Tabla tabla = tablaRepository.ObtenerPorCodigo(TablaConstante.FAMILIA_PRODUCTO);
					ClaseProducto claseProducto = claseProductoRepository.GetById(familiaProducto.ClaseProductoId);
					claseProducto.Correlativo += 1;
					claseProductoRepository.ActualizarCorrelativo(claseProducto);

					familiaProducto.Codigo = claseProducto.Correlativo.ToString(tabla.FormatoCorrelativo);

					newId = familiaProductoRepository.Insert(familiaProducto);

					dto.FamiliaProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(FamiliaProductoDTO dto)
		{
			try
			{
				FamiliaProducto familiaProducto = Mapper.Map<FamiliaProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = familiaProductoRepository.Update(familiaProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(FamiliaProductoDTO dto)
		{
			try
			{
				FamiliaProducto familiaProducto = Mapper.Map<FamiliaProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					productoRepository.EliminarPorFamiliaProductoId(new Producto()
					{
						FamiliaProductoId = familiaProducto.EntityId,
						FechaBaja = familiaProducto.FechaBaja
					});

					familiaProductoRepository.Delete(familiaProducto);

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
