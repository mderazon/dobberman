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
        bool CreateNewReport(Report report);

        [OperationContract]
        bool CreateNewUser(User user);
        
        [OperationContract]
        bool CreateNewAuthority(Authority authority);
    
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
        [DataMember]
        public byte[] Photo { get; set; }
        [DataMember]
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
        [DataMember]
        public int AuthorityId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FacebookPage { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
