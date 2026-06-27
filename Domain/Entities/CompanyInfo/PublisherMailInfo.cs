namespace Como.CRM.Api.Domain.Entities.CompanyInfo
{
    public class PublisherMailInfo 
    {
        public int Id { get; set; }
        public int PublisherInfoId { get; set; }
        public PublisherInfo PublisherInfo { get; set; }

        //Gmail → smtp.gmail.com
        //Outlook → smtp.office365.com
        //Corporate mail → mail.comocode.am
        public string Host { get; set; }

        public int Port { get; set; }

        // Como CRM
        public string FromName { get; set; }

        public string FromEmail { get; set; }

        // gmial ի դեպքում նույն մեյլնա
        public string UserName { get; set; }

        public string Password { get; set; }

    }
}
