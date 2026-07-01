using Como.CRM.Api.Attributes;

namespace Como.CRM.Api.DTOs.Auth
{
    public class ChangePasswordRequest
    {
        [FieldName(
            "Current Password",
            "Ընթացիկ գաղտնաբառ",
            "Текущий пароль",
            "მიმდინარე პაროლი")]
        public string CurrentPassword { get; set; } = string.Empty;

        [FieldName(
            "New Password",
            "Նոր գաղտնաբառ",
            "Новый пароль",
            "ახალი პაროლი")]
        public string NewPassword { get; set; } = string.Empty;

        [FieldName(
            "Confirm New Password",
            "Հաստատել նոր գաղտնաբառը",
            "Подтвердите новый пароль",
            "დაადასტურეთ ახალი პაროლი")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
