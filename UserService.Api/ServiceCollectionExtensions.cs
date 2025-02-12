using UserService.Repository.Interfaces;
using UserService.Repository;
using UserService.Application.Interfaces;
using UserService.Application;
using UserService.Application.Mappings;
using UserService.Middlewares;
using FluentValidation;
using UserService.Api.Validators;
using UserService.Api.ViewModels;

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
            services.AddAutoMapper(typeof( UserDtoToUser));  //remove the space between typeof and (
        }

        //we added the validator here but didnt add it in program.cs
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserViewModel>, CreateUserValidator>();
        }

        public static void AddControllerUtilities(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public static void ConfigureApplication(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
//remove empty line
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
//remove empty line
        }
    }
}
