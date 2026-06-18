using sapo_clone.API;
using sapo_clone.Application;
using sapo_clone.Application.Extensions;
using sapo_clone.Infrastructure;
using sapo_clone.Infrastructure.Data;
using sapo_clone.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

//For Development only
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin() // Hoặc điền cụ thể URL ngrok của ReactJS vào đây
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices();
//builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices();
builder.Services.AddCustomIdentity();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAppCors(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwaggerGen();
//builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

//app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.EnsureMigrations();

await app.SeedData();

app.Run();
