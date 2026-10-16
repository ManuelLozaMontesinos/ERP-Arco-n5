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
    public class PuestoBusiness : IPuestoBusiness
    {
        private readonly ITablaRepository tablaRepository;
        private readonly IPuestoRepository puestoRepository;

        public PuestoBusiness(
            IPuestoRepository puestoRepository,
            ITablaRepository tablaRepository
            )
        {
            this.puestoRepository = puestoRepository;
            this.tablaRepository = tablaRepository;
        }
        public PuestoDTO GetById(int id)
        {
            try
            {
                Puesto puesto = puestoRepository.GetById(id);

                PuestoDTO puestoDTO = Mapper.Map<PuestoDTO>(puesto);

                return puestoDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<PuestoDTO> Search(PuestoDTO dto)
        {
            try
            {
                List<Puesto> puestoList = puestoRepository.Search(Mapper.Map<Puesto>(dto));

                List<PuestoDTO> puestoDTOList = Mapper.Map<List<PuestoDTO>>(puestoList);

                return puestoDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PuestoDTO SingleSearch(PuestoDTO dto)
        {
            try
            {
                Puesto puesto = puestoRepository.SingleSearch(Mapper.Map<Puesto>(dto));

                PuestoDTO puestoDTO = Mapper.Map<PuestoDTO>(puesto);

                return puestoDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(PuestoDTO dto)
        {
            try
            {
                Puesto puesto = Mapper.Map<Puesto>(dto);

                int newId = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    dto.PuestoId = newId;
                    tx.Complete();
                }

                return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(PuestoDTO dto)
        {
            try
            {
                Puesto puesto = Mapper.Map<Puesto>(dto);

                int id = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    id = puestoRepository.Update(puesto);

                    tx.Complete();
                }

                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(PuestoDTO dto)
        {
            try
            {
                Puesto puesto = Mapper.Map<Puesto>(dto);

                using (TransactionScope tx = new TransactionScope())
                {
                    puestoRepository.Delete(new Puesto()
                    {
                        PuestoId = puesto.PuestoId,
                        UsuarioCreacionId = puesto.UsuarioCreacionId
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