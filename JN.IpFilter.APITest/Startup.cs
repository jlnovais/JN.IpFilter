using JN.IpFilter.APITest.HelperClasses;
using JN.IpFilter.APITest.Services;
using JN.IpFilter.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JN.IpFilter.APITest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IWeatherService, WeatherService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // virtual is used to override in tests
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            var filters = Configuration.GetIpFilters("IpFilters");
            var options = Configuration.GetIpFilterOptions("IpFilterMiddlewareOptions");

            app.UseIpFilter(filters, options);


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
