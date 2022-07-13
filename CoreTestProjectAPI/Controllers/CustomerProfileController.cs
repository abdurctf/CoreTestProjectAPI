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
using Utilities.Models;

namespace CoreTestProjectAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CustomerProfileController : ControllerBase
    {
        private readonly ICustomer_ProfileBLL _customer_ProfileBLL;

        public CustomerProfileController(ICustomer_ProfileBLL customer_ProfileBLL)
        {
            this._customer_ProfileBLL = customer_ProfileBLL;
        }
        [HttpPost]
        public IActionResult SaveCustomerProfile(APIServiceRequest objRequest)
        {


            APIServiceResponse objResponse = new APIServiceResponse();
            if (objRequest == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }

            try
            {
                string vMsg = string.Empty;

                AuthParam authParam = new AuthParam();
                authParam.BranchId = objRequest.BranchId;
                authParam.UserId = objRequest.UserId;
                authParam.FunctionId = objRequest.FunctionId;


                CUSTOMER_PROFILE_MAP objcustomerDTO = JsonConvert.DeserializeObject<CUSTOMER_PROFILE_MAP>(objRequest.BusinessData.ToString());


                CUSTOMER_PROFILE_MAP objCUSTOMER_PROFILE_MAP = _customer_ProfileBLL.SaveCustomerProfile(objcustomerDTO, authParam);


                //if (objcustomerDTO.isAdd)
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Saved Successfully");
                //    return Ok(objResponse);
                //}
                //else if (objcustomerDTO.isOld && !objcustomerDTO.isDelete)
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Updated Successfully");
                //    return Ok(objResponse);
                //}
                //else if (objcustomerDTO.isDelete )
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Deleted Successfully");
                //    return Ok(objResponse);
                //}

                if (objCUSTOMER_PROFILE_MAP.IsRequestSuccess)
                {

                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objCUSTOMER_PROFILE_MAP);
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, objCUSTOMER_PROFILE_MAP.Error_Msg + objCUSTOMER_PROFILE_MAP.Error_Code);
                return BadRequest(objResponse);


                //if (result == "Saved")
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Saved Successfully");
                //    return Ok(objResponse);
                //}
                //else if (result == "Updated")
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Updated Successfully");
                //    return Ok(objResponse);
                //}
                //else if (result == "Deleted")
                //{
                //    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Deleted Successfully");
                //    return Ok(objResponse);
                //}

                //objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, result);
                //return BadRequest(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.GetBaseException().Message);
                return BadRequest(objResponse);
            }

        }
        [HttpGet]
        public IActionResult GetCustomerProfile(string requestString)
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
            var Customer_Id = (arrStr["customer_id"] != null) ? arrStr["customer_id"].ToString() : "";


            CUSTOMER_ADDRESS_MAP result = null; //_customer_ProfileBLL.GetCustomerById(Customer_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }
        [HttpGet]
        public IActionResult GetCustomerProfileById(string requestString)
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
            var Customer_Id = arrStr["customer_id"] != null ? arrStr["customer_id"].ToString() : "";


            CUSTOMER_PROFILE_MAP result = _customer_ProfileBLL.GetCustomerProfileById(Customer_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }

        [HttpGet]
        public IActionResult GetCustomerProfileDetailsById(string requestString)
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
            var Customer_Id = arrStr["customer_id"] != null ? arrStr["customer_id"].ToString() : "";


            CUSTOMER_PROFILE_MAP result = _customer_ProfileBLL.GetCustomerProfileDetailsById(Customer_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }

        [HttpPost]
        public IActionResult SaveCustomerProfileNFT(APIServiceRequest objRequest)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (objRequest == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }

            try
            {
                string vMsg = string.Empty;

                AuthParam authParam = new AuthParam();
                authParam.BranchId = objRequest.BranchId;
                authParam.UserId = objRequest.UserId;
                authParam.FunctionId = objRequest.FunctionId;


                var objcustomerDTO = JsonConvert.DeserializeObject<CUSTOMER_PROFILE_MAP>(objRequest.BusinessData.ToString());
                string result = _customer_ProfileBLL.SaveCustomerProfileNFT(objcustomerDTO, authParam);
                if (result == "Saved")
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Saved Successfully");
                    return Ok(objResponse);
                }
                else if (result == "Updated")
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Updated Successfully");
                    return Ok(objResponse);
                }
                else if (result == "Deleted")
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Deleted Successfully");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, result);
                return BadRequest(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.GetBaseException().Message);
                return BadRequest(objResponse);
            }

        }

        [HttpPost]
        public IActionResult SaveCustomerProfileWithDetails(APIServiceRequest objRequest)
        {


            APIServiceResponse objResponse = new APIServiceResponse();
            if (objRequest == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }

            try
            {
                string vMsg = string.Empty;

                AuthParam authParam = new AuthParam();
                authParam.BranchId = objRequest.BranchId;
                authParam.UserId = objRequest.UserId;
                authParam.FunctionId = objRequest.FunctionId;


                var objcustomerDTO = JsonConvert.DeserializeObject<CUSTOMER_PROFILE_MAP>(objRequest.BusinessData.ToString());
                string result = _customer_ProfileBLL.SaveCustomerProfileWithDetails(objcustomerDTO);
                if (result == "Saved")
                  {
                      objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Saved Successfully");
                    return Ok(objResponse);
                }
                else if (result == "Updated")
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Updated Successfully");
                    return Ok(objResponse);
                }
                else if (result == "Deleted")
                {
                    objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, objcustomerDTO, "Data Deleted Successfully");
                    return Ok(objResponse);
                }

                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, result);
                return BadRequest(objResponse);
            }
            catch (Exception ex)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, ex.GetBaseException().Message);
                return BadRequest(objResponse);
            }

           }
    }
}
