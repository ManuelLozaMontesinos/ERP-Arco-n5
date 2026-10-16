using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalDependienteRepository : IPersonalDependienteRepository
	{
		private const string SP_PRE_NAME = "sp_personal_depen";
		public PersonalDependiente GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalDependiente),
						typeof(TipoDocumentoIdentidad),
						typeof(PaisEmisor),
						typeof(Sexo),
						typeof(VinculoFamiliar),
						typeof(TipoDocumentoVinculoFamiliar),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(MotivoBajaDependiente),
					},
					x =>
					{
						PersonalDependiente pd = x[0] as PersonalDependiente;
						TipoDocumentoIdentidad tdi = x[1] as TipoDocumentoIdentidad;
						PaisEmisor pe = x[2] as PaisEmisor;
						Sexo s = x[3] as Sexo;
						VinculoFamiliar vf = x[4] as VinculoFamiliar;
						TipoDocumentoVinculoFamiliar tdvf = x[5] as TipoDocumentoVinculoFamiliar;
						TipoVia tv = x[6] as TipoVia;
						TipoZona tz = x[7] as TipoZona;
						Distrito d = x[8] as Distrito;
						Provincia p = x[9] as Provincia;
						Departamento dp = x[10] as Departamento;
						MotivoBajaDependiente mbd = x[11] as MotivoBajaDependiente;

						pd.TipoDocumentoIdentidadId = tdi.EntityId;
						pd.TipoDocumentoIdentidad = tdi;
						pd.PaisEmisorId = pe?.EntityId;
						pd.PaisEmisor = pe;
						pd.SexoId = s.EntityId;
						pd.Sexo = s;
						pd.VinculoFamiliarId = vf.EntityId;
						pd.VinculoFamiliar = vf;
						pd.TipoDocumentoVinculoFamiliarId = tdvf.EntityId;
						pd.TipoDocumentoVinculoFamiliar = tdvf;
						pd.TipoViaId = tv.EntityId;
						pd.TipoVia = tv;
						pd.TipoZonaId = tz.EntityId;
						pd.TipoZona = tz;
						pd.DistritoId = d.EntityId;
						pd.Distrito = d;
						d.ProvinciaId = p.EntityId;
						d.Provincia = p;
						p.DepartamentoId = dp.EntityId;
						p.Departamento = dp;
						pd.MotivoBajaDependienteId = mbd?.EntityId;
						pd.MotivoBajaDependiente = mbd;

						return pd;
					},
					"tipo_doc_iden_id,pais_emisor_id,sexo_id,vincu_fami_id,tipo_d_v_fam_id,tipo_via_id,"
					+ "tipo_zona_id,distrito_id,provincia_id,departamento_id,motivo_b_depen_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<PersonalDependiente> Search(PersonalDependiente entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalDependiente),
						typeof(TipoDocumentoIdentidad),
						typeof(PaisEmisor),
						typeof(Sexo),
						typeof(VinculoFamiliar),
						typeof(TipoDocumentoVinculoFamiliar),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(MotivoBajaDependiente),
					},
					x =>
					{
						PersonalDependiente pd = x[0] as PersonalDependiente;
						TipoDocumentoIdentidad tdi = x[1] as TipoDocumentoIdentidad;
						PaisEmisor pe = x[2] as PaisEmisor;
						Sexo s = x[3] as Sexo;
						VinculoFamiliar vf = x[4] as VinculoFamiliar;
						TipoDocumentoVinculoFamiliar tdvf = x[5] as TipoDocumentoVinculoFamiliar;
						TipoVia tv = x[6] as TipoVia;
						TipoZona tz = x[7] as TipoZona;
						Distrito d = x[8] as Distrito;
						Provincia p = x[9] as Provincia;
						Departamento dp = x[10] as Departamento;
						MotivoBajaDependiente mbd = x[11] as MotivoBajaDependiente;

						pd.TipoDocumentoIdentidadId = tdi.EntityId;
						pd.TipoDocumentoIdentidad = tdi;
						pd.PaisEmisorId = pe?.EntityId;
						pd.PaisEmisor = pe;
						pd.SexoId = s.EntityId;
						pd.Sexo = s;
						pd.VinculoFamiliarId = vf.EntityId;
						pd.VinculoFamiliar = vf;
						pd.TipoDocumentoVinculoFamiliarId = tdvf.EntityId;
						pd.TipoDocumentoVinculoFamiliar = tdvf;
						pd.TipoViaId = tv.EntityId;
						pd.TipoVia = tv;
						pd.TipoZonaId = tz.EntityId;
						pd.TipoZona = tz;
						pd.DistritoId = d.EntityId;
						pd.Distrito = d;
						d.ProvinciaId = p.EntityId;
						d.Provincia = p;
						p.DepartamentoId = dp.EntityId;
						p.Departamento = dp;
						pd.MotivoBajaDependienteId = mbd?.EntityId;
						pd.MotivoBajaDependiente = mbd;

						return pd;
					},
					"tipo_doc_iden_id,pais_emisor_id,sexo_id,vincu_fami_id,tipo_d_v_fam_id,tipo_via_id,"
					+ "tipo_zona_id,distrito_id,provincia_id,departamento_id,motivo_b_depen_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalDependiente SingleSearch(PersonalDependiente entity)
		{
			try
			{
				List<PersonalDependiente> personalDependienteList = Search(entity);

				return personalDependienteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("sexoId", entity.SexoId);
				parameters.AddParameter("vinculoFamiliarId", entity.VinculoFamiliarId);
				parameters.AddParameter("tipoDocumentoVinculoFamiliarId", entity.TipoDocumentoVinculoFamiliarId);
				parameters.AddParameter("numeroDocumentoVinculoFamiliar", entity.NumeroDocumentoVinculoFamiliar);
				parameters.AddParameter("fechaConcepcion", entity.FechaConcepcion);
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
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
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
		public int Update(PersonalDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalDependienteId", entity.PersonalDependienteId);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("paisEmisorId", entity.PaisEmisorId);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("sexoId", entity.SexoId);
				parameters.AddParameter("vinculoFamiliarId", entity.VinculoFamiliarId);
				parameters.AddParameter("tipoDocumentoVinculoFamiliarId", entity.TipoDocumentoVinculoFamiliarId);
				parameters.AddParameter("numeroDocumentoVinculoFamiliar", entity.NumeroDocumentoVinculoFamiliar);
				parameters.AddParameter("fechaConcepcion", entity.FechaConcepcion);
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
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
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
		public void Delete(PersonalDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalDependienteId", entity.PersonalDependienteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarBaja(PersonalDependiente entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalDependienteId", entity.PersonalDependienteId);
				parameters.AddParameter("motivoBajaDependienteId", entity.MotivoBajaDependienteId);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarBaja"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
