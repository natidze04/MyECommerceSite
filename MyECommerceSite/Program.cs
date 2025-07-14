using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyECommerceSite.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Database + Identity Setup
// -------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>() // ← აუცილებელია როლებისთვის
    .AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------------
// MVC + Razor + Session
// -------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opts =>
{
    opts.IdleTimeout = TimeSpan.FromMinutes(30);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
});

var app = builder.Build();

// -------------------------
// Seed Roles & Admin
// -------------------------
await SeedRolesAndAdminAsync(app.Services); // ⬅ ამ ხაზს ვამატებთ

// -------------------------
// Middleware Pipeline
// -------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();  // აუცილებელია მანამდე ვიდრე Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

// -------------------------
// Seeder Function
// -------------------------
async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var adminRoleName = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRoleName))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRoleName));
    }

    var adminEmail = "admin@myecommerce.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, "Admin123!"); // პაროლი გამოცვალე მომავალში
    }

    if (!await userManager.IsInRoleAsync(adminUser, adminRoleName))
    {
        await userManager.AddToRoleAsync(adminUser, adminRoleName);
    }
}
