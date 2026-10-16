using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteProductoRepository : IComprobanteProductoRepository
	{
		private const string SP_PRE_NAME = "sp_comprob_prod";
		public ComprobanteProducto GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComprobanteProducto),
						typeof(Comprobante),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(TipoDescuento)
					},
					x =>
					{
						ComprobanteProducto cp = x[0] as ComprobanteProducto;
						Comprobante c = x[1] as Comprobante;
						Producto p = x[2] as Producto;
						FamiliaProducto fp = x[3] as FamiliaProducto;
						ClaseProducto cpr = x[4] as ClaseProducto;
						GrupoProducto gp = x[5] as GrupoProducto;
						TipoProducto tp = x[6] as TipoProducto;
						TipoDescuento tdc = x[7] as TipoDescuento;

						cp.Comprobante = c;
						cp.ComprobanteId = c.EntityId;
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
					"comprobante_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id,tipo_producto_id," +
					"tipo_dcto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ComprobanteProducto> Search(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("comprobanteProductoIdList", entity.ComprobanteProductoIdList);
			parameters.AddParameter("tipoProducto", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.TipoProducto?.Codigo);
			parameters.AddParameter("grupoProducto", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.Codigo);
			parameters.AddParameter("esCombo", entity?.Producto?.FamiliaProducto?.ClaseProducto?.GrupoProducto?.EsCombo);
			parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
			parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
			parameters.AddParameter("notNullRegistroReferencialId", entity.NotNullRegistroReferencialId);
			parameters.AddParameter("conSaldoNotaCredito", entity.ConSaldoNotaCredito);
			parameters.AddParameter("nullRegistroCanjeId", entity.NullRegistroCanjeId);
			parameters.AddParameter("registroComboId", entity.RegistroComboId);
			parameters.AddParameter("nullRegistroComboId", entity.NullRegistroComboId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComprobanteProducto),
						typeof(Comprobante),
						typeof(ImpuestoTasa),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),
						typeof(UnidadMedida),
						typeof(TipoDescuento),
						typeof(TipoDetraccion),
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv),
						typeof(Personal),
						typeof(Proveedor)
					},
					x =>
					{
						ComprobanteProducto cp = x[0] as ComprobanteProducto;
						Comprobante c = x[1] as Comprobante;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Producto p = x[3] as Producto;
						FamiliaProducto fp = x[4] as FamiliaProducto;
						ClaseProducto cpr = x[5] as ClaseProducto;
						GrupoProducto gp = x[6] as GrupoProducto;
						TipoProducto tp = x[7] as TipoProducto;
						UnidadMedida um = x[8] as UnidadMedida;
						TipoIgv ti = x[9] as TipoIgv;
						UnidadMedida um2 = x[10] as UnidadMedida;
						TipoDescuento tdc = x[11] as TipoDescuento;
						TipoDetraccion td = x[12] as TipoDetraccion;
						TipoAfectacionIgv ta = x[13] as TipoAfectacionIgv;
						TipoIgv ti2 = x[14] as TipoIgv;
						Personal per = x[15] as Personal;
						Proveedor prv = x[16] as Proveedor;

						cp.Producto = p;
						cp.ProductoId = p.EntityId;
						cp.Comprobante = c;
						cp.ComprobanteId = c.EntityId;
						c.ImpuestoTasa = it;
						c.ImpuestoTasaId = it.EntityId;
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
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;
						cp.UnidadMedida = um2;
						cp.UnidadMedidaId = um2?.EntityId;
						cp.TipoDescuento = tdc;
						cp.TipoDescuentoId = tdc.EntityId;
						cp.TipoDetraccion = td;
						cp.TipoDetraccionId = td.EntityId;
						cp.TipoAfectacionIgv = ta;
						cp.TipoAfectacionIgvId = ta.EntityId;
						ta.TipoIgv = ti2;
						ta.TipoIgvId = ti2.EntityId;
						cp.Personal = per;
						cp.PersonalId = per?.EntityId;
						if (!Equals(per, null))
						{
							per.Proveedor = prv;
							per.ProveedorId = prv.EntityId;
						}

						return cp;
					},
					"comprobante_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id,tipo_igv_id,unidad_medida_id,tipo_dcto_id,tipo_detrac_id,tipo_afecta_igv_id," +
					"tipo_igv_id,personal_id,proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ComprobanteProducto SingleSearch(ComprobanteProducto entity)
		{
			try
			{
				List<ComprobanteProducto> comprobanteProductoList = Search(entity);

				return comprobanteProductoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("productoId", entity.ProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("unidadMedidaId", entity.UnidadMedidaId);
				parameters.AddParameter("cantidadPresentacion", entity.CantidadPresentacion);
				parameters.AddParameter("cantidadEquivalente", entity.CantidadEquivalente);
				parameters.AddParameter("valorUnitarioLista", entity.ValorUnitarioLista);
				parameters.AddParameter("precioUnitarioLista", entity.PrecioUnitarioLista);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
				parameters.AddParameter("incluyeDescuentoListaPrecio", entity.IncluyeDescuentoListaPrecio);
				parameters.AddParameter("permiteDescuento", entity.PermiteDescuento);
				parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
				parameters.AddParameter("precioAutorizado", entity.PrecioAutorizado);
				parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
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
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("tipoIscId", entity.TipoIscId);
				parameters.AddParameter("cantidadNotaCredito", entity.CantidadNotaCredito);
				parameters.AddParameter("registroComboId", entity.RegistroComboId);
				parameters.AddParameter("cantidadEntregada", entity.CantidadEntregada);
				parameters.AddParameter("observacionDespacho", entity.ObservacionDespacho);
				parameters.AddParameter("entregaDespacho", entity.EntregaDespacho);
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
		public int Update(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("descripcion", entity.Descripcion);
				parameters.AddParameter("cantidad", entity.Cantidad);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("registroReferencialId", entity.RegistroReferencialId);
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
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
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
		public void Delete(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMonto(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("valorUnitario", entity.ValorUnitario);
				parameters.AddParameter("precioUnitario", entity.PrecioUnitario);
				parameters.AddParameter("montoBruto", entity.MontoBruto);
				parameters.AddParameter("montoDescuento1", entity.MontoDescuento1);
				parameters.AddParameter("montoDescuento2", entity.MontoDescuento2);
				parameters.AddParameter("montoNeto", entity.MontoNeto);
				parameters.AddParameter("montoIgv", entity.MontoIgv);
				parameters.AddParameter("montoIsc", entity.MontoIsc);
				parameters.AddParameter("montoTotal", entity.MontoTotal);
				parameters.AddParameter("tipoDetraccionId", entity.TipoDetraccionId);
				parameters.AddParameter("porcentajeDetraccion", entity.PorcentajeDetraccion);
				parameters.AddParameter("montoDetraccion", entity.MontoDetraccion);
				parameters.AddParameter("tipoAfectacionIgvId", entity.TipoAfectacionIgvId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMonto"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void EliminarPorComprobanteId(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_eliminarPorComprobanteId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarSolicitaAutorizacionDescuento(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("cantidadBloqueada", entity.CantidadBloqueada);
				parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarSolicitaAutorizacionDescuento"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarPrecioAutorizado(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("solicitaAutorizacionDescuento", entity.SolicitaAutorizacionDescuento);
				parameters.AddParameter("precioAutorizado", entity.PrecioAutorizado);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPrecioAutorizado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarPrecioMontoLote(List<ComprobanteProducto> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<ComprobanteProductoType> comprobanteProductoTypeList = new List<ComprobanteProductoType>();
				if (list != null)
				{
					comprobanteProductoTypeList = (from s in list
															select new ComprobanteProductoType
															{
																ComprobanteProductoId = s.ComprobanteProductoId,
																ComprobanteId = s.ComprobanteId,
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
																MontoDetraccion = s.MontoDetraccion,
																Estado = s.Estado,
																UsuarioCreacionId = s.UsuarioCreacionId,
															}).ToList();
				}

				parameters.AddTableParameter("comprobanteProductoList", "dbo.ComprobanteProductoType", comprobanteProductoTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPrecioMonto"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCantidadNotaCredito(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("cantidadNotaCredito", entity.CantidadNotaCredito);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCantidadNotaCredito"), parameters);

			}	
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ComprobanteProducto> ListarPendienteCanje(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("empresaId", entity.Comprobante.PeriodoEmpresaSede.EmpresaSede.EmpresaId);
			parameters.AddParameter("containsDescripcion", entity.Producto.Descripcion);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPendienteCanje"), parameters,
					new[]
					{
						typeof(ComprobanteProducto),
						typeof(Comprobante),
						typeof(ImpuestoTasa),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),
						typeof(UnidadMedida),
						typeof(TipoDetraccion),
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv)
					},
					x =>
					{
						ComprobanteProducto cp = x[0] as ComprobanteProducto;
						Comprobante c = x[1] as Comprobante;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Producto p = x[3] as Producto;
						FamiliaProducto fp = x[4] as FamiliaProducto;
						ClaseProducto cpr = x[5] as ClaseProducto;
						GrupoProducto gp = x[6] as GrupoProducto;
						TipoProducto tp = x[7] as TipoProducto;
						UnidadMedida um = x[8] as UnidadMedida;
						TipoIgv ti = x[9] as TipoIgv;
						UnidadMedida um2 = x[10] as UnidadMedida;
						TipoDetraccion td = x[11] as TipoDetraccion;
						TipoAfectacionIgv ta = x[12] as TipoAfectacionIgv;
						TipoIgv ti2 = x[13] as TipoIgv;

						cp.Producto = p;
						cp.ProductoId = p.EntityId;
						cp.Comprobante = c;
						cp.ComprobanteId = c.EntityId;
						c.ImpuestoTasa = it;
						c.ImpuestoTasaId = it.EntityId;
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
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;
						cp.UnidadMedida = um2;
						cp.UnidadMedidaId = um2?.EntityId;
						cp.TipoDetraccion = td;
						cp.TipoDetraccionId = td.EntityId;
						cp.TipoAfectacionIgv = ta;
						cp.TipoAfectacionIgvId = ta.EntityId;
						ta.TipoIgv = ti2;
						ta.TipoIgvId = ti2.EntityId;

						return cp;
					},
					"comprobante_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id,tipo_igv_id,unidad_medida_id,tipo_detrac_id,tipo_afecta_igv_id," +
					"tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ComprobanteProducto> ListarPorIdList(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("comprobanteProductoIdList", entity.ComprobanteProductoIdList);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listPorIdList"), parameters,
					new[]
					{
						typeof(ComprobanteProducto),
						typeof(Comprobante),
						typeof(ImpuestoTasa),
						typeof(Producto),
						typeof(FamiliaProducto),
						typeof(ClaseProducto),
						typeof(GrupoProducto),
						typeof(TipoProducto),
						typeof(UnidadMedida),
						typeof(TipoIgv),
						typeof(UnidadMedida),
						typeof(TipoDescuento),
						typeof(TipoDetraccion),
						typeof(TipoAfectacionIgv),
						typeof(TipoIgv)
					},
					x =>
					{
						ComprobanteProducto cp = x[0] as ComprobanteProducto;
						Comprobante c = x[1] as Comprobante;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Producto p = x[3] as Producto;
						FamiliaProducto fp = x[4] as FamiliaProducto;
						ClaseProducto cpr = x[5] as ClaseProducto;
						GrupoProducto gp = x[6] as GrupoProducto;
						TipoProducto tp = x[7] as TipoProducto;
						UnidadMedida um = x[8] as UnidadMedida;
						TipoIgv ti = x[9] as TipoIgv;
						UnidadMedida um2 = x[10] as UnidadMedida;
						TipoDescuento tdc = x[11] as TipoDescuento;
						TipoDetraccion td = x[12] as TipoDetraccion;
						TipoAfectacionIgv ta = x[13] as TipoAfectacionIgv;
						TipoIgv ti2 = x[14] as TipoIgv;

						cp.Producto = p;
						cp.ProductoId = p.EntityId;
						cp.Comprobante = c;
						cp.ComprobanteId = c.EntityId;
						c.ImpuestoTasa = it;
						c.ImpuestoTasaId = it.EntityId;
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
						p.TipoIgv = ti;
						p.TipoIgvId = ti.EntityId;
						cp.UnidadMedida = um2;
						cp.UnidadMedidaId = um2?.EntityId;
						cp.TipoDescuento = tdc;
						cp.TipoDescuentoId = tdc.EntityId;
						cp.TipoDetraccion = td;
						cp.TipoDetraccionId = td.EntityId;
						cp.TipoAfectacionIgv = ta;
						cp.TipoAfectacionIgvId = ta.EntityId;
						ta.TipoIgv = ti2;
						ta.TipoIgvId = ti2.EntityId;

						return cp;
					},
					"comprobante_id,impuesto_tasa_id,producto_id,familia_producto_id,clase_producto_id,grupo_producto_id," +
					"tipo_producto_id,unidad_medida_id,tipo_igv_id,unidad_medida_id,tipo_dcto_id,tipo_detrac_id,tipo_afecta_igv_id," +
					"tipo_igv_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarRegistroCanjeId(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);
				parameters.AddParameter("registroCanjeId", entity.RegistroCanjeId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarRegistroCanjeId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarNuloRegistroCanjeId(ComprobanteProducto entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarNuloRegistroCanjeId"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCantidadEntregada(ComprobanteProducto entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteProductoId", entity.ComprobanteProductoId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCantidadEntregada"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
