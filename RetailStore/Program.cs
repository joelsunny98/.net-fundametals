using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using RetailStore.Contracts;
using RetailStore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);
var dbContext = ConfigureDbContext(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

dbContext.Database.Migrate();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IProductBarCodeService, ProductBarCodeService>();
    services.AddControllers();
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

RetailStoreDbContext ConfigureDbContext(IServiceCollection services, ConfigurationManager configuration)
{
    var dbContextOptions = new DbContextOptionsBuilder<RetailStoreDbContext>().UseNpgsql(configuration.GetConnectionString("DbConnection")).Options;
    var dbContext = new RetailStoreDbContext(dbContextOptions);

    services.AddSingleton(dbContext);

    return dbContext;
}
