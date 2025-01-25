using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Orca.WebTemplate.Components;
using Orca.WebTemplate.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDbContext<ContosoDbContext>((sp, options) => {
    var configuration = sp.GetRequiredService<IConfiguration>();

    options.UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptions =>
    {
        sqlServerOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});

builder.Services
    .AddOrca()
    .AddEntityFrameworkStores<ContosoDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var endpoints = app.MapGroup("api");
endpoints.MapGroup("access-control").MapAccessControlApi();

app.Run();
