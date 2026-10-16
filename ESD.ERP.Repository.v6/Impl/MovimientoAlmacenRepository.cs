using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MovimientoAlmacenRepository : IMovimientoAlmacenRepository
	{
		private const string SP_PRE_NAME = "sp_mov_almacen";
		public MovimientoAlmacen GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MovimientoAlmacen),
						typeof(Almacen),
						typeof(TransaccionAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(Proveedor),
						typeof(TipoDocumentoComercial),
						typeof(Personal),
						typeof(Proveedor),
						typeof(CentroCosto),
						typeof(EstadoAprobacion),
						typeof(Usuario)
					},
					x =>
					{
						MovimientoAlmacen ma = x[0] as MovimientoAlmacen;
						Almacen a = x[1] as Almacen;
						TransaccionAlmacen ta = x[2] as TransaccionAlmacen;
						TipoMovimientoAlmacen tma = x[3] as TipoMovimientoAlmacen;
						Proveedor p = x[4] as Proveedor;
						TipoDocumentoComercial tdc = x[5] as TipoDocumentoComercial;
						Personal per = x[6] as Personal;
						Proveedor p2 = x[7] as Proveedor;
						CentroCosto cc = x[8] as CentroCosto;
						EstadoAprobacion ea = x[9] as EstadoAprobacion;
						Usuario u = x[10] as Usuario;

						ma.Almacen = a;
						ma.AlmacenId = a.EntityId;
						ma.TransaccionAlmacen = ta;
						ma.TransaccionAlmacenId = ta.EntityId;
						ma.TipoMovimientoAlmacen = tma;
						ma.TipoMovimientoAlmacenId = tma.EntityId;
						ma.Proveedor = p;
						ma.ProveedorId = p?.EntityId;
						ma.TipoDocumentoComercial = tdc;
						ma.TipoDocumentoComercialId = tdc?.EntityId;
						ma.Personal = per;
						ma.PersonalId = per?.EntityId;
						if (!Equals(per, null))
						{
							per.Proveedor = p2;
							per.ProveedorId = p2.EntityId;
						}
						ma.CentroCosto = cc;
						ma.CentroCostoId = cc?.EntityId;
						ma.EstadoAprobacion = ea;
						ma.EstadoAprobacionId = ea?.EntityId;
						ma.Usuario = u;
						ma.UsuarioCreacionId = u.EntityId;

						return ma;
					},
					"almacen_id,trans_alma_id,tipo_mov_alma_id,proveedor_id,tipo_doc_com_id,personal_id,proveedor_id," +
					"centro_costo_id,estado_aproba_id,usuario_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<MovimientoAlmacen> Search(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
			parameters.AddParameter("transaccionAlmacen", entity?.TransaccionAlmacen?.Codigo);
			parameters.AddParameter("transaccionAlmacenList", entity.TransaccionAlmacenList);
			parameters.AddParameter("notTransaccionAlmacenList", entity.NotTransaccionAlmacenList);
			parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
			parameters.AddParameter("tipoMovimientoAlmacen", entity?.TipoMovimientoAlmacen?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroRuc", entity?.Proveedor?.NumeroRuc);
			parameters.AddParameter("containsMovimientoAlmacen", entity.Codigo);
			parameters.AddParameter("containsNumeroGuiaRemision", entity.NumeroGuiaRemision);
			parameters.AddParameter("containsNumeroDocumento", entity.NumeroDocumento);
			parameters.AddParameter("almacenDestinoId", entity.AlmacenDestinoId);
			parameters.AddParameter("movimientoAlmacenReferencialId", entity.MovimientoAlmacenReferencialId);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("tieneGuiaRemision", entity.TieneGuiaRemision);
			parameters.AddParameter("guiaRemisionEnModificacion", entity.GuiaRemisionEnModificacion);
			parameters.AddParameter("containsPersonal", entity?.Personal?.Proveedor?.RazonSocial);
			parameters.AddParameter("centroCostoId", entity.CentroCostoId);
			parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
			parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
			parameters.AddParameter("estadoMovimientoAlmacen", entity?.EstadoMovimientoAlmacen?.Codigo);
			parameters.AddParameter("estadoMovimientoAlmacenList", entity.EstadoMovimientoAlmacenList);
			parameters.AddParameter("esEditable", entity.EsEditable);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MovimientoAlmacen),
						typeof(Almacen),
						typeof(TransaccionAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(Proveedor),
						typeof(Almacen),
						typeof(TipoDocumentoComercial),
						typeof(Personal),
						typeof(Proveedor),
						typeof(CentroCosto),
						typeof(EstadoAprobacion),
						typeof(Personal),
						typeof(Proveedor),
						typeof(EstadoMovimientoAlmacen),
						typeof(Usuario)
					},
					x =>
					{
						MovimientoAlmacen ma = x[0] as MovimientoAlmacen;
						Almacen a = x[1] as Almacen;
						TransaccionAlmacen ta = x[2] as TransaccionAlmacen;
						TipoMovimientoAlmacen tma = x[3] as TipoMovimientoAlmacen;
						Proveedor p = x[4] as Proveedor;
						Almacen ad = x[5] as Almacen;
						TipoDocumentoComercial tdc = x[6] as TipoDocumentoComercial;
						Personal per = x[7] as Personal;
						Proveedor p2 = x[8] as Proveedor;
						CentroCosto cc = x[9] as CentroCosto;
						EstadoAprobacion ea = x[10] as EstadoAprobacion;
						Personal per3 = x[11] as Personal;
						Proveedor p3 = x[12] as Proveedor;
						EstadoMovimientoAlmacen ema = x[13] as EstadoMovimientoAlmacen;
						Usuario u = x[14] as Usuario;

						ma.Almacen = a;
						ma.AlmacenId = a.EntityId;
						ma.TransaccionAlmacen = ta;
						ma.TransaccionAlmacenId = ta.EntityId;
						ma.TipoMovimientoAlmacen = tma;
						ma.TipoMovimientoAlmacenId = tma.EntityId;
						ma.Proveedor = p;
						ma.ProveedorId = p?.EntityId;
						ma.AlmacenDestino = ad;
						ma.AlmacenDestinoId = ad?.EntityId;
						ma.TipoDocumentoComercial = tdc;
						ma.TipoDocumentoComercialId = tdc?.EntityId;
						ma.Personal = per;
						ma.PersonalId = per?.EntityId;
						if (!Equals(per, null))
						{
							per.Proveedor = p2;
							per.ProveedorId = p2.EntityId;
						}
						ma.CentroCosto = cc;
						ma.CentroCostoId = cc?.EntityId;
						if (!Equals(ea, null))
						{
							ma.EstadoAprobacion = ea;
							ma.EstadoAprobacionId = ea?.EntityId;
						}
						ma.PersonalAutoriza = per3;
						ma.PersonalAutorizaId = per3?.EntityId;
						if (!Equals(per3, null))
						{
							per3.Proveedor = p3;
							per3.ProveedorId = p3.EntityId;
						}
						ma.EstadoMovimientoAlmacen = ema;
						ma.EstadoMovimientoAlmacenId = ema.EntityId;
						ma.Usuario = u;
						ma.UsuarioCreacionId = u.EntityId;

						return ma;
					},
					"almacen_id,trans_alma_id,tipo_mov_alma_id,proveedor_id,almacen_id,tipo_doc_com_id,personal_id,proveedor_id," +
					"centro_costo_id,estado_aproba_id,personal_id,proveedor_id,estado_mov_alm_id,usuario_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public MovimientoAlmacen SingleSearch(MovimientoAlmacen entity)
		{
			try
			{
				List<MovimientoAlmacen> movimientoAlmacenList = Search(entity);

				return movimientoAlmacenList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
				parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
				parameters.AddParameter("movimientoAlmacen", entity.Codigo);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("almacenDestinoId", entity.AlmacenDestinoId);
				parameters.AddParameter("movimientoAlmacenReferencialId", entity.MovimientoAlmacenReferencialId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("numeroOrdenCompra", entity.NumeroOrdenCompra);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serieDocumento", entity.SerieDocumento);
				parameters.AddParameter("numeroDocumento", entity.NumeroDocumento);
				parameters.AddParameter("tieneGuiaRemision", entity.TieneGuiaRemision);
				parameters.AddParameter("guiaRemisionEnModificacion", entity.GuiaRemisionEnModificacion);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("movimientoReclasificacionId", entity.MovimientoReclasificacionId);
				parameters.AddParameter("ordenReclasificacion", entity.OrdenReclasificacion);
				parameters.AddParameter("afectaSaldo", entity.AfectaSaldo);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
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
		public int Update(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("almacenDestinoId", entity.AlmacenDestinoId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("mesContableId", entity.MesContableId);
				parameters.AddParameter("ordenCompraId", entity.OrdenCompraId);
				parameters.AddParameter("numeroOrdenCompra", entity.NumeroOrdenCompra);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("serieDocumento", entity.SerieDocumento);
				parameters.AddParameter("numeroDocumento", entity.NumeroDocumento);
				parameters.AddParameter("tieneGuiaRemision", entity.TieneGuiaRemision);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
				parameters.AddParameter("personalAutorizaId", entity.PersonalAutorizaId);
				parameters.AddParameter("afectaSaldo", entity.AfectaSaldo);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
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
		public void Delete(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoMovimiento(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("afectaSaldo", entity.AfectaSaldo);
				parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoMovimiento"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<MovimientoAlmacen> BuscarPorEmpresa(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoId", entity?.PeriodoEmpresaSede?.PeriodoId);
			parameters.AddParameter("empresaId", entity?.PeriodoEmpresaSede?.EmpresaSede?.EmpresaId);
			parameters.AddParameter("almacenId", entity.AlmacenId);
			parameters.AddParameter("transaccionAlmacenId", entity.TransaccionAlmacenId);
			parameters.AddParameter("transaccionAlmacen", entity?.TransaccionAlmacen?.Codigo);
			parameters.AddParameter("tipoMovimientoAlmacenId", entity.TipoMovimientoAlmacenId);
			parameters.AddParameter("tipoMovimientoAlmacen", entity?.TipoMovimientoAlmacen?.Codigo);
			parameters.AddParameter("containsRazonSocial", entity?.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroRuc", entity?.Proveedor?.NumeroRuc);
			parameters.AddParameter("containsMovimientoAlmacen", entity.Codigo);
			parameters.AddParameter("containsNumeroGuiaRemision", entity.NumeroGuiaRemision);
			parameters.AddParameter("almacenDestinoId", entity.AlmacenDestinoId);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("estadoMovimientoAlmacenId", entity.EstadoMovimientoAlmacenId);
			parameters.AddParameter("estadoMovimientoAlmacen", entity?.EstadoMovimientoAlmacen?.Codigo);
			parameters.AddParameter("estadoMovimientoAlmacenList", entity.EstadoMovimientoAlmacenList);
			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_buscarPorEmpresa"), parameters,
					new[]
					{
						typeof(MovimientoAlmacen),
						typeof(Almacen),
						typeof(TransaccionAlmacen),
						typeof(TipoMovimientoAlmacen),
						typeof(Proveedor),
						typeof(Almacen),
						typeof(TipoDocumentoComercial),
						typeof(EstadoMovimientoAlmacen),
						typeof(Usuario)
					},
					x =>
					{
						MovimientoAlmacen ma = x[0] as MovimientoAlmacen;
						Almacen a = x[1] as Almacen;
						TransaccionAlmacen ta = x[2] as TransaccionAlmacen;
						TipoMovimientoAlmacen tma = x[3] as TipoMovimientoAlmacen;
						Proveedor p = x[4] as Proveedor;
						Almacen ad = x[5] as Almacen;
						TipoDocumentoComercial tdc = x[6] as TipoDocumentoComercial;
						EstadoMovimientoAlmacen ema = x[7] as EstadoMovimientoAlmacen;
						Usuario u = x[8] as Usuario;

						ma.Almacen = a;
						ma.AlmacenId = a.EntityId;
						ma.TransaccionAlmacen = ta;
						ma.TransaccionAlmacenId = ta.EntityId;
						ma.TipoMovimientoAlmacen = tma;
						ma.TipoMovimientoAlmacenId = tma.EntityId;
						ma.Proveedor = p;
						ma.ProveedorId = p?.EntityId;
						ma.AlmacenDestino = ad;
						ma.AlmacenDestinoId = ad?.EntityId;
						ma.TipoDocumentoComercial = tdc;
						ma.TipoDocumentoComercialId = tdc?.EntityId;
						ma.EstadoMovimientoAlmacen = ema;
						ma.EstadoMovimientoAlmacenId = ema.EntityId;
						ma.Usuario = u;
						ma.UsuarioCreacionId = u.EntityId;

						return ma;
					},
					"almacen_id,trans_alma_id,tipo_mov_alma_id,proveedor_id,almacen_id,tipo_doc_com_id,estado_mov_alm_id,usuario_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarGuiaRemision(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("numeroGuiaRemision", entity.NumeroGuiaRemision);
				parameters.AddParameter("tieneGuiaRemision", entity.TieneGuiaRemision);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarGuiaRemision"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarTieneGuiaRemision(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("tieneGuiaRemision", entity.TieneGuiaRemision);
				parameters.AddParameter("guiaRemisionEnModificacion", entity.GuiaRemisionEnModificacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarTieneGuiaRemision"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoAprobacion(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("personalAutorizaId", entity.PersonalAutorizaId);
				parameters.AddParameter("esEditable", entity.EsEditable);
				parameters.AddParameter("fechaAutorizacion", entity.FechaAutorizacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoAprobacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarMesContable(MovimientoAlmacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarMesContable"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
