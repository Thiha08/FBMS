using Autofac;
using FBMS.Core;
using FBMS.Core.Interfaces;
using FBMS.Core.Mail;
using FBMS.Infrastructure.Data;
using FBMS.Infrastructure.HangfireServices;
using FBMS.Infrastructure.Mail;
using FBMS.Infrastructure.Services;
using FBMS.SharedKernel.Interfaces;
using FBMS.Spider.Auth;
using FBMS.Spider.Downloader;
using FBMS.Spider.Pipeline;
using FBMS.Spider.Processor;
using FBMS.Spider.Scheduler;
using MediatR;
using MediatR.Pipeline;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;

namespace FBMS.Infrastructure
{
    public class DefaultInfrastructureModule : Module
    {
        private bool _isDevelopment = false;
        private List<Assembly> _assemblies = new List<Assembly>();

        public DefaultInfrastructureModule(bool isDevelopment, Assembly callingAssembly = null)
        {
            _isDevelopment = isDevelopment;

            var coreAssembly = Assembly.GetAssembly(typeof(DatabasePopulator));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            var spiderAssembly = Assembly.GetAssembly(typeof(ICrawlerProcessor));

            _assemblies.Add(coreAssembly);
            _assemblies.Add(infrastructureAssembly);
            _assemblies.Add(spiderAssembly);

            if (callingAssembly != null)
            {
                _assemblies.Add(callingAssembly);
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }
            RegisterCommonDependencies(builder);
        }

        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<EfRepository>().As<IRepository>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                .RegisterAssemblyTypes(_assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();
            }

            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<EmailTemplateProvider>().As<IEmailTemplateProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerDownloader>().As<ICrawlerDownloader>().InstancePerDependency();
            builder.RegisterType<CrawlerPageLinkReader>().As<ICrawlerPageLinkReader>().InstancePerDependency();
            builder.RegisterType<CrawlerPipeline>().As<ICrawlerPipeline>().InstancePerDependency();
            builder.RegisterType<CrawlerProcessor>().As<ICrawlerProcessor>().InstancePerDependency();
            builder.RegisterType<CrawlerScheduler>().As<ICrawlerScheduler>().InstancePerDependency();
            builder.RegisterType<CrawlerAuthorization>().As<ICrawlerAuthorization>().InstancePerDependency();
            builder.RegisterType<CrawlerService>().As<ICrawlerService>().InstancePerDependency();
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerDependency();
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerDependency();
            builder.RegisterType<MatchSchedulingService>().As<IMatchSchedulingService>().InstancePerDependency();
            builder.RegisterType<TransactionHostedService>().As<ITransactionHostedService>().InstancePerDependency();
            builder.RegisterType<SchedulingHostedService>().As<ISchedulingHostedService>().InstancePerDependency();
        }

        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add development only services
        }

        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add production only services
        }


    }
}
