using Airbnb.Application.Services;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace Airbnb.Tests
{
    public class TestBase : IDisposable
    {
        protected readonly DbContextOptions<AirbnbDbContext> _dbContextOptions;
        protected readonly ServiceProvider _serviceProvider;
        //private readonly IServiceScope _serviceScope;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationBuilder _builder;

        public TestBase()
        {
            // Create New Dat 
            _dbContextOptions = new DbContextOptionsBuilder<AirbnbDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var services = new ServiceCollection();

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockMediator = new Mock<IMediator>();

            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            AddServices(services, mockWebHostEnvironment, _builder.Configuration, mockMediator);

            _serviceProvider = services.BuildServiceProvider();
          //  _serviceScope = _serviceProvider.CreateScope();
            _scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            InitializeDatabase().GetAwaiter();

        }
        private void AddServices(ServiceCollection services, Mock<IWebHostEnvironment> mockWebHostEnvironment, IConfiguration configuration, Mock<IMediator> mockMediator)
        {
            services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<AirbnbDbContext>()
               .AddSignInManager<SignInManager<AppUser>>()
               .AddDefaultTokenProviders();

            var context = new Mock<IHttpContextAccessor>();
            context.SetupGet(x => x.HttpContext)
                            .Returns(new DefaultHttpContext());

            services.AddDbContext<AirbnbDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            // what do?
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetAssembly(new Application.Mapester.BookingMap().GetType())!);
            services.AddSingleton(config);
            // services.AddScoped<IMapper, ServiceMapper>(); why this is used in ICPC Test Base
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReviewService, ReviewServices>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();


        }
        public Mock<IHttpContextAccessor> GetMockHttpContextAccessor(string userId)
        {
            var identity = new GenericIdentity("Account");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            var httpContext = new DefaultHttpContext();
            httpContext.User.AddIdentity(identity);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return httpContextAccessor;
        }
        public SignInManager<AppUser> GetSignInManager()
        {
            return _serviceProvider.GetRequiredService<SignInManager<AppUser>>();
        }
        public IUnitOfWork GetUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }
        public UserManager<AppUser> GetUserManager()
        {
            return _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        }
        public RoleManager<IdentityRole> GetRoleManager()
        {
            return _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }
        public IAuthService GetAuthService()
        {
            return _serviceProvider.GetRequiredService<IAuthService>();
        }

        public void Dispose()
        {
            var context = _serviceProvider.GetRequiredService<AirbnbDbContext>();
            context.Database.EnsureDeleted();
        }
        private async Task InitializeDatabase()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AirbnbDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.EnsureCreatedAsync();

                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Owner"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
        }
    }
}
