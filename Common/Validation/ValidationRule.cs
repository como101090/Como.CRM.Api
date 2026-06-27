namespace Como.CRM.Api.Common.Validation
{
    public sealed class ValidationRule
    {
        public bool Required { get; set; }

        public decimal? MinLength { get; set; }

        public decimal? MaxLength { get; set; }

      //  public string? Format { get; set; }

        public IReadOnlyList<string>? AllowedValues { get; set; }
    }
}
