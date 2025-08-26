namespace Chatbot.Common.Enums
{
    public enum LoginType
    {
        [StringValue(@"Đăng nhập chuẩn")]
        CHUAN = 1,
        [StringValue(@"Xác thực thông qua SSO - Hue")]
        SSOHUE = 2,
        [StringValue(@"Xác thực thông qua SSO - HueS")]
        SSOHUES = 3,
    }
}
