using LaneSelection.Domain.Factories;
using LaneSelection.Domain.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace LaneSelection.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IConveyorFactory, ConveyorFactory>();
            services.AddSingleton<ILoadFactory, LoadFactory>();
            services.Scan(b => b.FromAssemblies(typeof(IDestinationPolicy).Assembly)
                .AddClasses(c => c.AssignableTo<IDestinationPolicy>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }
    }
}
