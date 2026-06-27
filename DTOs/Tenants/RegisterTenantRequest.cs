using Como.CRM.Api.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Como.CRM.Api.DTOs.Tenants;

public class RegisterTenantRequest
{

    //[FieldName(
    //"Registered Name","Ռեգիստրում գրանցված անվանում", "Наименование, зарегистрированное в реестре", "რეესტრში რეგისტრირებული დასახელება")]
    //public string LegalName { get; set; } = string.Empty;

    [FieldName("Brand Name", "Բրենդի անվանում", "Название бренда", "ბრენდის სახელი")]
    public string BrandName { get; set; } = string.Empty;

    [FieldName("Company Email","Ընկերության էլ. փոստ","Электронная почта компании","კომპანიის ელფოსტა")]
    public string CompanyEmail { get; set; } = string.Empty;

    [FieldName("Contact Phone","Կոնտակտային հեռախոսահամար","Контактный номер телефона","საკონტაქტო ტელეფონის ნომერი")]
    public string ContactPhone { get; set; } = string.Empty;
}
