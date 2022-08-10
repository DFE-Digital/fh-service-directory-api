using Autofac;
using fh_service_directory_api.core.Concretions.Services.Domain.Postcode;
using fh_service_directory_api.core.Interfaces.Services.Domain.Postcode;

namespace fh_service_directory_api.core.Configuration;

public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<IPostcodeLocationClientService>().As<PostcodeLocationClientService>().InstancePerLifetimeScope();
    }
}