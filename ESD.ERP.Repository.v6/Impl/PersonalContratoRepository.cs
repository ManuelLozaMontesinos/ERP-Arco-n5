using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class PersonalContratoRepository : IPersonalContratoRepository
	{
		private const string SP_PRE_NAME = "sp_personal_cont";
		public PersonalContrato GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(PersonalContrato)
					},
					x =>
					{
						PersonalContrato pc = x[0] as PersonalContrato;

						return pc;
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
		public List<PersonalContrato> Search(PersonalContrato entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(PersonalContrato)
					},
					x =>
					{
						PersonalContrato pc = x[0] as PersonalContrato;

						return pc;
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
		public PersonalContrato SingleSearch(PersonalContrato entity)
		{
			try
			{
				List<PersonalContrato> personalContratoList = Search(entity);

				return personalContratoList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(PersonalContrato entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaSusc", entity.FechaSuscripcion);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("motivoFinPeriId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("cargoPersoId", entity.CargoPersonalId);
				parameters.AddParameter("regLaboralId", entity.RegimenLaboralId);
				parameters.AddParameter("tipoContratoId", entity.TipoContratoId);
				parameters.AddParameter("esDiscapa", entity.EsDiscapa);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("observa", entity.Observa);
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
		public int Update(PersonalContrato entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalContratoId", entity.PersonalContratoId);
				parameters.AddParameter("personalId", entity.PersonalId);
				parameters.AddParameter("fechaSusc", entity.FechaSuscripcion);
				parameters.AddParameter("fechaInicio", entity.FechaInicio);
				parameters.AddParameter("fechaFin", entity.FechaFin);
				parameters.AddParameter("motivoFinPeriId", entity.MotivoFinPeriodoId);
				parameters.AddParameter("cargoPersoId", entity.CargoPersonalId);
				parameters.AddParameter("regLaboralId", entity.RegimenLaboralId);
				parameters.AddParameter("tipoContratoId", entity.TipoContratoId);
				parameters.AddParameter("esDiscapa", entity.EsDiscapa);
				parameters.AddParameter("monedaId", entity.MonedaId);
				parameters.AddParameter("monto", entity.Monto);
				parameters.AddParameter("observa", entity.Observa);
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
		public void Delete(PersonalContrato entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("personalContratoId", entity.PersonalContratoId);
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
