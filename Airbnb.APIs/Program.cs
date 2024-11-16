using Airbnb.APIs.Extensions;
using Airbnb.APIs.Utility;
using Airbnb.Application.Chatting;
using Airbnb.Application.Rea_Time;

namespace Airbnb.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddSignalR();

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerConfigurations();

			await builder.Services.JWTConfigurations(builder.Configuration);
			builder.Services.AddApplicationServices(builder.Configuration);

			var app = builder.Build();

			await ExtensionMethods.ApplyMigrations(app);

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
			app.MapHub<ChatHub>("chatHub");
			app.MapHub<NotificationHub>("notificationHub");

			app.Run();
		}
	}
}
