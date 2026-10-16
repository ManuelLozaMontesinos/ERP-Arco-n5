using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class JornadaLaboralRepository : IJornadaLaboralRepository
	{
		private const string SP_PRE_NAME = "sp_jornada_lab";
		public JornadaLaboral GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(JornadaLaboral)
					},
					x =>
					{
						JornadaLaboral jl = x[0] as JornadaLaboral;

						return jl;
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
		public List<JornadaLaboral> Search(JornadaLaboral entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("dia", entity.Dia.Codigo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(JornadaLaboral)
					},
					x =>
					{
						JornadaLaboral jl = x[0] as JornadaLaboral;

						return jl;
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
		public JornadaLaboral SingleSearch(JornadaLaboral entity)
		{
			try
			{
				List<JornadaLaboral> jornadaLabList = Search(entity);

				return jornadaLabList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(JornadaLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("horaIngreso", entity.HoraIngreso);
				parameters.AddParameter("cantidadHoras", entity.CantidadHoras);
				parameters.AddParameter("diaId", entity.DiaId);
				parameters.AddParameter("diaHastaId", entity.DiaHastaId);
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
		public int Update(JornadaLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("jornadaLabId", entity.JornadaLaboralId);
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("horaIngreso", entity.HoraIngreso);
				parameters.AddParameter("cantidadHoras", entity.CantidadHoras);
				parameters.AddParameter("diaId", entity.DiaId);
				parameters.AddParameter("diaHastaId", entity.DiaHastaId);
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
		public void Delete(JornadaLaboral entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("jornadaLabId", entity.JornadaLaboralId);
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
