using DatabaseManager.DataAccess.DbContext;
using DatabaseManager.DataAccess.Repository;
using DatabaseManager.DataAccess.Repository.IRepository;
using DatabaseManager.Models;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
var connectionStringShard1 = builder.Configuration.GetConnectionString("WebDbContextConnectionShard1")
                       ?? throw new InvalidOperationException("Connection string 'WebDbContextConnectionShard1' not found.");
var connectionStringShard2 = builder.Configuration.GetConnectionString("WebDbContextConnectionShard2")
                             ?? throw new InvalidOperationException("Connection string 'WebDbContextConnectionShard2' not found.");

builder.Services.AddDbContext<WebDbContextShard1>(options =>
    options.UseSqlServer(connectionStringShard1,
        optionsAction => optionsAction.MigrationsAssembly("DatabaseManager.Web")));
builder.Services.AddDbContext<WebDbContextShard2>(options =>
    options.UseSqlServer(connectionStringShard2,
        optionsAction => optionsAction.MigrationsAssembly("DatabaseManager.Web")));

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<Customer>();
builder.Services.AddScoped<Order>();
builder.Services.AddScoped<Payment>();
builder.Services.AddScoped<Product>();
builder.Services.AddScoped<Review>();
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions
{
    ProgressBar = true,
    TimeOut = 5000,
    PositionClass = "toast-bottom-right"
});

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
app.UseAuthentication(); ;
app.UseAuthorization();
app.UseNToastNotify();
app.MapRazorPages();

app.Run();
