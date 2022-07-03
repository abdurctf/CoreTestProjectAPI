using AutoMapper;
using Core.TestProjectBLL.BLLContracts;
using Core.TestProjectModels.Entities;
using Core.TestProjectModels.Models;
using DBAccessor.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.TestProjectBLL.BLLRepositories
{
    public class Customer_IntroducerBLL : ICustomer_IntroducerBLL
    {
        private readonly ICustomer_IntroducerService _customer_IntroducerService;
        private readonly IMapper _mapper;

        public Customer_IntroducerBLL(ICustomer_IntroducerService customer_IntroducerService, IMapper mapper)
        {
            this._customer_IntroducerService = customer_IntroducerService;
            this._mapper = mapper;
        }

        public CUSTOMER_INTRODUCER_MAP GetCustomerIntroducerById(string introducer_Id)
        {
            return _mapper.Map<CUSTOMER_INTRODUCER, CUSTOMER_INTRODUCER_MAP>(_customer_IntroducerService.GetCustomerIntroducerById(introducer_Id));
        }
        public string SaveCustomerIntroducer(CUSTOMER_INTRODUCER_MAP objcustomer_introducer)
        {
            return _customer_IntroducerService.SaveCustomerIntroducer(_mapper.Map<CUSTOMER_INTRODUCER_MAP,CUSTOMER_INTRODUCER>(objcustomer_introducer));
        }
    }
}
