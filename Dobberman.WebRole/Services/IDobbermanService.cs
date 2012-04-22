using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TAUP2C.Dobberman.WebRole.Models;



namespace TAUP2C.Dobberman.WebRole.Services
{
    [ServiceContract]
    public interface IDobbermanService
    {

        [OperationContract]
        List<Report> GetReportsByUserId(int userId);

        [OperationContract]
        List<Report> GetReportsByAuthorityId(int authorityId);

        // returns a list of all authorities
        [OperationContract]
        List<Authority> GetAllAuthorities();

        // returns a list of authorities sorted by the number of reports associated with them, does not get the reports themselves
        [OperationContract]
        List<Authority> GetSortedAuthorities();

        // returns a list of all reports with a location that is not null
        [OperationContract]
        List<Report> GetAllReportsWithLocation();

        [OperationContract]
        int CreateNewReport(Report report);

        [OperationContract]
        int CreateNewUser(User user);
        
        [OperationContract]
        int CreateNewAuthority(Authority authority);
    
    }

    

    [DataContract]
    public class Report
    {
        [DataMember]
        public int ReportId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string Mood { get; set; }
        [DataMember(IsRequired = false)]
        public byte[] Photo { get; set; }
        [DataMember(IsRequired = false)]
        public string Location { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public int AuthorityId { get; set; }
        [DataMember]
        public Authority Authority { get; set; }
    }

    [DataContract]
    public class Authority
    {
        [DataMember(IsRequired=false)]
        public int AuthorityId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FacebookPage { get; set; }
        [DataMember]
        public List<Report> Reports { get; set; }

    }

    [DataContract]
    public class User
    {
        [DataMember(IsRequired=false)]
        public int UserId { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
