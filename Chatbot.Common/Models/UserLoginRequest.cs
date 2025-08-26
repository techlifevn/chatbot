using Chatbot.Common.Enums;

namespace Chatbot.Common.Models
{
    public class UserLoginRequest
    {
        public Guid Id { get; set; }
        public bool IsSupperUser { get; set; }
        public string Sid { get; set; }
        public string CitizenId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string HoVaTen { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsSSOUser { get; set; }
        public bool IsVNeID_Verified { get; set; }
        public int OrganId { get; set; }
        public int PageId { get; set; }
        public string OrganCode { get; set; }
        public string OrganName { get; set; }
        public string AccessToken { get; set; }
        public DateTime Version { get; set; }
        public LoginType TypeLogin { get; set; }
    }
}
