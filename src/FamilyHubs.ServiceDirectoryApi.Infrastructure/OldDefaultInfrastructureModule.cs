//using Autofac;
//using FamilyHubs.SharedKernel;
//using FamilyHubs.SharedKernel.Interfaces;
//using fh_service_directory_api.core.Entities;
//using fh_service_directory_api.infrastructure.Persistence.Repository;
//using MediatR;
//using MediatR.Pipeline;
//using System.Reflection;
//using Module = Autofac.Module;

//namespace fh_service_directory_api.infrastructure;

//public class OldDefaultInfrastructureModule : Module
//{
//    private readonly bool _isDevelopment = false;
//    private readonly List<Assembly> _assemblies = new List<Assembly>();

//    public OldDefaultInfrastructureModule(bool isDevelopment, Assembly? callingAssembly = null)
//    {
//        _isDevelopment = isDevelopment;
//        var coreAssembly = Assembly.GetAssembly(typeof(OpenReferralOrganisation));
//        var infrastructureAssembly = Assembly.GetAssembly(typeof(SSCAStartupSetup));
//        if (coreAssembly != null)
//        {
//            _assemblies.Add(coreAssembly);
//        }

//        if (infrastructureAssembly != null)
//        {
//            _assemblies.Add(infrastructureAssembly);
//        }

//        if (callingAssembly != null)
//        {
//            _assemblies.Add(callingAssembly);
//        }
//    }

//    protected override void Load(ContainerBuilder builder)
//    {
//        RegisterCommonDependencies(builder);
//    }

//    private void RegisterCommonDependencies(ContainerBuilder builder)
//    {
//        builder.RegisterGeneric(typeof(EfRepository<>))
//          .As(typeof(IRepository<>))
//          .As(typeof(IReadRepository<>))
//          .InstancePerLifetimeScope();

//        builder
//          .RegisterType<Mediator>()
//          .As<IMediator>()
//          .InstancePerLifetimeScope();

//        builder
//          .RegisterType<DomainEventDispatcher>()
//          .As<IDomainEventDispatcher>()
//          .InstancePerLifetimeScope();

//        builder.Register<ServiceFactory>(context =>
//        {
//            var c = context.Resolve<IComponentContext>();

//            return t => c.Resolve(t);
//        });

//        var mediatrOpenTypes = new[]
//        {
//      typeof(IRequestHandler<,>),
//      typeof(IRequestExceptionHandler<,,>),
//      typeof(IRequestExceptionAction<,>),
//      typeof(INotificationHandler<>),
//    };

//        foreach (var mediatrOpenType in mediatrOpenTypes)
//        {
//            builder
//              .RegisterAssemblyTypes(_assemblies.ToArray())
//              .AsClosedTypesOf(mediatrOpenType)
//              .AsImplementedInterfaces();
//        }
//    }
//}
