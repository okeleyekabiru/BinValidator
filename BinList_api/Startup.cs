using BinList_api.Models;
using BinList_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BinList_api
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
            services.Configure<BinConfig>(Configuration.GetSection("BinConfig"));
            services.AddHttpClient<IBinValidator, BinValidator>(opt => {
                opt.DefaultRequestHeaders.Clear();
                opt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddSwaggerGen();
            var googleCred = Configuration.GetValue<GoogleConfig>("Google");

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = "UnAuthorized";
            })
         .AddGoogle(options =>
         {
             IConfigurationSection googleAuthNSection =
                 Configuration.GetSection("Google");

             options.ClientId = googleAuthNSection["ClientId"];
             options.ClientSecret = googleAuthNSection["ClientSecret"];
         }).AddFacebook(opt => {
             IConfigurationSection fbAuthNSection =
                 Configuration.GetSection("OAuthFacebook");
             opt.AppId = fbAuthNSection["AppId"];
             opt.AppSecret = fbAuthNSection["AppSecret"];
         });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
