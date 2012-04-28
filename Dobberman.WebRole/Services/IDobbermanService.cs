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

        // returns a list of all reports with a location that is not null
        [OperationContract]
        List<Report> GetAllReportsWithLocation();

        // returns a list of all authorities
        [OperationContract]
        List<Authority> GetAllAuthorities();

        // returns a list of authorities sorted by the number of reports associated with them, does not get the reports themselves
        [OperationContract]
        List<Authority> GetSortedAuthorities();

        // returns a list of authorities of specific category. the authorities are sorted by score
        // TODO : return only top and buttom 3
        [OperationContract]
        List<Authority> GetAuthoritiesByCategoryId(int categoryId);
        
        [OperationContract]
        List<Category> GetAllCategories();


        #region Create methods
        [OperationContract]
        int CreateNewReport(Report report);

        [OperationContract]
        int CreateNewUser(User user);
        
        [OperationContract]
        int CreateNewAuthority(Authority authority);

        [OperationContract]
        int CreateNewCategory(Category category);

        #endregion

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
        public string Photo { get; set; }
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
        [DataMember(IsRequired = false)]
        public int CategoryId { get; set; }
        [DataMember]
        public Category Category { get; set; }
        [DataMember(IsRequired = false)]
        public double Score { get; set; }

    }

    [DataContract]
    public class Category
    {
        [DataMember(IsRequired = false)]
        public int CategoryId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember(IsRequired = false)]
        public string Description { get; set; }
        [DataMember(IsRequired = false)]
        public string Picture { get; set; }
        [DataMember]
        public List<Authority> Authorities { get; set; }

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
