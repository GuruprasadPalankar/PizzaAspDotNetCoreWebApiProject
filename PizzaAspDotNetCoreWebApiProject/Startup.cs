using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using PizzaAspDotNetCoreWebApiProject.Data;
using PizzaAspDotNetCoreWebApiProject.PizzaValidations;
using PizzaAspDotNetCoreWebApiProject.PizzaRepository;
using PizzaAspDotNetCoreWebApiProject.NLog;
using PizzaAspDotNetCoreWebApiProject.Extension;

namespace PizzaAspDotNetCoreWebApiProject
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
            services.AddControllers();
            services.AddDbContext<PizzaDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("PizzaDbConnection")));
            services.AddControllers().AddXmlSerializerFormatters();
            services.AddScoped<IPizzaDbContext, PizzaDbContext>();
            services.AddScoped<IValidations, Validations>();
            services.AddScoped<IPizzaRepositoryLogic, PizzaRepositoryLogic>();
            services.AddScoped<IPizzaNLog, PizzaNLog>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PizzaDbContext pizzaDbContext, IPizzaNLog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            pizzaDbContext.Database.Migrate();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
