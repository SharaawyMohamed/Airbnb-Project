using Airbnb.APIs.MiddelWairs;
using Airbnb.Application.Features.PaymentBooking.Command.CreateBooking;
using Airbnb.Application.Rea_Time;
using Airbnb.Application.Resolvers;
using Airbnb.Application.Services;
using Airbnb.Application.Settings;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Airbnb.APIs.Extensions
{
	public static class ApplicationServices
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			#region Database Connection 

			Services.AddDbContext<AirbnbDbContext>(options =>
				{
					options.UseSqlServer(Configuration.GetConnectionString("RemoteConnection"));
					options.UseLazyLoadingProxies();
				});
			#endregion

			#region Identity-Configurations

			Services.AddIdentity<AppUser, IdentityRole>(options =>
				{
					options.SignIn.RequireConfirmedEmail = true;
					options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
				})
				.AddEntityFrameworkStores<AirbnbDbContext>()
				.AddDefaultTokenProviders();

			#endregion

			#region Redis Connection

			Services.AddStackExchangeRedisCache(option =>
				{
					option.Configuration = Configuration.GetConnectionString("RedisConnection");

				});

			#endregion


			#region Use NewtonSoft Package for json serializeation
			Services.AddControllers();
			Services.AddMvc()
				 .AddNewtonsoftJson(options =>
				 {
					 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					 options.SerializerSettings.Formatting = Formatting.Indented;
				 });
			#endregion

			#region General Services

			Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			Services.AddMemoryCache();
			Services.AddScoped<UserResolver>();
			Services.AddScoped<IPropertyService, PropertyService>();
			Services.AddScoped<IAuthService, AuthService>();
			Services.AddScoped<IUserService, UserService>();
			Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Services.AddScoped<IReviewService, ReviewServices>();
			Services.AddScoped<IReviewRepository, ReviewRepository>();
			Services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
			Services.AddTransient<IMailService, MailService>();
			Services.AddTransient<ExceptionMiddleWare>();
			Services.AddFluentValidationAutoValidation();
			Services.AddSingleton<UserConnectionManager>();
			#endregion

			#region Mediator Service

			Services.AddMediatR(cgf =>
			{
				cgf.RegisterServicesFromAssemblies(typeof(CreateBookingCommandHandler).Assembly);
				cgf.NotificationPublisher = new TaskWhenAllPublisher();

			});
			#endregion

			#region Fluent Validation Service

			Services.AddFluentValidation(fv =>
				{
					fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
				});
			#endregion

			#region Error Response
			//Services.AddProblemDetails(options =>
			//{
			//	options.CustomizeProblemDetails = (context) =>
			//	{
			//		context.ProblemDetails.Extensions[""]
			//	};
			//})
			#endregion

			return Services;
		}
	}
}
