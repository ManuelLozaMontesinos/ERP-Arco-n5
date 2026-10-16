using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProveedorListaPrecioRepository : IProveedorListaPrecioRepository
	{
		private const string SP_PRE_NAME = "sp_prove_lista_p";
		public ProveedorListaPrecio GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ProveedorListaPrecio),
						typeof(Proveedor),
						typeof(ImpuestoTasa)
					},
					x =>
					{
						ProveedorListaPrecio plp = x[0] as ProveedorListaPrecio;
						Proveedor p = x[1] as Proveedor;
						ImpuestoTasa it = x[2] as ImpuestoTasa;

						plp.Proveedor = p;
						plp.ProveedorId = p.EntityId;
						plp.ImpuestoTasa = it;
						plp.ImpuestoTasaId = it.EntityId;

						return plp;
					},
					"proveedor_id,impuesto_tasa_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<ProveedorListaPrecio> Search(ProveedorListaPrecio entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ProveedorListaPrecio),
						typeof(Moneda)
					},
					x =>
					{
						ProveedorListaPrecio plp = x[0] as ProveedorListaPrecio;
						Moneda m = x[1] as Moneda;

						plp.Moneda = m;
						plp.MonedaId = m.EntityId;

						return plp;
					},
					"moneda_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ProveedorListaPrecio SingleSearch(ProveedorListaPrecio entity)
		{
			try
			{
				List<ProveedorListaPrecio> proveedorListaPrecioList = Search(entity);

				return proveedorListaPrecioList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ProveedorListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
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
		public int Update(ProveedorListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorListaPrecioId", entity.ProveedorListaPrecioId);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("impuestoTasaId", entity.ImpuestoTasaId);
				parameters.AddParameter("monedaId", entity.MonedaId);
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
		public void Delete(ProveedorListaPrecio entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorListaPrecioId", entity.ProveedorListaPrecioId);
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
