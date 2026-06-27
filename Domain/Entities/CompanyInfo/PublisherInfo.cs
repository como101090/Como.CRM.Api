namespace Como.CRM.Api.Domain.Entities.CompanyInfo
{
    public class PublisherInfo
    {
        public int Id { get; set; }

        public string TaxCode { get; set; }

        public string TaxName { get; set; }

        public string BrendName { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<PublisherPhoneInfo> Phones { get; set; } = [];
        public ICollection<PublisherMailInfo> Mails { get; set; } = [];
        public ICollection<PublisherBankInfo> Banks { get; set; } = [];
    }
}
