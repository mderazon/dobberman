using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.Entity;
using TAUP2C.Dobberman.WebRole.Models;


namespace TAUP2C.Dobberman.WebRole.Services
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

        public List<Report> GetReportsByAuthorityId(int authorityId)
        {
            List<Report> reports = new List<Report>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<ReportEntity> reportsQuery = context.Authorities
                    .Where(c => c.AuthorityId == authorityId)
                    .SelectMany(c => c.Reports);

                foreach (var report in reportsQuery)
                {
                    reports.Add(TranslateReportEntityToReport(report));
                }
            }
            return reports;

        }

        public List<Report> GetAllReportsWithLocation()
        {
            List<Report> reports = new List<Report>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<ReportEntity> reportsQuery =
                from n in context.Reports
                where (n.Location != null) && (n.Location != "")
                select n;

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
        
        public List<Authority> GetSortedAuthorities()
        {
            List<Authority> authorities = new List<Authority>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                var sortedAuthorities = context.Authorities.OrderBy(auth => auth.Score);


                foreach (var authority in sortedAuthorities)
                {
                    authorities.Add(TranslateAuthorityEntityToAuthority(authority));                   
                }
            }
            return authorities;
        }

        public List<Authority> GetAuthoritiesByCategoryId(int categoryId)
        {
            List<Authority> authorities = new List<Authority>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<AuthorityEntity> sortedAuthorities =
                    from n in context.Authorities
                    where n.CategoryId == categoryId
                    orderby n.Score
                    select n;

                foreach (var authority in sortedAuthorities)
                {
                    authorities.Add(TranslateAuthorityEntityToAuthority(authority));
                }
            }
            return authorities;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (DobbermanEntities context = new DobbermanEntities())
            {
                IQueryable<CategoryEntity> sortedCategories =
                    from n in context.Categories
                    orderby n.Name
                    select n;

                foreach (var category in sortedCategories)
                {
                    categories.Add(TranslateCategoryEntityToCategory(category));
                }
            }
            return categories;
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
                AuthorityEntity reportAuthority = (from p
                                        in context.Authorities
                                                where p.AuthorityId == reportEntity.AuthorityId
                                                select p).FirstOrDefault();
                switch (report.Mood)
                {
                    case "positive":
                        reportAuthority.AccumulatedScore += 100;
                        break;
                    case "concerned":
                        reportAuthority.AccumulatedScore += 50;
                        break;
                    case "negative":
                        //add zero to AccumulatedScore
                        break;
                }
                // change the score of authority to the new weighted score
                reportAuthority.Score = reportAuthority.AccumulatedScore / (reportAuthority.Reports.Count + 1);
                // add new report to the context
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
                    Name = authority.Name,
                    CategoryId = authority.CategoryId,
                    FacebookPage = authority.FacebookPage,
                    AccumulatedScore = 0,
                    Score = 50,

                };
                context.AddToAuthorities(authorityEntity);
                context.SaveChanges();
                return authorityEntity.AuthorityId;
            }
            
        }

        public int CreateNewCategory(Category category)
        {
            using (DobbermanEntities context = new DobbermanEntities())
            {
                // find out if category already exists by its name
                CategoryEntity validateCategory = (from p
                                             in context.Categories
                                                     where p.Name == category.Name
                                                     select p).FirstOrDefault();
                if (!(validateCategory == null)) return validateCategory.CategoryId;

                CategoryEntity categoryEntity = new CategoryEntity()
                {
                    Name = category.Name,
                    Description = category.Description,
                    Picture = category.Picture,

                };
                context.AddToCategories(categoryEntity);
                context.SaveChanges();
                return categoryEntity.CategoryId;
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
                FacebookLink = reportEntity.FacebookLink,
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
                Score = authorityEntity.Score,
                Logo = authorityEntity.Logo,
                CategoryId = authorityEntity.CategoryId,                
                Category = TranslateCategoryEntityToCategory(authorityEntity.Category),                
            };
            return authority;
        }

        private Category TranslateCategoryEntityToCategory(CategoryEntity categoryEntity)
        {
            Category category = new Category()
            {
                CategoryId = categoryEntity.CategoryId,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description,
                Picture = categoryEntity.Picture,
            };
            return category;
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
