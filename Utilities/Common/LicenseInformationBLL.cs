using BuLicense;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Common
{
    public class LicenseInformationBLL
    {
        private readonly IConfiguration _config;
        public LicenseInformationBLL(IConfiguration config)
        {
            _config = config;
        }
        public List<LicenseModel> GetLicenseInformation(out string pErrorMessage)
        {
            LSCommonFunctions lSCommonFunctions = new LSCommonFunctions(_config);
            string LicenceFilePath = lSCommonFunctions.GetLSAppConfigbyKey("LicenseFileLoc");
            List<LicenseModel> objBuLicenseModel = new List<LicenseModel>();
            try
            {
                objBuLicenseModel = LicenseValidator.ReadLicense(LicenceFilePath);
                pErrorMessage = null;
            }
            catch (Exception ex)
            {
                pErrorMessage = ex.Message;
            }
            return objBuLicenseModel;
        }
    }
}
