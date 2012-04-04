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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class DobbermanService : IDobbermanService
    {
        

        public Report GetReportById(string reportId)
        {
            using (DobbermanEntities dob = new DobbermanEntities())
            {
                Report r = new Report();
                var report = (from r in dob.Reports
            }
            {
                Author a = new Author();
                var author = (from p in pubs.authors
                              where p.au_id == authorId
                              select p).First();

                a.authorCity = author.city;
                a.authorFName = author.au_fname;
                a.authorId = author.au_id;
                a.authorLName = author.au_lname;

                return a;
            }
        }
    }
}
