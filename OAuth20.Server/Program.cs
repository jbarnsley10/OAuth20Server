/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuth20.Server.Configuration;
using OAuth20.Server.Models.Entities;
using OAuth20.Server.Services;
using OAuth20.Server.Services.CodeServce;
using System;

var builder = WebApplication.CreateBuilder(args);
var configServices = builder.Configuration;

builder.Services.Configure<OAuthOptions>(configServices.GetSection("OAuthOptions"));
builder.Services.AddScoped<IAuthorizeResultService, AuthorizeResultService>();
builder.Services.AddSingleton<IInMemoryUserManager, InMemoryUserManager>();
builder.Services.AddSingleton<ICodeStoreService, CodeStoreService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseQueryStrings = true;
    options.LowercaseUrls = true;
});
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.Run();
