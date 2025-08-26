namespace Chatbot.Common
{
    public class SystemConstants
    {
        public const string ConnectionSqlServer = "ConnectionString";
        public const int pageSize = 20;
        public const int pageIndex = 1;
        public const double clarifyMargin = 0.05;
        public const double lowConfidence = 0.45;

        public class AppSettings
        {
            public const string SynchronizationConnectionString = "SynchronizationConnectionString";
            public const string Alerts = "Alerts";
            public const string UserInfo = "UserInfo";
            public const string DefaultLanguageId = "vi";
            public const string Token = "Token";
            public const string Key = "TTTDT";
            public const string APICSDLHSCV = "APICSDLHSCV:Address";
            public const string APICSDLCQS = "APICSDLCQS:Address";
            public const string APICSDLSoHoa = "APICSDLSoHoa:Address";
            public const string APICSDLPAHT = "APICSDLPAHT:Address";
            public const string MessageAuthorize = "Không xác thực được thông tin người dùng";

            public const string HSCVAddress = "PMHSCV:Address";
            public const string ApiHSCVAddress = "PMHSCV:ApiAddress";
            public const string HSCVToken = "PMHSCV:Token";
            public const string HSCVAppID = "PMHSCV:AppID";
            public const string HSCVAppAccessToken = "PMHSCV:AppAccessToken";
            public const string SsoAddress = "Login:SsoAddress";


            public const int ExpiresUtcMinutes = 30;
            public const string JWTSecurityKey = "xt7oidxpIRs9uDVnZEu9kZKqmumiF1e1RINb3UMlwCGA3O3Xywc8OZkOs4dmwf";
        }
    }

}
