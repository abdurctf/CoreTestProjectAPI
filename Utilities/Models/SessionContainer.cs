using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class SessionContainer
    {
        public SessionContainer()
        {
            LoginUser = new LoginUserInfo();
        }

        private string scInstituteId;
        public string InstituteId
        {
            get { return scInstituteId; }
            set { scInstituteId = value; }
        }

        private string scInstituteName;
        public string InstituteName
        {
            get { return scInstituteName; }
            set { scInstituteName = value; }
        }

        private string scInstituteType;
        public string InstituteType
        {
            get { return scInstituteType; }
            set { scInstituteType = value; }
        }

        private string scInstituteServiceType;
        public string InstituteServiceType
        {
            get { return scInstituteServiceType; }
            set { scInstituteServiceType = value; }
        }

        public string InstituteOperationMode { get; set; }
        public string InstituteHeadOffice { get; set; }
        private string scLocalCurrency;
        public string LocalCurrency
        {
            get { return scLocalCurrency; }
            set { scLocalCurrency = value; }
        }

        public string CountryId { get; set; }
        public string CountryNm { get; set; }
        private string scFunctionId;
        public string FunctionId
        {
            get { return scFunctionId; }
            set { scFunctionId = value; }
        }

        private string scFastPath;
        public string FastPath
        {
            get { return scFastPath; }
            set { scFastPath = value; }
        }

        private string scServerDate;
        public string ServerDate
        {
            get { return scServerDate; }
            set { scServerDate = value; }
        }
        public string UserAnyBrOperationFlag { get; set; }
        private LoginUserInfo scObjLoginUserInfo;
        public LoginUserInfo LoginUser
        {
            get { return scObjLoginUserInfo; }
            set { scObjLoginUserInfo = value; }
        }

        private string scApiAccessToken;
        public string ApiAccessToken
        {
            get { return scApiAccessToken; }
            set { scApiAccessToken = value; }
        }
        public string AuthMode { get; set; }

    }
}
