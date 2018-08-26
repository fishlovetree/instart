using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Repository
{
    public class AutofacRepository
    {
        static IContainer container = null;

        public static T Resolve<T>() {
            try {
                if (container == null) {
                    Register();
                }
            }
            catch (Exception ex) {
                throw new System.Exception("IOC实例化出错!" + ex.Message);
            }
            return container.Resolve<T>();
        }

        public static void Register() {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SchoolRepository>().As<ISchoolRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MajorRepository>().As<IMajorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherRepository>().As<ITeacherRepository>().InstancePerLifetimeScope();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BannerRepository>().As<IBannerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DivisionRepository>().As<IDivisionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CampusRepository>().As<ICampusRepository>().InstancePerLifetimeScope();
            builder.RegisterType<WorksRepository>().As<IWorksRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PartnerRepository>().As<IPartnerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AboutInstartRepository>().As<IAboutInstartRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ContactRepository>().As<IContactRepository>().InstancePerLifetimeScope();
            builder.RegisterType<StarStudentRepository>().As<IStarStudentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourseApplyRepository>().As<ICourseApplyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SchoolApplyRepository>().As<ISchoolApplyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsRepository>().As<IStatisticsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LogRepository>().As<ILogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RecruitRepository>().As<IRecruitRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProgramRepository>().As<IProgramRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<StudioRepository>().As<IStudioRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CopysRepository>().As<ICopysRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HereMoreRepository>().As<IHereMoreRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherQuestionRepository>().As<ITeacherQuestionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MajorApplyRepository>().As<IMajorApplyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<WorksCommentRepository>().As<IWorksCommentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourseOrderRepository>().As<ICourseOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProgramApplyRepository>().As<IProgramApplyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyApplyRepository>().As<ICompanyApplyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourseSystemRepository>().As<ICourseSystemRepository>().InstancePerLifetimeScope();
            container = builder.Build();
        }
    }
}
