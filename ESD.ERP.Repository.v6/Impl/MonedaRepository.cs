using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class MonedaRepository : IMonedaRepository
	{
		private const string SP_PRE_NAME = "sp_moneda";
		public Moneda GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Moneda)
					},
					x =>
					{
						Moneda m = x[0] as Moneda;

						return m;
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
		public List<Moneda> Search(Moneda entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("esLocal", entity.EsLocal);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Moneda)
					},
					x =>
					{
						Moneda m = x[0] as Moneda;

						return m;
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
		public Moneda SingleSearch(Moneda entity)
		{
			try
			{
				List<Moneda> monedaList = Search(entity);

				return monedaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Moneda entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("moneda", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("simbolo", entity.Simbolo);
				parameters.AddParameter("esLocal", entity.EsLocal);
				parameters.AddParameter("codigoIso", entity.CodigoIso);
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
		public int Update(Moneda entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("moneda", entity.Codigo);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("simbolo", entity.Simbolo);
				parameters.AddParameter("esLocal", entity.EsLocal);
				parameters.AddParameter("codigoIso", entity.CodigoIso);
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
		public void Delete(Moneda entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("estado", entity.Estado);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
      public Moneda ObtenerPorCodigo(string codigo)
      {
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(Moneda)
					},
					x =>
					{
						Moneda m = x[0] as Moneda;

						return m;
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
