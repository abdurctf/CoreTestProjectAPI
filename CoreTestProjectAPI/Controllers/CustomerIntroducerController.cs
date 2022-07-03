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
    public class CustomerIntroducerController : ControllerBase
    {
        private readonly ICustomer_IntroducerBLL _customer_IntroducerBLL;

        public CustomerIntroducerController(ICustomer_IntroducerBLL customer_IntroducerBLL)
        {
            this._customer_IntroducerBLL = customer_IntroducerBLL;
        }
        [HttpPost]
        public IActionResult SaveCustomerIntroducer(string requestString)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            if (requestString == null)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = "Request is null";
                return BadRequest(objResponse);
            }
            APIServiceRequest objRequest = CommonAPIFormat.GetDeserializedRequest(requestString);

            //var arrStr = JObject.Parse(objRequest.BusinessData.ToString());
            //var introducer_Id = (arrStr["INTRODUCER_ID"] != null) ? arrStr["INTRODUCER_ID"].ToString() : "";

            var objcustomerIntroducerDTO = JsonConvert.DeserializeObject<CUSTOMER_INTRODUCER_MAP>(objRequest.BusinessData.ToString());            
            string result = _customer_IntroducerBLL.SaveCustomerIntroducer(objcustomerIntroducerDTO);

            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);


        }
        [HttpGet]
        public IActionResult GetCustomerIntroducer(string requestString)
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
            var introducer_Id = (arrStr["INTRODUCER_ID"] != null) ? arrStr["INTRODUCER_ID"].ToString() : "";


            CUSTOMER_INTRODUCER_MAP result = _customer_IntroducerBLL.GetCustomerIntroducerById(introducer_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);


        }
        [HttpGet]
        public IActionResult GetCustomerIntroducerById(string requestString)
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
            var introducer_Id = (arrStr["INTRODUCER_ID"] != null) ? arrStr["INTRODUCER_ID"].ToString() : "";


            CUSTOMER_INTRODUCER_MAP result = _customer_IntroducerBLL.GetCustomerIntroducerById(introducer_Id);
            if (result != null)
            {
                objResponse = CommonAPIFormat.CreateAPIResponseWithBizData(objRequest, result, "");
                return Ok(objResponse);
            }

            objResponse = CommonAPIFormat.CreateAPIResponseWithErrorMsg(objRequest, "No Data Found");
            return NotFound(objResponse);

        }
    }
}
