using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class RegimenPensionarioComisionRepository : IRegimenPensionarioComisionRepository
	{
		private const string SP_PRE_NAME = "sp_reg_pens_comi";
		public RegimenPensionarioComision GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(RegimenPensionarioComision),
						typeof(TipoAporteComision)
					},
					x =>
					{
						RegimenPensionarioComision rpc = x[0] as RegimenPensionarioComision;
						TipoAporteComision tac = x[1] as TipoAporteComision;

						rpc.TipoAporteComisionId = tac.EntityId;
						rpc.TipoAporteComision = tac;

						return rpc;
					},
					"tipo_apo_comi_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<RegimenPensionarioComision> Search(RegimenPensionarioComision entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("regimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
			parameters.AddParameter("tipoAporteComisionId", entity.TipoAporteComisionId);
			parameters.AddParameter("fechaBajaNull", entity.FechaBajaNull);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(RegimenPensionarioComision),
						typeof(TipoAporteComision)
					},
					x =>
					{
						RegimenPensionarioComision rpc = x[0] as RegimenPensionarioComision;
						TipoAporteComision tac = x[1] as TipoAporteComision;

						rpc.TipoAporteComisionId = tac.EntityId;
						rpc.TipoAporteComision = tac;

						return rpc;
					},
					"tipo_apo_comi_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public RegimenPensionarioComision SingleSearch(RegimenPensionarioComision entity)
		{
			try
			{
				List<RegimenPensionarioComision> regimenPensionarioComisionList = Search(entity);

				return regimenPensionarioComisionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(RegimenPensionarioComision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("RegimenPensionarioEsquemaId", entity.RegimenPensionarioEsquemaId);
				parameters.AddParameter("TipoAporteComisionId", entity.TipoAporteComisionId);
				parameters.AddParameter("porcentaje", entity.Porcentaje);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public int Update(RegimenPensionarioComision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("regimenPensionarioComisionId", entity.RegimenPensionarioComisionId);
				parameters.AddParameter("porcentaje", entity.Porcentaje);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public void Delete(RegimenPensionarioComision entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("regimenPensionarioComisionId", entity.RegimenPensionarioComisionId);
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
