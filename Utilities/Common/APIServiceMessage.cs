using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Utilities.Common
{
    public class APIServiceRequest
    {
        #region Properties Declared

        private string msgRequestId;
        public string RequestId
        {
            get { return msgRequestId; }
            set { msgRequestId = value; }
        }
        private string msgRequestClientIP;
        public string RequestCliedIP
        {
            get { return msgRequestClientIP; }
            set { msgRequestClientIP = value; }
        }
        private string msgRequestClientAgent;
        public string RequestCliedAgent
        {
            get { return msgRequestClientAgent; }
            set { msgRequestClientAgent = value; }
        }
        private string msgRequestAppIP;
        public string RequestAppIP
        {
            get { return msgRequestAppIP; }
            set { msgRequestAppIP = value; }
        }
        private string msgRequestAppBaseUrl;
        public string RequestAppBaseUrl
        {
            get { return msgRequestAppBaseUrl; }
            set { msgRequestAppBaseUrl = value; }
        }
        private dynamic msgBusinessData;
        public dynamic BusinessData
        {
            get { return msgBusinessData; }
            set { msgBusinessData = value; }
        }
        private string msgFunctionId;
        public string FunctionId
        {
            get { return msgFunctionId; }
            set { msgFunctionId = value; }
        }
        private string msgBranchId;
        public string BranchId
        {
            get { return msgBranchId; }
            set { msgBranchId = value; }
        }
        private string msgUserId;
        public string UserId
        {
            get { return msgUserId; }
            set { msgUserId = value; }
        }
        private string msgInstitueId;
        public string InstitueId
        {
            get { return msgInstitueId; }
            set { msgInstitueId = value; }
        }
        private string msgSessionId;
        public string SessionId
        {
            get { return msgSessionId; }
            set { msgSessionId = value; }
        }
        private string msgRequestDateTime;
        public string RequestDateTime
        {
            get { return msgRequestDateTime; }
            set { msgRequestDateTime = value; }
        }
        private int msgSessionTimeout;
        public int SessionTimeout
        {
            get { return msgSessionTimeout; }
            set { msgSessionTimeout = value; }
        }

        #endregion
    }
    public class APIServiceResponse
    {
        #region Properties Declared

        private string msgResponseId;
        public string ResponseId
        {
            get { return msgResponseId; }
            set { msgResponseId = value; }
        }
        private string msgResponseDateTime;
        public string ResponseDateTime
        {
            get { return msgResponseDateTime; }
            set { msgResponseDateTime = value; }
        }
        private bool msgResponseStatus;
        public bool ResponseStatus
        {
            get { return msgResponseStatus; }
            set { msgResponseStatus = value; }
        }
        private string msgResponseMessage;
        public string ResponseMessage
        {
            get { return msgResponseMessage; }
            set { msgResponseMessage = value; }
        }
        private string msgRequestId;
        public string RequestId
        {
            get { return msgRequestId; }
            set { msgRequestId = value; }
        }
        private string msgResponseBusinessData;
        public string ResponseBusinessData
        {
            get { return msgResponseBusinessData; }
            set { msgResponseBusinessData = value; }
        }
        private string msgFunctionId;
        public string FunctionId
        {
            get { return msgFunctionId; }
            set { msgFunctionId = value; }
        }
        private string msgBranchId;
        public string BranchId
        {
            get { return msgBranchId; }
            set { msgBranchId = value; }
        }
        private string msgUserId;
        public string UserId
        {
            get { return msgUserId; }
            set { msgUserId = value; }
        }
        private string msgInstitueId;
        public string InstitueId
        {
            get { return msgInstitueId; }
            set { msgInstitueId = value; }
        }
        private string msgSessionId;
        public string SessionId
        {
            get { return msgSessionId; }
            set { msgSessionId = value; }
        }
        private string msgRequestDateTime;
        public string RequestDateTime
        {
            get { return msgRequestDateTime; }
            set { msgRequestDateTime = value; }
        }

        #endregion
    }

    public enum contentType
    {
        [Description("application/json")]
        json,
        [Description("application/xml")]
        xml
    }

    public static class EnumStringValue
    {
        public static string ToEnumStringValue(this contentType val)
        {
            var field = val.GetType().GetField(val.ToString());
            var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length > 0)
            {
                return (customAttributes[0] as DescriptionAttribute).Description;
            }
            else
            {
                return val.ToString();
            }
        }
    }
    public class APIRequest
    {
        public static APIServiceResponse MakeRequest(string requestUrl, string uriData, string bodyData, string contentType, HttpMethod httpMethod)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            try
            {
                string vFullUrl = requestUrl + uriData;
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(4);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                    if (httpMethod == HttpMethod.Get)
                    {
                        var task = client.GetAsync(vFullUrl).ContinueWith((taskwithresponse) =>
                        {
                            var response = taskwithresponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            objResponse = JsonConvert.DeserializeObject<APIServiceResponse>(jsonString.Result);
                        });
                        task.Wait();
                    }
                    else if (httpMethod == HttpMethod.Post)
                    {
                        HttpContent contentPost = new StringContent(bodyData, Encoding.UTF8, contentType);

                        var task = client.PostAsync(vFullUrl, contentPost).ContinueWith((taskwithresponse) =>
                        {
                            var response = taskwithresponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            objResponse = JsonConvert.DeserializeObject<APIServiceResponse>(jsonString.Result);
                        });
                        task.Wait();
                    }
                    return objResponse;
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = (ex.InnerException != null) ? ex.Message + " :: " + ex.InnerException.Message : ex.Message;
                return objResponse;
            }
        }
        public static APIServiceResponse MakeRequest(string requestUrl, string uriData, string bodyData, string contentType, HttpMethod httpMethod, string headerToken)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            try
            {
                string vFullUrl = requestUrl + uriData;
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(4);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headerToken);
                    if (httpMethod == HttpMethod.Get)
                    {
                        var task = client.GetAsync(vFullUrl).ContinueWith((taskwithresponse) =>
                        {
                            var response = taskwithresponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            objResponse = JsonConvert.DeserializeObject<APIServiceResponse>(jsonString.Result);
                        });
                        task.Wait();
                    }
                    else if (httpMethod == HttpMethod.Post)
                    {
                        HttpContent contentPost = new StringContent(bodyData, Encoding.UTF8, contentType);
                        var task = client.PostAsync(vFullUrl, contentPost).ContinueWith((taskwithresponse) =>
                        {
                            var response = taskwithresponse.Result;
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            objResponse = JsonConvert.DeserializeObject<APIServiceResponse>(jsonString.Result);

                        });
                        task.Wait();
                    }
                    return objResponse;
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseStatus = false;
                objResponse.ResponseMessage = (ex.InnerException != null) ? ex.Message + " :: " + ex.InnerException.Message : ex.Message;
                return objResponse;
            }
        }
    }

    public class CommonAPIFormat
    {
        public static HttpResponseMessage CreateResponse(APIServiceResponse objResponse)
        {
            var vRes = JsonConvert.SerializeObject(objResponse);
            var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(vRes, Encoding.UTF8, "application/json")
            };
            return vResponse;
        }
        public static HttpResponseMessage CreateResponseWithDynamic(dynamic objResponse)
        {
            var vRes = JsonConvert.SerializeObject(objResponse);
            var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(vRes, Encoding.UTF8, "application/json")
            };
            return vResponse;
        }
        public static HttpResponseMessage CreateResponseWithString(string objResponse)
        {
            var vRes = JsonConvert.SerializeObject(objResponse);
            var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(vRes, Encoding.UTF8, "application/json")
            };
            return vResponse;
        }
        public static HttpResponseMessage CreateResponseWithException(APIServiceResponse objResponse, Exception ex)
        {
            objResponse.ResponseStatus = false;
            objResponse.ResponseMessage = ex.InnerException != null ? ex.GetBaseException().Message : ex.Message;
            var vRes = JsonConvert.SerializeObject(objResponse);
            var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(vRes, Encoding.UTF8, "application/json")
            };
            return vResponse;
        }
        public static APIServiceResponse CreateAPIResponseWithBizData(APIServiceRequest request, dynamic ResponseBizData)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = true;
            objResponse.UserId = request.UserId;
            objResponse.BranchId = request.BranchId;
            objResponse.ResponseDateTime = DateTime.Now.ToString();
            objResponse.RequestId = request.RequestId;
            objResponse.ResponseBusinessData = JsonConvert.SerializeObject(ResponseBizData);
            
            return objResponse;
        }
        [Obsolete(message:"this is obsolete")]
        public static APIServiceResponse CreateAPIResponseWithBizData(APIServiceRequest request, dynamic ResponseBizData, string message)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = true;
            objResponse.UserId = request.UserId;
            objResponse.BranchId = request.BranchId;
            objResponse.ResponseDateTime = DateTime.Now.ToString();
            objResponse.RequestId = request.RequestId;
            objResponse.ResponseBusinessData = JsonConvert.SerializeObject(ResponseBizData);
            objResponse.ResponseMessage = message;
            return objResponse;
        }
        public static APIServiceResponse CreateAPIResponseWithOnlyMsg(APIServiceRequest request, string message)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = true;
            objResponse.UserId = request.UserId;
            objResponse.BranchId = request.BranchId;
            objResponse.ResponseDateTime = DateTime.Now.ToString();
            objResponse.RequestId = request.RequestId;
            objResponse.ResponseMessage = message;
            return objResponse;
        }
        public static APIServiceResponse CreateAPIResponseWithErrorMsg(APIServiceRequest request, string message)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = false;
            objResponse.UserId = request.UserId;
            objResponse.BranchId = request.BranchId;
            objResponse.ResponseDateTime = DateTime.Now.ToString();
            objResponse.RequestId = request.RequestId;
            objResponse.ResponseMessage = message;
            return objResponse;
        }
        public static APIServiceResponse CreateAPIResponseWithErrorMsgAndObject(APIServiceRequest request, dynamic ResponseBizData, string message)
        {
            APIServiceResponse objResponse = new APIServiceResponse();
            objResponse.ResponseStatus = false;
            objResponse.UserId = request.UserId;
            objResponse.BranchId = request.BranchId;
            objResponse.ResponseDateTime = DateTime.Now.ToString();
            objResponse.RequestId = request.RequestId;
            objResponse.ResponseMessage = message;
            objResponse.ResponseBusinessData = JsonConvert.SerializeObject(ResponseBizData);
            return objResponse;
        }

        public static APIServiceRequest GetDeserializedRequest(string requestString)
        {
            var requestStringDecoded = (requestString != null) ? System.Convert.FromBase64String(requestString) : null;
            APIServiceRequest request = (requestStringDecoded != null) ? JsonConvert.DeserializeObject<APIServiceRequest>(Encoding.UTF8.GetString(requestStringDecoded)) : new APIServiceRequest();
            return request;
        }
        public static string GetSerializedRequest(APIServiceRequest request)
        {
            string req = JsonConvert.SerializeObject(request);
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(req);
            string toReturn = System.Convert.ToBase64String(bytes);
            return toReturn;
        }
        //public static HttpResponseMessage CreateResponseExceptionWithDynamic(dynamic objResponse, Exception ex)
        //{
        //    var vRes = JsonConvert.SerializeObject(objResponse);
        //    var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent(vRes, Encoding.UTF8, "application/json")
        //    };
        //    return vResponse;
        //}
        //public static HttpResponseMessage CreateResponseExceptionWithString(string objResponse, Exception ex)
        //{
        //    var vRes = JsonConvert.SerializeObject(objResponse);
        //    var vResponse = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent(vRes, Encoding.UTF8, "application/json")
        //    };
        //    return vResponse;
        //}
    }
}
