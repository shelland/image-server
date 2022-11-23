#region Usings

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelland.ImageServer.Infrastructure.Extensions;
using Shelland.ImageServer.Infrastructure.Extensions.Pipeline;

#endregion

namespace Shelland.ImageServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(this.configuration);
            services.AddConfigOptions(this.configuration);
            services.AddImageProcessing(this.configuration, this.webHostEnvironment);
            services.AddHelperServices();
            services.AddRateLimiting(this.configuration);
            services.AddHostedServices();
            services.AddModules();
            services.AddLogs();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            app.AddRateLimitingPipeline(this.configuration);

            if (this.webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseAppCors(this.configuration);
            app.UseRouting();
            app.UseAuthorization();
            app.UseAppCachedStaticFiles(this.configuration);
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}