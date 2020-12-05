using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PrimeNumbers.PrimesDB.Core;

namespace PrimeNumbers.PrimesDB.API
{
    public class Startup
    {
        private PrimeCacheDb _primeCacheDb;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            _primeCacheDb = CreatePrimeCacheDb();

            services.AddSingleton<PrimeCacheDb>(_primeCacheDb);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrimeNumbers.PrimesDB.API", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeNumbers.PrimesDB.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationLifetime.ApplicationStopping.Register(OnStop);
        }

        private void OnStop()
        {
            _primeCacheDb.Dispose();
        }

        private static PrimeCacheDb CreatePrimeCacheDb()
        {
            var connectionParameters = new ConnectionParameters
            {
                Server = "192.168.1.18",
                Port = 3306,
                Database = "myDb",
                Username = "root",
                Password = "qwer1234"
            };
            return new PrimeCacheDbFactory(connectionParameters).CreateConnection();
        }
    }
}
