namespace Como.CRM.Api.Domain.Entities.CompanyInfo
{
    public class PublisherPhoneInfo
    {
        public int Id { get; set; }
        public int PublisherId { get; set; }
        public PublisherInfo PublisherInfo { get; set; }

        public string PhoneType { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        
    }
}
