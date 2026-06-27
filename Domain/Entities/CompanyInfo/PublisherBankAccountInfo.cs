namespace Como.CRM.Api.Domain.Entities.CompanyInfo
{
    public class PublisherBankAccountInfo
    {
        public int Id { get; set; }
        public int BankInfoId { get; set; }
        public PublisherBankInfo BankInfo { get; set; }
        public string CurrencyName { get; set; } 
        public string AccountCode { get; set; } = string.Empty;
        public string AccountNumber { get; set; }
        
    }
}
