using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ComprobanteCobranzaRepository : IComprobanteCobranzaRepository
	{
		private const string SP_PRE_NAME = "sp_compro_cobra";
		public ComprobanteCobranza GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ComprobanteCobranza)
					},
					x =>
					{
						ComprobanteCobranza cc = x[0] as ComprobanteCobranza;

						return cc;
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
		public List<ComprobanteCobranza> Search(ComprobanteCobranza entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("comprobanteId", entity.ComprobanteId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ComprobanteCobranza)
					},
					x =>
					{
						ComprobanteCobranza cc = x[0] as ComprobanteCobranza;

						return cc;
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
		public ComprobanteCobranza SingleSearch(ComprobanteCobranza entity)
		{
			try
			{
				List<ComprobanteCobranza> comprobanteCobranzaList = Search(entity);

				return comprobanteCobranzaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ComprobanteCobranza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteId", entity.ComprobanteId);
				parameters.AddParameter("fecha", entity.Fecha);
				parameters.AddParameter("numeroOperacion", entity.NumeroOperacion);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("empresaCuentaBancariaId", entity.EmpresaCuentaBancariaId);
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
		public int Update(ComprobanteCobranza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteCobranzaId", entity.ComprobanteCobranzaId);
				parameters.AddParameter("numeroOperacion", entity.NumeroOperacion);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("empresaCuentaBancariaId", entity.EmpresaCuentaBancariaId);
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
		public void Delete(ComprobanteCobranza entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("comprobanteCobranzaId", entity.ComprobanteCobranzaId);
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
