using DB;
using DB.Repository;
using DB.Repository.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefactorThis.DB.Repository;
using RefactorThis.Mapper;
using RefactorThis.Mapper.Interface;

namespace RefactorThis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json");
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddDbContext<ProductsContext>(options => options.UseSqlite(Configuration["ConnectionStrings:Default‌​Connection"]));

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ApplicationLogger>>();
            services.AddSingleton(typeof(ILogger), logger);

            services.AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IProductOptionRepository, ProductOptionRepository>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IProductMapper, ProductMapper>()
                .AddTransient<IProductOptionMapper, ProductOptionMapper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}