
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using Castle.Core.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;


namespace Airbnb.Tests
{
    public class TestBase : IDisposable
    {
        private readonly DbContextOptions<AirbnbDbContext> _dbContextOptions;
        private readonly WebApplicationBuilder _builder;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TestBase()
        {
            //_dbContextOptions: Configures an in-memory database using a unique name for isolation between tests.
            _dbContextOptions = new DbContextOptionsBuilder<AirbnbDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            //ServiceCollection: A container for registering services and their implementations.
            var Services = new ServiceCollection();

            // mockWebHostEnvironment: Mocks the web host environment for testing.
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            //_builder: Creates a web application builder and adds configuration from appsettings.json.
            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //mockMediator: Mocks the IMediator service for testing.
            var mockMideator = new Mock<IMediator>();

            // implement method name Add Services();
            _serviceProvider= Services.BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();
            _serviceScopeFactory= _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            // Inetializer needed here


        }

        private void AddServices(IServiceCollection Services,
            Mock<IWebHostEnvironment> mockWebHostEnvironment,
           Microsoft.Extensions.Configuration.IConfiguration configuration,
            Mock<IMediator> mediator)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
                  .AddEntityFrameworkStores<AirbnbDbContext>()
                  .AddSignInManager<SignInManager<AppUser>>()
                  .AddDefaultTokenProviders();
            var context = new Mock<IHttpContextAccessor>();

            context.SetupGet(x => x.HttpContext)
                .Returns(new DefaultHttpContext());


            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            Services.AddTransient<IUnitOfWork, UnitOfWork>();
            Services.AddSingleton(mockWebHostEnvironment.Object);
            Services.AddSingleton(configuration);
            Services.AddSingleton(mediator.Object);
            Services.AddMemoryCache();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}