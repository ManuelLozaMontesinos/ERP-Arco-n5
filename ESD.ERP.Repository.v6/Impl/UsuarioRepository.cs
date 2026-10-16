using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class UsuarioRepository : IUsuarioRepository
	{
		private const string SP_PRE_NAME = "sp_usuario";
		public Usuario GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Usuario)
					},
					x =>
					{
						Usuario u = x[0] as Usuario;

						return u;
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
		public List<Usuario> Search(Usuario entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("usuarioLogon", entity.UsuarioLogon);
			parameters.AddParameter("containsNombreCompleto", entity.NombreCompleto);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Usuario)
					},
					x =>
					{
						Usuario u = x[0] as Usuario;

						return u;
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
		public Usuario SingleSearch(Usuario entity)
		{
			try
			{
				List<Usuario> usuarioList = Search(entity);

				return usuarioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Usuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioLogon", entity.UsuarioLogon);
				parameters.AddParameter("nombreCompleto", entity.NombreCompleto);
				parameters.AddParameter("password", entity.Password);
				parameters.AddParameter("clavePublica", entity.ClavePublica);
				parameters.AddParameter("cambiarContrasenaProximo", entity.CambiarContrasenaProximo);
				parameters.AddParameter("noPuedeCambiarContrasena", entity.NoPuedeCambiarContrasena);
				parameters.AddParameter("noExpira", entity.NoExpira);
				parameters.AddParameter("fechaExpiracion", entity.FechaExpiracion);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("emailAlterno", entity.EmailAlterno);
				parameters.AddParameter("codigoRecuperacionContrasena", entity.CodigoRecuperacionContrasena);
				parameters.AddParameter("tieneHorario", entity.TieneHorario);
				parameters.AddParameter("origenRegistroId", entity.OrigenRegistroId);
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
		public int Update(Usuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("nombreCompleto", entity.NombreCompleto);
				parameters.AddParameter("cambiarContrasenaProximo", entity.CambiarContrasenaProximo);
				parameters.AddParameter("noPuedeCambiarContrasena", entity.NoPuedeCambiarContrasena);
				parameters.AddParameter("noExpira", entity.NoExpira);
				parameters.AddParameter("fechaExpiracion", entity.FechaExpiracion);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("emailAlterno", entity.EmailAlterno);
				parameters.AddParameter("TieneHorario", entity.TieneHorario);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.UPDATE), parameters);

				return entity.EntityId;
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void Delete(Usuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Usuario ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(Usuario)
					},
					x =>
					{
						Usuario u = x[0] as Usuario;

						return u;
					},
					string.Empty
				).FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarEstado(Usuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarEstado"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarContrasena(Usuario entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("password", entity.Password);
				parameters.AddParameter("cambiarContrasenaProximo", entity.CambiarContrasenaProximo);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarContrasena"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
