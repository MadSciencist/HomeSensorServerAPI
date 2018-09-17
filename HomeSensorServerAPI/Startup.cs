using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace HomeSensorServerAPI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var dbConnectionString = Configuration["ConnectionStrings:MySqlConnection"];

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(dbConnectionString, mysqlOptions => mysqlOptions.ServerVersion(new Version(10, 1, 29), ServerType.MariaDb));
            });

            services.AddTransient<INodeRepository, NodeRepository>();
            services.AddTransient<ISensorRepository, SensorRepository>();
            services.AddTransient<IStreamingDeviceRepository, StreamingDeviceRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISystemSettingsRepository, SystemSettingsRepository>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(json => json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
     

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("dev", new Info { Title = "Home Sensor Server API", Version = "v1", Contact = new Contact { Email = "mkrysz1337@gmail.com" } });
            });

            AddJwtAuthentication(services);
            AddAuthorizationPolicies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if (env.IsProduction())
            {
                Console.WriteLine("PROD");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/dev/swagger.json", "Home Sensor Server API");
            });
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["AuthenticationJwt:Issuer"],
                    ValidAudience = Configuration["AuthenticationJwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationJwt:Key"])),
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });
        }

        private void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
                options.AddPolicy("Manager", policy => policy.RequireClaim("Manager"));
                options.AddPolicy("Viewer", policy => policy.RequireClaim("Viewer"));
                options.AddPolicy("Sensor", policy => policy.RequireClaim("Sensor"));
            });
        }
    }
}
