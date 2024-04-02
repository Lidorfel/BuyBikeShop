using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BuyBikeShopContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));
builder.Services.AddIdentity<Customer,IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    }
    ).AddEntityFrameworkStores<BuyBikeShopContext>().AddDefaultTokenProviders();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7); // Sets the session timeout
    options.Cookie.HttpOnly = true; // Makes the cookie inaccessible to client-side scripts
    options.Cookie.IsEssential = true; // Marks the session cookie as essential for your application
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStaticFiles();

app.UseSession(); // Add this line here to ensure session middleware is used

app.UseRouting();



app.UseAuthentication(); // Ensure authentication middleware is used
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

