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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi();
            services.AddConfigOptions(Configuration);
            services.AddHelperServices();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddModules();
            builder.RegisterType<ExceptionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseRouting();
            app.UseAuthorization();

            app.AddCachedStaticFiles(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}