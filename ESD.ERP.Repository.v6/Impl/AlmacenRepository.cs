using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class AlmacenRepository : IAlmacenRepository
	{
		private const string SP_PRE_NAME = "sp_almacen";
		public Almacen GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Almacen),
						typeof(EmpresaSede),
						typeof(Empresa),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						Almacen a = x[0] as Almacen;
						EmpresaSede es = x[1] as EmpresaSede;
						Empresa e = x[2] as Empresa;
						TipoVia tv = x[3] as TipoVia;
						TipoZona tz = x[4] as TipoZona;
						Distrito d = x[5] as Distrito;
						Provincia p = x[6] as Provincia;
						Departamento dp = x[7] as Departamento;

						a.EmpresaSede = es;
						a.EmpresaSedeId = es.EntityId;
						es.Empresa = e;
						es.EmpresaId = e.EntityId;
						a.TipoVia = tv;
						a.TipoViaId = tv.EntityId;
						a.TipoZona = tz;
						a.TipoZonaId = tz.EntityId;
						a.Distrito = d;
						a.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return a;
					},
					"empresa_sede_id,empresa_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Almacen> Search(Almacen entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaSede.EmpresaId);
			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("notAlmacenId", entity.NotAlmacenId);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("esVenta", entity.EsVenta);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Almacen),
						typeof(EmpresaSede),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						Almacen a = x[0] as Almacen;
						EmpresaSede es = x[1] as EmpresaSede;
						TipoVia tv = x[2] as TipoVia;
						TipoZona tz = x[3] as TipoZona;
						Distrito d = x[4] as Distrito;
						Provincia p = x[5] as Provincia;
						Departamento dp = x[6] as Departamento;

						a.EmpresaSede = es;
						a.EmpresaSedeId = es.EntityId;
						a.TipoVia = tv;
						a.TipoViaId = tv.EntityId;
						a.TipoZona = tz;
						a.TipoZonaId = tz.EntityId;
						a.Distrito = d;
						a.DistritoId = d.EntityId;
						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return a;
					},
					"empresa_sede_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Almacen SingleSearch(Almacen entity)
		{
			try
			{
				List<Almacen> almacenList = Search(entity);

				return almacenList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Almacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("esVenta", entity.EsVenta);
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
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("origenRegistroId", entity.OrigenRegistroId);
				parameters.AddParameter("servidorImpresion", entity.ServidorImpresion);
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
		public int Update(Almacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenId", entity.AlmacenId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("esVenta", entity.EsVenta);
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
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
				parameters.AddParameter("servidorImpresion", entity.ServidorImpresion);
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
		public void Delete(Almacen entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("almacenId", entity.AlmacenId);
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
