using System.IO;
using Amozegar.Data;
using Amozegar.Data.SeedData;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Add Db Context
builder.Services.AddDbContext<AmozegarContext>(option =>
{
    option.UseSqlServer("Data Source=.; Initial Catalog=Amozegar_DB; Integrated Security=true; TrustServerCertificate=True");
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AmozegarContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
});

#endregion

var app = builder.Build();

#region Set Seed Datas

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await SeedData.InitializeAsync(services);
//}

#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error404");
app.UseHttpsRedirection();
app.UseRouting();
app.Use(async (context, next) => {

    var path = context.Request.Path.Value?.ToLower();
    if (context.User.Identity.IsAuthenticated &&
        (path.Contains("/account/login") || path.Contains("/account/register")))
    {
        context.Response.Redirect("/");
    }

    await next.Invoke();
});
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
