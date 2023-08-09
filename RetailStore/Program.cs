using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RetailStore.Application;
using RetailStore.Contracts;
using RetailStore.Infrastructure;
using RetailStore.Persistence;
using RetailStore.Repository;
using RetailStore.Services;
using System.Reflection;
using Twilio.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IRetailStoreDbContext>();
    await dbContext.RunMigrations();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddDbContext<RetailStoreDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
    services.AddScoped<IRetailStoreDbContext>(provider => provider.GetService<RetailStoreDbContext>());

    services.AddScoped<IProductBarCodeService, ProductBarCodeService>();
    services.AddScoped<IPremiumCodeService, PremiumCodeService>();
    services.AddControllers();
    services.AddHttpClient<ITwilioRestClient, TwilioClient>();
    services.AddScoped<ITwilioRestClient, TwilioClient>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    services.AddMediatR(configure => configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddFluentValidationAutoValidation();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration);
}
