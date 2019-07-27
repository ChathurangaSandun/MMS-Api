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
using Accoon.MMS.Api.Persistence.IdentityDatabaseContext;
using Accoon.MMS.Api.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using Accoon.MMS.Api.Presenter.Models.Settings;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Accoon.MMS.Api.Infastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Accoon.MMS.Api.Infastructure.Helper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Accoon.MMS.Api.Application.Interfaces.Services.Auth;
using Accoon.MMS.Api.Application.UserCases.AccountActor.login;
using Accoon.MMS.Api.Persistence.Repositories;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            // register repositories
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(UserRepository))
                .AddClasses(classes => classes.InExactNamespaceOf<UserRepository>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());


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
            services.AddMediatR(typeof(LoginRequestHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(LoginResponseHandler).GetTypeInfo().Assembly);



            // health check
            services.AddHealthChecks()
               .AddSqlServer(connectionString); // sql server health check

            // idnetity framework
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Accoon.MMS.Api.Persistence")));



            services.AddTransient<ITokenFactory, Accoon.MMS.Api.Infastructure.Auth.TokenFactory>();
            services.AddTransient<IJwtFactory, Accoon.MMS.Api.Infastructure.Auth.JwtFactory>();
            services.AddTransient<IJwtTokenHandler, Accoon.MMS.Api.Infastructure.Auth.JwtTokenHandler>();
            services.AddTransient<IJwtTokenValidator, JwtTokenValidator>();


            // Register the ConfigurationBuilder instance of AuthSettings
            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var identityBuilder = services.AddIdentityCore<AppUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MMS web api", Version = "v1" });                
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });



            // Now register our services with Autofac container.
            var builder = new ContainerBuilder();

            //builder.RegisterModule(new CoreModule());
            //builder.RegisterModule(new InfrastructureModule());

            // Presenters
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Presenter")).SingleInstance();

            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);


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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
