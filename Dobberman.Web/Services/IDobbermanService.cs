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
        Report GetReportById(string userId);
    
    }

    [DataContract]
    public class Report
    {
        [DataMember]
        public int ReportId { get; set; }
        [DataMember]
        public string ReportDescription { get; set; }
        [DataMember]
        public string Mood { get; set; }
        [DataMember]
        public byte[] Photo { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public Authority Authority { get; set; }
    }
}
