using Como.CRM.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Como.CRM.Api.DTOs.Auth;

public class LoginRequest
{
    [FieldName(
        "User Name",
        "Օգտանուն",
        "Имя пользователя",
        "მომხმარებლის სახელი")]
    public string UserName { get; set; } = string.Empty;

    [FieldName(
        "Password",
        "Գաղտնաբառ",
        "Пароль",
        "პაროლი")]
    public string Password { get; set; } = string.Empty;
}
