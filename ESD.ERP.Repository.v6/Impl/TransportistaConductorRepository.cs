using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TransportistaConductorRepository : ITransportistaConductorRepository
	{
		private const string SP_PRE_NAME = "sp_transp_condu";
		public TransportistaConductor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TransportistaConductor)
					},
					x =>
					{
						TransportistaConductor tc = x[0] as TransportistaConductor;

						return tc;
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
		public List<TransportistaConductor> Search(TransportistaConductor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("transportistaId", entity.TransportistaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TransportistaConductor),
						typeof(TipoDocumentoIdentidad)
					},
					x =>
					{
						TransportistaConductor tc = x[0] as TransportistaConductor;
						TipoDocumentoIdentidad tdi = x[1] as TipoDocumentoIdentidad;

						tc.TipoDocumentoIdentidad = tdi;
						tc.TipoDocumentoIdentidadId = tdi.EntityId;

						return tc;
					},
					"tipo_doc_iden_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TransportistaConductor SingleSearch(TransportistaConductor entity)
		{
			try
			{
				List<TransportistaConductor> transportistaConductorList = Search(entity);

				return transportistaConductorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TransportistaConductor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaId", entity.TransportistaId);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("numeroLicencia", entity.NumeroLicencia);
				parameters.AddParameter("telefono", entity.Telefono);
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
		public int Update(TransportistaConductor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaConductorId", entity.TransportistaConductorId);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("numeroLicencia", entity.NumeroLicencia);
				parameters.AddParameter("telefono", entity.Telefono);
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
		public void Delete(TransportistaConductor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("transportistaConductorId", entity.TransportistaConductorId);
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
