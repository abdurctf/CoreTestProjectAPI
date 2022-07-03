using Core.TestProjectBLL.BLLContracts;
using Core.TestProjectModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Common;

namespace CoreTestProjectAPI.Controllers
{
  
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomer_AddressBLL _customer_AddressBLL;

        public CustomerAddressController(ICustomer_AddressBLL customer_AddressBLL)
        {
            this._customer_AddressBLL = customer_AddressBLL;
        }

        [HttpPost]
        public IActionResult SaveCustomerAddress(string requestString)
        {

            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);
            var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
            //var address_Id = (arrStr["ADDRESS_ID"] != null) ? arrStr["ADDRESS_ID"].ToString() : "";
            //var addr_type_id = (arrStr["addr_type_id"] != null) ? arrStr["addr_type_id"].ToString() : "";
            //var address = (arrStr["address"] != null) ? arrStr["address"].ToString() : "";
            //var city = (arrStr["city"] != null) ? arrStr["city"].ToString() : "";
            //var zip_code = (arrStr["zip_code"] != null) ? arrStr["zip_code"].ToString() : "";
            //var country_id = (arrStr["country_id"] != null) ? arrStr["country_id"].ToString() : "";
            //var district_id = (arrStr["district_id"] != null) ? arrStr["district_id"].ToString() : "";
            //var thana_id = (arrStr["thana_id"] != null) ? arrStr["thana_id"].ToString() : "";
            //var phone = (arrStr["phone"] != null) ? arrStr["phone"].ToString() : "";
            //var mobile = (arrStr["mobile"] != null) ? arrStr["mobile"].ToString() : "";
            //var email = (arrStr["email"] != null) ? arrStr["email"].ToString() : "";


            CUSTOMER_ADDRESS_MAP objcustomerAddressDTO = JsonConvert.DeserializeObject<CUSTOMER_ADDRESS_MAP>(objRequest.BusinessData.ToString());

            string result = _customer_AddressBLL.SaveCustomerAddress(objcustomerAddressDTO);

           // CUSTOMER_ADDRESS_MAP result = _customer_AddressBLL.GetCustomerAddressById(address_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }
        [HttpGet]
        public IActionResult GetCustomerAddress(string requestString)
        {

            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);

            var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
            var address_Id = (arrStr["ADDRESS_ID"] != null) ? arrStr["ADDRESS_ID"].ToString() : "";


            CUSTOMER_ADDRESS_MAP result = _customer_AddressBLL.GetCustomerAddressById(address_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }
        [HttpGet]
        public IActionResult GetCustomerAddressById(string requestString)
        {


            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);

            var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
            var address_Id = (arrStr["ADDRESS_ID"] != null) ? arrStr["ADDRESS_ID"].ToString() : "";


            CUSTOMER_ADDRESS_MAP result = _customer_AddressBLL.GetCustomerAddressById(address_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }
        [HttpGet]
        public IActionResult GetCountryDDL(string requestString)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);
            try
            {
                var resultList = _customer_AddressBLL.GetCountryDDL();



                if (resultList.Count > 0)
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, resultList, "");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
                return NotFound(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.Message);
                return BadRequest(objResponse);
            }
        }

        [HttpGet]
        public IActionResult GetDivisionDDL(string requestString)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);
            try
            {
                var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
                var countryId = (arrStr["country_id"] != null) ? arrStr["country_id"].ToString() : "";

                var resultList = _customer_AddressBLL.GetDivisionDDL(countryId);
                if (resultList.Count > 0)
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, resultList, "");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
                return NotFound(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.Message);
                return BadRequest(objResponse);
            }
        }

        [HttpGet]
        public IActionResult GetDistrictDDL(string requestString)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);
            try
            {
                var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
                var divisionId = (arrStr["division_id"] != null) ? arrStr["division_id"].ToString() : "";

                var resultList = _customer_AddressBLL.GetDistrictDDL(divisionId);
                if (resultList.Count > 0)
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, resultList, "");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
                return NotFound(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.Message);
                return BadRequest(objResponse);
            }
        }
        [HttpGet]
        public IActionResult GetThanaDDL(string requestString)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);
            try
            {
                var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
                var districtId = (arrStr["district_id"] != null) ? arrStr["district_id"].ToString() : "";

                var resultList = _customer_AddressBLL.GetThanaDDL(districtId);
                if (resultList.Count > 0)
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, resultList, "");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
                return NotFound(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.Message);
                return BadRequest(objResponse);
            }
        }

    }
}
