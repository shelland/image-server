using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelland.ImageServer.Infrastructure.Extensions;
using Shelland.ImageServer.Infrastructure.Extensions.Pipeline;
using Shelland.ImageServer.Infrastructure.Filters;

namespace Shelland.ImageServer
{
    public class Startup
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(Configuration);
            services.AddConfigOptions(Configuration);
            services.AddImageProcessing(Configuration, this.webHostEnvironment);
            services.AddHelperServices();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddModules();
            builder.RegisterType<ExceptionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            if (this.webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseAppCors(Configuration);

            app.UseRouting();
            app.UseAuthorization();

            app.UseAppCachedStaticFiles(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}