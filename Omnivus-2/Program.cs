using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Omnivus_2.Models.Data;
using Omnivus_2.Models.Identity;
using Omnivus_2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(x => x.User.RequireUniqueEmail = true).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaims>();
builder.Services.AddScoped<IProfileManager, ProfileManager>();
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin";
    x.AccessDeniedPath = "/access-denied";
});





var app = builder.Build();

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

//kunna inlogningsbiten
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
