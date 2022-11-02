using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Blazored.Toast;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using PetTracker.Server.Areas.Identity;
using PetTracker.Server.Data;
using PetTracker.Server.Entities;
using PetTracker.Server.Interfaces;
using PetTracker.Server.Repositories;
using PetTracker.Server.Repository;
using PetTracker.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<PetTrackerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PetTrackerDbConnectionString")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ApplicationUserClaimsPrincipalFactory>();

var emailConfig = builder.Configuration.GetSection(EmailConfiguration.EmailConfig).Get<EmailConfiguration>();

builder.Services.AddMailKit(optionsBuilder =>
{
    optionsBuilder.UseMailKit(new MailKitOptions
    {
        Server = emailConfig.SmtpServer,
        Port = emailConfig.Port,
        SenderName = emailConfig.SenderName,
        SenderEmail = emailConfig.From,
        Account = emailConfig.UserName,
        Password = emailConfig.Password,
        Security = true
    });
});

builder.Services.AddScoped<IPetsRepository, PetsRepository>(pr => new PetsRepository(
    pr.GetRequiredService<ILogger<PetsRepository>>(),
    pr.GetRequiredService<IHttpClientFactory>(),
    pr.GetRequiredService<PetTrackerDbContext>(),
    pr.GetRequiredService<IMapper>(),
    builder.Configuration.GetValue<string>("LocationDataRelativeUrl"),
    builder.Configuration.GetValue<string>("AirDataRelativeUrl"),
    builder.Configuration.GetValue<string>("FlameDetectionDataRelativeUrl")
));

builder.Services.AddScoped<IGpsRepository, GpsRepository>(gr => new GpsRepository(
    gr.GetRequiredService<ILogger<GpsRepository>>(),
    gr.GetRequiredService<IHttpClientFactory>(),
    gr.GetRequiredService<PetTrackerDbContext>(),
    gr.GetRequiredService<IMapper>(),
    builder.Configuration.GetValue<string>("Google:MapsAPIKeyCode")));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAirSensorsRepository, AirSensorsRepository>();
builder.Services.AddScoped<IFlameDetectionSensorsRepository, FlameDetectionSensorsRepository>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<GetAndCheckPetsLocationService>();
builder.Services.AddHostedService<PeriodicLocationCheckerHostedService>();

builder.Services.AddBlazoredToast();

builder.Services.AddHttpClient("PetTracker", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PetTrackerBaseUrl"]);
});

builder.Services.AddHttpClient("GMPlaces", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Google:MapsPlacesBaseUrl"]);
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<CroatianIdentityErrorDescriber>()
    .AddEntityFrameworkStores<PetTrackerDbContext>()
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, PetTrackerDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("full_name");
        options.ApiResources.Single().UserClaims.Add("full_name");
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

var keyVaultSecretClient = new SecretClient(new Uri(builder.Configuration.GetValue<string>("KeyVault:KeyVaultUrl")), new DefaultAzureCredential());
var googleClientId = keyVaultSecretClient.GetSecret(builder.Configuration.GetValue<string>("Google:KeyVaultClientIdSecretName")).Value.Value;
var googleClientSecret = keyVaultSecretClient.GetSecret(builder.Configuration.GetValue<string>("Google:KeyVaultClientSecretSecretName")).Value.Value;

builder.Services.AddAuthentication()
    .AddIdentityServerJwt()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<PetTrackerDbContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/api"), blazor =>
{
    blazor.UseEndpoints(endpoints =>
    {
        endpoints.MapFallbackToFile("index.html");
    });
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), api =>
{
    api.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
    });
});

app.Run();
