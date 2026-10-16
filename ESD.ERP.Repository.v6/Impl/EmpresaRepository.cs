using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EmpresaRepository : IEmpresaRepository
	{
		private const string SP_PRE_NAME = "sp_empresa";
		public Empresa GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Empresa),
						typeof(Sector),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						Empresa e = x[0] as Empresa;
						Sector s = x[1] as Sector;
						TipoVia tv = x[2] as TipoVia;
						TipoZona tz = x[3] as TipoZona;
						Distrito d = x[4] as Distrito;
						Provincia p = x[5] as Provincia;
						Departamento dp = x[6] as Departamento;

						e.Sector = s;
						e.SectorId = s.EntityId;
						e.TipoVia = tv;
						e.TipoViaId = tv.EntityId;
						e.TipoZona = tz;
						e.TipoZonaId = tz.EntityId;
						e.Distrito = d;
						e.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return e;
					},
					"sector_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Empresa> Search(Empresa entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Empresa),
						typeof(Sector),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						Empresa e = x[0] as Empresa;
						Sector s = x[1] as Sector;
						TipoVia tv = x[2] as TipoVia;
						TipoZona tz = x[3] as TipoZona;
						Distrito d = x[4] as Distrito;
						Provincia p = x[5] as Provincia;
						Departamento dp = x[6] as Departamento;

						e.Sector = s;
						e.SectorId = s.EntityId;
						e.TipoVia = tv;
						e.TipoViaId = tv.EntityId;
						e.TipoZona = tz;
						e.TipoZonaId = tz.EntityId;
						e.Distrito = d;
						e.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return e;
					},
					"sector_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Empresa SingleSearch(Empresa entity)
		{
			try
			{
				List<Empresa> empresaList = Search(entity);

				return empresaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Empresa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("sectorId", entity.SectorId);
				parameters.AddParameter("ruc", entity.RUC);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("nombreComercial", entity.NombreComercial);
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
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("numeroCuentaDetraccion", entity.NumeroCuentaDetraccion);
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
		public int Update(Empresa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("sectorId", entity.SectorId);
				parameters.AddParameter("ruc", entity.RUC);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("nombreComercial", entity.NombreComercial);
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
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("numeroCuentaDetraccion", entity.NumeroCuentaDetraccion);
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
		public void Delete(Empresa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("estado", entity.Estado);

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
