using FamousQuoteQuiz.Application;
using FamousQuoteQuiz.Infrastructure;
using FamousQuoteQuiz.Infrastructure.Database;
using FamousQuoteQuiz.Web.Constants;
using FamousQuoteQuiz.Web.Services;
using FamousQuoteQuiz.Web.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Register application services
RegisterWebServices(builder.Services);

// Configure authentication
ConfigureAuthentication(builder.Services);

// Configure session
ConfigureSession(builder.Services);

// Register application and infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
ConfigureMiddleware(app);

// Initialize database
await InitializeDatabaseAsync(app);

app.Run();

static void RegisterWebServices(IServiceCollection services)
{
    services.AddHttpContextAccessor();
    services.AddScoped<IUserClaimsService, UserClaimsService>();
    services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
}

static void ConfigureAuthentication(IServiceCollection services)
{
    // Use ephemeral (in-memory only) data protection keys
    // Keys are regenerated on each app restart, invalidating old cookies
    services.AddDataProtection()
        .SetApplicationName("FamousQuoteQuiz")
        .DisableAutomaticKeyGeneration();

    // Configure ephemeral data protection provider
    services.AddSingleton<IDataProtectionProvider>(serviceProvider =>
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"FamousQuoteQuiz-{Guid.NewGuid()}");
        return DataProtectionProvider.Create(new DirectoryInfo(tempPath));
    });

    // Configure cookie authentication
    services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = AuthConstants.LoginPath;
            options.AccessDeniedPath = AuthConstants.LoginPath;
            options.ExpireTimeSpan = TimeSpan.FromHours(AuthConstants.SessionExpirationHours);
            options.SlidingExpiration = true;
            options.Cookie.MaxAge = null; // Session cookie
        });

    services.AddAuthorization();
}

static void ConfigureSession(IServiceCollection services)
{
    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
}

static void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseSession();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await AppDbContext.SeedAsync(db);
}
