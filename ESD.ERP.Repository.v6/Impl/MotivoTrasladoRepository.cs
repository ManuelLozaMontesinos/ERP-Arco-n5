using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MotivoTrasladoRepository : IMotivoTrasladoRepository
	{
		private const string SP_PRE_NAME = "sp_motivo_trasla";
		public MotivoTraslado GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(MotivoTraslado)
					},
					x =>
					{
						MotivoTraslado mt = x[0] as MotivoTraslado;

						return mt;
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
		public List<MotivoTraslado> Search(MotivoTraslado entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(MotivoTraslado)
					},
					x =>
					{
						MotivoTraslado mt = x[0] as MotivoTraslado;

						return mt;
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
		public MotivoTraslado SingleSearch(MotivoTraslado entity)
		{
			try
			{
				List<MotivoTraslado> motivoTrasladoList = Search(entity);

				return motivoTrasladoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(MotivoTraslado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoTraslado", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
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
		public int Update(MotivoTraslado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoTrasladoId", entity.MotivoTrasladoId);
				parameters.AddParameter("motivoTraslado", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
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
		public void Delete(MotivoTraslado entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("motivoTrasladoId", entity.MotivoTrasladoId);
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
