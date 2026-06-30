using Como.CRM.Api.Enums;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Common.Business.Tenant
{
    public class TenantBusinessMessages : IBusinessMessageProvider
    {
        public string GetMessage(string code, Language language)
        {
            return code switch
            {

                TenantBusinessCodes.BrendNameAlreadyExists => language switch
                {
                    Language.Hy => "Տվյալ բրենդի անվանումով ընկերություն արդեն գրանցված է։",
                    Language.Ru => "Компания с таким названием бренда уже зарегистрирована.",
                    Language.Ka => "კომპანია ამ ბრენდის სახელით უკვე რეგისტრირებულია.",
                    _ => "A company with this brand name is already registered."
                },

                TenantBusinessCodes.LegalNameAlreadyExists => language switch
                {
                    Language.Hy => "Այս իրավաբանական անվանումով ընկերություն արդեն գրանցված է։",
                    Language.Ru => "Компания с таким юридическим наименованием уже зарегистрирована.",
                    Language.Ka => "კომპანია ამ იურიდიული სახელწოდებით უკვე რეგისტრირებულია.",
                    _ => "A company with this legal name is already registered."
                },

                TenantBusinessCodes.CompanyEmailAlreadyExists => language switch
                {
                    Language.Hy => "Այս էլ․ փոստով ընկերություն արդեն գրանցված է։",
                    Language.Ru => "Компания с таким адресом электронной почтой уже зарегистрирована.",
                    Language.Ka => "კომპანია ამ ელფოსტით უკვე რეგისტრირებულია.",
                    _ => "A company with this email is already registered."
                },

                TenantBusinessCodes.PhoneAlreadyExists => language switch
                {
                    Language.Hy => "Այս հեռախոսահամարով ընկերություն արդեն գրանցված է։",
                    Language.Ru => "Компания с таким номером телефона уже зарегистрирована.",
                    Language.Ka => "კომპანია ამ ტელეფონის ნომრით უკვე რეგისტრირებულია.",
                    _ => "A company with this phone number is already registered."
                },

                TenantBusinessCodes.HostAlreadyExists => language switch
                {
                    Language.Hy => "Այս հոսթով ընկերություն արդեն գրանցված է։",
                    Language.Ru => "Компания с таким хостом уже зарегистрирована.",
                    Language.Ka => "კომპანია ამ ჰოსტით უკვე რეგისტრირებულია.",
                    _ => "A company with this host already exists."
                },

                TenantBusinessCodes.TenantNotFound => language switch
                {
                    Language.Hy => "Ընկերությունը չի գտնվել։",
                    Language.Ru => "Компания не найдена.",
                    Language.Ka => "კომპანია ვერ მოიძებნა.",
                    _ => "Company not found."
                },

                TenantBusinessCodes.TenantSuspended => language switch
                {
                    Language.Hy => "Ընկերության հաշիվը կասեցված է։",
                    Language.Ru => "Учетная запись компании приостановлена.",
                    Language.Ka => "კომპანიის ანგარიში შეჩერებულია.",
                    _ => "The company account has been suspended."
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
