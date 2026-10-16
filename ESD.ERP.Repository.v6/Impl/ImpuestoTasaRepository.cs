using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ImpuestoTasaRepository : IImpuestoTasaRepository
	{
		private const string SP_PRE_NAME = "sp_impuesto_tasa";
		public ImpuestoTasa GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ImpuestoTasa)
					},
					x =>
					{
						ImpuestoTasa it = x[0] as ImpuestoTasa;

						return it;
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
		public List<ImpuestoTasa> Search(ImpuestoTasa entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("tipoImpuesto", entity.TipoImpuesto?.Codigo);
			parameters.AddParameter("periodo", entity.Periodo);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ImpuestoTasa),
						typeof(TipoImpuesto)
					},
					x =>
					{
						ImpuestoTasa it = x[0] as ImpuestoTasa;
						TipoImpuesto ti = x[1] as TipoImpuesto;

						it.TipoImpuesto = ti;
						it.TipoImpuestoId = ti.EntityId;

						return it;
					},
					"tipo_impuesto_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ImpuestoTasa SingleSearch(ImpuestoTasa entity)
		{
			try
			{
				List<ImpuestoTasa> impuestoTasaList = Search(entity);

				return impuestoTasaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ImpuestoTasa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("tipoImpuestoId", entity.TipoImpuestoId);
				parameters.AddParameter("tasa", entity.Tasa);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public int Update(ImpuestoTasa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("tipoImpuestoId", entity.TipoImpuestoId);
				parameters.AddParameter("tasa", entity.Tasa);
				parameters.AddParameter("fechaAlta", entity.FechaAlta);
				parameters.AddParameter("fechaBaja", entity.FechaBaja);
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
		public void Delete(ImpuestoTasa entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
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
