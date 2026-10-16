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
	public class GrupoProductoBusiness : IGrupoProductoBusiness
	{
		private readonly IGrupoProductoRepository grupoProductoRepository;
		private readonly IClaseProductoRepository claseProductoRepository;
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly IProductoRepository productoRepository;

		public GrupoProductoBusiness(
			IGrupoProductoRepository grupoProductoRepository,
			IClaseProductoRepository claseProductoRepository,
			IFamiliaProductoRepository familiaProductoRepository,
			IProductoRepository productoRepository
			)
		{
			this.grupoProductoRepository = grupoProductoRepository;
			this.claseProductoRepository = claseProductoRepository;
			this.familiaProductoRepository = familiaProductoRepository;
			this.productoRepository = productoRepository;
		}
		public GrupoProductoDTO GetById(int id)
		{
			try
			{
				GrupoProducto grupoProducto = grupoProductoRepository.GetById(id);

				GrupoProductoDTO grupoProductoDTO = Mapper.Map<GrupoProductoDTO>(grupoProducto);

				return grupoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<GrupoProductoDTO> Search(GrupoProductoDTO dto)
		{
			try
			{
				List<GrupoProducto> grupoProductoList = grupoProductoRepository.Search(Mapper.Map<GrupoProducto>(dto));

				List<GrupoProductoDTO> grupoProductoDTOList = Mapper.Map<List<GrupoProductoDTO>>(grupoProductoList);

				return grupoProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public GrupoProductoDTO SingleSearch(GrupoProductoDTO dto)
		{
			try
			{
				GrupoProducto grupoProducto = grupoProductoRepository.SingleSearch(Mapper.Map<GrupoProducto>(dto));

				GrupoProductoDTO grupoProductoDTO = Mapper.Map<GrupoProductoDTO>(grupoProducto);

				return grupoProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(GrupoProductoDTO dto)
		{
			try
			{
				GrupoProducto grupoProducto = Mapper.Map<GrupoProducto>(dto);

				int newId = 0;

				grupoProducto.NotGrupoProductoId = grupoProducto.GrupoProductoId;
				grupoProducto = grupoProductoRepository.SingleSearch(grupoProducto);
				if (grupoProducto != null)
					throw new Exception(SR.GetString("EX_grupo_producto_ya_registrado", grupoProducto.Codigo));

				grupoProducto = Mapper.Map<GrupoProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					newId = grupoProductoRepository.Insert(grupoProducto);

					dto.GrupoProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(GrupoProductoDTO dto)
		{
			try
			{
				GrupoProducto grupoProducto = Mapper.Map<GrupoProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = grupoProductoRepository.Update(grupoProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(GrupoProductoDTO dto)
		{
			try
			{
				GrupoProducto grupoProducto = Mapper.Map<GrupoProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					List<ClaseProducto> claseProductoList = claseProductoRepository.Search(new ClaseProducto() { GrupoProductoId = grupoProducto.EntityId});
					foreach(ClaseProducto claseProducto in claseProductoList)
               {
						List<FamiliaProducto> familiaProductoList = familiaProductoRepository.Search(new FamiliaProducto() 
							{ FamiliaProductoId = claseProducto.EntityId });

						foreach (FamiliaProducto familiaProducto in familiaProductoList)
							productoRepository.EliminarPorFamiliaProductoId(new Producto()
							{
								FamiliaProductoId = familiaProducto.EntityId,
								FechaBaja = grupoProducto.FechaBaja
							});

						familiaProductoRepository.EliminarPorClaseProductoId(new FamiliaProducto()
						{
							ClaseProductoId = claseProducto.EntityId,
							FechaBaja = grupoProducto.FechaBaja
						});
					}

					claseProductoRepository.EliminarPorGrupoProductoId(new ClaseProducto() { 
						GrupoProductoId = grupoProducto.EntityId,
						FechaBaja = grupoProducto.FechaBaja
					});

					grupoProductoRepository.Delete(grupoProducto);

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
