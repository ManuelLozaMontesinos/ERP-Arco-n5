using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EmpresaSedeRepository : IEmpresaSedeRepository
	{
		private const string SP_PRE_NAME = "sp_empresa_sede";
		public EmpresaSede GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EmpresaSede),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						EmpresaSede es = x[0] as EmpresaSede;
						Distrito d = x[1] as Distrito;
						Provincia p = x[2] as Provincia;
						Departamento dp = x[3] as Departamento;

						es.Distrito = d;
						es.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return es;
					},
					"distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<EmpresaSede> Search(EmpresaSede entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("esCentral", entity.EsCentral);
			parameters.AddParameter("notEmpresaSedeId", entity.NotEmpresaSedeId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EmpresaSede),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						EmpresaSede es = x[0] as EmpresaSede;
						TipoVia tv = x[1] as TipoVia;
						TipoZona tz = x[2] as TipoZona;
						Distrito d = x[3] as Distrito;
						Provincia p = x[4] as Provincia;
						Departamento dp = x[5] as Departamento;

						es.TipoVia = tv;
						es.TipoViaId = tv.EntityId;
						es.TipoZona = tz;
						es.TipoZonaId = tz.EntityId;
						es.Distrito = d;
						es.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return es;
					},
					"tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EmpresaSede SingleSearch(EmpresaSede entity)
		{
			try
			{
				List<EmpresaSede> empresaSedeList = Search(entity);

				return empresaSedeList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("empresaSede", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("esCentral", entity.EsCentral);
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
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("telefono2", entity.Telefono2);
				parameters.AddParameter("email", entity.Email);
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
		public int Update(EmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("esCentral", entity.EsCentral);
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
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("telefono2", entity.Telefono2);
				parameters.AddParameter("email", entity.Email);
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
		public void Delete(EmpresaSede entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
