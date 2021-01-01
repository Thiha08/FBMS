using FBMS.Infrastructure.Data;
using FBMS.Infrastructure.HostedServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FBMS.Infrastructure
{
    public static class StartupSetup
	{
		public static void AddDbContext(this IServiceCollection services, string connectionString) =>
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString)); // will be created in web project root

		public static void AddIdentityCore(this IServiceCollection services) =>
			services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();

		public static void AddHostedService(this IServiceCollection services) =>
			services.AddHostedService<TransactionHostedService>();
	}
}
