using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class DistritoRepository : IDistritoRepository
	{
		private const string SP_PRE_NAME = "sp_distrito";
		public Distrito GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)

					},
					x =>
					{
						Distrito d = x[0] as Distrito;
						Provincia p = x[1] as Provincia;
						Departamento dp = x[2] as Departamento;

						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return d;
					},
					"provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Distrito> Search(Distrito entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("departamento", entity.Provincia?.Departamento?.Codigo);
			parameters.AddParameter("provincia", entity.Provincia?.Codigo);
			parameters.AddParameter("provinciaId", entity.ProvinciaId);
			parameters.AddParameter("distrito", entity.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)

					},
					x =>
					{
						Distrito d = x[0] as Distrito;
						Provincia p = x[1] as Provincia;
						Departamento dp = x[2] as Departamento;

						d.Provincia = p;
						d.ProvinciaId = p.EntityId;
						p.Departamento = dp;
						p.DepartamentoId = dp.DepartamentoId;

						return d;
					},
					"provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Distrito SingleSearch(Distrito entity)
		{
			try
			{
				List<Distrito> distritoList = Search(entity);

				return distritoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Distrito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("provinciaId", entity.ProvinciaId);
				parameters.AddParameter("distrito", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(Distrito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("provinciaId", entity.ProvinciaId);
				parameters.AddParameter("distrito", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public void Delete(Distrito entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("distritoId", entity.DistritoId);
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
