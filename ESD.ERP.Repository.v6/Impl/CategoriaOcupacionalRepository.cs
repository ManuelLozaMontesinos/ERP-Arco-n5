using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class CategoriaOcupacionalRepository : ICategoriaOcupacionalRepository
	{
		private const string SP_PRE_NAME = "sp_cate_ocupa";
		public CategoriaOcupacional GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(CategoriaOcupacional)
					},
					x =>
					{
						CategoriaOcupacional co = x[0] as CategoriaOcupacional;

						return co;
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
		public List<CategoriaOcupacional> Search(CategoriaOcupacional entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(CategoriaOcupacional)
					},
					x =>
					{
						CategoriaOcupacional co = x[0] as CategoriaOcupacional;

						return co;
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
		public CategoriaOcupacional SingleSearch(CategoriaOcupacional entity)
		{
			try
			{
				List<CategoriaOcupacional> categoriaOcupacionalList = Search(entity);

				return categoriaOcupacionalList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(CategoriaOcupacional entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("categoriaOcupacional", entity.Codigo);
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
		public int Update(CategoriaOcupacional entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("categoriaOcupacionalId", entity.CategoriaOcupacionalId);
				parameters.AddParameter("categoriaOcupacional", entity.Codigo);
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
		public void Delete(CategoriaOcupacional entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("categoriaOcupacionalId", entity.CategoriaOcupacionalId);
				parameters.AddParameter("estado", entity.Estado);

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
