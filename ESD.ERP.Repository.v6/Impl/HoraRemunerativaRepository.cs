using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class HoraRemunerativaRepository : IHoraRemunerativaRepository
	{
		private const string SP_PRE_NAME = "sp_hora_rem";
		public HoraRemunerativa GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(HoraRemunerativa),
						typeof(VariableCalculo)
					},
					x =>
					{
						HoraRemunerativa hr = x[0] as HoraRemunerativa;
						VariableCalculo vc = x[1] as VariableCalculo;

						hr.VariableCalculoId = vc.EntityId;
						hr.VariableCalculo = vc;

						return hr;
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
		public List<HoraRemunerativa> Search(HoraRemunerativa entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(HoraRemunerativa),
						typeof(VariableCalculo)
					},
					x =>
					{
						HoraRemunerativa hr = x[0] as HoraRemunerativa;
						VariableCalculo vc = x[1] as VariableCalculo;

						hr.VariableCalculoId = vc.EntityId;
						hr.VariableCalculo = vc;

						return hr;
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
		public HoraRemunerativa SingleSearch(HoraRemunerativa entity)
		{
			try
			{
				List<HoraRemunerativa> horaRemunerativaList = Search(entity);

				return horaRemunerativaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(HoraRemunerativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("horaRemunerativa", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
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
		public int Update(HoraRemunerativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("horaRemunerativaId", entity.HoraRemunerativaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
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
		public void Delete(HoraRemunerativa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("horaRemunerativaId", entity.HoraRemunerativaId);
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
