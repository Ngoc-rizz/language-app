namespace Lexi.Api.DTOs.Auth;

public class GoogleLoginRequest
{ 
    // ID Token do Flutter (package google_sign_in) lấy được sau khi user đăng nhập Google,
    // gửi lên backend để verify tại đây - KHÔNG bao giờ tự tin vào thông tin user
    // gửi thẳng từ client (email, name...) mà không verify qua Google.
    public string IdToken { get; set; } = string.Empty;
}