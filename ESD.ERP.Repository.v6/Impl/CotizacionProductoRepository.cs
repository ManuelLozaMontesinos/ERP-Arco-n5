using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
    public class CotizacionProductoRepository : ICotizacionProductoRepository
    {
        private const string SP_PRE_NAME = "sp_cotiza_prod";
        public CotizacionProducto GetById(int id)
        {
            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
                    new[]
                    {
                        typeof(CotizacionProducto),
                        typeof(Cotizacion),
                        typeof(Producto),
                        typeof(FamiliaProducto),
                        typeof(ClaseProducto),
                        typeof(GrupoProducto),
                        typeof(TipoProducto),
                        typeof(TipoDescuento)
                    },
                    x =>
                    {
                        CotizacionProducto cp = x[0] as CotizacionProducto;
                        Cotizacion c = x[1] as Cotizacion;
                        Producto p = x[2] as Producto;
                        FamiliaProducto fp = x[3] as FamiliaProducto;
                        ClaseProducto cpr = x[4] as ClaseProducto;
                        GrupoProducto gp = x[5] as GrupoProducto;
                        TipoProducto tp = x[6] as TipoProducto;
                        TipoDescuento tdc = x[7] as TipoDescuento;

                        cp.Cotizacion = c;
                        cp.CotizacionId = c.EntityId;
                        cp.Producto = p;
                        cp.ProductoId = p.EntityId;
                        p.FamiliaProducto = fp;
                        p.FamiliaProductoId = fp.EntityId;
                        fp.ClaseProducto = cpr;
                        fp.ClaseProductoId = cpr.EntityId;
                        cpr.GrupoProducto = gp;
                        cpr.GrupoProductoId = gp.EntityId;
                        gp.TipoProducto = tp;
                        gp.TipoProductoId = tp.EntityId;
                        cp.TipoDescuento = tdc;
                        cp.TipoDescuentoId = tdc.EntityId;

                        return cp;
                    },
                    "cotiza_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id,tipo_producto_id," +
                    "tipo_dcto_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public List<CotizacionProducto> Search(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("cotizacionId", entity.CotizacionId);
            parameters.AddParameter("cotizacionProductoIdList", entity.CotizacionProductoIdList);
            parameters.AddParameter("tipoProducto", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.TipoProducto?.Codigo);
            parameters.AddParameter("grupoProducto", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.Codigo);
            parameters.AddParameter("esCombo", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.EsCombo);
            parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
            parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
            parameters.AddParameter("ConCantidadPendiente", entity.ConCantidadPendiente);
            parameters.AddParameter("sinSaldo", entity.SinSaldo);
            parameters.AddParameter("registroComboId", entity.RegistroComboId);
            parameters.AddParameter("nullRegistroComboId", entity.NullRegistroComboId);
            parameters.AddParameter("estado", entity.Estado);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(CotizacionProducto),
                        typeof(Cotizacion),
                        typeof(ImpuestoTasa),
                        typeof(Producto),
                        typeof(FamiliaProducto),
                        typeof(ClaseProducto),
                        typeof(TipoDetraccion),
                        typeof(GrupoProducto),
                        typeof(TipoProducto),
                        typeof(UnidadMedida),
                        typeof(TipoIgv),
                        typeof(UnidadMedida),
                        typeof(TipoDescuento),
                        typeof(TipoAfectacionIgv),
                        typeof(TipoIgv)
                    },
                    x =>
                    {
                        CotizacionProducto cp = x[0] as CotizacionProducto;
                        Cotizacion c = x[1] as Cotizacion;
                        ImpuestoTasa it = x[2] as ImpuestoTasa;
                        Producto p = x[3] as Producto;
                        FamiliaProducto fp = x[4] as FamiliaProducto;
                        ClaseProducto cpr = x[5] as ClaseProducto;
                        TipoDetraccion td = x[6] as TipoDetraccion;
                        GrupoProducto gp = x[7] as GrupoProducto;
                        TipoProducto tp = x[8] as TipoProducto;
                        UnidadMedida um = x[9] as UnidadMedida;
                        TipoIgv ti = x[10] as TipoIgv;
                        UnidadMedida um2 = x[11] as UnidadMedida;
                        TipoDescuento tdc = x[12] as TipoDescuento;
                        TipoAfectacionIgv tai = x[13] as TipoAfectacionIgv;
                        TipoIgv ti2 = x[14] as TipoIgv;

                        cp.Producto = p;
                        cp.ProductoId = p.EntityId;
                        cp.Cotizacion = c;
                        cp.CotizacionId = c.EntityId;
                        c.ImpuestoTasa = it;
                        c.ImpuestoTasaId = it.EntityId;
                        p.FamiliaProducto = fp;
                        p.FamiliaProductoId = fp.EntityId;
                        fp.ClaseProducto = cpr;
                        fp.ClaseProductoId = cpr.EntityId;
                        cpr.TipoDetraccion = td;
                        cpr.TipoDetraccionId = td.EntityId;
                        cpr.GrupoProducto = gp;
                        cpr.GrupoProductoId = gp.EntityId;
                        gp.TipoProducto = tp;
                        gp.TipoProductoId = tp.EntityId;
                        p.UnidadMedida = um;
                        p.UnidadMedidaId = um.EntityId;
                        p.TipoIgv = ti;
                        p.TipoIgvId = ti.EntityId;
                        cp.UnidadMedida = um2;
                        cp.UnidadMedidaId = um2?.EntityId;
                        cp.TipoDescuento = tdc;
                        cp.TipoDescuentoId = tdc.EntityId;
                        cp.TipoAfectacionIgv = tai;
                        cp.TipoAfectacionIgvId = tai.EntityId;
                        tai.TipoIgv = ti2;
                        tai.TipoIgvId = ti2.EntityId;

                        return cp;
                    },
                    "cotiza_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,tipo_detrac_id,grupo_producto_id," +
                    "tipo_producto_id,unidad_medida_id,tipo_igv_id,unidad_medida_id,tipo_dcto_id,tipo_afecta_igv_id,tipo_igv_id"
                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

       public List<CotizacionProducto> EnvioProduccion(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

            parameters.AddParameter("cotizacionId", entity.CotizacionId);

            try
            {
                return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
                    new[]
                    {
                        typeof(CotizacionProducto),
                        typeof(Cotizacion),
                        typeof(Producto)
                    },
                    x =>
                    {

                        CotizacionProducto cp = x[0] as CotizacionProducto;
                        Cotizacion c = x[1] as Cotizacion;
                        Producto p = x[2] as Producto;

                        cp.Producto = p;
                        cp.ProductoId = p.EntityId;
                        cp.Cotizacion = c;
                        cp.CotizacionId = c.EntityId;

                        return cp;
                    }, "cotiza_id,producto_id"

                );
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public CotizacionProducto SingleSearch(CotizacionProducto entity)
        {
            try
            {
                List<CotizacionProducto> cotizacionProductoList = Search(entity);

                return cotizacionProductoList.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public int Insert(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionId", entity.CotizacionId);
                parameters.AddParameter("productoId", entity.ProductoId);
                parameters.AddParameter("descripcion", entity.Descripcion);
                parameters.AddParameter("cantidad", entity.Cantidad);
                parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
                parameters.AddParameter("cantidadPresentacion", entity.CantidadPresentacion);
                parameters.AddParameter("cantidadEquivalente", entity.CantidadEquivalente);
                parameters.AddParameter("cantidadComprobante", entity.CantidadComprobante);
                parameters.AddParameter("valorUnitarioLista", entity.ValorUnitarioLista);
                parameters.AddParameter("precioUnitarioLista", entity.PrecioUnitarioLista);
                parameters.AddParameter("valorUnitario", entity.ValorUnitario);
                parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
                parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
                parameters.AddParameter("incluyeDescuentoListaPrecio", entity.IncluyeDescuentoListaPrecio);
                parameters.AddParameter("permiteDescuento", entity.PermiteDescuento);
                parameters.AddParameter("sinSaldo", entity.SinSaldo);
                parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
                parameters.AddParameter("precioAutorizado", entity.PrecioAutorizado);
                parameters.AddParameter("montoBruto", entity.MontoBruto);
                parameters.AddParameter("tipoDescuentoId", entity.TipoDescuentoId);
                parameters.AddParameter("descuentoValor", entity.DescuentoValor);
                parameters.AddParameter("porcentajeDescuento", entity.PorcentajeDescuento);
                parameters.AddParameter("montoDescuento1", entity.MontoDescuento1);
                parameters.AddParameter("montoDescuento2", entity.MontoDescuento2);
                parameters.AddParameter("montoNeto", entity.MontoNeto);
                parameters.AddParameter("montoIgv", entity.MontoIgv);
                parameters.AddParameter("montoIsc", entity.MontoIsc);
                parameters.AddParameter("montoTotal", entity.MontoTotal);
                parameters.AddParameter("pesoNeto", entity.PesoNeto);
                parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
                parameters.AddParameter("tipoIscId", entity.TipoIscId);
                parameters.AddParameter("registroComboId", entity.RegistroComboId);
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
        public int Update(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                parameters.AddParameter("descripcion", entity.Descripcion);
                parameters.AddParameter("cantidad", entity.Cantidad);
                parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
                parameters.AddParameter("cantidadPresentacion", entity.CantidadPresentacion);
                parameters.AddParameter("cantidadEquivalente", entity.CantidadEquivalente);
                parameters.AddParameter("cantidadComprobante", entity.CantidadComprobante);
                parameters.AddParameter("valorUnitario", entity.ValorUnitario);
                parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
                parameters.AddParameter("sinSaldo", entity.SinSaldo);
                parameters.AddParameter("montoBruto", entity.MontoBruto);
                parameters.AddParameter("tipoDescuentoId", entity.TipoDescuentoId);
                parameters.AddParameter("descuentoValor", entity.DescuentoValor);
                parameters.AddParameter("porcentajeDescuento", entity.PorcentajeDescuento);
                parameters.AddParameter("montoDescuento1", entity.MontoDescuento1);
                parameters.AddParameter("montoDescuento2", entity.MontoDescuento2);
                parameters.AddParameter("montoNeto", entity.MontoNeto);
                parameters.AddParameter("montoIgv", entity.MontoIgv);
                parameters.AddParameter("montoIsc", entity.MontoIsc);
                parameters.AddParameter("montoTotal", entity.MontoTotal);
                parameters.AddParameter("pesoNeto", entity.PesoNeto);
                parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
                parameters.AddParameter("tipoIscId", entity.TipoIscId);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

                return entity.EntityId;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void Delete(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void EliminarPorComprobanteId(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionId", entity.CotizacionId);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorCotizacionId"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarSolicitaAutorizacionDescuento(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
                parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarSolicitaAutorizacionDescuento"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarPrecioAutorizado(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
                parameters.AddParameter("precioAutorizado", entity.PrecioAutorizado);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPrecioAutorizado"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarPrecioMontoLote(List<CotizacionProducto> list)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                List<CotizacionProductoType> cotizacionProductoTypeList = new List<CotizacionProductoType>();
                if (list != null)
                {
                    cotizacionProductoTypeList = (from s in list
                                                  select new CotizacionProductoType
                                                  {
                                                      CotizacionProductoId = s.CotizacionProductoId,
                                                      CotizacionId = s.CotizacionId,
                                                      ProductoId = s.ProductoId,
                                                      Cantidad = s.Cantidad,
                                                      ValorUnitario = s.ValorUnitario,
                                                      PrecioUnitario = s.PrecioUnitario,
                                                      PrecioAutorizado = s.PrecioAutorizado,
                                                      CantidadBloqueada = s.CantidadBloqueada,
                                                      SolicitaAutorizacionDescuento = s.SolicitaAutorizacionDescuento,
                                                      MontoBruto = s.MontoBruto,
                                                      MontoDescuento1 = s.MontoDescuento1,
                                                      MontoDescuento2 = s.MontoDescuento2,
                                                      MontoNeto = s.MontoNeto,
                                                      MontoIgv = s.MontoIgv,
                                                      MontoIsc = s.MontoIsc,
                                                      MontoTotal = s.MontoTotal,
                                                      Estado = s.Estado,
                                                      UsuarioCreacionId = (s.UsuarioModificacionId.HasValue && s.UsuarioModificacionId.Value > 0) ? s.UsuarioModificacionId.Value : s.UsuarioCreacionId,
                                                  }).ToList();
                }

                parameters.AddTableParameter("cotizacionProductoList", "dbo.CotizacionProductoType", cotizacionProductoTypeList);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPrecioMonto"), parameters);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarCantidadComprobante(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionId", entity.CotizacionId);
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCantidadComprobante"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
        public void ActualizarSinSaldo(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                parameters.AddParameter("sinSaldo", entity.SinSaldo);
                int usuarioAccionId = (entity.UsuarioModificacionId.HasValue && entity.UsuarioModificacionId.Value > 0) ? entity.UsuarioModificacionId.Value : entity.UsuarioCreacionId;
                parameters.AddParameter("usuarioAccionId", usuarioAccionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarSinSaldo"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }

        public void ActualizarSaldoProduccion(CotizacionProducto entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("cotizacionProductoId", entity.CotizacionProductoId);
                parameters.AddParameter("cantidad", entity.CantidadParcial);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarSaldoProduccion"), parameters);
            }
            catch(Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
    }
}
