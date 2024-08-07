using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
using Template.Api.Common;
using Template.Api.Interfaces;
using Template.Api.Persistence;
using Template.Api.Services;

namespace Template.Api
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
            #region Configure Services

            services.AddControllers(options => { options.ReturnHttpNotAcceptable = true; })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddRouting(options => { options.LowercaseUrls = true; });

            services.AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                })
                .AddMvc(options => options.Conventions.Add(new VersionByNamespaceConvention()))
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, VersioningOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<ResponseContentTypeFilter>();
            });

            #endregion

            #region Custom Dependencies

            services.AddDbContext<UserContext>(options => options.UseInMemoryDatabase("users"));
            services.AddHttpClient<ISomeSystemApiClient, SomeSystemApiClient>();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            #region Configure

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = Assembly.GetEntryAssembly().GetName().Name;

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(
                             api => api.GroupName))
                {
                    options.SwaggerEndpoint($"{description.GroupName}/swagger.yaml",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion
        }
    }
}
