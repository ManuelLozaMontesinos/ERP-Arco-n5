using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProveedorOportunidadRepository : IProveedorOportunidadRepository
	{
		private const string SP_PRE_NAME = "sp_prove_opo";
		public ProveedorOportunidad GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProveedorOportunidad)
					},
					x =>
					{
						ProveedorOportunidad po = x[0] as ProveedorOportunidad;

						return po;
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
		public List<ProveedorOportunidad> Search(ProveedorOportunidad entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("oportunidadVentaId", entity.OportunidadVentaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProveedorOportunidad)
					},
					x =>
					{
						ProveedorOportunidad po = x[0] as ProveedorOportunidad;

						return po;
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
		public ProveedorOportunidad SingleSearch(ProveedorOportunidad entity)
		{
			try
			{
				List<ProveedorOportunidad> proveedorOportunidadList = Search(entity);

				return proveedorOportunidadList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProveedorOportunidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("oportunidadVentaId", entity.OportunidadVentaId);
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
		public int Update(ProveedorOportunidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorOportunidadId", entity.ProveedorOportunidadId);
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
		public void Delete(ProveedorOportunidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorOportunidadId", entity.ProveedorOportunidadId);
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
