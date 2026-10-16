using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PersonalBusiness : IPersonalBusiness
	{
		private readonly IPersonalRepository personalRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IProveedorRepository proveedorRepository;
		private readonly IPersonalPeriodoLaboralRepository personalPeriodoLaboralRepository;

		public PersonalBusiness(
			IPersonalRepository personalRepository,
			ITablaBusiness tablaBusiness,
			IProveedorRepository proveedorRepository,
			IPersonalPeriodoLaboralRepository personalPeriodoLaboralRepository
			)
		{
			this.personalRepository = personalRepository;
			this.tablaBusiness = tablaBusiness;
			this.proveedorRepository = proveedorRepository;
			this.personalPeriodoLaboralRepository = personalPeriodoLaboralRepository;
		}
		public PersonalDTO GetById(int id)
		{
			try
			{
				Personal personal = personalRepository.GetById(id);

				PersonalDTO personalDTO = Mapper.Map<PersonalDTO>(personal);

				return personalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PersonalDTO> Search(PersonalDTO dto)
		{
			try
			{
				List<Personal> personalList = personalRepository.Search(Mapper.Map<Personal>(dto));

				List<PersonalDTO> personalDTOList = Mapper.Map<List<PersonalDTO>>(personalList);

				return personalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PersonalDTO SingleSearch(PersonalDTO dto)
		{
			try
			{
				Personal personal = personalRepository.SingleSearch(Mapper.Map<Personal>(dto));

				PersonalDTO personalDTO = Mapper.Map<PersonalDTO>(personal);

				return personalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PersonalDTO dto)
		{
			try
			{
				Proveedor proveedor = new()
            {
					EmpresaId = dto.EmpresaSedeDTO.EmpresaId,
					NotProveedorId = dto.ProveedorId,
					TipoDocumentoIdentidadId = dto.ProveedorDTO.TipoDocumentoIdentidadId,
					NumeroDocumentoIdentidad = dto.ProveedorDTO.NumeroDocumentoIdentidad
				};
				proveedor = proveedorRepository.SingleSearch(proveedor);
				if (proveedor != null)
					throw new Exception(SR.GetString("EX_numero_documento_ya_registrado", proveedor.TipoDocumentoIdentidad.Abreviatura, proveedor.NumeroDocumentoIdentidad));

				Personal personal = Mapper.Map<Personal>(dto);

				int newId = 0;

				using (TransactionScope tx = new())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = null;
					if (personal.Proveedor.EntityId == 0) {
						tablaCorrelativoDTO = new TablaCorrelativoDTO() { Tabla = TablaConstante.PROVEEDOR };
						personal.Proveedor.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

						personal.ProveedorId = proveedorRepository.Insert(personal.Proveedor);
					}else
						proveedorRepository.Update(personal.Proveedor);

					tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.PERSONAL,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO(){
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = personal.EmpresaSede.EmpresaId }
						},
						UsuarioAccionId = personal.UsuarioCreacionId
					};

					personal.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = personalRepository.Insert(personal);

					personalPeriodoLaboralRepository.Insert(new PersonalPeriodoLaboral(){
						PersonalId = newId,
						FechaIngreso = personal.FechaIngreso,
						FechaAlta = personal.FechaAlta,
						UsuarioCreacionId = personal.UsuarioCreacionId
					});

					dto.PersonalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PersonalDTO dto)
		{
			try
			{
				Proveedor proveedor = new()
            {
					NotProveedorId = dto.ProveedorId,
					TipoDocumentoIdentidadId = dto.ProveedorDTO.TipoDocumentoIdentidadId,
					NumeroDocumentoIdentidad = dto.ProveedorDTO.NumeroDocumentoIdentidad
				};
				proveedor = proveedorRepository.SingleSearch(proveedor);
				if (proveedor != null)
					throw new Exception(SR.GetString("EX_numero_documento_ya_registrado", proveedor.TipoDocumentoIdentidad.Abreviatura, proveedor.NumeroDocumentoIdentidad));

				Personal personal = Mapper.Map<Personal>(dto);

				int id = 0;

				using (TransactionScope tx = new())
				{
					proveedorRepository.Update(personal.Proveedor);
					id = personalRepository.Update(personal);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PersonalDTO dto)
		{
			try
			{
				Personal personal = Mapper.Map<Personal>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					personalRepository.Delete(personal);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
