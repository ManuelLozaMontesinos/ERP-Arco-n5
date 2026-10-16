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
    public class ContactosBusiness : IContactosBusiness
    {
        private readonly ITablaRepository tablaRepository;
        private readonly IContactosRepository contactosRepository;

        public ContactosBusiness(
            IContactosRepository contactosRepository,
            ITablaRepository tablaRepository
            )
        {
            this.contactosRepository = contactosRepository;
            this.tablaRepository = tablaRepository;
        }
        public ContactosDTO GetById(int id)
        {
            try
            {
                Contactos contactos = contactosRepository.GetById(id);

                ContactosDTO contactosDTO = Mapper.Map<ContactosDTO>(contactos);

                return contactosDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ContactosDTO> Search(ContactosDTO dto)
        {
            try
            {
                List<Contactos> contactosList = contactosRepository.Search(Mapper.Map<Contactos>(dto));

                List<ContactosDTO> contactosDTOList = Mapper.Map<List<ContactosDTO>>(contactosList);

                return contactosDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ContactosDTO SingleSearch(ContactosDTO dto)
        {
            try
            {
                Contactos contactos = contactosRepository.SingleSearch(Mapper.Map<Contactos>(dto));

                ContactosDTO contactosDTO = Mapper.Map<ContactosDTO>(contactos);

                return contactosDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(ContactosDTO dto)
        {
            try
            {
                Contactos contactos = Mapper.Map<Contactos>(dto);

                int newId = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    newId = contactosRepository.Insert(contactos);

                    dto.ContactosId = newId;
                    tx.Complete();
                }

                return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(ContactosDTO dto)
        {
            try
            {
                Contactos contactos = Mapper.Map<Contactos>(dto);

                int id = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    id = contactosRepository.Update(contactos);

                    tx.Complete();
                }

                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(ContactosDTO dto)
        {
            try
            {
                Contactos contactos = Mapper.Map<Contactos>(dto);

                using (TransactionScope tx = new TransactionScope())
                {
                    contactosRepository.Delete(new Contactos()
                    {
                       ContactosId = dto.ContactosId,
                       UsuarioCreacionId = contactos.UsuarioCreacionId
                    });


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