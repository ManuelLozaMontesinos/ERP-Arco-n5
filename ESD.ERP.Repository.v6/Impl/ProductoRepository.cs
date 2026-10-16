using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProductoRepository : IProductoRepository
	{
		private const string SP_PRE_NAME = "sp_producto";
		public Producto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(Color),
						typeof(EstadoProducto),
						typeof(EstadoComercialProducto)
					},
					x =>
					{
						Producto p = x[0] as Producto;
						FamiliaProducto fp = x[1] as FamiliaProducto;
						ClaseProducto cp = x[2] as ClaseProducto;
						GrupoProducto gp = x[3] as GrupoProducto;
						TipoProducto tp = x[4] as TipoProducto;
						UnidadMedida um = x[5] as UnidadMedida;
						Color c = x[6] as Color;
						EstadoProducto ep = x[7] as EstadoProducto;
						EstadoComercialProducto ecp = x[8] as EstadoComercialProducto;

						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cp;
						fp.ClaseProductoId = cp.EntityId;
						cp.GrupoProducto = gp;
						cp.GrupoProductoId = gp.EntityId;
						gp.TipoProducto = tp;
						gp.TipoProductoId = tp.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						p.Color = c;
						p.ColorId = c?.EntityId;
						p.EstadoProducto = ep;
						p.EstadoProductoId = ep.EntityId;
						p.EstadoComercialProducto = ecp;
						p.EstadoComercialProductoId = ecp.EntityId;

						return p;
					},
					"familia_producto_id,clase_producto_id,grupo_producto_id,tipo_producto_id," +
					"unidad_medida_id,color_id,estado_producto_id,estado_com_prod_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Producto> Search(Producto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.FamiliaProducto?.ClaseProducto?.GrupoProducto?.TipoProducto?.EmpresaId);
			parameters.AddParameter("tipoProductoId", entity.FamiliaProducto?.ClaseProducto?.GrupoProducto?.TipoProductoId);
			parameters.AddParameter("grupoProductoId", entity.FamiliaProducto?.ClaseProducto?.GrupoProductoId);
			parameters.AddParameter("claseProductoId", entity.FamiliaProducto?.ClaseProductoId);
			parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
			parameters.AddParameter("producto", entity.Codigo);
			parameters.AddParameter("codRef", entity.CodigoReferencial);
			parameters.AddParameter("containsDescripcion", entity.Descripcion);
			parameters.AddParameter("esCombo", entity.FamiliaProducto?.ClaseProducto?.GrupoProducto?.EsCombo);
			parameters.AddParameter("productoList", entity.ProductoList);
			parameters.AddParameter("periodo", entity.Periodo);
			parameters.AddParameter("esOperacion", entity.EsOperacion);
			parameters.AddParameter("estado", entity.Estado);
			parameters.AddParameter("estadoProd", entity.EstadoProducto?.Codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),
						typeof(Color),
						typeof(EstadoProducto),
						typeof(EstadoComercialProducto)
					},
					x =>
					{
						Producto p = x[0] as Producto;
						FamiliaProducto fp = x[1] as FamiliaProducto;
						ClaseProducto cp = x[2] as ClaseProducto;
						GrupoProducto gp = x[3] as GrupoProducto;
						TipoProducto tp = x[4] as TipoProducto;
						UnidadMedida um = x[5] as UnidadMedida;
						TipoIgv ti = x[6] as TipoIgv;
						Color c = x[7] as Color;
						EstadoProducto ep = x[8] as EstadoProducto;
						EstadoComercialProducto ecp = x[9] as EstadoComercialProducto;

						p.FamiliaProducto = fp;
						p.FamiliaProductoId = fp.EntityId;
						fp.ClaseProducto = cp;
						fp.ClaseProductoId = cp.EntityId;
						cp.GrupoProducto = gp;
						cp.GrupoProductoId = gp.EntityId;
						gp.TipoProducto = tp;
						gp.TipoProductoId = tp.EntityId;
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						p.Color = c;
						p.ColorId = c?.EntityId;
						p.EstadoProducto = ep;
						p.EstadoProductoId = ep.EntityId;
						p.EstadoComercialProducto = ecp;
						p.EstadoComercialProductoId = ecp.EntityId;

						return p;
					},
					"familia_producto_id,clase_producto_id,grupo_producto_id,tipo_producto_id," +
					"unidad_medida_id,tipo_igv_id,color_id,estado_producto_id,estado_com_prod_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Producto SingleSearch(Producto entity)
		{
			try
			{
				List<Producto> productoList = Search(entity);

				return productoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Producto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("producto", entity.Codigo);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("codigoReferencial", entity.CodigoReferencial);
				parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("tipoIgvId", entity.TipoIgvId);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("valorReposicion", entity.ValorReposicion);
				parameters.AddParameter("volumen", entity.Volumen);
				parameters.AddParameter("tienePresentacionVenta", entity.TienePresentacionVenta);
				parameters.AddParameter("esOperacion", entity.EsOperacion);
				parameters.AddParameter("esPerecible", entity.EsPerecible);
				parameters.AddParameter("detalleRequerido", entity.DetalleRequerido);
				parameters.AddParameter("masterPack", entity.MasterPack);
				parameters.AddParameter("colorId", entity.ColorId);
				parameters.AddParameter("estadoProductoId", entity.EstadoProductoId);
				parameters.AddParameter("estadoComercialProductoId", entity.EstadoComercialProductoId);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public int Update(Producto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("codigoReferencial", entity.CodigoReferencial);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("tipoIgvId", entity.TipoIgvId);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("valorReposicion", entity.ValorReposicion);
				parameters.AddParameter("volumen", entity.Volumen);
				parameters.AddParameter("esPerecible", entity.EsPerecible);
				parameters.AddParameter("detalleRequerido", entity.DetalleRequerido);
				parameters.AddParameter("masterPack", entity.MasterPack);
				parameters.AddParameter("colorId", entity.ColorId);
				parameters.AddParameter("estadoProductoId", entity.EstadoProductoId);
				parameters.AddParameter("estadoComercialProductoId", entity.EstadoComercialProductoId);
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
		public void Delete(Producto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("estadoProductoId", entity.EstadoProductoId);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarPorFamiliaProductoId(Producto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("familiaProductoId", entity.FamiliaProductoId);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorFamiliaProductoId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarTienePresentacionVenta(Producto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("tienePresentacionVenta", entity.TienePresentacionVenta);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarTienePresentacionVenta"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarPesoMasterPack(Producto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("codigoReferencial", entity.CodigoReferencial);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("masterPack", entity.MasterPack);
				parameters.AddParameter("volumen", entity.Volumen);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPesoMasterPack"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
	  }
	  public void ActualizarEstado(Producto entity)
	  {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.ESTADO), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
	  }
    }
}