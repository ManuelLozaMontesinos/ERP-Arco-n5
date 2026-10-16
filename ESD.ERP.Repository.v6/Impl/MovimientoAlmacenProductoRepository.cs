using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenProductoRepository : IMovimientoAlmacenProductoRepository
	{
		private const string SP_PRE_NAME = "sp_mov_alm_prod";
		public MovimientoAlmacenProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacenProducto),
						typeof(MovimientoAlmacen),
						typeof(TipoMovimientoAlmacen)
					},
					x =>
					{
						MovimientoAlmacenProducto map = x[0] as MovimientoAlmacenProducto;
						MovimientoAlmacen ma = x[1] as MovimientoAlmacen;
						TipoMovimientoAlmacen tma = x[2] as TipoMovimientoAlmacen;

						map.MovimientoAlmacen = ma;
						map.MovimientoAlmacenId = ma.EntityId;
						ma.TipoMovimientoAlmacen = tma;
						ma.TipoMovimientoAlmacenId = tma.EntityId;

						return map;
					},
					"mov_almacen_id,tipo_mov_alma_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<MovimientoAlmacenProducto> Search(MovimientoAlmacenProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
			parameters.AddParameter("conCantidadReclasificacion", entity.ConCantidadReclasificacion);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacenProducto),
						typeof(MovimientoAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(Producto),
						typeof(UnidadMedida),
						typeof(Producto)
					},
					x =>
					{
						MovimientoAlmacenProducto map = x[0] as MovimientoAlmacenProducto;
						MovimientoAlmacen ma = x[1] as MovimientoAlmacen;
						TipoMovimientoAlmacen tma = x[2] as TipoMovimientoAlmacen;
						Producto p = x[3] as Producto;
						UnidadMedida um = x[4] as UnidadMedida;
						Producto p2 = x[5] as Producto;

						map.MovimientoAlmacen = ma;
						map.MovimientoAlmacenId = ma.EntityId;
						ma.TipoMovimientoAlmacen = tma;
						ma.TipoMovimientoAlmacenId = tma.EntityId;
						map.Producto = p;
						map.ProductoId = p.EntityId;
						p.UnidadMedida = um;
						p.UnidadMedidaId = um.EntityId;
						map.ProductoReclasificacion = p2;
						map.ProductoReclasificacionId = p2?.EntityId;

						return map;
					},
					"mov_almacen_id,tipo_mov_alma_id,producto_id,unidad_medida_id,producto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacenProducto SingleSearch(MovimientoAlmacenProducto entity)
		{
			try
			{
				List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = Search(entity);

				return movimientoAlmacenProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidadDocumento", entity.CantidadDocumento);
				parameters.AddParameter("pesoNetoDocumento", entity.PesoNetoDocumento);
				parameters.AddParameter("pesoBrutoDocumento", entity.PesoBrutoDocumento);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("productoReclasificacionId", entity.ProductoReclasificacionId);
				parameters.AddParameter("cantidadReclasificacion", entity.CantidadReclasificacion);
				parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
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
		public int Update(MovimientoAlmacenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenProductoId", entity.MovimientoAlmacenProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidadDocumento", entity.CantidadDocumento);
				parameters.AddParameter("pesoNetoDocumento", entity.PesoNetoDocumento);
				parameters.AddParameter("pesoBrutoDocumento", entity.PesoBrutoDocumento);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("pesoNeto", entity.PesoNeto);
				parameters.AddParameter("pesoBruto", entity.PesoBruto);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
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
		public void Delete(MovimientoAlmacenProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenProductoId", entity.MovimientoAlmacenProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarProductoReclasificacion(MovimientoAlmacenProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenProductoId", entity.MovimientoAlmacenProductoId);
				parameters.AddParameter("productoReclasificacionId", entity.ProductoReclasificacionId);
				parameters.AddParameter("cantidadReclasificacion", entity.CantidadReclasificacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarProductoReclasificacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
