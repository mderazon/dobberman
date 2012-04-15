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

        public List<Report> GetReportsByUserId(int userId)
        {
            List<Report> reports = new List<Report>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<ReportEntity> reportsQuery = context.Users
                    .Where(c => c.UserId == userId)
                    .SelectMany(c => c.Reports);

                foreach (var report in reportsQuery)
                {
                    reports.Add(TranslateReportEntityToReport(report));
                }
            }
            return reports;

        }

        public List<Authority> GetAllAuthorities()
        {
            List<Authority> authorities = new List<Authority>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<AuthorityEntity> sortedAuthorities =
                    from n in context.Authorities
                    orderby n.Name
                    select n;

                foreach (var authority in sortedAuthorities)
                {
                    authorities.Add(TranslateAuthorityEntityToAuthority(authority));
                }
            }
            return authorities;
        }

        public List<Report> GetAllReportsWithLocation()
        {
            List<Report> reports = new List<Report>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<ReportEntity> reportsQuery =
                from n in context.Reports
                    where n.Location != null
                    select n;

                foreach (var report in reportsQuery)
                {
                    reports.Add(TranslateReportEntityToReport(report));
                }
            }
            return reports;
        }
        #region Create methods
        public int CreateNewReport(Report report)
        {
            ReportEntity reportEntity = new ReportEntity()
            {
                AuthorityId = report.AuthorityId,
                UserId = report.UserId,
                Description = report.Description,
                Date = report.Date,
                Location = report.Location,
                Photo = report.Photo,
                Mood = report.Mood,
            };
            using (DobbermanEntities context = new DobbermanEntities())
            {
                context.AddToReports(reportEntity);
                context.SaveChanges();
            }
            return reportEntity.ReportId;
        }

        public int CreateNewUser(User user)
        {
            using (DobbermanEntities context = new DobbermanEntities())
            {
                // find out if user already exists by its email
                UserEntity validateUser = (from p
                                             in context.Users
                                         where p.Email == user.Email
                                         select p).FirstOrDefault();
                if (!(validateUser == null)) return validateUser.UserId;

                UserEntity userEntity = new UserEntity()
                {
                    Email = user.Email,
                    Name = user.Name,
                };
                context.AddToUsers(userEntity);
                context.SaveChanges();
                return userEntity.UserId;
            }
            
        }
        public int CreateNewAuthority(Authority authority)
        {
            using (DobbermanEntities context = new DobbermanEntities())
            {
                // find out if authority already exists by its name
                AuthorityEntity validateAuthority = (from p
                                             in context.Authorities
                                           where p.Name == authority.Name
                                           select p).FirstOrDefault();
                if (!(validateAuthority == null)) return validateAuthority.AuthorityId;

                AuthorityEntity authorityEntity = new AuthorityEntity()
                {
                    FacebookPage = authority.FacebookPage,
                    Name = authority.Name,
                };
                context.AddToAuthorities(authorityEntity);
                context.SaveChanges();
                return authorityEntity.AuthorityId;
            }
            
        }
        #endregion



        // Translation methods, used to convert between Entity objects represented in the DobbermanModel and the objects in the service data contracts
        #region Translators
        private Report TranslateReportEntityToReport(ReportEntity reportEntity)
        {
            Report report = new Report()
            {
                ReportId = reportEntity.ReportId,
                Date = reportEntity.Date,
                Description = reportEntity.Description,
                Mood = reportEntity.Mood,
                Photo = reportEntity.Photo,
                Location = reportEntity.Location,
                UserId = reportEntity.UserId,
                AuthorityId = reportEntity.AuthorityId,
                User = TranslateUserEntityToUser(reportEntity.User),
                Authority = TranslateAuthorityEntityToAuthority(reportEntity.Authority),
            };
            return report;
        }


        private Authority TranslateAuthorityEntityToAuthority(AuthorityEntity authorityEntity)
        {
            Authority authority = new Authority()
            {
                AuthorityId = authorityEntity.AuthorityId,
                Name = authorityEntity.Name,
                FacebookPage = authorityEntity.FacebookPage,
            };
            return authority;
        }

        private User TranslateUserEntityToUser(UserEntity userEntity)
        {
            User user = new User()
            {
                UserId = userEntity.UserId,
                Email = userEntity.Email,
                Name = userEntity.Name,
            };
            return user;
        }
        #endregion
    }
}
