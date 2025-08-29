using Chatbot.Common;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Service;
using Chatbot.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<DataContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.ConnectionSqlServer))
    .ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = new PathString("/Login");
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddSignalR();

builder.Services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IDbConnectionService, DbConnectionService>();
builder.Services.AddTransient<IIntentService, IntentService>();
builder.Services.AddTransient<IPatternService, PatternService>();
builder.Services.AddTransient<IResponseService, ResponseService>();
builder.Services.AddTransient<ISynonymService, SynonymService>();
builder.Services.AddTransient<IKeywordBoostService, KeywordBoostService>();
builder.Services.AddTransient<IChatbotService, ChatbotService>();

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<ChatHub>("/chatbot");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
