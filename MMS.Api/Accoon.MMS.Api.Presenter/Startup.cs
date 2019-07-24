using Accoon.MMS.Api.Presenter.Infastructure;
using Accoon.MMS.Api.Presenter.Middlewares;
using Accoon.MMS.Api.Application.Infastructure.Automapper;
using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer;
using Accoon.MMS.Api.Persistence.DatabaseContext;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Accoon.MMS.Api.Presenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // fluent validation
                .AddFluentValidation(
                fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();
                    fv.ImplicitlyValidateChildProperties = true;
                })
                // handle 404 error
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problems = new CustomBadRequest(context);
                        return new BadRequestObjectResult(problems);
                    };

                    options.ClientErrorMapping[404] = new ClientErrorData() { Link = "", Title = "Not found resources" };
                });

            // register auto mapper
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            // register db context and migration assebly
            var connectionString = Configuration.GetConnectionString("Context").ToString();
            services.AddDbContext<DefaultDatabaseContext>
                (options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Accoon.MMS.Api.Persistence")));
            services.AddTransient<IDatabaseContext, DefaultDatabaseContext>();

            // register mediatr and command handlers
            services.AddMediatR(typeof(CreateCustomerHandler).GetTypeInfo().Assembly);

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            // health check
            services.AddHealthChecks()
               .AddSqlServer(connectionString); // sql server health check
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // healthcheck middleware
            app.UseHealthChecks("/hc",
                new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable swagger middleware 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // handle error handling globaly using middleware
            app.ConfigureExceptionHandler(env);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
