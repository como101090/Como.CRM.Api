using Como.CRM.Api.Enums;


namespace Como.CRM.Api.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FieldNameAttribute : Attribute
    {
        public string En { get; }

        public string? Hy { get; }

        public string? Ru { get; }

        public string? Ka { get; }

        public FieldNameAttribute(
            string en,
            string? hy = null,
            string? ru = null,
            string? ka = null)
        {
            En = en;
            Hy = hy;
            Ru = ru;
            Ka = ka;
        }

        public string Get(Language language)
        {
            return language switch
            {
                Language.Hy => string.IsNullOrWhiteSpace(Hy) ? En : Hy,
                Language.Ru => string.IsNullOrWhiteSpace(Ru) ? En : Ru,
                Language.Ka => string.IsNullOrWhiteSpace(Ka) ? En : Ka,
                _ => En
            };
        }
    }
}