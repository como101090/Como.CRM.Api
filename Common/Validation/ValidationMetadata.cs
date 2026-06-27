namespace Como.CRM.Api.Common.Validation
{
    /// <summary>
    /// Եսի են եր որ ուզում էի API ներ սարքեի որ Field ինչ վալիդացաներ ունի
    /// </summary>
    public sealed class ValidationMetadata
    {
        public string Field { get; set; } = string.Empty;

        public ValidationRule Rule { get; set; } = new();
    }
}
