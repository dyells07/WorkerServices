
using RDLC_Demo.Models;
using RDLC_Demo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();
builder.Services.AddTransient<IRepositoryVoucher, VoucherRepository>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<AppDbConnection>(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Report}/{action=Index}/{id?}");

app.Run();