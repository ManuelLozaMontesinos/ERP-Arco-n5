using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TransportistaUnidadRepository : ITransportistaUnidadRepository
	{
		private const string SP_PRE_NAME = "sp_transp_unid";
		public TransportistaUnidad GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TransportistaUnidad)
					},
					x =>
					{
						TransportistaUnidad tu = x[0] as TransportistaUnidad;

						return tu;
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
		public List<TransportistaUnidad> Search(TransportistaUnidad entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("transportistaId", entity.TransportistaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TransportistaUnidad)
					},
					x =>
					{
						TransportistaUnidad tu = x[0] as TransportistaUnidad;

						return tu;
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
		public TransportistaUnidad SingleSearch(TransportistaUnidad entity)
		{
			try
			{
				List<TransportistaUnidad> transportistaUnidadList = Search(entity);

				return transportistaUnidadList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TransportistaUnidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaId", entity.TransportistaId);
				parameters.AddParameter("marca", entity.Marca);
				parameters.AddParameter("modelo", entity.Modelo);
				parameters.AddParameter("placa", entity.Placa);
				parameters.AddParameter("placa2", entity.Placa2);
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
		public int Update(TransportistaUnidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaUnidadId", entity.TransportistaUnidadId);
				parameters.AddParameter("marca", entity.Marca);
				parameters.AddParameter("modelo", entity.Modelo);
				parameters.AddParameter("placa", entity.Placa);
				parameters.AddParameter("placa2", entity.Placa2);
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
		public void Delete(TransportistaUnidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaUnidadId", entity.TransportistaUnidadId);
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
