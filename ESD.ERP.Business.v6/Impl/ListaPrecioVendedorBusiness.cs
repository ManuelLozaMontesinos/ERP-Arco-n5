using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;
using System;
using System.Collections.Generic;
using System.Transactions;

// ListaPrecioVendedorBusiness

namespace ESD.ERP.Business.Impl
{
    public class ListaPrecioVendedorBusiness : IListaPrecioVendedorBusiness
    {
        private readonly IListaPrecioVendedorRepository listaPrecioVendedorRepository;
        private readonly IListaPrecioBusiness listaPrecioBusiness;

        public ListaPrecioVendedorBusiness(
            IListaPrecioVendedorRepository listaPrecioVendedorRepository,
            IListaPrecioBusiness listaPrecioBusiness
            )
        {
            this.listaPrecioVendedorRepository = listaPrecioVendedorRepository;
            this.listaPrecioBusiness = listaPrecioBusiness;
        }
        public ListaPrecioVendedorDTO GetById(int id)
        {
            try
            {
                ListaPrecioVendedor ListaPrecioVendedor = listaPrecioVendedorRepository.GetById(id);

                ListaPrecioVendedorDTO ListaPrecioVendedorDTO = Mapper.Map<ListaPrecioVendedorDTO>(ListaPrecioVendedor);

                return ListaPrecioVendedorDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ListaPrecioVendedorDTO> Search(ListaPrecioVendedorDTO dto)
        {
            try
            {
                List<ListaPrecioVendedor> listaPrecioProveedorProductoList = listaPrecioVendedorRepository.Search(Mapper.Map<ListaPrecioVendedor>(dto));

                List<ListaPrecioVendedorDTO> listaPrecioProveedorProductoDTOList = Mapper.Map<List<ListaPrecioVendedorDTO>>(listaPrecioProveedorProductoList);

                return listaPrecioProveedorProductoDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ListaPrecioVendedorDTO SingleSearch(ListaPrecioVendedorDTO dto)
        {
            try
            {
                ListaPrecioVendedor ListaPrecioVendedor = listaPrecioVendedorRepository.SingleSearch(Mapper.Map<ListaPrecioVendedor>(dto));

                ListaPrecioVendedorDTO ListaPrecioVendedorDTO = Mapper.Map<ListaPrecioVendedorDTO>(ListaPrecioVendedor);

                return ListaPrecioVendedorDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(ListaPrecioVendedorDTO dto)
        {
            try
            {
                ListaPrecioVendedor ListaPrecioVendedor = Mapper.Map<ListaPrecioVendedor>(dto);

                int newId = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    newId = listaPrecioVendedorRepository.Insert(ListaPrecioVendedor);

                    dto.ListaPrecioVendedorId = newId;
                    tx.Complete();
                }

                return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(ListaPrecioVendedorDTO dto)
        {
            try
            {
                ListaPrecioVendedor ListaPrecioVendedor = Mapper.Map<ListaPrecioVendedor>(dto);

                int id = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    id = listaPrecioVendedorRepository.Update(ListaPrecioVendedor);

                    tx.Complete();
                }

                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(ListaPrecioVendedorDTO dto)
        {
            try
            {
                ListaPrecioVendedor ListaPrecioVendedor = Mapper.Map<ListaPrecioVendedor>(dto);

                using (TransactionScope tx = new TransactionScope())
                {
                    listaPrecioVendedorRepository.Delete(ListaPrecioVendedor);

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void ActualizarListaPrecioVendedorId(ListaPrecioVendedorDTO dto)
        {
            try
            {
                ListaPrecioVendedor listaPrecioVendedor = Mapper.Map<ListaPrecioVendedor>(dto);

                using (TransactionScope tx = new TransactionScope())
                {
                    listaPrecioVendedorRepository.ActualizarListaPrecioVendedorId(listaPrecioVendedor);

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
