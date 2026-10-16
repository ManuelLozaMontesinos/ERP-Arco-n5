using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ClaseProductoBusiness : IClaseProductoBusiness
	{
		private readonly IClaseProductoRepository claseProductoRepository;
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly IProductoRepository productoRepository;
		public ClaseProductoBusiness(
			IClaseProductoRepository claseProductoRepository,
			IFamiliaProductoRepository familiaProductoRepository,
			IProductoRepository productoRepository
			)
		{
			this.claseProductoRepository = claseProductoRepository;
			this.familiaProductoRepository = familiaProductoRepository;
			this.productoRepository = productoRepository;

		}
		public ClaseProductoDTO GetById(int id)
		{
			try
			{
				ClaseProducto claseProducto = claseProductoRepository.GetById(id);

				ClaseProductoDTO claseProductoDTO = Mapper.Map<ClaseProductoDTO>(claseProducto);

				return claseProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ClaseProductoDTO> Search(ClaseProductoDTO dto)
		{
			try
			{
				List<ClaseProducto> claseProductoList = claseProductoRepository.Search(Mapper.Map<ClaseProducto>(dto));

				List<ClaseProductoDTO> claseProductoDTOList = Mapper.Map<List<ClaseProductoDTO>>(claseProductoList);

				return claseProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ClaseProductoDTO SingleSearch(ClaseProductoDTO dto)
		{
			try
			{
				ClaseProducto claseProducto = claseProductoRepository.SingleSearch(Mapper.Map<ClaseProducto>(dto));

				ClaseProductoDTO claseProductoDTO = Mapper.Map<ClaseProductoDTO>(claseProducto);

				return claseProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ClaseProductoDTO dto)
		{
			try
			{
				ClaseProducto claseProducto = Mapper.Map<ClaseProducto>(dto);

				int newId = 0;

				claseProducto.NotClaseProductoId = claseProducto.ClaseProductoId;
				claseProducto = claseProductoRepository.SingleSearch(claseProducto);
				if (claseProducto != null)
					throw new Exception(SR.GetString("EX_clase_producto_ya_registrada", claseProducto.Codigo));

				claseProducto = Mapper.Map<ClaseProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					newId = claseProductoRepository.Insert(claseProducto);

					dto.ClaseProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ClaseProductoDTO dto)
		{
			try
			{
				ClaseProducto claseProducto = Mapper.Map<ClaseProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = claseProductoRepository.Update(claseProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ClaseProductoDTO dto)
		{
			try
			{
				ClaseProducto claseProducto = Mapper.Map<ClaseProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					List<FamiliaProducto> familiaProductoList = familiaProductoRepository.Search(new FamiliaProducto()
					{ ClaseProductoId = claseProducto.EntityId });

					foreach (FamiliaProducto familiaProducto in familiaProductoList)
						productoRepository.EliminarPorFamiliaProductoId(new Producto()
						{
							FamiliaProductoId = familiaProducto.EntityId,
							FechaBaja = claseProducto.FechaBaja
						});

					familiaProductoRepository.EliminarPorClaseProductoId(new FamiliaProducto()
					{
						ClaseProductoId = claseProducto.EntityId,
						FechaBaja = claseProducto.FechaBaja
					});

					claseProductoRepository.Delete(claseProducto);

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
