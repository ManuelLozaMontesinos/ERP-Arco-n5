using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class EmpresaCuentaBancariaRepository : IEmpresaCuentaBancariaRepository
	{
		private const string SP_PRE_NAME = "sp_empresa_cta";
		public EmpresaCuentaBancaria GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(EmpresaCuentaBancaria)
					},
					x =>
					{
						EmpresaCuentaBancaria ec = x[0] as EmpresaCuentaBancaria;

						return ec;
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
		public List<EmpresaCuentaBancaria> Search(EmpresaCuentaBancaria entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("bancoId", entity.BancoId);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(EmpresaCuentaBancaria),
						typeof(Banco),
						typeof(Moneda)
					},
					x =>
					{
						EmpresaCuentaBancaria ec = x[0] as EmpresaCuentaBancaria;
						Banco b = x[1] as Banco;
						Moneda m = x[2] as Moneda;

						ec.Banco = b;
						ec.BancoId = b.EntityId;
						ec.Moneda = m;
						ec.MonedaId = m.EntityId;

						return ec;
					},
					"banco_id,moneda_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public EmpresaCuentaBancaria SingleSearch(EmpresaCuentaBancaria entity)
		{
			try
			{
				List<EmpresaCuentaBancaria> empresaCuentaBancariaList = Search(entity);

				return empresaCuentaBancariaList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(EmpresaCuentaBancaria entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("bancoId", entity.BancoId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("cuenta", entity.Cuenta);
				parameters.AddParameter("cuentaInterbancaria", entity.CuentaInterbancaria);
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
		public int Update(EmpresaCuentaBancaria entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaCuentaBancariaId", entity.EmpresaCuentaBancariaId);
				parameters.AddParameter("nombre", entity.Nombre);
				parameters.AddParameter("cuenta", entity.Cuenta);
				parameters.AddParameter("cuentaInterbancaria", entity.CuentaInterbancaria);
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
		public void Delete(EmpresaCuentaBancaria entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaCuentaBancariaId", entity.EmpresaCuentaBancariaId);
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
