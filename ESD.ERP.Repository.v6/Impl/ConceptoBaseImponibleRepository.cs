using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ConceptoBaseImponibleRepository : IConceptoBaseImponibleRepository
	{
		private const string SP_PRE_NAME = "sp_concep_base_im";
		public ConceptoBaseImponible GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ConceptoBaseImponible)
					},
					x =>
					{
						ConceptoBaseImponible cbi = x[0] as ConceptoBaseImponible;

						return cbi;
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
		public List<ConceptoBaseImponible> Search(ConceptoBaseImponible entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ConceptoBaseImponible)
					},
					x =>
					{
						ConceptoBaseImponible cbi = x[0] as ConceptoBaseImponible;

						return cbi;
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
		public ConceptoBaseImponible SingleSearch(ConceptoBaseImponible entity)
		{
			try
			{
				List<ConceptoBaseImponible> conceptoBaseImponibleList = Search(entity);

				return conceptoBaseImponibleList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ConceptoBaseImponible entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoBaseImponibleId", entity.TipoBaseImponibleId);
				parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);
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
		public int Update(ConceptoBaseImponible entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("conceptoBaseImponibleId", entity.ConceptoBaseImponibleId);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(ConceptoBaseImponible entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("conceptoBaseImponibleId", entity.ConceptoBaseImponibleId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public List<ConceptoBaseImponible> ListarPorConceptoRemunerativoId(ConceptoBaseImponible entity)
      {
			Dictionary<string, object> parameters = new();

			parameters.AddParameter("conceptoRemunerativoId", entity.ConceptoRemunerativoId);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorConceptoRemunerativoId"), parameters,
					new[]
					{
						typeof(ConceptoBaseImponible),
						typeof(TipoBaseImponible)
					},
					x =>
					{
						ConceptoBaseImponible cbi = x[0] as ConceptoBaseImponible;
						TipoBaseImponible tbi = x[1] as TipoBaseImponible;

						cbi.TipoBaseImponibleId = tbi.EntityId;
						cbi.TipoBaseImponible = tbi;
						return cbi;
					},
					"tipo_base_impo_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
   }
}
