using Como.CRM.Api.Attributes;
using Como.CRM.Api.Common.Constants;
using Como.CRM.Api.Common.Validation;
using Como.CRM.Api.Enums;
using Como.CRM.Api.Services.Abstractions;
using System.Collections.Concurrent;
using System.Reflection;

namespace Como.CRM.Api.Services.Implementations
{
    public class ErrorMessageService : IErrorMessageService
    {
        private readonly ICurrentLanguage _currentLanguage;

        private static readonly ConcurrentDictionary<
            (Type Type, string Property, Language Language),
            string> _fieldNameCache = new();

        public ErrorMessageService(
            ICurrentLanguage currentLanguage)
        {
            _currentLanguage = currentLanguage;
        }

        public string GetMessage(
            Type modelType,
            string propertyName,
            string code,
            ValidationRule? rule)
        {
            var language = _currentLanguage.Language;

            var fieldName = GetFieldName(
                modelType,
                propertyName,
                language);

            return code switch
            {
                ValidationCodes.Required => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտը պարտադիր է։",
                    Language.Ru => $"Поле «{fieldName}» обязательно для заполнения.",
                    Language.Ka => $"ველი „{fieldName}“ სავალდებულოა.",
                    _ => $"The '{fieldName}' field is required."
                },

                ValidationCodes.MinLength => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտը պետք է պարունակի առնվազն {rule?.MinLength} նիշ։",
                    Language.Ru => $"Поле «{fieldName}» должно содержать минимум {rule?.MinLength} символов.",
                    Language.Ka => $"ველი „{fieldName}“ უნდა შეიცავდეს მინიმუმ {rule?.MinLength} სიმბოლოს.",
                    _ => $"The '{fieldName}' field must contain at least {rule?.MinLength} characters."
                },

                ValidationCodes.MaxLength => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտը չի կարող գերազանցել {rule?.MaxLength} նիշը։",
                    Language.Ru => $"Поле «{fieldName}» не должно превышать {rule?.MaxLength} символов.",
                    Language.Ka => $"ველი „{fieldName}“ არ უნდა აღემატებოდეს {rule?.MaxLength} სიმბოლოს.",
                    _ => $"The '{fieldName}' field must not exceed {rule?.MaxLength} characters."
                },

                ValidationCodes.InvalidFormat => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտի ձևաչափը սխալ է։",
                    Language.Ru => $"Неверный формат поля «{fieldName}».",
                    Language.Ka => $"ველი „{fieldName}“ ფორმატი არასწორია.",
                    _ => $"The '{fieldName}' field format is invalid."
                },

                ValidationCodes.AlreadyExists => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտի արժեքն արդեն գոյություն ունի։",
                    Language.Ru => $"Значение поля «{fieldName}» уже существует.",
                    Language.Ka => $"ველი „{fieldName}“ მნიშვნელობა უკვე არსებობს.",
                    _ => $"The '{fieldName}' value already exists."
                },

                _ => language switch
                {
                    Language.Hy => $"«{fieldName}» դաշտի արժեքը սխալ է։",
                    Language.Ru => $"Значение поля «{fieldName}» некорректно.",
                    Language.Ka => $"ველი „{fieldName}“ მნიშვნელობა არასწორია.",
                    _ => $"The '{fieldName}' value is invalid."
                }
            };
        }

        private static string GetFieldName(
            Type modelType,
            string propertyName,
            Language language)
        {
            var key = (modelType, propertyName, language);

            return _fieldNameCache.GetOrAdd(key, _ =>
            {
                var property = modelType.GetProperty(
                    propertyName,
                    BindingFlags.Public | BindingFlags.Instance);

                var attribute = property?
                    .GetCustomAttribute<FieldNameAttribute>();

                return attribute?.Get(language) ?? propertyName;
            });
        }
    }
}
