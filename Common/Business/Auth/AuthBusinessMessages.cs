using Como.CRM.Api.Enums;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Common.Business.Auth
{
    public class AuthBusinessMessages : IBusinessMessageProvider
    {
        public string GetMessage(string code, Language language)
        {
            return code switch
            {
                AuthBusinessCodes.UserNotFound => language switch
                {
                    Language.Hy => "Օգտատերը չի գտնվել։",
                    Language.Ru => "Пользователь не найден.",
                    Language.Ka => "მომხმარებელი ვერ მოიძებნა.",
                    _ => "User not found."
                },

                AuthBusinessCodes.InvalidUserNameOrPassword => language switch
                {
                    Language.Hy => "Մուտքագրված օգտանունը կամ գաղտնաբառը սխալ է։",
                    Language.Ru => "Неверное имя пользователя или пароль.",
                    Language.Ka => "მომხმარებლის სახელი ან პაროლი არასწორია.",
                    _ => "Invalid username or password."
                },

                AuthBusinessCodes.PasswordConfirmationDoesNotMatch => language switch
                {
                    Language.Hy => "Նոր գաղտնաբառը և հաստատման գաղտնաբառը չեն համընկնում։",
                    Language.Ru => "Новый пароль и подтверждение пароля не совпадают.",
                    Language.Ka => "ახალი პაროლი და მისი დადასტურება არ ემთხვევა.",
                    _ => "The new password and confirmation password do not match."
                },

                AuthBusinessCodes.UserSuspended => language switch
                {
                    Language.Hy => "Օգտատիրոջ հաշիվը կասեցված է։",
                    Language.Ru => "Учетная запись пользователя приостановлена.",
                    Language.Ka => "მომხმარებლის ანგარიში შეჩერებულია.",
                    _ => "The user account has been suspended."
                },

                AuthBusinessCodes.InvalidCredentials => language switch
                {
                    Language.Hy => "Մուտքագրված օգտանունը կամ գաղտնաբառը սխալ է։",
                    Language.Ru => "Неверное имя пользователя или пароль.",
                    Language.Ka => "მომხმარებლის სახელი ან პაროლი არასწორია.",
                    _ => "Invalid username or password."
                },

                AuthBusinessCodes.CurrentPasswordIsWrong => language switch
                {
                    Language.Hy => "Ընթացիկ գաղտնաբառը սխալ է։",
                    Language.Ru => "Текущий пароль указан неверно.",
                    Language.Ka => "მიმდინარე პაროლი არასწორია.",
                    _ => "The current password is incorrect."
                },

                AuthBusinessCodes.NewPasswordMustBeDifferent => language switch
                {
                    Language.Hy => "Նոր գաղտնաբառը պետք է տարբերվի ընթացիկ գաղտնաբառից։",
                    Language.Ru => "Новый пароль должен отличаться от текущего.",
                    Language.Ka => "ახალი პაროლი უნდა განსხვავდებოდეს მიმდინარე პაროლისგან.",
                    _ => "The new password must be different from the current password."
                },

                AuthBusinessCodes.Unauthorized => language switch
                {
                    Language.Hy => "Անհրաժեշտ է մուտք գործել համակարգ։",
                    Language.Ru => "Необходимо выполнить вход в систему.",
                    Language.Ka => "სისტემაში ავტორიზაცია აუცილებელია.",
                    _ => "Authentication is required."
                },

                _ => language switch
                {
                    Language.Hy => "Գործողությունը հնարավոր չէ կատարել։",
                    Language.Ru => "Невозможно выполнить операцию.",
                    Language.Ka => "ოპერაციის შესრულება შეუძლებელია.",
                    _ => "The operation cannot be completed."
                }
            };
        }
    }
}
