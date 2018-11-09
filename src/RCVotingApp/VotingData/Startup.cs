using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Mesh.AspNetCore.Data;

namespace VotingData
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRouting();
            services.AddSingleton<HttpClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseReliableCollections((builder) =>
            {
                builder.MapRoute("api/votesdata/all/{voter}", (context) =>
                {
                    var key = (context.GetRouteValue("voter").ToString().ToUpper()[0] - 'A') % 3;
                    return key;
                });
                builder.MapRoute("api/votesdata/votes/{voter}", (context) =>
                {
                    var key = (context.GetRouteValue("voter").ToString().ToUpper()[0] - 'A') % 3;
                    return key;
                });
            });


            app.UseMvc();

        }
    }
}
