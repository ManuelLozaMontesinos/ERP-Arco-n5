using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;

namespace ESD.ERP.Business.Impl
{
	public class ProveedorBusiness : IProveedorBusiness
	{
		private readonly IProveedorRepository proveedorRepository;
		private readonly ITablaBusiness tablaBusiness;

		public ProveedorBusiness(
			IProveedorRepository proveedorRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.proveedorRepository = proveedorRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public ProveedorDTO GetById(int id)
		{
			try
			{
				Proveedor proveedor = proveedorRepository.GetById(id);

				ProveedorDTO proveedorDTO = Mapper.Map<ProveedorDTO>(proveedor);

				return proveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorDTO> Search(ProveedorDTO dto)
		{
			try
			{
				List<Proveedor> proveedorList = proveedorRepository.Search(Mapper.Map<Proveedor>(dto));

				List<ProveedorDTO> proveedorDTOList = Mapper.Map<List<ProveedorDTO>>(proveedorList);

				return proveedorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorDTO SingleSearch(ProveedorDTO dto)
		{
			try
			{
				Proveedor proveedor = proveedorRepository.SingleSearch(Mapper.Map<Proveedor>(dto));

				ProveedorDTO proveedorDTO = Mapper.Map<ProveedorDTO>(proveedor);

				return proveedorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	  public int Insert(ProveedorDTO dto)
	  {
			try
			{
            Proveedor proveedor = new()
            {
					EmpresaId = dto.EmpresaId,
               NotProveedorId = dto.ProveedorId,
               TipoDocumentoIdentidadId = dto.TipoDocumentoIdentidadId,
               NumeroDocumentoIdentidad = dto.NumeroDocumentoIdentidad
            };
            proveedor = proveedorRepository.SingleSearch(proveedor);
				if (proveedor != null)
					throw new Exception(SR.GetString("EX_numero_documento_ya_registrado", proveedor.TipoDocumentoIdentidad.Abreviatura, proveedor.NumeroDocumentoIdentidad));

				proveedor = Mapper.Map<Proveedor>(dto);
				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{

					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO() { Tabla = TablaConstante.PROVEEDOR };
					proveedor.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = proveedorRepository.Insert(proveedor);

					dto.ProveedorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
	  }
	  public int Update(ProveedorDTO dto)
	  {
			try
			{
            Proveedor proveedor = new Proveedor
            {
               NotProveedorId = dto.ProveedorId,
               TipoDocumentoIdentidadId = dto.TipoDocumentoIdentidadId,
               NumeroDocumentoIdentidad = dto.NumeroDocumentoIdentidad
            };
            proveedor = proveedorRepository.SingleSearch(proveedor);
				if (proveedor != null)
					throw new Exception(SR.GetString(" ", proveedor.TipoDocumentoIdentidad.Abreviatura, proveedor.NumeroDocumentoIdentidad));

				proveedor = Mapper.Map<Proveedor>(dto);
				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = proveedorRepository.Update(proveedor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
	  }
      public void Delete(ProveedorDTO dto)
      {
            try
            {
                Proveedor proveedor = Mapper.Map<Proveedor>(dto);

                using (TransactionScope tx = new())
                {

                    proveedorRepository.Delete(new Proveedor()
                    {
                        ProveedorId = dto.ProveedorId,
                        UsuarioCreacionId = dto.UsuarioCreacionId,
                    });

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
      }
      public void ActualizarTipoProveedor(ProveedorDTO dto)
      {
			try
			{
				Proveedor proveedor = Mapper.Map<Proveedor>(dto);

				using (TransactionScope tx = new())
				{
					proveedorRepository.ActualizarTipoProveedor(proveedor);

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
