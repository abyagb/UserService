using UserService.Repository.Interfaces;
using UserService.Repository;
using UserService.Application.Interfaces;
using UserService.Application;
using UserService.Application.Mappings;
using UserService.Middlewares;
using FluentValidation;
using UserService.Api.Validators;
using UserService.Api.ViewModels;
using UserService.Application.Validators;
using UserService.Api.Mappings;
using UserService.Validators;
using UserService.ViewModels;


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
            services.AddScoped<IEndUserService, EndUserService>();
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(UserModelProfile));
        }


        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserViewModel>, CreateUserValidator>();

            services.AddScoped<IEndUserValidator,EndUserValidator>();

            services.AddScoped<IValidator<EditUserViewModel>, EditUserValidator>();
        }

        public static void AddControllerUtilities(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public static void ConfigureApplication(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();
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

