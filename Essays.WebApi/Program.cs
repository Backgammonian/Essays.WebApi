using Essays.WebApi.Data;
using Essays.WebApi.Data.Implementations;
using Essays.WebApi.Data.Interfaces;
using Essays.WebApi.Repositories.Implementations;
using Essays.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Essays.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IRandomGenerator, RandomGenerator>();
            builder.Services.AddScoped<ISubjectCategoryRepository, SubjectCategoryRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IEssayRepository, EssayRepository>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            if (args.Length > 0 &&
                args[0].ToLower() == "seeddata")
            {
                await SeedData(app);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static async Task SeedData(IApplicationBuilder app)
        {
            Console.WriteLine("(SeedData) Seeding the database");

            using var serviceScope = app.ApplicationServices.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
            if (dataContext == null)
            {
                Console.WriteLine("(SeedData) Can't seed the database!");

                return;
            }

            var seeder = new Seeder(dataContext);
            await seeder.Seed();
        }
    }
}