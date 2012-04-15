using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TAUP2C.Dobberman.Web.Models;



namespace TAUP2C.Dobberman.Web.Services
{
    [ServiceContract]
    public interface IDobbermanService
    {
        [OperationContract]
        List<Report> GetReportsByUserId(int userId);

        [OperationContract]
        List<Authority> GetAllAuthorities();

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
