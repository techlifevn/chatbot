using Chatbot.Common.Enums;
using Chatbot.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Web;

namespace Chatbot.Common.Extension
{
    public static class HttpRequestExtensions
    {
        public static void AddCookie(this HttpResponse response, string cookieName, string value)
        {
            response.Cookies.Append(cookieName, value,
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    Expires = DateTime.Now.AddMinutes(5)
                }
            );
        }
        public static string GetCookie(this HttpRequest request, string cookieName)
        {
            var cookies = request.Cookies.Select((header) => $"{header.Key}");
            if (cookies.Contains(cookieName))
            {
                return request.Cookies[cookieName];
            }

            return string.Empty;
        }
        public static void RemoveCookie(this HttpRequest request, string cookieName)
        {
            var cookies = request.Cookies.Select((header) => $"{header.Key}");
            if (cookies.Contains(cookieName))
            {
                request.HttpContext.Response.Cookies.Delete(cookieName);
            }
        }
        public static string GetRawUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{httpContext.Request.Path}{httpContext.Request.QueryString}";
        }
        public static string GetRawUrlNoneQuery(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{httpContext.Request.Path}";
        }
        //public static string GetRawUrlSSO(this HttpRequest request)
        //{
        //    var httpContext = request.HttpContext;
        //    return $"{request.Scheme}://{request.Host}/LoginWithSSO";
        //}  
        public static string GetRawUrlSSO(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{request.Scheme}://{request.Host}/sso";
        }
        public static string GetRawUrlSSOVNeID(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{request.Scheme}://{request.Host}/LoginWithSSOVNeID/";
        }
        public static string GetFullUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }
        public static string GetFullUrl(this HttpRequest request, string url)
        {
            return $"{request.Scheme}://{request.Host}{url}";
        }
        public static string GetScheme(this HttpRequest request, string host)
        {
            return $"{request.Scheme}://{host}";
        }
        public static string GetRawUrl(this HttpRequest request, string url, bool IsQuery = true)
        {
            var httpContext = request.HttpContext;
            if (!IsQuery)
            {
                return $"{url}";
            }
            else
                return $"{url}{HttpUtility.UrlDecode(httpContext.Request.QueryString.ToString())}";
        }
        public static string GetBackUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            string url = httpContext.Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(url)) return HttpUtility.UrlEncode(url);
            else return "/";
        }

        public static string GetReturnUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            string url = HttpUtility.UrlDecode(httpContext.Request.Headers["Referer"]);
            if (!string.IsNullOrEmpty(url)) return url;
            else return request.GetBackUrl();
        }
        public static Uri GetUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80)
            };
            return uriBuilder.Uri;
        }
        public static string GetLanguageId(this HttpRequest request)
        {
            try
            {
                string langId = request.Cookies[".AspNetCore.Culture"];
                if (!string.IsNullOrEmpty(langId))
                {
                    return langId.Substring(2, 2);
                }
                else
                {
                    return "vi";
                }
            }
            catch
            {
                return "vi";
            }

        }
        public static string GetUserName(this HttpRequest request)
        {
            string userName = request.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
                return userName;
            else return "";
        }
        public static UserLoginRequest GetUser(this HttpRequest request)
        {
            try
            {
                if (request.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = request.HttpContext.User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return null;

                    return new UserLoginRequest()
                    {
                        Id = Guid.Parse(userId),
                        Sid = request.HttpContext.User.FindFirst(ClaimTypes.Sid).Value,
                        UserName = request.HttpContext.User.FindFirst("UserName").Value,
                        CitizenId = request.HttpContext.User.FindFirst("CitizenId").Value,
                        AvatarUrl = request.HttpContext.User.FindFirst("AvatarUrl").Value,
                        HoVaTen = request.HttpContext.User.FindFirst("FullName").Value,
                        FirstName = request.HttpContext.User.FindFirst("FirstName").Value,
                        LastName = request.HttpContext.User.FindFirst("LastName").Value,
                        OrganCode = request.HttpContext.User.FindFirst("OrganCode").Value,
                        OrganName = request.HttpContext.User.FindFirst("OrganName").Value,
                        PhoneNumber = request.HttpContext.User.FindFirst("PhoneNumber")?.Value ?? "",
                        Address = request.HttpContext.User.FindFirst("Address")?.Value ?? "",
                        IsSSOUser = Convert.ToBoolean(request.HttpContext.User.FindFirst("IsSSOUser")?.Value ?? "false"),
                        IsVNeID_Verified = Convert.ToBoolean(request.HttpContext.User.FindFirst("IsVNeID_Verified")?.Value ?? "false"),
                        OrganId = Convert.ToInt32(request.HttpContext.User.FindFirst("OrganId").Value),
                        PageId = Convert.ToInt32(request.HttpContext.User.FindFirst("PageId").Value),
                        IsSupperUser = Convert.ToBoolean((request.HttpContext.User.FindFirst("IsSupperUser")?.Value ?? "false")),
                        AccessToken = request.HttpContext.User.FindFirst("AccessToken")?.Value,
                        Version = Convert.ToDateTime(request.HttpContext.User.FindFirst("Version")?.Value ?? DateTime.MinValue.ToString()),
                        TypeLogin = EnumHelper.GetEnumValue<LoginType>(request.HttpContext.User.FindFirst("TypeLogin")?.Value ?? LoginType.CHUAN.ToString())
                    };
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public static DateTime GetVersion(this HttpRequest request)
        {
            try
            {
                if (request.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = request.HttpContext.User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return DateTime.MinValue;

                    return Convert.ToDateTime(request.HttpContext.User.FindFirst("Version")?.Value ?? DateTime.MinValue.ToString());
                }
                else
                    return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime GetVersion(this ClaimsPrincipal User)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return DateTime.MinValue;

                    return Convert.ToDateTime(User.FindFirst("Version")?.Value ?? DateTime.MinValue.ToString());
                }
                else return DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static int? GetPageId(this ClaimsPrincipal User)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return null;

                    return Convert.ToInt32(User.FindFirst("PageId").Value);
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public static Guid GetUserId(this ClaimsPrincipal User)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return Guid.Empty;

                    return Guid.Parse(userId);
                }
                else return Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }
        public static UserLoginRequest GetUser(this ClaimsPrincipal User, params string[] permissionInRole)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst("Id").Value;
                    if (string.IsNullOrEmpty(userId)) return null;

                    return new UserLoginRequest()
                    {
                        Id = Guid.Parse(userId),
                        UserName = User.FindFirst("UserName").Value,
                        CitizenId = User.FindFirst("CitizenId").Value,
                        AvatarUrl = User.FindFirst("AvatarUrl").Value,
                        HoVaTen = User.FindFirst("FullName").Value,
                        PhoneNumber = User.FindFirst("PhoneNumber").Value,
                        OrganCode = User.FindFirst("OrganCode").Value,
                        OrganName = User.FindFirst("OrganName").Value,
                        IsSSOUser = Convert.ToBoolean(User.FindFirst("IsSSOUser")?.Value ?? "false"),
                        IsVNeID_Verified = Convert.ToBoolean(User.FindFirst("IsVNeID_Verified")?.Value ?? "false"),
                        OrganId = Convert.ToInt32(User.FindFirst("OrganId").Value),
                        PageId = Convert.ToInt32(User.FindFirst("PageId").Value),
                        IsSupperUser = Convert.ToBoolean((User.FindFirst("IsSupperUser")?.Value ?? "false")),
                        AccessToken = User.FindFirst("AccessToken")?.Value,
                        Version = Convert.ToDateTime(User.FindFirst("Version")?.Value ?? DateTime.MinValue.ToString()),
                        TypeLogin = EnumHelper.GetEnumValue<LoginType>(User.FindFirst("TypeLogin")?.Value ?? LoginType.CHUAN.ToString())
                    };
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public static string GetDomain(this HttpRequest request)
        {
            return $"{request.Host}";
        }
        public static string GetUrlDomain(this HttpRequest request, string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;

            return $"{request.Scheme}://{url}";
        }
        public static string GetUrl(this HttpRequest request, string url)
        {
            if (string.IsNullOrEmpty(url)) return string.Empty;

            return $"{request.Scheme}://{request.Host}{url.Trim()}";
        }
        public static string GetUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}";
        }
    }
}
