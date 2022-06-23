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
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using OutdoorsmanBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OutdoorsmanBackend
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

            //service method for using a JWT authentication schema 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                //parameters for a JWT token to be valid
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //is the calling server valid?
                    ValidateIssuer = true,
                    //is the recipient receiving the token valid
                    ValidateAudience = true,
                    //is the token not expired and is the signing key of the issuer still valid?
                    ValidateLifetime = true,
                    //is the token valid?
                    ValidateIssuerSigningKey = true,
                    //does the issuer match?
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    //does the audience match?
                    ValidAudience = Configuration["Jwt:Audience"],
                    //does the key match?
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            //enables CORS Cross-Origin Resource Sharing
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder
                         //only allows our front-end access to the database
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                        );
            });

            //links the context files to the database
            services.AddDbContext<ProductContext>(opt =>
           opt.UseSqlite("Data Source=app.db"));

            services.AddDbContext<UserContext>(opt =>
           opt.UseSqlite("Data Source=app.db"));

            services.AddDbContext<OrderContext>(opt =>
           opt.UseSqlite("Data Source=app.db"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OutdoorsmanBackend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OutdoorsmanBackend v1"));
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
