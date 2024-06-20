using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using proiect.Data;
using Microsoft.AspNetCore.Identity;
using proiect.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
 options.Conventions.AuthorizeFolder("/Movies");
 options.Conventions.AllowAnonymousToPage("/Movies/Index");
 options.Conventions.AllowAnonymousToPage("/Movies/Details");  
 options.Conventions.AuthorizeFolder("/Members", "AdminPolicy");
 options.Conventions.AuthorizeFolder("/Genres", "AdminPolicy");
 options.Conventions.AuthorizeFolder("/Producers", "AdminPolicy");
}); 
builder.Services.AddDbContext<proiectContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("proiectContext") ?? throw new InvalidOperationException("Connection string 'proiectContext' not found.")));

builder.Services.AddDbContext<LibraryIdentityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("proiectContext") ?? throw new InvalidOperationException("Connection string 'proiectContext' not found.")));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
