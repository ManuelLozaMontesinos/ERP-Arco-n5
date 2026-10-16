using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RelojRepository : IRelojRepository
	{
		private const string SP_PRE_NAME = "sp_reloj";
		public Reloj GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Reloj)
					},
					x =>
					{
						Reloj r = x[0] as Reloj;

						return r;
					},
					string.Empty
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Reloj> Search(Reloj entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Reloj)
					},
					x =>
					{
						Reloj r = x[0] as Reloj;

						return r;
					},
					string.Empty
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Reloj SingleSearch(Reloj entity)
		{
			try
			{
				List<Reloj> relojList = Search(entity);

				return relojList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Reloj entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("rutaArchivo", entity.RutaArchivo);
				parameters.AddParameter("esCentral", entity.EsCentral);
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
		public int Update(Reloj entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("relojId", entity.RelojId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("rutaArchivo", entity.RutaArchivo);
				parameters.AddParameter("esCentral", entity.EsCentral);
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
		public void Delete(Reloj entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("relojId", entity.RelojId);
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
