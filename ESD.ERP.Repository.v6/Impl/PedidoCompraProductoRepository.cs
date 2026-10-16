using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class PedidoCompraProductoRepository : IPedidoCompraProductoRepository
	{
		private const string SP_PRE_NAME = "sp_pedido_c_prod";
		public PedidoCompraProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PedidoCompraProducto)
					},
					x =>
					{
						PedidoCompraProducto pcp = x[0] as PedidoCompraProducto;

						return pcp;
					},
					string.Empty
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PedidoCompraProducto> Search(PedidoCompraProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
			parameters.AddParameter("stockEnProveedor", entity.StockEnProveedor);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PedidoCompraProducto),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida)
					},
					x =>
					{
						PedidoCompraProducto pcp = x[0] as PedidoCompraProducto;
						Producto p = x[1] as Producto;
						FamiliaProducto fp = x[2] as FamiliaProducto;
						ClaseProducto cpr = x[3] as ClaseProducto;
						GrupoProducto gp = x[4] as GrupoProducto;
						TipoProducto tp = x[5] as TipoProducto;
						UnidadMedida um = x[6] as UnidadMedida;

						pcp.Producto = p;
						pcp.ProductoId = p?.EntityId;

						if (!Equals(p, null))
                  {
							p.FamiliaProducto = fp;
							p.FamiliaProductoId = fp.EntityId;
							p.UnidadMedida = um;
							p.UnidadMedidaId = um.EntityId;
						}

						if (!Equals(fp, null))
						{
							fp.ClaseProducto = cpr;
							fp.ClaseProductoId = cpr.EntityId;
						}

						if (!Equals(cpr, null))
						{
							cpr.GrupoProducto = gp;
							cpr.GrupoProductoId = gp.EntityId;
						}

						if (!Equals(cpr, null))
						{
							gp.TipoProducto = tp;
							gp.TipoProductoId = tp.EntityId;
						}

						return pcp;
					},
					"producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PedidoCompraProducto SingleSearch(PedidoCompraProducto entity)
		{
			try
			{
				List<PedidoCompraProducto> pedidoCompraProductoList = Search(entity);

				return pedidoCompraProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PedidoCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraId", entity.PedidoCompraId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("stockEnProveedor", entity.StockEnProveedor);
				parameters.AddParameter("esAdicional", entity.EsAdicional);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);
				parameters.AddNewIdParameter();

				int id = DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.INSERT), parameters);

				return id;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Update(PedidoCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraProductoId", entity.PedidoCompraProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("stockEnProveedor", entity.StockEnProveedor);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(PedidoCompraProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraProductoId", entity.PedidoCompraProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<PedidoCompraProducto> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<PedidoCompraProductoType> pedidoCompraProductoTypeList = new();
				pedidoCompraProductoTypeList = (from s in list
															 select new PedidoCompraProductoType
															 {
																 PedidoCompraProductoId = s.PedidoCompraProductoId,
																 PedidoCompraId = s.PedidoCompraId,
																 ProductoId = s.ProductoId.Value,
																 Cantidad = s.Cantidad,
																 PesoNeto = s.PesoNeto,
																 PesoBruto = s.PesoBruto,
																 StockEnProveedor = s.StockEnProveedor,
																 EsAdicional = s.EsAdicional,
																 Estado = s.Estado,
																 UsuarioCreacionId = s.UsuarioCreacionId,
															 }).ToList();

				parameters.AddTableParameter("pedidoCompraProductoList", "dbo.PedidoCompraProductoType", pedidoCompraProductoTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarProducto(PedidoCompraProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("pedidoCompraProductoId", entity.PedidoCompraProductoId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarProducto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
