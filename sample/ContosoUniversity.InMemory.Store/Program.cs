using Balea;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("balea.json", optional: false, reloadOnChange: true);

builder.Services
    .AddBalea(options =>
    {
        options.ClaimTypeMap = new ClaimTypeMap
        {
            PermissionClaimType = "permissions"
        };
    })
    .AddInMemoryStore()
    .AddAuthorization(options =>
    {
        options.Events.UnauthorizedFallback = AuthorizationFallbackAction.RedirectToAction("Account", "AccessDenied");
    });

builder.Services
    .AddAuthentication(configureOptions =>
    {
        configureOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        configureOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, setup =>
    {
        setup.LoginPath = "/Account/Login";
        setup.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    await app.ImportConfigurationAsync();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
