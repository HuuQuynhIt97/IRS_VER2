using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using IRS.Helpers;
using IRS.Installer;
using System.IO;
using System;
using IRS.Data;
using Microsoft.EntityFrameworkCore;

namespace IRS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            string rootdir = Directory.GetCurrentDirectory();

            try
            {
                Aspose.Cells.License cellLicense = new Aspose.Cells.License();
                string filePath = rootdir + "/wwwroot/" + "Aspose.Total.lic";
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                cellLicense.SetLicense(fileStream);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             });

            services.InstallServicesInAssembly(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.SetIsOriginAllowed(_ => true).
                AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = @"wwwroot/ClientApp";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseSpaStaticFiles();
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = @"wwwroot/ClientApp";
            //    //if (env.IsDevelopment())
            //    //{
            //    //    spa.Options.SourcePath = @"../dmr-app";
            //    //    spa.UseAngularCliServer(npmScript: "start");
            //    //}
            //});
        }
    }
}
