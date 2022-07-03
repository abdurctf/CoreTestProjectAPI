using Core.TestProjectModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using DBAccessor.Contracts;
using Core.TestProjectBLL.BLLContracts;
using AutoMapper;
using Core.TestProjectModels.Entities;
using Utilities.Models;

namespace Core.TestProjectBLL.BLLRepositories
{
    public class Customer_ProfileBLL : ICustomer_ProfileBLL
    {
        private readonly ICustomer_ProfileService _customer_ProfileService;
        private readonly IMapper _mapper;

        public Customer_ProfileBLL(ICustomer_ProfileService customer_ProfileService, IMapper mapper)
        {
            this._customer_ProfileService = customer_ProfileService;
            this._mapper = mapper;
        }
        public CUSTOMER_PROFILE_MAP SaveCustomerProfile(CUSTOMER_PROFILE_MAP objcustomerDTO, AuthParam authParam)
        {
            //CUSTOMER_PROFILE objModel = _mapper.Map<CUSTOMER_PROFILE_MAP, CUSTOMER_PROFILE>(objcustomerDTO);            
            //return  _customer_ProfileService.SaveCustomerProfile(objModel, authParam);

            return _mapper.Map< CUSTOMER_PROFILE, CUSTOMER_PROFILE_MAP>(_customer_ProfileService.SaveCustomerProfile(_mapper.Map<CUSTOMER_PROFILE_MAP, CUSTOMER_PROFILE>(objcustomerDTO), authParam));
        }

        public CUSTOMER_PROFILE_MAP GetCustomerProfileById(string Customer_Id)
        {
            return _mapper.Map<CUSTOMER_PROFILE, CUSTOMER_PROFILE_MAP>(_customer_ProfileService.GetCustomerProfileById(Customer_Id));
        }
        public CUSTOMER_PROFILE_MAP GetCustomerProfileDetailsById(string Customer_Id)
        {
            return _mapper.Map<CUSTOMER_PROFILE, CUSTOMER_PROFILE_MAP>(_customer_ProfileService.GetCustomerProfileDetailsById(Customer_Id));
        }


        public string SaveCustomerProfileWithDetails(CUSTOMER_PROFILE_MAP objcustomerDTO)
        {
            return _customer_ProfileService.SaveCustomerProfileWithDetails(_mapper.Map<CUSTOMER_PROFILE_MAP, CUSTOMER_PROFILE>(objcustomerDTO));
        }

        public string SaveCustomerProfileNFT(CUSTOMER_PROFILE_MAP objcustomerDTO, AuthParam authParam)
        {
            return _customer_ProfileService.SaveCustomerProfileNFT(_mapper.Map<CUSTOMER_PROFILE_MAP, CUSTOMER_PROFILE>(objcustomerDTO), authParam);
        }

    }
}
