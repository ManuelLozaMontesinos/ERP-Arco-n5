using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class VendedorRepository : IVendedorRepository
	{
		private const string SP_PRE_NAME = "sp_vendedor";
		public Vendedor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Vendedor),
						typeof(Proveedor)
					},
					x =>
					{
						
						Vendedor v = x[0] as Vendedor;
						Proveedor p = x[1] as Proveedor;

						v.Proveedor = p;
						v.ProveedorId = p.EntityId;

						return v;
					},
					"proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Vendedor> Search(Vendedor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.Proveedor.EmpresaId);
			parameters.AddParameter("proveedorId", entity.ProveedorId);
			parameters.AddParameter("containsRazonSocial", entity.Proveedor?.RazonSocial);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.Proveedor?.NumeroDocumentoIdentidad);
			parameters.AddParameter("usuarioId", entity.Proveedor?.UsuarioId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Vendedor),
						typeof(Proveedor)
					},
					x =>
					{

						Vendedor v = x[0] as Vendedor;
						Proveedor p = x[1] as Proveedor;

						v.Proveedor = p;
						v.ProveedorId = p.EntityId;

						return v;
					},
					"proveedor_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Vendedor SingleSearch(Vendedor entity)
		{
			try
			{
				List<Vendedor> vendedorList = Search(entity);

				return vendedorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Vendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedor", entity.Codigo);
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("servidorImpresion", entity.ServidorImpresion);
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
		public int Update(Vendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorId", entity.VendedorId);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("servidorImpresion", entity.ServidorImpresion);
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
		public void Delete(Vendedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("vendedorId", entity.VendedorId);
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
