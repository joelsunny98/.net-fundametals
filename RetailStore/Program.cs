using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using RetailStore.Contracts;
using Twilio.Rest.Api.V2010.Account;
using RetailStore.Services;
using Twilio.Clients;
using SixLabors.ImageSharp;

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
}
