using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class OrdenProduccionProductoRepository : IOrdenProduccionProductoRepository
	{
		private const string SP_PRE_NAME = "sp_orden_p_prod";
		public OrdenProduccionProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(OrdenProduccionProducto)
					},
					x =>
					{
						OrdenProduccionProducto opp = x[0] as OrdenProduccionProducto;

						return opp;
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
        public List<OrdenProduccionProducto> Search(OrdenProduccionProducto entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
            parameters.AddParameter("esInsumo", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.EsInsumo);
            parameters.AddParameter("estado", entity.Estado);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(OrdenProduccionProducto),
                        typeof(Producto),
                        typeof(FamiliaProducto),
                        typeof(ClaseProducto),
                        typeof(GrupoProducto),
                        typeof(TipoProducto),
                        typeof(UnidadMedida),
						typeof(EstadoDespacho),
						typeof(Personal),
						typeof(Proveedor)

                    },
                    x =>
                    {
                        OrdenProduccionProducto opp = x[0] as OrdenProduccionProducto;
                        Producto p = x[1] as Producto;
                        FamiliaProducto fp = x[2] as FamiliaProducto;
                        ClaseProducto cpr = x[3] as ClaseProducto;
                        GrupoProducto gp = x[4] as GrupoProducto;
                        TipoProducto tp = x[5] as TipoProducto;
                        UnidadMedida um = x[6] as UnidadMedida;
						EstadoDespacho es = x[7] as EstadoDespacho;
						Personal ps = x[8] as Personal;
						Proveedor pvd = x[9] as Proveedor;


                        opp.Producto = p;
                        opp.ProductoId = p.EntityId;
                        p.FamiliaProducto = fp;
                        p.FamiliaProductoId = fp.EntityId;
                        fp.ClaseProducto = cpr;
                        fp.ClaseProductoId = cpr.EntityId;
                        cpr.GrupoProducto = gp;
                        cpr.GrupoProductoId = gp.EntityId;
                        gp.TipoProducto = tp;
                        gp.TipoProductoId = tp.EntityId;
                        p.UnidadMedida = um;
                        p.UnidadMedidaId = um.EntityId;
						opp.EstadoDespacho = es;
						opp.EstadoDespachoId = es?.EntityId;
						opp.Personal = ps;
						opp.PersonalDespachoId = ps?.EntityId;
                        if (!Equals(ps, null))
                        {
                            ps.Proveedor = pvd;
                            ps.ProveedorId = pvd.EntityId;
                        }
						

                        return opp;
                    },
                    "producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
                    "tipo_producto_id,unidad_medida_id,estado_despacho_id,personal_id,proveedor_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public OrdenProduccionProducto SingleSearch(OrdenProduccionProducto entity)
		{
			try
			{
				List<OrdenProduccionProducto> ordenProduccionProductoList = Search(entity);

				return ordenProduccionProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(OrdenProduccionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionId", entity.OrdenProduccionId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("cantidadEntregada", entity.CantidadEntregada);
				parameters.AddParameter("entregadoA", entity.EntregadoA);
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
		public int Update(OrdenProduccionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionProductoId", entity.OrdenProduccionProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(OrdenProduccionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ordenProduccionProductoId", entity.OrdenProduccionProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
        public void UpdateOrden(OrdenProduccionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.RegistroReferencialId);
				parameters.AddParameter("descripcion", entity.Descripcion);
                parameters.AddParameter("cantidad", entity.Cantidad);
                parameters.AddParameter("estado", entity.Estado);
                parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEnvioProd"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
		public void actualizarEnvioProd(OrdenProduccionProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cotizacionProductoId", entity.RegistroReferencialId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_envioProduccionUpdate"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}

        public void actualizarInsumos(OrdenProduccionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("ordenProduccionProductoId", entity.OrdenProduccionProductoId);
                parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarInsumo"), parameters);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

    }
}
