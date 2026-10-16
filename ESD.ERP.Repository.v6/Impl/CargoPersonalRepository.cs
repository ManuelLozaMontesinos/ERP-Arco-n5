using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CargoPersonalRepository : ICargoPersonalRepository
	{
		private const string SP_PRE_NAME = "sp_cargo_perso";
		public CargoPersonal GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CargoPersonal)
					},
					x =>
					{
						CargoPersonal cp = x[0] as CargoPersonal;

						return cp;
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
		public List<CargoPersonal> Search(CargoPersonal entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CargoPersonal)
					},
					x =>
					{
						CargoPersonal cp = x[0] as CargoPersonal;

						return cp;
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
		public CargoPersonal SingleSearch(CargoPersonal entity)
		{
			try
			{
				List<CargoPersonal> cargoPersonalList = Search(entity);

				return cargoPersonalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CargoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cargoPersonal", entity.Codigo);
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
		public int Update(CargoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cargoPersonalId", entity.CargoPersonalId);
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
		public void Delete(CargoPersonal entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("cargoPersonalId", entity.CargoPersonalId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public CargoPersonal ObtenerPorCodigo(string codigo)
		{
			Dictionary<string, object> parameters = new();
			parameters.AddParameter("codigo", codigo);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.GET_BY_CODE), parameters,
					new[]
					{
						typeof(CargoPersonal)
					},
					x =>
					{
						CargoPersonal cp = x[0] as CargoPersonal;

						return cp;
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
