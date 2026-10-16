using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalRemuneracionRepository : IPersonalRemuneracionRepository
	{
		private const string SP_PRE_NAME = "sp_personal_rem";
		public PersonalRemuneracion GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalRemuneracion),
						typeof(Moneda)
					},
					x =>
					{
						PersonalRemuneracion pr = x[0] as PersonalRemuneracion;
						Moneda m = x[1] as Moneda;

						pr.MonedaId = m.EntityId;
						pr.Moneda = m;

						return pr;
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
		public List<PersonalRemuneracion> Search(PersonalRemuneracion entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("personalId", entity.PersonalId);
			parameters.AddParameter("tipoRemuneracionId", entity.TipoRemuneracionId);
			parameters.AddParameter("monedaId", entity.MonedaId);
			parameters.AddParameter("fechaBajaNull", entity.FechaBajaNull);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalRemuneracion),
						typeof(TipoRemuneracion),
						typeof(Moneda)
					},
					x =>
					{
						PersonalRemuneracion pr = x[0] as PersonalRemuneracion;
						TipoRemuneracion tr = x[1] as TipoRemuneracion;
						Moneda m = x[2] as Moneda;

						pr.TipoRemuneracionId = tr.EntityId;
						pr.TipoRemuneracion = tr;
						pr.MonedaId = m.EntityId;
						pr.Moneda = m;

						return pr;
					},
					"tipo_remunera_id,moneda_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public PersonalRemuneracion SingleSearch(PersonalRemuneracion entity)
		{
			try
			{
				List<PersonalRemuneracion> personalRemuneracionList = Search(entity);

				return personalRemuneracionList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("tipoRemuneracionId", entity.TipoRemuneracionId);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("monto", entity.Monto);
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
		public int Update(PersonalRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalRemuneracionId", entity.PersonalRemuneracionId);
				parameters.AddParameter("monto", entity.Monto);
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
		public void Delete(PersonalRemuneracion entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalRemuneracionId", entity.PersonalRemuneracionId);
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
