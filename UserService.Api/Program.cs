using UserService.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllerUtilities();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();

var app = builder.Build();
app.ConfigureApplication();