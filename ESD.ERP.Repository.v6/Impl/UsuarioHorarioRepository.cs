using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;
using ESD.ERP.Types;

namespace ESD.ERP.Repository.Impl
{
	public class UsuarioHorarioRepository : IUsuarioHorarioRepository
	{
		private const string SP_PRE_NAME = "sp_usuario_horario";
		public UsuarioHorario GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(UsuarioHorario),
						typeof(Usuario),
						typeof(Dia)
					},
					x =>
					{
						UsuarioHorario uh = x[0] as UsuarioHorario;
						Usuario u = x[1] as Usuario;
						Dia d = x[2] as Dia;

						return uh;
					},
					"usuario_id,dia_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<UsuarioHorario> Search(UsuarioHorario entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("usuarioId", entity.UsuarioId);
			parameters.AddParameter("dia", entity?.Dia?.Codigo);
			parameters.AddParameter("tieneHorarioAsignado", entity.TieneHorarioAsignado);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(UsuarioHorario),
						typeof(Usuario),
						typeof(Dia)
					},
					x =>
					{
						UsuarioHorario uh = x[0] as UsuarioHorario;
						Usuario u = x[1] as Usuario;
						Dia d = x[2] as Dia;

						return uh;
					},
					"usuario_id,dia_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public UsuarioHorario SingleSearch(UsuarioHorario entity)
		{
			try
			{
				List<UsuarioHorario> usuarioHorarioList = Search(entity);

				return usuarioHorarioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(UsuarioHorario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("diaId", entity.DiaId);
				parameters.AddParameter("horaInicio", entity.HoraInicio);
				parameters.AddParameter("horaFin", entity.HoraFin);
				parameters.AddParameter("horaExtension", entity.HoraExtension);
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
		public int Update(UsuarioHorario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioHorarioId", entity.UsuarioHorarioId);
				parameters.AddParameter("horaInicio", entity.HoraInicio);
				parameters.AddParameter("horaFin", entity.HoraFin);
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
		public void Delete(UsuarioHorario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioHorarioId", entity.UsuarioHorarioId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<UsuarioHorario> ListarPorUsuarioId(UsuarioHorario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);

				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, "_listarPorusuarioId"), parameters,
					new[]
					{
						typeof(UsuarioHorario),
						typeof(Dia)
					},
					x =>
					{
						UsuarioHorario uh = x[0] as UsuarioHorario;
						Dia d = x[1] as Dia;

						uh.Dia = d;
						uh.DiaId = d.EntityId;

						return uh;
					},
					"dia_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarLote(List<UsuarioHorario> list)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				List<UsuarioHorarioType> usuarioHorarioTypeList = new();
				usuarioHorarioTypeList = (from s in list
															 select new UsuarioHorarioType
															 {
																 UsuarioHorarioId = s.UsuarioHorarioId,
																 UsuarioId = s.UsuarioId,
																 DiaId = s.DiaId,
																 HoraInicio = s.HoraInicio,
																 HoraFin = s.HoraFin,
																 HoraExtension = s.HoraExtension,
																 Estado = s.Estado,
																 UsuarioCreacionId = s.UsuarioCreacionId,
															 }).ToList();

				parameters.AddTableParameter("usuarioHorarioList", "dbo.UsuarioHorarioType", usuarioHorarioTypeList);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarLote"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
