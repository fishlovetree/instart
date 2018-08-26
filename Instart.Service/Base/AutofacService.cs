using Autofac;
using Instart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Service.Base
{
    public class AutofacService
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
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleService>().As<IArticleService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerLifetimeScope();
            builder.RegisterType<SchoolService>().As<ISchoolService>().InstancePerLifetimeScope();
            builder.RegisterType<MajorService>().As<IMajorService>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherService>().As<ITeacherService>().InstancePerLifetimeScope();
            builder.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
            builder.RegisterType<DivisionService>().As<IDivisionService>().InstancePerLifetimeScope();
            builder.RegisterType<CampusService>().As<ICampusService>().InstancePerLifetimeScope();
            builder.RegisterType<WorksService>().As<IWorksService>().InstancePerLifetimeScope();
            builder.RegisterType<PartnerService>().As<IPartnerService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseService>().As<ICourseService>().InstancePerLifetimeScope();
            builder.RegisterType<AboutInstartService>().As<IAboutInstartService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactService>().As<IContactService>().InstancePerLifetimeScope();
            builder.RegisterType<StarStudentService>().As<IStarStudentService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseApplyService>().As<ICourseApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<SchoolApplyService>().As<ISchoolApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<StatisticsService>().As<IStatisticsService>().InstancePerLifetimeScope();
            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();
            builder.RegisterType<RecruitService>().As<IRecruitService>().InstancePerLifetimeScope();
            builder.RegisterType<ProgramService>().As<IProgramService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();
            builder.RegisterType<StudioService>().As<IStudioService>().InstancePerLifetimeScope();
            builder.RegisterType<CopysService>().As<ICopysService>().InstancePerLifetimeScope();
            builder.RegisterType<HereMoreService>().As<IHereMoreService>().InstancePerLifetimeScope();
            builder.RegisterType<TeacherQuestionService>().As<ITeacherQuestionService>().InstancePerLifetimeScope();
            builder.RegisterType<MajorApplyService>().As<IMajorApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<WorksCommentService>().As<IWorksCommentService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseOrderService>().As<ICourseOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<ProgramApplyService>().As<IProgramApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyApplyService>().As<ICompanyApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseSystemService>().As<ICourseSystemService>().InstancePerLifetimeScope();
            container = builder.Build();
        }
    }
}
