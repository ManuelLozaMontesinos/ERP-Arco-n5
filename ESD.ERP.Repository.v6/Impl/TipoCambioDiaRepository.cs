using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class TipoCambioDiaRepository : ITipoCambioDiaRepository
	{
		private const string SP_PRE_NAME = "sp_tipo_camb_dia";
		public TipoCambioDia GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(TipoCambioDia)
					},
					x =>
					{
						TipoCambioDia tcd = x[0] as TipoCambioDia;

						return tcd;
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
		public List<TipoCambioDia> Search(TipoCambioDia entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoCambioId", entity.TipoCambioId);
			parameters.AddParameter("tipoCambio", entity?.TipoCambio?.Codigo);
			parameters.AddParameter("fecha", entity.Fecha);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(TipoCambioDia)
					},
					x =>
					{
						TipoCambioDia tcd = x[0] as TipoCambioDia;

						return tcd;
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
		public TipoCambioDia SingleSearch(TipoCambioDia entity)
		{
			try
			{
				List<TipoCambioDia> tipoCambioDiaList = Search(entity);

				return tipoCambioDiaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(TipoCambioDia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoCambioId", entity.TipoCambioId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("montoCompra", entity.MontoCompra);
				parameters.AddParameter("montoVenta", entity.MontoVenta);
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
		public int Update(TipoCambioDia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoCambioDiaId", entity.TipoCambioDiaId);
				parameters.AddParameter("montoCompra", entity.MontoCompra);
				parameters.AddParameter("montoVenta", entity.MontoVenta);
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
		public void Delete(TipoCambioDia entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoCambioDiaId", entity.TipoCambioDiaId);
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
