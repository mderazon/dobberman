using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;
using TAUP2C.Dobberman.Web.Models;


namespace TAUP2C.Dobberman.Web.Services
{
    public class DobbermanService : IDobbermanService
    {
        
        Report IDobbermanService.GetReportById(string userId)
        {
            Report report = new Report();
            // TODO add code to get from database
            report.ReportId = 1;
            report.ReportDescription = "What a pretty little report";
            report.Mood = "Happy";
            report.Authority = (new Authority());
            report.User = new User();

            return report;
            
        }
    }
}
