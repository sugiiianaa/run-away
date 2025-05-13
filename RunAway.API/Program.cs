using RunAway.Application;
using RunAway.Infrastructure;
using RunAway.Infrastructure.Extensions;

// PostgreSQL timestamp behavior
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application and infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add authentication and authorization
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication middleware BEFORE authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();