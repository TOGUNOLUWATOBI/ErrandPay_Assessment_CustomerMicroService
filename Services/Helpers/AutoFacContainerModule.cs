using Autofac;
using Repository;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public class AutoFacContainerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyService).Assembly)
                .AssignableTo<IAutoDependencyService>()
                .As<IAutoDependencyService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyRepository).Assembly)
                .AssignableTo<IAutoDependencyRepository>()
                .As<IAutoDependencyService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
