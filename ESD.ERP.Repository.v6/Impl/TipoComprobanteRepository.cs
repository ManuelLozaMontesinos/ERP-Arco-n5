using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoComprobanteRepository : ITipoComprobanteRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_comprob";
		public TipoComprobante GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoComprobante)
					},
					x =>
					{
						TipoComprobante tc = x[0] as TipoComprobante;

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
		public List<TipoComprobante> Search(TipoComprobante entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoComprobanteList", entity.CodigoList);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoComprobante)
					},
					x =>
					{
						TipoComprobante tc = x[0] as TipoComprobante;

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
		public TipoComprobante SingleSearch(TipoComprobante entity)
		{
			try
			{
				List<TipoComprobante> tipoComprobanteList = Search(entity);

				return tipoComprobanteList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoComprobante", entity.Codigo);
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
		public int Update(TipoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
				parameters.AddParameter("tipoComprobante", entity.Codigo);
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
		public void Delete(TipoComprobante entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoComprobanteId", entity.TipoComprobanteId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public TipoComprobante ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(TipoComprobante)
					},
					x =>
					{
						TipoComprobante tc = x[0] as TipoComprobante;

						return tc;
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
