using UserService.Repository.Interfaces;
using UserService.Repository;
using UserService.Application.Interfaces;
using UserService.Application;
using UserService.Application.Mappings;

namespace UserService.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
           services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEndUserService,EndUserService>();
            services.AddAutoMapper(typeof( UserDtoToUser));
        }

        public static void AddControllerUtilities(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public static void ConfigureApplication(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}
