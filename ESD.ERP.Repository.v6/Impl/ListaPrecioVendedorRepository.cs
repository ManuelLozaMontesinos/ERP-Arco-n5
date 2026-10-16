using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ListaPrecioVendedorRepository : IListaPrecioVendedorRepository
	{
		private const string SP_PRE_NAME = "sp_lista_p_vendedor";
		public ListaPrecioVendedor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(ListaPrecioVendedor)
					},
					x =>
					{
						ListaPrecioVendedor lpp = x[0] as ListaPrecioVendedor;

						return lpp;
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
		public List<ListaPrecioVendedor> Search(ListaPrecioVendedor entity)
		{
		    Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);
					    
            parameters.AddParameter("vendedorId", entity.VendedorId);
            parameters.AddParameter("empresaId", entity.ListaPrecio?.EmpresaSede?.EmpresaId);
            parameters.AddParameter("empresaSedeId", entity.ListaPrecio?.EmpresaSedeId);
            parameters.AddParameter("monedaId", entity.ListaPrecio?.MonedaId);
            parameters.AddParameter("estado", entity.Estado);

            try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(ListaPrecioVendedor),
                        typeof(Vendedor),
						typeof(ListaPrecio),
                        typeof(EmpresaSede),
						typeof(ImpuestoTasa),
						typeof(Moneda),
						
					},
					x =>
					{
						ListaPrecioVendedor lpp = x[0] as ListaPrecioVendedor;
						Vendedor v = x[1] as Vendedor;
						ListaPrecio lp = x[2] as ListaPrecio;
						EmpresaSede es = x[3] as EmpresaSede;
						ImpuestoTasa it = x[4] as ImpuestoTasa;
						Moneda m = x[5] as Moneda;

                        lpp.Vendedor = v;
                        lpp.VendedorId = v.EntityId;
						lpp.ListaPrecio = lp;
						lpp.ListaPrecioId = lp.EntityId;
                        lp.EmpresaSede = es;
                        lp.EmpresaSedeId = es.EntityId;
                        lp.ImpuestoTasa = it;
                        lp.ImpuestoTasaId = it.EntityId;
                        lp.Moneda = m;
                        lp.MonedaId = m.EntityId;

                        return lpp;
					},
                    "vendedor_id,lista_precio_id,empresa_sede_id,impuesto_tasa_id,moneda_id"
                );
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public ListaPrecioVendedor SingleSearch(ListaPrecioVendedor entity)
		{
			try
			{
				List<ListaPrecioVendedor> listaPrecioProveedorList = Search(entity);

				return listaPrecioProveedorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(ListaPrecioVendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("listaPrecioId", entity.ListaPrecioId);
				parameters.AddParameter("vendedorId", entity.VendedorId);
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
		public int Update(ListaPrecioVendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ListaPrecioVendedorId", entity.ListaPrecioVendedorId);
				parameters.AddParameter("estado", entity.Estado);
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
		public void Delete(ListaPrecioVendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("ListaPrecioVendedorId", entity.ListaPrecioVendedorId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}

        public void ActualizarListaPrecioVendedorId(ListaPrecioVendedor entity)
        {
            Dictionary<string, object> parameters = new();

            try
            {
                parameters.AddParameter("vendedorId", entity.VendedorId);
                parameters.AddParameter("listaPrecioIdList", entity.ListaPrecioIdList);
                parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

                DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarListaPrecioVendedorId"), parameters);

            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                throw;
            }
        }
    }
}
