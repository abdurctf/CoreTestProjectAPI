using AutoMapper;
using Core.TestProjectBLL.BLLContracts;
using Core.TestProjectModels.Entities;
using Core.TestProjectModels.Models;
using DBAccessor.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.CommonState;
using Utilities.Models;

namespace Core.TestProjectBLL.BLLRepositories
{
    public class Customer_AddressBLL : ICustomer_AddressBLL
    {
        private readonly ICustomer_AddressService _customer_AddressService;
        private readonly IMapper _mapper;

        public Customer_AddressBLL(ICustomer_AddressService customer_AddressService, IMapper mapper)
        {
            this._customer_AddressService = customer_AddressService;
            this._mapper = mapper;
        }        
        public string SaveCustomerAddress(CUSTOMER_ADDRESS_MAP objcustomerAddressDTO)
        {
            return _customer_AddressService.SaveCustomerAddress(_mapper.Map<CUSTOMER_ADDRESS_MAP, CUSTOMER_ADDRESS>(objcustomerAddressDTO));
        }

        public CUSTOMER_ADDRESS_MAP GetCustomerAddressById(string address_Id)
        {
            return _mapper.Map<CUSTOMER_ADDRESS, CUSTOMER_ADDRESS_MAP>(_customer_AddressService.GetCustomerAddressById(address_Id));
        }

        public List<LSListItem> GetCountryDDL()
        {
            return _customer_AddressService.GetCountryDDL();
        }
        public List<LSListItem> GetDivisionDDL(string countryId)
        {
            return _customer_AddressService.GetDivisionDDL(countryId);
        }
        public List<LSListItem> GetDistrictDDL(string divisionId)
        {
            return _customer_AddressService.GetDistrictDDL(divisionId);
        }
        public List<LSListItem> GetThanaDDL(string districtId)
        {
            return _customer_AddressService.GetThanaDDL(districtId);
        }

    }
}
