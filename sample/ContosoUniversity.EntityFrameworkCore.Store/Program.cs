using Balea;
using Balea.Store.EntityFrameworkCore;
using ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBalea(options =>
    {
        options.ClaimTypeMap = new ClaimTypeMap
        {
            RoleClaimType = JwtClaimTypes.Role,
            NameClaimType = JwtClaimTypes.Name,
        };

        options.ClaimTypeMap.AllowedSubjectClaimTypes.Clear();
        options.ClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
    })
    .AddEntityFrameworkCoreStore((sp, options) =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();

        options.UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
    })
    .AddAuthorization();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:6001";
        options.ClientId = "interactive";
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.ResponseType = "code";
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");
        options.Scope.Add("grades");

        options.UsePkce = true;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = JwtClaimTypes.Role,
            NameClaimType = JwtClaimTypes.Name
        };

        options.MapInboundClaims = false;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    await app.MigrateDbContextAsync<BaleaDbContext>(async (db, acc) => await BaleaSeeder.Seed(db, acc));
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
