using Airbnb.Application.Resolvers;
using Airbnb.Application.Services;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Airbnb.Tests
{
    public class TestBase : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _scope;
        public TestBase()
        {
            var Services = new ServiceCollection();

            Services.AddMemoryCache();
            Services.AddScoped<UserResolver>();
            Services.AddScoped<IPropertyService, PropertyService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IReviewService, ReviewServices>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();


        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}