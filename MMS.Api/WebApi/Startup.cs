using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MMS.Api.BussinessServices.Concrets.Services;
using MMS.Api.Common.Concretes;
using MMS.Api.Common.Filters;
using MMS.Api.Common.Interfaces;
using MMS.Api.Common.Paginations;
using MMS.Api.DataAccessServices.Concretes.Repositories;
using MMS.Api.DataAccessServices.Entities;
using WebApi.Infastructure;
using WebApi.Middlewares;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
                
        public void ConfigureServices(IServiceCollection services)
        {
            // register automapper
            services.AddAutoMapper(new Type[] { typeof(AutoMapperProfile) });

            // read pagination option data to class
            services.Configure<PaginationOption>(Configuration.GetSection("PaginationOption"));

            // pagination option filter 
            services.AddTransient<PaginationOptionFilter>();

            // register mvc
            services.AddMvc(

                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problems = new CustomBadRequest(context);
                        return new BadRequestObjectResult(problems);
                    };

                    options.ClientErrorMapping[404] = new ClientErrorData() { Link = "", Title = "Not found resources" };
                });

            var connectionString = Configuration.GetConnectionString("MMSDbContext").ToString();
            services.AddDbContext<MMSDbContext>
                (options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("MMS.Api.DataAccessServices.Entities")));


            // Register interfaces and classes 
            // https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
            // register base classes
            services.AddTransient<IService, ServiceBase>();
            services.AddTransient(typeof(IRepository<,,>), typeof(RepositoryBase<,,>));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(CustomerRepository))
               .AddClasses(classes => classes.InExactNamespaceOf<CustomerRepository>())
               .AsImplementedInterfaces()
               .WithTransientLifetime());

            // services
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(CustomerService))
                .AddClasses(classes => classes.InNamespaceOf<CustomerService>().Where(c => c.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // handle error handling globaly using middleware
            app.ConfigureExceptionHandler(env);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
