using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TransportistaRepository : ITransportistaRepository
	{
		private const string SP_PRE_NAME = "sp_transpo";
		public Transportista GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Transportista)
					},
					x =>
					{
						Transportista t = x[0] as Transportista;

						return t;
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
		public List<Transportista> Search(Transportista entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("containsRazonSocial", entity.RazonSocial);
			parameters.AddParameter("numeroRuc", entity.NumeroRuc);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Transportista)
					},
					x =>
					{
						Transportista t = x[0] as Transportista;

						return t;
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
		public Transportista SingleSearch(Transportista entity)
		{
			try
			{
				List<Transportista> transportistaList = Search(entity);

				return transportistaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Transportista entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("numeroRuc", entity.NumeroRuc);
				parameters.AddParameter("razonSocial", entity.RazonSocial);
				parameters.AddParameter("numeroInscripcionVehicular", entity.NumeroInscripcionVehicular);
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
		public int Update(Transportista entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaId", entity.TransportistaId);
				parameters.AddParameter("razonSocial", entity.RazonSocial);
				parameters.AddParameter("numeroInscripcionVehicular", entity.NumeroInscripcionVehicular);
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
		public void Delete(Transportista entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaId", entity.TransportistaId);
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
