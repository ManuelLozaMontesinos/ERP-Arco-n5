using System;
using System.Collections.Generic;
using System.Linq;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository.Impl
{
	public class ProveedorRepository : IProveedorRepository
	{
		private const string SP_PRE_NAME = "sp_proveedor";
		public Proveedor GetById(int id)
		{
			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME,StoredProcActionConstant.GET_BY_ID), id,
					new[]
					{
						typeof(Proveedor),
						typeof(TipoNaturalezaJuridica),
						typeof(TipoDocumentoIdentidad),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento)
					},
					x =>
					{
						Proveedor p = x[0] as Proveedor;
						TipoNaturalezaJuridica tnj = x[1] as TipoNaturalezaJuridica;
						TipoDocumentoIdentidad tdi = x[2] as TipoDocumentoIdentidad;
						TipoVia tv = x[3] as TipoVia;
						TipoZona tz = x[4] as TipoZona;
						Distrito d = x[5] as Distrito;
						Provincia prv = x[6] as Provincia;
						Departamento dp = x[7] as Departamento;

						p.TipoNaturalezaJuridicaId = tnj.EntityId;
						p.TipoNaturalezaJuridica = tnj;
						p.TipoDocumentoIdentidadId = tdi.EntityId;
						p.TipoDocumentoIdentidad = tdi;
						p.TipoVia = tv;
						p.TipoViaId = tv.EntityId;
						p.TipoZona = tz;
						p.TipoZonaId = tz.EntityId;
						p.Distrito = d;
						p.DistritoId = d.EntityId;
						d.Provincia = prv;
						d.ProvinciaId = prv.EntityId;
						prv.Departamento = dp;
						prv.DepartamentoId = dp.DepartamentoId;

						return p;
					},
					"tipo_nat_juri_id,tipo_doc_iden_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public List<Proveedor> Search(Proveedor entity)
		{
			Dictionary<string, object> parameters = RepositoryHelper.CreateParameterListForDynamicQuery(entity.PageSize, entity.OrderBy);

			parameters.AddParameter("empresaId", entity.EmpresaId);
			parameters.AddParameter("notProveedorId", entity.NotProveedorId);
			parameters.AddParameter("tipoNaturalezaJuridica", entity.TipoNaturalezaJuridica?.Codigo);
			parameters.AddParameter("proveedor", entity.Codigo);
			parameters.AddParameter("containsRazonSocial", entity.RazonSocial);
			parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
			parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
			parameters.AddParameter("numeroRuc", entity.NumeroRuc);
			parameters.AddParameter("usuarioId", entity.UsuarioId);
			parameters.AddParameter("tipoProveedor", entity.TipoProveedor?.Codigo);
			parameters.AddParameter("esProspecto", entity.EsProspecto);
			parameters.AddParameter("estado", entity.Estado);

			try
			{
				return DbFactory.Query(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.SEARCH), parameters,
					new[]
					{
						typeof(Proveedor),
						typeof(TipoNaturalezaJuridica),
						typeof(TipoDocumentoIdentidad),
						typeof(TipoVia),
						typeof(TipoZona),
						typeof(Distrito),
						typeof(Provincia),
						typeof(Departamento),
						typeof(TipoProfesionOficio),
						typeof(TipoVentaCliente)
					},
					x =>
					{
						Proveedor p = x[0] as Proveedor;
						TipoNaturalezaJuridica tnj = x[1] as TipoNaturalezaJuridica;
						TipoDocumentoIdentidad tdi = x[2] as TipoDocumentoIdentidad;
						TipoVia tv = x[3] as TipoVia;
						TipoZona tz = x[4] as TipoZona;
						Distrito d = x[5] as Distrito;
						Provincia prv = x[6] as Provincia;
						Departamento dp = x[7] as Departamento;
						TipoProfesionOficio tpo = x[8] as TipoProfesionOficio;
						TipoVentaCliente tvc = x[9] as TipoVentaCliente;

						p.TipoNaturalezaJuridicaId = tnj.EntityId;
						p.TipoNaturalezaJuridica = tnj;
						p.TipoDocumentoIdentidadId = tdi.EntityId;
						p.TipoDocumentoIdentidad = tdi;
						p.TipoVia = tv;
						p.TipoViaId = tv.EntityId;
						p.TipoZona = tz;
						p.TipoZonaId = tz.EntityId;
						p.Distrito = d;
						p.DistritoId = d.EntityId;
						d.Provincia = prv;
						d.ProvinciaId = prv.EntityId;
						prv.Departamento = dp;
						prv.DepartamentoId = dp.DepartamentoId;
						p.TipoProfesionOficioId = tpo?.EntityId;
						p.TipoProfesionOficio = tpo;
						p.TipoVentaCliente = tvc;
						p.TipoVentaClienteId = tvc?.EntityId;

						return p;
					},
					"tipo_nat_juri_id,tipo_doc_iden_id,tipo_via_id,tipo_zona_id,distrito_id,provincia_id,departamento_id," +
					"tipo_prof_ofi_id,tipo_venta_c_id"
				);
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public Proveedor SingleSearch(Proveedor entity)
		{
			try
			{
				List<Proveedor> proveedorList = Search(entity);

				return proveedorList.FirstOrDefault();
			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public int Insert(Proveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("empresaId", entity.EmpresaId);
				parameters.AddParameter("proveedor", entity.Codigo);
				parameters.AddParameter("tipoNaturalezaJuridicaId", entity.TipoNaturalezaJuridicaId);
				parameters.AddParameter("razonSocial", entity.RazonSocial);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("numeroRuc", entity.NumeroRuc);
				parameters.AddParameter("numeroCci", entity.NumeroCCI);
				parameters.AddParameter("contacto", entity.Contacto);
				parameters.AddParameter("tipoViaId", entity.TipoViaId);
				parameters.AddParameter("nombreVia", entity.NombreVia);
				parameters.AddParameter("numeroVia", entity.NumeroVia);
				parameters.AddParameter("departamento", entity.Departamento);
				parameters.AddParameter("interior", entity.Interior);
				parameters.AddParameter("manzana", entity.Manzana);
				parameters.AddParameter("lote", entity.Lote);
				parameters.AddParameter("kilometro", entity.Kilometro);
				parameters.AddParameter("block", entity.Block);
				parameters.AddParameter("etapa", entity.Etapa);
				parameters.AddParameter("tipoZonaId", entity.TipoZonaId);
				parameters.AddParameter("nombreZona", entity.NombreZona);
				parameters.AddParameter("referencia", entity.Referencia);
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("web", entity.Web);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("tipoProveedorId", entity.TipoProveedorId);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("origenRegistroId", entity.OrigenRegistroId);
				parameters.AddParameter("esLocal", entity.EsLocal);
				parameters.AddParameter("esProspecto", entity.EsProspecto);
				parameters.AddParameter("tipoDocumentoIdentidad2Id", entity.TipoDocumentoIdentidad2Id);
				parameters.AddParameter("numeroDocumentoIdentidad2", entity.NumeroDocumentoIdentidad2);
				parameters.AddParameter("cargo", entity.Cargo);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("tipoProfesionOficioId", entity.TipoProfesionOficioId);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("tipoVentaClienteId", entity.TipoVentaClienteId);
				parameters.AddParameter("leadSourceId", entity.LeadSourceId);
				parameters.AddParameter("codigoExterno", entity.CodigoExterno);
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
		public int Update(Proveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("razonSocial", entity.RazonSocial);
				parameters.AddParameter("abreviatura", entity.Abreviatura);
				parameters.AddParameter("apellidoPaterno", entity.ApellidoPaterno);
				parameters.AddParameter("apellidoMaterno", entity.ApellidoMaterno);
				parameters.AddParameter("nombres", entity.Nombres);
				parameters.AddParameter("tipoDocumentoIdentidadId", entity.TipoDocumentoIdentidadId);
				parameters.AddParameter("numeroDocumentoIdentidad", entity.NumeroDocumentoIdentidad);
				parameters.AddParameter("numeroRuc", entity.NumeroRuc);
				parameters.AddParameter("numeroCci", entity.NumeroCCI);
				parameters.AddParameter("contacto", entity.Contacto);
				parameters.AddParameter("tipoViaId", entity.TipoViaId);
				parameters.AddParameter("nombreVia", entity.NombreVia);
				parameters.AddParameter("numeroVia", entity.NumeroVia);
				parameters.AddParameter("departamento", entity.Departamento);
				parameters.AddParameter("interior", entity.Interior);
				parameters.AddParameter("manzana", entity.Manzana);
				parameters.AddParameter("lote", entity.Lote);
				parameters.AddParameter("kilometro", entity.Kilometro);
				parameters.AddParameter("block", entity.Block);
				parameters.AddParameter("etapa", entity.Etapa);
				parameters.AddParameter("tipoZonaId", entity.TipoZonaId);
				parameters.AddParameter("nombreZona", entity.NombreZona);
				parameters.AddParameter("referencia", entity.Referencia);
				parameters.AddParameter("telefono", entity.Telefono);
				parameters.AddParameter("distritoId", entity.DistritoId);
				parameters.AddParameter("email", entity.Email);
				parameters.AddParameter("web", entity.Web);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("condicionPagoId", entity.CondicionPagoId);
				parameters.AddParameter("esLocal", entity.EsLocal);
				parameters.AddParameter("tipoDocumentoIdentidad2Id", entity.TipoDocumentoIdentidad2Id);
				parameters.AddParameter("numeroDocumentoIdentidad2", entity.NumeroDocumentoIdentidad2);
				parameters.AddParameter("cargo", entity.Cargo);
				parameters.AddParameter("fechaNacimiento", entity.FechaNacimiento);
				parameters.AddParameter("tipoProfesionOficioId", entity.TipoProfesionOficioId);
				parameters.AddParameter("empresaSedeId", entity.EmpresaSedeId);
				parameters.AddParameter("tipoVentaClienteId", entity.TipoVentaClienteId);
				parameters.AddParameter("leadSourceId", entity.LeadSourceId);
				parameters.AddParameter("codigoExterno", entity.CodigoExterno);
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
	  public void Delete(Proveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, StoredProcActionConstant.DELETE), parameters);
				
			}
			catch (Exception e)
			{ 
				Logger.Error(e.ToString());
				throw;
			}
	  }
      public void ActualizarUsuario(Proveedor entity)
      {
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("usuarioId", entity.UsuarioId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarUsuario"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
		public void ActualizarTipoProveedor(Proveedor entity)
		{
			Dictionary<string, object> parameters = new();

			try
			{
				parameters.AddParameter("proveedorId", entity.ProveedorId);
				parameters.AddParameter("tipoProveedorId", entity.TipoProveedorId);
				parameters.AddParameter("usuarioAccionId", entity.UsuarioCreacionId);

				DbFactory.ExecCommand(DbCommand.Create(SP_PRE_NAME, "_actualizarTipoProveedor"), parameters);

			}
			catch (Exception e)
			{
				Logger.Error(e.ToString());
				throw;
			}
		}
	}
}
