using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using ForgeAppTesting.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using ForgeAppTesting.Services;

namespace ForgeAppTesting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<BookstoreDatabaseSettings>(
                Configuration.GetSection(nameof(BookstoreDatabaseSettings)));
            services.Configure<Bim5dDatabaseSettings>(
                Configuration.GetSection(nameof(Bim5dDatabaseSettings)));

            services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);
            services.AddSingleton<IBim5dDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<Bim5dDatabaseSettings>>().Value);

            services.AddSingleton<BookService>();
            services.AddSingleton<PartidaCertificacionService>();
            services.AddSingleton<GoodPracticesService>();

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapHub<Controllers.DesignAutomationHub>("/api/signalr/designautomation");
            });
        }
    }
}
