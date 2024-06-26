
using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddSignalR();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200","http://localhost:4200" ));

app.UseHttpsRedirection();

app.UseAuthentication();    

app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("/hubs/presence");
app.MapHub<MessageHub>("/hubs/message");


try
{
    var context = services.GetRequiredService<DataContext>();

    var userManager = services.GetRequiredService<UserManager<AppUsers>>();

    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    
    await context.Database.MigrateAsync();
    
    await Seed.SeedUser(userManager, roleManager);
} 
catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

// await app.RunAsync();
app.Run();