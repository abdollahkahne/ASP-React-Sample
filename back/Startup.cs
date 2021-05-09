using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using back.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace back
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
            
            services.AddScoped<IDataRepository,DataRepository>();          

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "back", Version = "v1" });
            });
            services.AddMemoryCache();
            services.AddSingleton<IQuestionCache,QuestionCache>();

            services.AddAuthentication(options =>{
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>{
                options.Authority=Configuration["Auth0:Authority"];
                options.Audience=Configuration["Auth0:Audience"];
            });
            services.AddHttpClient();
            services.AddAuthorization(option =>{
                option.AddPolicy("QuestionAuthor",policy =>{
                    // policy.RequireAuthenticatedUser();
                    // policy.AddRequirements(new MustBeQuestionAuthorRequirement());
                    policy.Requirements.Add(new MustBeQuestionAuthorRequirement());
                });
            });
            // This can be added Multiple times for different Handler. But What happen 
            // if we inject IAuthorizationHandler somewhere?!
            services.AddScoped<IAuthorizationHandler,MustBeQuestionAuthorHandler>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "back v1"));
            }

            app.UseHttpsRedirection();

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
