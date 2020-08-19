using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rules.Data;
using Rules.Data.Profiles;
using Rules.Service;
using Rules.Service.Interfaces;
using Rules.Service.Profiles;
using Rules.WebApi.Middleware;
using Rules.WebApi.Profiles;

namespace Rules.WebApi
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
            services.AddScoped<IRulesRepository, RulesRepository>();
            services.AddScoped<IRulesService, RulesService>();
            services.AddDbContext<RulesContext>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("Rules")));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new RulesApiProfile());
                mc.AddProfile(new RulesRepositoryProfile());
                mc.AddProfile(new RulesServiceProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "RulesOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Rules API",
                        Version = "1"
                    });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware(typeof(RulesErrorsHandlerMiddleware));
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/RulesOpenAPISpecification/swagger.json",
                    "Rules API"
                    );
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
