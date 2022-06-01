using AccessControl.Extensions;
using CoreIndustriaHuitzil.Models;
using Microsoft.EntityFrameworkCore;
using ServiceIndustriaHuitzil.Services;
using System.Text.Json.Serialization;

namespace waRESTIndustriaHuitzil
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJWTTokenServices(Configuration);
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddSqlServer<IndustriaHuitzilDbContext>(
                    Configuration.GetConnectionString("DefaultConnection"));
            services.AddCors(options => options.AddPolicy("AllowWebApp",
                                builder => builder.AllowAnyOrigin()
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()));
            services.AddDbContext<IndustriaHuitzilDbContext>(
                               options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IIndustriaHuitzilService, IndustriaHuitzilService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowWebApp");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
