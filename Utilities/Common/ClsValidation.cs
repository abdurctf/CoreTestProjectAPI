using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;

namespace Utilities.Common
{
    public class ClsValidation
    {
        private readonly IConfiguration _configuration;
        private readonly string _spPrefix;
        public ClsValidation(IConfiguration configuration)
        {
            _configuration = configuration;
            _spPrefix = _configuration.GetValue<string>("SpPrefix");
        }
        public static string CalculateMid(string pMaxVal, string pMinval)
        {
            Nullable<double> vMinValue = null;
            Nullable<double> vMaxValue = null;
            try
            {

                vMinValue = Convert.ToDouble(pMinval);
                vMaxValue = Convert.ToDouble(pMaxVal);

            }
            catch
            {
                if (vMinValue == null)
                {
                    return "InValidMin";
                }
                else if (vMaxValue == null)
                {
                    return "InValidMax";
                }
            }
            if (vMinValue > vMaxValue)
                return "Greater";
            else
                return Convert.ToString((vMinValue + vMaxValue) / 2);
        }
        public static string CompareMinMax(string pMaxVal, string pMinval)
        {
            Decimal vMinValue = 0;
            Decimal vMaxValue = 0;
            try
            {

                vMinValue = Convert.ToDecimal(pMinval);
                vMaxValue = Convert.ToDecimal(pMaxVal);

            }
            catch
            {
                return "InValidMaxMin";
            }
            if (vMinValue > vMaxValue)
                return "Greater";
            else
                return "OK";
        }
        /// <summary>
        /// Biplob Mallick        
        /// st=string that you validate.
        /// df=Give date formate.
        ///  DateTimeFormatInfo df = new DateTimeFormatInfo();
        ///  df.ShortDatePattern = "dd/MM/yyyy";
        /// </summary>
        /// <param name="st"></param>
        /// <param name="df"></param>
        /// <returns></returns>
        public static bool ValidateDateTime(string st, DateTimeFormatInfo df)
        {

            try
            {
                string[] AryDate = st.Split('/');
                if (AryDate[0].Length != 2 || AryDate[1].Length != 2 || AryDate[2].Length != 4)
                    return false;

                DateTime dt = Convert.ToDateTime(st, df);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Find greatr/small and date validation.
        /// Greater
        /// </summary>
        /// <param name="MaxDate"></param>
        /// <param name="MinDate"></param>
        /// <returns></returns>
        public static string DatetimeMaxMin(string pMaxDate, string pMinDate)
        {
            DateTime vMinValue = new DateTime();
            DateTime vMaxValue = new DateTime();
            string vMin = null;
            string vMax = null;
            // DateTime dt = new DateTime();
            try
            {
                vMaxValue = Convert.ToDateTime(pMaxDate);
                vMax = vMaxValue.ToString("dd-MMM-yyyy");

                vMinValue = Convert.ToDateTime(pMinDate);
                vMin = vMinValue.ToString("dd-MMM-yyyy");

            }
            catch
            {
                if (vMax == null)
                {
                    return "InValidMax";
                }
                if (vMin == null)
                {
                    return "InValidMin";
                }
            }
            if (vMaxValue < vMinValue)
                return "Greater";
            if (vMaxValue == vMinValue)
                return "Equal";

            else
                return "OK";
        }
        /// <summary>
        /// Author: Aminul Bari     Date: 07-Oct-08
        /// Purpose: Find out whether pDate1 is Greater/Equal/Smaller than pDate2.
        /// Retunrs string value as "Greater" or "Smaller" or "Equal" or "Invalid"
        /// </summary>
        /// <param name="pDate1"></param>
        /// <param name="pDate2"></param>
        /// <returns></returns>
        public static string DateComperison(string pDate1, string pDate2)
        {
            DateTime vDate1 = new DateTime();
            DateTime vDate2 = new DateTime();
            try
            {
                vDate1 = Convert.ToDateTime(pDate1);
                vDate2 = Convert.ToDateTime(pDate2);
                if (vDate1 > vDate2)
                    return "Greater";
                else if (vDate1 < vDate2)
                    return "Smaller";
                else
                    return "Equal";
            }
            catch
            {
                return "Invalid";
            }
        }
        //public static string GetTransacDate(string BracnchId)
        //{
        //    string tranDate = string.Empty;
        //    CDataAccess objCDataAccess = CDataAccess.NewCDataAccess();
        //    DbCommand objDbCommand = objCDataAccess.GetMyCommand(false, IsolationLevel.ReadCommitted, "application", false);
        //    string sql = "select d.tran_dt from sms_branch_day_log d where d.branch_id = '" + BracnchId + "'";

        //    using (DbDataReader dr = objCDataAccess.ExecuteReader(objDbCommand, sql, CommandType.Text, null))
        //    {
        //        while (dr.Read())
        //        {
        //            tranDate = dr["TRAN_DT"].ToString();

        //        }
        //        return tranDate;

        //    }
        //}
      
        public static string GetDateFormat(string pDateValue)
        {
            DateTime vValue = new DateTime();
            string vDateFormate = null;
            if (!string.IsNullOrEmpty(pDateValue))
            {

                try
                {
                    vValue = Convert.ToDateTime(pDateValue);
                    vDateFormate = vValue.ToString("dd-MMM-yyyy");

                }
                catch
                {
                    if (vDateFormate == null)
                    {
                        return "InValid";
                    }

                }


            }
            return vDateFormate;

        }


        public static string GetDateFormatddmmyyyy(string DateValue)
        {
            DateTime Value = new DateTime();
            string DateFormate = null;

            try
            {
                Value = Convert.ToDateTime(DateValue);
                DateFormate = Value.ToString("dd/MM/yyyy");

            }
            catch
            {
                if (DateFormate == null)
                {
                    return "InValid";
                }

            }
            return DateFormate;

        }
        public static string GetDateFormatyyyymmdd(string DateValue)
        {
            DateTime Value = new DateTime();
            string DateFormate = null;

            try
            {
                Value = Convert.ToDateTime(DateValue);
                DateFormate = Value.ToString("yy/MM/dd");

            }
            catch
            {
                if (DateFormate == null)
                {
                    return "InValid";
                }

            }
            return DateFormate;

        }
        public string D_M_Y_Format(string date)
        {
            string date1 = "";
            date1 = date.Substring(3, date.Length - 8) + '/';
            date1 = date1 + date.Substring(0, date.Length - 8) + '/';
            date1 = date1 + date.Substring(6, date.Length - 6);
            return date1;
        }
        public static string GetDBDateFormat(string pDateValue)
        {
            string[] strArDateValue = pDateValue.Split('/');
            string strTemp = strArDateValue[0];
            strArDateValue[0] = strArDateValue[1];
            strArDateValue[1] = strTemp;
            pDateValue = strArDateValue[0] + "/" + strArDateValue[1] + "/" + strArDateValue[2];
            DateTime vValue = new DateTime();
            string vDateFormate = null;
            if (!string.IsNullOrEmpty(pDateValue))
            {
                try
                {
                    vValue = Convert.ToDateTime(pDateValue);
                    vDateFormate = vValue.ToString("dd-MMM-yyyy");
                }
                catch
                {
                    if (vDateFormate == null)
                    {
                        return "InValid";
                    }
                }
            }
            return vDateFormate;
        }


        /// <summary>
        /// Author: Soumitra     Date: 27-Jan-2014
        /// Purpose: Two degits year problem. 
        /// Retunrs two degit year as 2030 instated of 1930
        /// </summary>

        public static string GetDateFromddmmyy(string DateValue)
        {

            CultureInfo current = CultureInfo.CurrentCulture;
            DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)current.DateTimeFormat.Clone();
            dateTimeFormatInfo.Calendar.TwoDigitYearMax = 2099;
            var date = DateTime.Parse(DateValue,
                dateTimeFormatInfo);
            return date.ToShortDateString();
        }
    }
}
