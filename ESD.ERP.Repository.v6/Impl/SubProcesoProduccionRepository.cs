using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class SubProcesoProduccionRepository : ISubProcesoProduccionRepository
	{
		private const string SP_PRE_NAME = "sp_sub_proce_p";
		public SubProcesoProduccion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(SubProcesoProduccion)
					},
					x =>
					{
						SubProcesoProduccion spp = x[0] as SubProcesoProduccion;

						return spp;
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
		public List<SubProcesoProduccion> Search(SubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("procesoProduccionId", entity.ProcesoProduccionId);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(SubProcesoProduccion),
						typeof(ProcesoProduccion)
					},
					x =>
					{
						SubProcesoProduccion spp = x[0] as SubProcesoProduccion;
						ProcesoProduccion pp = x[1] as ProcesoProduccion;

						spp.ProcesoProduccion = pp;
						spp.ProcesoProduccionId = pp.EntityId;

						return spp;
					},
					"proce_prod_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public SubProcesoProduccion SingleSearch(SubProcesoProduccion entity)
		{
			try
			{
				List<SubProcesoProduccion> subProcesoProduccionList = Search(entity);

				return subProcesoProduccionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(SubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("procesoProduccionId", entity.ProcesoProduccionId);
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
		public int Update(SubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
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
		public void Delete(SubProcesoProduccion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("subProcesoProduccionId", entity.SubProcesoProduccionId);
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
