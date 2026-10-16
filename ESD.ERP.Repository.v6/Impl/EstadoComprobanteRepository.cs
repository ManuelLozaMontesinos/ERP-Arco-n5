using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EstadoComprobanteRepository : IEstadoComprobanteRepository
	{
		private const string SP_PRE_NAME = "sp_estado_compro";
		public EstadoComprobante GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EstadoComprobante)
					},
					x =>
					{
						EstadoComprobante ec = x[0] as EstadoComprobante;

						return ec;
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
		public List<EstadoComprobante> Search(EstadoComprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estadoComprobanteList", entity.CodigoList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EstadoComprobante)
					},
					x =>
					{
						EstadoComprobante ec = x[0] as EstadoComprobante;

						return ec;
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
		public EstadoComprobante SingleSearch(EstadoComprobante entity)
		{
			try
			{
				List<EstadoComprobante> estadoComprobanteList = Search(entity);

				return estadoComprobanteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EstadoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComprobante", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
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
		public int Update(EstadoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("estadoComprobante", entity.Codigo);
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
		public void Delete(EstadoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("estadoComprobanteId", entity.EstadoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EstadoComprobante ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(EstadoComprobante)
					},
					x =>
					{
						EstadoComprobante ec = x[0] as EstadoComprobante;

						return ec;
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
