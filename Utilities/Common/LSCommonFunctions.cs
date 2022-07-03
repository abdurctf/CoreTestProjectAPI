using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Utilities.Common
{
    public class LSCommonFunctions
    {
        private readonly IConfiguration _configuration;
        public LSCommonFunctions(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static string GetAnyDateString(string pInputDt, string pInputFormat, string pOutputFormat)
        {
            string vOutputDate = string.Empty;
            try
            {
                DateTime resultmessageId;

                var dateTime = DateTime.TryParseExact(pInputDt, pInputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultmessageId);
                vOutputDate = resultmessageId.ToString(pOutputFormat);
            }
            catch
            {
                return "NULL";
            }
            return vOutputDate;
        }
        public static DateTime GetAnyDateFormat(string pInputDt, string pInputFormat)
        {
            DateTime resultmessageId;
            try
            {
                var dateTime = DateTime.TryParseExact(pInputDt, pInputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultmessageId);

            }
            catch
            {
                throw;
            }
            return resultmessageId;


        }
        public static string SetDbDateTime(DateTime? pDt)
        {
            if (pDt != null)
            {
                DateTime dt = (DateTime)pDt;
                return "to_date('" + dt.ToString("MM-dd-yyyy hh:mm:ss tt") + "', 'MM-DD-RRRR HH:MI:SS AM')";
            }
            return "''";
        }
        public static string GetNewBuid()
        {
            return Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
        }
        public static string ValidateRequiredObject(object obj)
        {
            string result = string.Empty;
            string vValue = string.Empty;
            try
            {

                PropertyInfo[] propertyInfoNew = obj.GetType().GetProperties();
                foreach (PropertyInfo piNew in propertyInfoNew)
                {

                    var isRequired = obj.GetType().GetProperty(piNew.Name).GetCustomAttributes(typeof(RequiredAttribute), false);

                    if (isRequired.Length > 0)
                    {
                        vValue = obj.GetType().GetProperty(piNew.Name).GetValue(obj).ToString();
                        if (string.IsNullOrEmpty(vValue))
                        {
                            if (string.IsNullOrEmpty(result))
                                result = piNew.Name;
                            else
                            {
                                result = result + ", " + piNew.Name;
                            }
                        }
                    }
                }

                return result;
            }
            catch
            {
                return result;
            }
        }
        public static string GetTableRowData(object obj)
        {
            string result = string.Empty;
            string vValue = string.Empty;
            try
            {

                PropertyInfo[] propertyInfoNew = obj.GetType().GetProperties();
                foreach (PropertyInfo piNew in propertyInfoNew)
                {

                    string vColumnName = string.Empty;
                    try
                    {
                        vColumnName = ((ColumnAttribute)((obj.GetType().GetProperty(piNew.Name).GetCustomAttributes(typeof(ColumnAttribute), true))).First()).Name;
                    }
                    catch
                    {
                    }


                    if (!string.IsNullOrEmpty(vColumnName))
                    {
                        string vDataType = string.Empty;
                        if (piNew.PropertyType.Name.Contains("Nullable`1"))
                        {
                            if (piNew.PropertyType.IsGenericType)
                                vDataType = piNew.PropertyType.GenericTypeArguments[0].Name;
                        }
                        else
                        {
                            vDataType = piNew.PropertyType.Name;
                        }

                        try
                        {
                            vValue = obj.GetType().GetProperty(piNew.Name).GetValue(obj).ToString();
                            if (string.IsNullOrEmpty(vValue))
                            {
                                vValue = "null";
                            }
                            else if (vDataType.ToUpper().Contains("DATE"))
                            {
                                vValue = Convert.ToDateTime(vValue).ToString("yyyy-MM-ddTHH:mm:ss");
                            }
                        }
                        catch
                        {
                            vValue = "null";
                        }

                        if (string.IsNullOrEmpty(result))
                        {
                            if (vDataType.ToUpper().Contains("BOOL") || vDataType.ToUpper().Contains("INT") || vDataType.ToUpper().Contains("DOUBLE") || vDataType.ToUpper().Contains("FLOAT") || vDataType.ToUpper().Contains("DECIMAL") || vValue == "null")
                            {
                                result = "\"" + vColumnName + "\"" + ":" + vValue;
                            }
                            else
                            {
                                result = "\"" + vColumnName + "\"" + ":" + "\"" + vValue + "\"";
                            }
                        }
                        else
                        {
                            if (vDataType.ToUpper().Contains("BOOL") || vDataType.ToUpper().Contains("INT") || vDataType.ToUpper().Contains("DOUBLE") || vDataType.ToUpper().Contains("FLOAT") || vDataType.ToUpper().Contains("DECIMAL") || vValue == "null")
                            {
                                result = result + ", " + "\"" + vColumnName + "\"" + ":" + vValue;
                            }
                            else
                            {
                                result = result + ", " + "\"" + vColumnName + "\"" + ":" + "\"" + vValue + "\"";
                            }

                        }
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result = "{" + result + "}";
                }

                return result;
            }
            catch
            {
                return result;
            }
        }
        public string GetApiConfigbyKey(string apikey)
        {
            try
            {
                var result = _configuration.GetValue<string>("add:" + apikey + ":value");//_configuration.GetSection("ApiConfig");//(System.Collections.Specialized.NameValueCollection)System.Web.Configuration.WebConfigurationManager.GetSection("ApiConfig");
                //if (!string.IsNullOrEmpty(apikey))
                //{
                    //string result = ApiConfSection[apikey];
                    if (!string.IsNullOrEmpty(result))
                        return result.ToString();
                    else
                        throw new Exception("Api Link Not Found");
                //}
                //else
                //{
                //    throw new Exception("Invalid or Empty Api Key");
                //}
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Api Config Error: " + ex.GetBaseException().Message);
            }
        }
        public string GetLSAppConfigbyKey(string pKey)
        {
            try
            {
                var LSAppConfig = _configuration.GetValue<string>("add:" + pKey + ":value");
                //if (LSAppConfig != null)
                //{
                    if (!string.IsNullOrEmpty(LSAppConfig))
                    {
                        return LSAppConfig;
                    }
                    else
                    {
                        throw new Exception("Invalid or Empty App Key");
                    }
                //}
                //else
                //{
                //    throw new Exception("Invalid App Config");
                //}
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Api Config Error: " + ex.GetBaseException().Message);
            }
        }
        public static DateTime? GetDate(string pDateValue)
        {
            DateTime? vDateFormate = null;
            try
            {
                vDateFormate = DateTime.Parse(pDateValue);
            }
            catch
            {
                if (vDateFormate == null)
                {
                    return null;
                }
            }
            return vDateFormate;
        }
    }
}
