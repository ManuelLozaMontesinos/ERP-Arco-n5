using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RegimenPensionarioEsquemaRepository : IRegimenPensionarioEsquemaRepository
	{
		private const string SP_PRE_NAME = "sp_reg_pens_esq";
		public RegimenPensionarioEsquema GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RegimenPensionarioEsquema),
						typeof(TipoEsquemaPension)
					},
					x =>
					{
						RegimenPensionarioEsquema rpe = x[0] as RegimenPensionarioEsquema;
						TipoEsquemaPension tep = x[1] as TipoEsquemaPension;

						rpe.TipoEsquemaPensionId = tep.EntityId;
						rpe.TipoEsquemaPension = tep;

						return rpe;
					},
					"tipo_esque_pen_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<RegimenPensionarioEsquema> Search(RegimenPensionarioEsquema entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("regimenPensionarioId", entity.RegimenPensionarioId);
			parameters.AddParameter("tipoEsquemaPensionId", entity.TipoEsquemaPensionId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RegimenPensionarioEsquema),
						typeof(TipoEsquemaPension)
					},
					x =>
					{
						RegimenPensionarioEsquema rpe = x[0] as RegimenPensionarioEsquema;
						TipoEsquemaPension tep = x[1] as TipoEsquemaPension;

						rpe.TipoEsquemaPensionId = tep.EntityId;
						rpe.TipoEsquemaPension = tep;

						return rpe;
					},
					"tipo_esque_pen_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public RegimenPensionarioEsquema SingleSearch(RegimenPensionarioEsquema entity)
		{
			try
			{
				List<RegimenPensionarioEsquema> regimenPensionarioEsquemaList = Search(entity);

				return regimenPensionarioEsquemaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RegimenPensionarioEsquema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("regimenPensionarioId", entity.RegimenPensionarioId);
				parameters.AddParameter("tipoEsquemaPensionId", entity.TipoEsquemaPensionId);
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
		public int Update(RegimenPensionarioEsquema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("regimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
				parameters.AddParameter("tipoEsquemaPensionId", entity.TipoEsquemaPensionId);
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
		public void Delete(RegimenPensionarioEsquema entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("regimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
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
