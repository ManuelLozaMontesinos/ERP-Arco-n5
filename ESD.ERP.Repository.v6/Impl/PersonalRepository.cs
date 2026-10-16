using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalRepository : IPersonalRepository
	{
		private const string SP_PRE_NAME = "sp_personal";
		public Personal GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Personal),
						typeof(EmpresaSede),
						typeof(PaisEmisor),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(Pais),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(CategoriaOcupacional),
						typeof(CargoPersonal),
						typeof(Ocupacion),
						typeof(RegimenSalud),
						typeof(RegimenPensionario),
						typeof(TipoPagoRemuneracion)
					},
					x =>
					{
						Personal p = x[0] as Personal;
						EmpresaSede es = x[1] as EmpresaSede;
						PaisEmisor pe = x[2] as PaisEmisor;
						Proveedor prv = x[3] as Proveedor;
						TipoDocumentoIdentidad tdi = x[4] as TipoDocumentoIdentidad;
						TipoVia tv = x[5] as TipoVia;
						TipoZona tz = x[6] as TipoZona;
						Distrito d = x[7] as Distrito;
						Provincia pvc = x[8] as Provincia;
						Departamento dp = x[9] as Departamento;
						Pais pa = x[10] as Pais;
						TipoVia tv2 = x[11] as TipoVia;
						TipoZona tz2 = x[12] as TipoZona;
						Distrito d2 = x[13] as Distrito;
						Provincia pvc2 = x[14] as Provincia;
						Departamento dp2 = x[15] as Departamento;
						CategoriaOcupacional co = x[16] as CategoriaOcupacional;
						CargoPersonal cp = x[17] as CargoPersonal;
						Ocupacion o = x[18] as Ocupacion;
						RegimenSalud rs = x[19] as RegimenSalud;
						RegimenPensionario rp = x[20] as RegimenPensionario;
						TipoPagoRemuneracion tpr = x[21] as TipoPagoRemuneracion;

						p.EmpresaSedeId = es.EntityId;
						p.EmpresaSede = es;
						p.PaisEmisorId = pe?.EntityId;
						p.PaisEmisor = pe;
						p.ProveedorId = prv.EntityId;
						p.Proveedor = prv;
						prv.TipoDocumentoIdentidadId = tdi.EntityId;
						prv.TipoDocumentoIdentidad = tdi;
						prv.TipoVia = tv;
						prv.TipoViaId = tv.EntityId;
						prv.TipoZona = tz;
						prv.TipoZonaId = tz.EntityId;
						prv.Distrito = d;
						prv.DistritoId = d.EntityId;
						d.Provincia = pvc;
						d.ProvinciaId = pvc.EntityId;
						pvc.Departamento = dp;
						pvc.DepartamentoId = dp.DepartamentoId;
						p.PaisId = pa.EntityId;
						p.Pais = pa;
						p.TipoVia = tv2;
						p.TipoViaId = tv2.EntityId;
						p.TipoZona = tz2;
						p.TipoZonaId = tz2.EntityId;
						p.Distrito = d2;
						p.DistritoId = d2.EntityId;
						d2.Provincia = pvc2;
						d2.ProvinciaId = pvc2.EntityId;
						pvc2.Departamento = dp2;
						pvc2.DepartamentoId = dp2.DepartamentoId;
						p.CategoriaOcupacionalId = co?.EntityId;
						p.CategoriaOcupacional = co;
						p.CargoPersonalId = cp.EntityId;
						p.CargoPersonal = cp;
						p.OcupacionId = o.EntityId;
						p.Ocupacion = o;
						p.RegimenSaludId = rs.EntityId;
						p.RegimenSalud = rs;
						p.RegimenPensionarioId = rp.EntityId;
						p.RegimenPensionario = rp;
						p.TipoPagoRemuneracionId = tpr.EntityId;
						p.TipoPagoRemuneracion = tpr;

						return p;
					},
					"empresa_sede_id,pais_emisor_id,proveedor_id,tipo_doc_iden_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id,"
						+ "pais_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id,cate_ocupa_id,cargo_perso_id,"
						+ "ocupacion_id,reg_salud_id,reg_pension_id,tipo_pago_remu_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Personal> Search(Personal entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("usuarioId", entity.Proveedor?.UsuarioId);
			parameters.AddParameter("nullFechaCese", entity.NullFechaCese);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Personal),
						typeof(EmpresaSede),
						typeof(Proveedor),
						typeof(TipoDocumentoIdentidad),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(CentroCosto)
					},
					x =>
					{
						Personal p = x[0] as Personal;
						EmpresaSede es = x[1] as EmpresaSede;
						Proveedor prv = x[2] as Proveedor;
						TipoDocumentoIdentidad tdi = x[3] as TipoDocumentoIdentidad;
						TipoVia tv = x[4] as TipoVia;
						TipoZona tz = x[5] as TipoZona;
						Distrito d = x[6] as Distrito;
						Provincia pvc = x[7] as Provincia;
						Departamento dp = x[8] as Departamento;
						CentroCosto cc = x[9] as CentroCosto;

						p.EmpresaSedeId = es.EntityId;
						p.EmpresaSede = es;
						p.ProveedorId = prv.EntityId;
						p.Proveedor = prv;
						prv.TipoDocumentoIdentidad = tdi;
						prv.TipoDocumentoIdentidadId = tdi.EntityId;
						prv.TipoVia = tv;
						prv.TipoViaId = tv.EntityId;
						prv.TipoZona = tz;
						prv.TipoZonaId = tz.EntityId;
						prv.Distrito = d;
						prv.DistritoId = d.EntityId;
						d.Provincia = pvc;
						d.ProvinciaId = pvc.EntityId;
						pvc.Departamento = dp;
						pvc.DepartamentoId = dp.DepartamentoId;
						p.CentroCostoId = cc?.EntityId;
						p.CentroCosto = cc;

						return p;
					},
					"empresa_sede_id,proveedor_id,tipo_doc_iden_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id," +
					"departamento_id,centro_costo_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Personal SingleSearch(Personal entity)
		{
			try
			{
				List<Personal> personalList = Search(entity);

				return personalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Personal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personal", entity.Codigo);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
				parameters.AddParameter("numeroFotocheck", entity.NumeroFotocheck);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("sexoId", entity.SexoId);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("paisId", entity.PaisId);
				parameters.AddParameter("tipoViaId", entity.TipoViaId);
				parameters.AddParameter("nombreVia", entity.NombreVia);
				parameters.AddParameter("numeroVia", entity.NumeroVia);
				parameters.AddParameter("departamento", entity.Departamento);
				parameters.AddParameter("interior", entity.Interior);
				parameters.AddParameter("manzana", entity.Manzana);
				parameters.AddParameter("lote", entity.Lote);
				parameters.AddParameter("kilometro", entity.Kilometro);
				parameters.AddParameter("block", entity.Block);
				parameters.AddParameter("etapa", entity.Etapa);
				parameters.AddParameter("tipoZonaId", entity.TipoZonaId);
				parameters.AddParameter("nombreZona", entity.NombreZona);
				parameters.AddParameter("referencia", entity.Referencia);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("esCentroAsistencial", entity.EsCentroAsistencial);
				parameters.AddParameter("domiciliado", entity.Domiciliado);
				parameters.AddParameter("tipoTrabajadorId", entity.TipoTrabajadorId);
				parameters.AddParameter("categoriaOcupacionalId", entity.CategoriaOcupacionalId);
				parameters.AddParameter("regimenLaboralId", entity.RegimenLaboralId);
				parameters.AddParameter("situacionEducativaId", entity.SituacionEducativaId);
				parameters.AddParameter("cargoPersonalId", entity.CargoPersonalId);
				parameters.AddParameter("ocupacionId", entity.OcupacionId);
				parameters.AddParameter("regimenSaludId", entity.RegimenSaludId);
				parameters.AddParameter("epsId", entity.EPSId);
				parameters.AddParameter("regimenPensionarioId", entity.RegimenPensionarioId);
				parameters.AddParameter("regimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
				parameters.AddParameter("fechaInscripcionRegimenPensionario", entity.FechaInscripcionRegimenPensionario);
				parameters.AddParameter("numeroCuspp", entity.NumeroCuspp);
				parameters.AddParameter("aportaSctr", entity.AportaSctr);
				parameters.AddParameter("sctrPensionId", entity.SctrPensionId);
				parameters.AddParameter("sctrSaludId", entity.SctrSaludId);
				parameters.AddParameter("tipoModalidadFormativaId", entity.TipoModalidadFormativaId);
				parameters.AddParameter("esDiscapacitado", entity.EsDiscapacitado);
				parameters.AddParameter("tipoContratoId", entity.TipoContratoId);
				parameters.AddParameter("situacionEspecialTrabajadorId", entity.SituacionEspecialTrabajadorId);
				parameters.AddParameter("perioricidadRemunerativaId", entity.PeriodicidadRemunerativaId);
				parameters.AddParameter("estadoCivilId", entity.EstadoCivilId);
				parameters.AddParameter("tipoPagoRemuneracionId", entity.TipoPagoRemuneracionId);
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("NumeroCuentaAbono", entity.NumeroCuentaAbono);
				parameters.AddParameter("bancoCTSId", entity.BancoCTSId);
				parameters.AddParameter("numeroCuentaCTS", entity.NumeroCuentaCTS);
				parameters.AddParameter("esRegimenAtipico", entity.EsRegimenAtipico);
				parameters.AddParameter("esJornadaTrabajoMaxima", entity.EsJornadaTrabajoMaxima);
				parameters.AddParameter("esJornadaTrabajoNocturna", entity.EsJornadaTrabajoNocturna);
				parameters.AddParameter("esMiembroSindicalizado", entity.EsMiembroSindicalizado);
				parameters.AddParameter("esExonerado5ta", entity.EsExonerado5ta);
				parameters.AddParameter("asignacionFamiliar", entity.AsignacionFamiliar);
				parameters.AddParameter("fechaIngreso", entity.FechaIngreso);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
				parameters.AddParameter("convenioDobleTributacionId", entity.ConvenioDobleTributacionId);
				parameters.AddParameter("situacionTrabajadorId", entity.SituacionTrabajadorId);
				parameters.AddParameter("esEventual", entity.EsEventual);
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
		public int Update(Personal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
				parameters.AddParameter("numeroFotocheck", entity.NumeroFotocheck);
				parameters.AddParameter("sexoId", entity.SexoId);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("paisId", entity.PaisId);
				parameters.AddParameter("tipoViaId", entity.TipoViaId);
				parameters.AddParameter("nombreVia", entity.NombreVia);
				parameters.AddParameter("numeroVia", entity.NumeroVia);
				parameters.AddParameter("departamento", entity.Departamento);
				parameters.AddParameter("interior", entity.Interior);
				parameters.AddParameter("manzana", entity.Manzana);
				parameters.AddParameter("lote", entity.Lote);
				parameters.AddParameter("kilometro", entity.Kilometro);
				parameters.AddParameter("block", entity.Block);
				parameters.AddParameter("etapa", entity.Etapa);
				parameters.AddParameter("tipoZonaId", entity.TipoZonaId);
				parameters.AddParameter("nombreZona", entity.NombreZona);
				parameters.AddParameter("referencia", entity.Referencia);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("esCentroAsistencial", entity.EsCentroAsistencial);
				parameters.AddParameter("domiciliado", entity.Domiciliado);
				parameters.AddParameter("tipoTrabajadorId", entity.TipoTrabajadorId);
				parameters.AddParameter("categoriaOcupacionalId", entity.CategoriaOcupacionalId);
				parameters.AddParameter("regimenLaboralId", entity.RegimenLaboralId);
				parameters.AddParameter("situacionEducativaId", entity.SituacionEducativaId);
				parameters.AddParameter("cargoPersonalId", entity.CargoPersonalId);
				parameters.AddParameter("ocupacionId", entity.OcupacionId);
				parameters.AddParameter("regimenSaludId", entity.RegimenSaludId);
				parameters.AddParameter("epsId", entity.EPSId);
				parameters.AddParameter("regimenPensionarioId", entity.RegimenPensionarioId);
				parameters.AddParameter("regimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
				parameters.AddParameter("fechaInscripcionRegimenPensionario", entity.FechaInscripcionRegimenPensionario);
				parameters.AddParameter("numeroCuspp", entity.NumeroCuspp);
				parameters.AddParameter("aportaSctr", entity.AportaSctr);
				parameters.AddParameter("sctrPensionId", entity.SctrPensionId);
				parameters.AddParameter("sctrSaludId", entity.SctrSaludId);
				parameters.AddParameter("tipoModalidadFormativaId", entity.TipoModalidadFormativaId);
				parameters.AddParameter("esDiscapacitado", entity.EsDiscapacitado);
				parameters.AddParameter("tipoContratoId", entity.TipoContratoId);
				parameters.AddParameter("situacionEspecialTrabajadorId", entity.SituacionEspecialTrabajadorId);
				parameters.AddParameter("perioricidadRemunerativaId", entity.PeriodicidadRemunerativaId);
				parameters.AddParameter("estadoCivilId", entity.EstadoCivilId);
				parameters.AddParameter("tipoPagoRemuneracionId", entity.TipoPagoRemuneracionId);
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("NumeroCuentaAbono", entity.NumeroCuentaAbono);
				parameters.AddParameter("bancoCTSId", entity.BancoCTSId);
				parameters.AddParameter("numeroCuentaCTS", entity.NumeroCuentaCTS);
				parameters.AddParameter("esRegimenAtipico", entity.EsRegimenAtipico);
				parameters.AddParameter("esJornadaTrabajoMaxima", entity.EsJornadaTrabajoMaxima);
				parameters.AddParameter("esJornadaTrabajoNocturna", entity.EsJornadaTrabajoNocturna);
				parameters.AddParameter("esMiembroSindicalizado", entity.EsMiembroSindicalizado);
				parameters.AddParameter("esExonerado5ta", entity.EsExonerado5ta);
				parameters.AddParameter("asignacionFamiliar", entity.AsignacionFamiliar);
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("centroCostoId", entity.CentroCostoId);
				parameters.AddParameter("convenioDobleTributacionId", entity.ConvenioDobleTributacionId);
				parameters.AddParameter("esEventual", entity.EsEventual);
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
		public void Delete(Personal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarCese(Personal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("motivoFinPeriodoId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("fechaCese", entity.FechaCese);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarCese"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public void ActualizarFechaIngreso(Personal entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaIngreso", entity.FechaIngreso);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarFechaIngreso"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}