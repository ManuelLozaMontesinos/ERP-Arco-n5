using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoDocumentoIdentidadRepository : ITipoDocumentoIdentidadRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_doc_iden";
		public TipoDocumentoIdentidad GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoDocumentoIdentidad)
					},
					x =>
					{
						TipoDocumentoIdentidad tdi = x[0] as TipoDocumentoIdentidad;

						return tdi;
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
		public List<TipoDocumentoIdentidad> Search(TipoDocumentoIdentidad entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoDocumentoIdentidad", entity.Codigo);
			parameters.AddParameter("notTipoDocumentoIdentidadList", entity.NotTipoDocumentoIdentidadList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoDocumentoIdentidad)
					},
					x =>
					{
						TipoDocumentoIdentidad tdi = x[0] as TipoDocumentoIdentidad;

						return tdi;
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
		public TipoDocumentoIdentidad SingleSearch(TipoDocumentoIdentidad entity)
		{
			try
			{
				List<TipoDocumentoIdentidad> tipoDocumentoIdentidadList = Search(entity);

				return tipoDocumentoIdentidadList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoDocumentoIdentidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoIdentidad", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("formatoValidacion", entity.FormatoValidacion);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
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
		public int Update(TipoDocumentoIdentidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("formatoValidacion", entity.FormatoValidacion);
				parameters.AddParameter("codigoSunat", entity.CodigoSunat);
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
		public void Delete(TipoDocumentoIdentidad entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoDocumentoIdentidad ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(TipoDocumentoIdentidad)
					},
					x =>
					{
						TipoDocumentoIdentidad tdi = x[0] as TipoDocumentoIdentidad;

						return tdi;
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
	}
}
