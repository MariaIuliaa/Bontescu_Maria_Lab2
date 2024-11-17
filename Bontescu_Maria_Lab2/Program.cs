using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bontescu_Maria_Lab2.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});
// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Books");
    options.Conventions.AllowAnonymousToPage("/Books/Index - Copy");
    options.Conventions.AllowAnonymousToPage("/Books/Details");
    options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");

});

builder.Services.AddDbContext<Bontescu_Maria_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Bontescu_Maria_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Bontescu_Maria_Lab2Context' not found."))
);

builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Bontescu_Maria_Lab2Context")
        ?? throw new InvalidOperationException("Connection string 'Bontescu_Maria_Lab2Context' not found."))
);

// Adaugã identitatea o singurã datã
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<LibraryIdentityContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
