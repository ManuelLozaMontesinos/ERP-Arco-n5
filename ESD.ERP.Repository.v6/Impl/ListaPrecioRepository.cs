using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioRepository : IListaPrecioRepository
	{
		private const string SP_PRE_NAME = "sp_lista_precio";
		public ListaPrecio GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecio),
						typeof(EmpresaSede),
						typeof(ImpuestoTasa)
					},
					x =>
					{
						ListaPrecio lp = x[0] as ListaPrecio;
						EmpresaSede es = x[1] as EmpresaSede;
						ImpuestoTasa it = x[2] as ImpuestoTasa;

						lp.EmpresaSede = es;
						lp.EmpresaSedeId = es.EntityId;
						lp.ImpuestoTasa = it;
						lp.ImpuestoTasaId = it.EntityId;

						return lp;
					},
					"empresa_sede_id,impuesto_tasa_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ListaPrecio> Search(ListaPrecio entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaSede.EmpresaId);
			parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
			parameters.AddParameter("containsNombre", entity.Nombre);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecio),
						typeof(EmpresaSede),
						typeof(ImpuestoTasa),
						typeof(Moneda)
					},
					x =>
					{
						ListaPrecio lp = x[0] as ListaPrecio;
						EmpresaSede es = x[1] as EmpresaSede;
						ImpuestoTasa it = x[2] as ImpuestoTasa;
						Moneda m = x[3] as Moneda;

						lp.EmpresaSede = es;
						lp.EmpresaSedeId = es.EntityId;
						lp.ImpuestoTasa = it;
						lp.ImpuestoTasaId = it.EntityId;
						lp.Moneda = m;
						lp.MonedaId = m.EntityId;

						return lp;
					},
					"empresa_sede_id,impuesto_tasa_id,moneda_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecio SingleSearch(ListaPrecio entity)
		{
			try
			{
				List<ListaPrecio> listaPrecioList = Search(entity);

				return listaPrecioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("monedaId", entity.MonedaId);
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
		public int Update(ListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
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
		public void Delete(ListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
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
