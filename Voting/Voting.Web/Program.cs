using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Voting.Web.Models;
using Voting.Web.Persistance;

namespace Voting.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IWebHost host = BuildWebHost(args);
			LoadData(host);
			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();

		private static void LoadData(IWebHost host) {
			using (IServiceScope scope = host.Services.CreateScope()) {
				IServiceProvider services = scope.ServiceProvider;

				try {
					// Requires using MvcMovie.Models;
					var context = services.GetRequiredService<VotingContext>();
					DbInitializer.Init(context);
				} catch (Exception ex) {
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}

			host.Run();
		}
	}
}
