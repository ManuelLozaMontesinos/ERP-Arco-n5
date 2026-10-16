using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoRemuneracionRepository : ITipoRemuneracionRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_remunera";
		public TipoRemuneracion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoRemuneracion),
						typeof(VariableCalculo)
					},
					x =>
					{
						TipoRemuneracion tr = x[0] as TipoRemuneracion;
						VariableCalculo vc = x[1] as VariableCalculo;

						tr.VariableCalculoId = vc.EntityId;
						tr.VariableCalculo = vc;

						return tr;
					},
					"var_calc_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<TipoRemuneracion> Search(TipoRemuneracion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoRemuneracion),
						typeof(VariableCalculo)
					},
					x =>
					{
						TipoRemuneracion tr = x[0] as TipoRemuneracion;
						VariableCalculo vc = x[1] as VariableCalculo;

						tr.VariableCalculoId = vc.EntityId;
						tr.VariableCalculo = vc;

						return tr;
					},
					"var_calc_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoRemuneracion SingleSearch(TipoRemuneracion entity)
		{
			try
			{
				List<TipoRemuneracion> tipoRemuneraList = Search(entity);

				return tipoRemuneraList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("variableCalculoId", entity.VariableCalculoId);
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
		public int Update(TipoRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoRemuneracionId", entity.TipoRemuneracionId);
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
		public void Delete(TipoRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoRemuneraId", entity.TipoRemuneracionId);
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
