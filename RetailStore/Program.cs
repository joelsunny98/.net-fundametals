using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using RetailStore.Repository;
using System.Reflection;

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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
    services.AddMediatR(configure => configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}

RetailStoreDbContext ConfigureDbContext(IServiceCollection services, ConfigurationManager configuration)
{
    var dbContextOptions = new DbContextOptionsBuilder<RetailStoreDbContext>().UseNpgsql(configuration.GetConnectionString("DbConnection")).Options;
    var dbContext = new RetailStoreDbContext(dbContextOptions);

    services.AddSingleton(dbContext);

    return dbContext;
}
