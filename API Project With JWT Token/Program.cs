using API_Project_With_JWT_Token;
using Infrastructure;
using Application;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebService(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedprovider = scope.ServiceProvider;
    try
    {
        var usermanager = scopedprovider.GetRequiredService<UserManager<ApplicationUser>>();
        var rolemanager = scopedprovider.GetRequiredService<RoleManager<IdentityRole>>();
        var identityContext = scopedprovider.GetRequiredService<ApplicationDbContext>();
        await ApplicationIdentityDbContextSeed.SeedAsync(identityContext, usermanager, rolemanager);
    }
    catch (Exception ex)
    {
        //throw new Exception(ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
