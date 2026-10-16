using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class GuiaRemisionRepository : IGuiaRemisionRepository
	{
		private const string SP_PRE_NAME = "sp_guia_remision";
		public GuiaRemision GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(GuiaRemision),
						typeof(PeriodoEmpresaSede),
						typeof(EmpresaSede),
						typeof(TipoDocumentoComercial),
						typeof(MovimientoAlmacen),
						typeof(TipoDocumentoComercial),
						typeof(Proveedor),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),

						typeof(Proveedor),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),

						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(TransportistaUnidad),
						typeof(Transportista),
						typeof(TransportistaConductor),
						typeof(MotivoTraslado),
						typeof(EstadoAprobacion),
						typeof(EstadoGuiaRemision),
						typeof(EstadoSunat)
					},
					x =>
					{
						GuiaRemision gr = x[0] as GuiaRemision;
						PeriodoEmpresaSede pes = x[1] as PeriodoEmpresaSede;
						EmpresaSede es = x[2] as EmpresaSede;
						TipoDocumentoComercial tdc = x[3] as TipoDocumentoComercial;
						MovimientoAlmacen ma = x[4] as MovimientoAlmacen;
						TipoDocumentoComercial tdc2 = x[5] as TipoDocumentoComercial;
						Proveedor p = x[6] as Proveedor;
						TipoVia tv = x[7] as TipoVia;
						TipoZona tz = x[8] as TipoZona;
						Distrito d = x[9] as Distrito;
						Provincia prv = x[10] as Provincia;
						Departamento dp = x[11] as Departamento;
						Distrito d2 = x[12] as Distrito;
						Provincia prv2 = x[13] as Provincia;
						Departamento dp2 = x[14] as Departamento;
						Proveedor p2 = x[15] as Proveedor;
						TipoVia tv2 = x[16] as TipoVia;
						TipoZona tz2 = x[17] as TipoZona;
						Distrito d3 = x[18] as Distrito;
						Provincia prv3 = x[19] as Provincia;
						Departamento dp3 = x[20] as Departamento;
						Distrito d4 = x[21] as Distrito;
						Provincia prv4 = x[22] as Provincia;
						Departamento dp4 = x[23] as Departamento;
						TransportistaUnidad tu = x[24] as TransportistaUnidad;
						Transportista tra = x[25] as Transportista;
						TransportistaConductor tc = x[26] as TransportistaConductor;
						MotivoTraslado mt = x[27] as MotivoTraslado;
						EstadoAprobacion ea = x[28] as EstadoAprobacion;
						EstadoGuiaRemision egr = x[29] as EstadoGuiaRemision;
						EstadoSunat esu = x[30] as EstadoSunat;

						gr.PeriodoEmpresaSede = pes;
						gr.PeriodoEmpresaSedeId = pes.EntityId;
						pes.EmpresaSede = es;
						pes.EmpresaSedeId = es.EntityId;
						gr.TipoDocumentoComercial = tdc;
						gr.TipoDocumentoComercialId = tdc.EntityId;
						gr.MovimientoAlmacen = ma;
						gr.MovimientoAlmacenId = ma?.EntityId;
						if (!Equals(ma, null))
						{
							ma.TipoDocumentoComercial = tdc2;
							ma.TipoDocumentoComercialId = tdc2?.EntityId;
						}

						gr.Proveedor = p;
						gr.ProveedorId = p.EntityId;
						p.TipoVia = tv;
						p.TipoViaId = tv.EntityId;
						p.TipoZona = tz;
						p.TipoZonaId = tz.EntityId;
						p.Distrito = d;
						p.DistritoId = d.EntityId;
						d.Provincia = prv;
						d.ProvinciaId = prv.EntityId;
						prv.Departamento = dp;
						prv.DepartamentoId = dp.EntityId;

						if (!Equals(d2, null))
						{
							gr.Distrito = d2;
							gr.DistritoId = d2.EntityId;
							d2.Provincia = prv2;
							d2.ProvinciaId = prv2.EntityId;
							prv2.Departamento = dp2;
							prv2.DepartamentoId = dp2.EntityId;
						}

						gr.ProveedorDestino = p2;
						gr.ProveedorDestinoId = p2.EntityId;
						p2.TipoVia = tv2;
						p2.TipoViaId = tv2.EntityId;
						p2.TipoZona = tz2;
						p2.TipoZonaId = tz2.EntityId;
						p2.Distrito = d3;
						p2.DistritoId = d3.EntityId;
						d3.Provincia = prv3;
						d3.ProvinciaId = prv3.EntityId;
						prv3.Departamento = dp3;
						prv3.DepartamentoId = dp3.EntityId;

						if (!Equals(d4, null))
						{
							gr.DistritoLlegada = d4;
							gr.DistritoLlegadaId = d4.EntityId;
							d4.Provincia = prv4;
							d4.ProvinciaId = prv4.EntityId;
							prv4.Departamento = dp4;
							prv4.DepartamentoId = dp4.EntityId;
						}

						gr.TransportistaUnidad = tu;
						gr.TransportistaUnidadId = tu?.EntityId;

						if (!Equals(tu, null))
						{
							tu.Transportista = tra;
							tu.TransportistaId = tra.EntityId;
						}

						gr.TransportistaConductor = tc;
						gr.TransportistaConductorId = tc?.EntityId;
						gr.MotivoTraslado = mt;
						gr.MotivoTrasladoId = mt.EntityId;
						gr.EstadoAprobacion = ea;
						gr.EstadoAprobacionId = ea?.EntityId;
						gr.EstadoGuiaRemision = egr;
						gr.EstadoGuiaRemisionId = egr.EntityId;
						gr.EstadoSunat = esu;
						gr.EstadoSunatId = esu?.EntityId;

						return gr;
					},
					"periodo_emp_sede_id,empresa_sede_id,tipo_doc_com_id,mov_almacen_id,tipo_doc_com_id," +
					"proveedor_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id," +
					"distrito_id,provincia_id,departamento_id," +
					"proveedor_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id," +
					"distrito_id,provincia_id,departamento_id," +
					"transp_unid_id,transpo_id,transp_condu_id,motivo_trasla_id,estado_aproba_id,estado_guia_r_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<GuiaRemision> Search(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
			parameters.AddParameter("serie", entity.Serie);
			parameters.AddParameter("containsNumero", entity.Numero);
			parameters.AddParameter("containsRazonSocial", entity.MovimientoAlmacen?.Proveedor?.RazonSocial);
			parameters.AddParameter("razonSocialGR", entity.ProveedorDestino?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.MovimientoAlmacen?.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("fechaDesde", entity.FechaDesde);
			parameters.AddParameter("fechaHasta", entity.FechaHasta);
			parameters.AddParameter("esInterno", entity.EsInterno);
			parameters.AddParameter("esVenta", entity.EsVenta);
			parameters.AddParameter("estadoAprobacion", entity?.EstadoAprobacion?.Codigo);
			parameters.AddParameter("estadoGuiaRemisionId", entity.EstadoGuiaRemisionId);
			parameters.AddParameter("docRef", entity.MovimientoAlmacen?.NumeroDocumento);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(GuiaRemision),
						typeof(MovimientoAlmacen),
						typeof(TipoDocumentoComercial),
						typeof(Proveedor),
						typeof(Proveedor),
						typeof(Usuario),
						typeof(EstadoAprobacion),
						typeof(EstadoGuiaRemision),
						typeof(EstadoSunat)
					},
					x =>
					{
						GuiaRemision gr = x[0] as GuiaRemision;
						MovimientoAlmacen ma = x[1] as MovimientoAlmacen;
						TipoDocumentoComercial tdc = x[2] as TipoDocumentoComercial;
						Proveedor p = x[3] as Proveedor;
						Proveedor p2 = x[4] as Proveedor;
						Usuario u = x[5] as Usuario;
						EstadoAprobacion ea = x[6] as EstadoAprobacion;
						EstadoGuiaRemision egr = x[7] as EstadoGuiaRemision;
						EstadoSunat es = x[8] as EstadoSunat;

						gr.MovimientoAlmacen = ma;
						gr.MovimientoAlmacenId = ma?.EntityId;
						if (!Equals(ma, null))
						{
							ma.TipoDocumentoComercial = tdc;
							ma.TipoDocumentoComercialId = tdc?.EntityId;
						}

						gr.Proveedor = p;
						gr.ProveedorId = p.EntityId;
						gr.ProveedorDestino = p2;
						gr.ProveedorDestinoId = p2.EntityId;
						gr.Usuario = u;
						gr.UsuarioCreacionId = u.EntityId;
						gr.EstadoAprobacion = ea;
						gr.EstadoAprobacionId = ea?.EntityId;
						gr.EstadoGuiaRemision = egr;
						gr.EstadoGuiaRemisionId = egr.EntityId;
						gr.EstadoSunat = es;
						gr.EstadoSunatId = es?.EntityId;

						return gr;
					},
					"mov_almacen_id,tipo_doc_com_id,proveedor_id,proveedor_id,usuario_id,estado_aproba_id,estado_guia_r_id,estado_sunat_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public GuiaRemision SingleSearch(GuiaRemision entity)
		{
			try
			{
				List<GuiaRemision> guiaRemisionList = Search(entity);

				return guiaRemisionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("periodoEmpresaSedeId", entity.PeriodoEmpresaSedeId);
				parameters.AddParameter("tipoDocumentoComercialId", entity.TipoDocumentoComercialId);
				parameters.AddParameter("movimientoAlmacenId", entity.MovimientoAlmacenId);
				parameters.AddParameter("serie", entity.Serie);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("fechaInicioTraslado", entity.FechaInicioTraslado);
				parameters.AddParameter("costoFlete", entity.CostoFlete);
				parameters.AddParameter("esInterno", entity.EsInterno);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("puntoPartida", entity.PuntoPartida);
				parameters.AddParameter("codigoSunatPartida", entity.CodigoSunatPartida);
				parameters.AddParameter("tipoDocumentoComercialReferenciaId", entity.TipoDocumentoComercialReferenciaId);
				parameters.AddParameter("serieDocumentoReferencia", entity.SerieDocumentoReferencia);
				parameters.AddParameter("numeroDocumentoReferencia", entity.NumeroDocumentoReferencia);
				parameters.AddParameter("proveedorDestinoId", entity.ProveedorDestinoId);
				parameters.AddParameter("distritoLlegadaId", entity.DistritoLlegadaId);
				parameters.AddParameter("puntoLlegada", entity.PuntoLlegada);
				parameters.AddParameter("codigoSunatLlegada", entity.CodigoSunatLlegada);
				parameters.AddParameter("transportistaUnidadId", entity.TransportistaUnidadId);
				parameters.AddParameter("transportistaConductorId", entity.TransportistaConductorId);
				parameters.AddParameter("motivoTrasladoId", entity.MotivoTrasladoId);
				parameters.AddParameter("esVenta", entity.EsVenta);
				parameters.AddParameter("impreso", entity.Impreso);
				parameters.AddParameter("pesoBrutoTotal", entity.PesoBrutoTotal);
				parameters.AddParameter("esModificacion", entity.EsModificacion);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("estadoGuiaRemisionId", entity.EstadoGuiaRemisionId);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("observacionAnulacion", entity.ObservacionAnulacion);
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
		public int Update(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("fechaInicioTraslado", entity.FechaInicioTraslado);
				parameters.AddParameter("costoFlete", entity.CostoFlete);
				parameters.AddParameter("esInterno", entity.EsInterno);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("puntoPartida", entity.PuntoPartida);
				parameters.AddParameter("codigoSunatPartida", entity.CodigoSunatPartida);
				parameters.AddParameter("tipoDocumentoComercialReferenciaId", entity.TipoDocumentoComercialReferenciaId);
				parameters.AddParameter("serieDocumentoReferencia", entity.SerieDocumentoReferencia);
				parameters.AddParameter("numeroDocumentoReferencia", entity.NumeroDocumentoReferencia);
				parameters.AddParameter("proveedorDestinoId", entity.ProveedorDestinoId);
				parameters.AddParameter("distritoLlegadaId", entity.DistritoLlegadaId);
				parameters.AddParameter("puntoLlegada", entity.PuntoLlegada);
				parameters.AddParameter("codigoSunatLlegada", entity.CodigoSunatLlegada);
				parameters.AddParameter("transportistaUnidadId", entity.TransportistaUnidadId);
				parameters.AddParameter("transportistaConductorId", entity.TransportistaConductorId);
				parameters.AddParameter("motivoTrasladoId", entity.MotivoTrasladoId);
				parameters.AddParameter("pesoBrutoTotal", entity.PesoBrutoTotal);
				parameters.AddParameter("observacion", entity.Observacion);
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
		public void Delete(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("estadoGuiaRemisionId", entity.EstadoGuiaRemisionId);
				parameters.AddParameter("observacionAnulacion", entity.ObservacionAnulacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstado(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("estadoGuiaRemisionAnteriorId", entity.EstadoGuiaRemisionAnteriorId);
				parameters.AddParameter("estadoGuiaRemisionId", entity.EstadoGuiaRemisionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstadoAprobacion(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("estadoAprobacionId", entity.EstadoAprobacionId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstadoAprobacion"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEmisionElectronica(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("estadoSunatId", entity.EstadoSunatId);
				parameters.AddParameter("comprobanteElectronicoId", entity.ComprobanteElectronicoId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEmisionElectronica"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEmision(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("guiaRemisionId", entity.GuiaRemisionId);
				parameters.AddParameter("numero", entity.Numero);
				parameters.AddParameter("fechaInicioTraslado", entity.FechaInicioTraslado);
				parameters.AddParameter("costoFlete", entity.CostoFlete);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("puntoPartida", entity.PuntoPartida);
				parameters.AddParameter("codigoSunatPartida", entity.CodigoSunatPartida);
				parameters.AddParameter("tipoDocumentoComercialReferenciaId", entity.TipoDocumentoComercialReferenciaId);
				parameters.AddParameter("serieDocumentoReferencia", entity.SerieDocumentoReferencia);
				parameters.AddParameter("numeroDocumentoReferencia", entity.NumeroDocumentoReferencia);
				parameters.AddParameter("proveedorDestinoId", entity.ProveedorDestinoId);
				parameters.AddParameter("distritoLlegadaId", entity.DistritoLlegadaId);
				parameters.AddParameter("puntoLlegada", entity.PuntoLlegada);
				parameters.AddParameter("codigoSunatLlegada", entity.CodigoSunatLlegada);
				parameters.AddParameter("transportistaUnidadId", entity.TransportistaUnidadId);
				parameters.AddParameter("transportistaConductorId", entity.TransportistaConductorId);
				parameters.AddParameter("motivoTrasladoId", entity.MotivoTrasladoId);
				parameters.AddParameter("pesoBrutoTotal", entity.PesoBrutoTotal);
				parameters.AddParameter("observacion", entity.Observacion);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEmision"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}

		public void ActualizarPeso(GuiaRemision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("movAlmacenId", entity.MovimientoAlmacenId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarPeso"), parameters);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
