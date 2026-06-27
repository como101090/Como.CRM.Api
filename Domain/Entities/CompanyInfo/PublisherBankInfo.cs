namespace Como.CRM.Api.Domain.Entities.CompanyInfo
{
    public class PublisherBankInfo
    {
        public int Id { get; set; }

        public int PublisherInfoId { get; set; }
        public PublisherInfo PublisherInfo { get; set; }

        public string Code { get; set; }

        public string BankName { get; set; }

        public string BankDockName { get; set; }

        public ICollection<PublisherBankAccountInfo> Accounts { get; set; } = [];
    }

}
